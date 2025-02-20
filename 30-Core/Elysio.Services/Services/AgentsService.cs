using Elysio.Data;
using Elysio.Entities;
using Elysio.Models.Enums;
using Elysio.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Elysio.Services
{
    public class AgentChat
    {
        private readonly List<IChatCompletionService> _agents = new();
        private readonly ChatHistory _history;
        private readonly HashSet<string> _approverNames;
        private bool _isComplete;
        private readonly int _maxIterations;
        private int _currentIteration;

        public bool IsComplete => _isComplete;

        public AgentChat(IEnumerable<IChatCompletionService> agents, IEnumerable<string> approverNames, int maxIterations = 10)
        {
            _agents.AddRange(agents);
            _approverNames = new HashSet<string>(approverNames);
            _maxIterations = maxIterations;
            _history = new ChatHistory();
            _history.AddSystemMessage("You are participating in a group chat. Work together to assist the user.");
        }

        public void AddChatMessage(string content, string role)
        {
            switch (role.ToLower())
            {
                case "system":
                    _history.AddSystemMessage(content);
                    break;
                case "assistant":
                    _history.AddAssistantMessage(content);
                    break;
                case "user":
                    _history.AddUserMessage(content);
                    break;
            }
        }

        public async IAsyncEnumerable<IReadOnlyList<ChatMessageContent>> InvokeAsync()
        {
            while (!_isComplete && _currentIteration < _maxIterations)
            {
                foreach (var agent in _agents)
                {
                    var responses = await agent.GetChatMessageContentsAsync(_history);

                    if (responses.Count > 0)
                    {
                        var response = responses[0];
                        if (!string.IsNullOrEmpty(response.Content))
                        {
                            _history.AddAssistantMessage(response.Content);
                            yield return responses;

                            // Check if this is an approver and they approved
                            var agentName = response.Metadata?["AgentName"]?.ToString();
                            if (agentName != null && 
                                _approverNames.Contains(agentName) && 
                                response.Content.Contains("approve", StringComparison.OrdinalIgnoreCase))
                            {
                                _isComplete = true;
                                yield break;
                            }
                        }
                    }
                }
                _currentIteration++;
            }
        }
    }

    public class AgentsService(
            IOllamaService ollamaService,
            ApplicationDbContext dbContext) : IAgentsService
    {
        public async IAsyncEnumerable<string> ChatWithAgents(string initialMessage, Guid conversationId)
        {
            var conversation = await dbContext.Conversations
                .Include(c => c.Room)
                .ThenInclude(r => r.Agents)
                .FirstOrDefaultAsync(c => c.Id == conversationId);

            if (conversation == null)
                throw new InvalidOperationException($"Conversation {conversationId} not found");

            var agents = conversation.Room.Agents
                .OrderBy(a => a.CreatedAt)
                .ToList();

            if (agents.Count < 2)
                throw new InvalidOperationException("Room must have at least 2 agents for a conversation");

            // Create chat agents for each participant
            var chatAgents = agents.Select(agent => 
                ollamaService.CreateCompletionService(agent.Name, agent.Prompt)).ToList();

            // Create chat with the agents and set the last agent as approver
            var chat = new AgentChat(chatAgents, new[] { agents.Last().Name });

            // Store and add initial user message
            await StoreMessage(initialMessage, conversationId, RolesEnum.User);
            chat.AddChatMessage(initialMessage, "user");

            // Process the chat
            await foreach (var responses in chat.InvokeAsync())
            {
                foreach (var message in responses)
                {
                    var agentName = message.Metadata?["AgentName"]?.ToString();
                    if (!string.IsNullOrEmpty(agentName) && !string.IsNullOrEmpty(message.Content))
                    {
                        var agentEntity = agents.First(a => a.Name == agentName);
                        
                        // Store the message
                        await StoreMessage(message.Content, conversationId, RolesEnum.Agent, agentEntity.Id);

                        // Yield the response
                        yield return $"{message.Content}|{RolesEnum.Agent}|{agentName}";
                    }
                }
            }
        }

        private async Task StoreMessage(string content, Guid conversationId, RolesEnum role, Guid? agentId = null)
        {
            var message = new Message
            {
                Id = Guid.NewGuid(),
                Content = content,
                Role = role,
                ConversationId = conversationId,
                AgentId = agentId,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            dbContext.Messages.Add(message);
            await dbContext.SaveChangesAsync();
        }
    }
}