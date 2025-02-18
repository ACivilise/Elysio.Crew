using Elysio.Data;
using Elysio.Entities;
using Elysio.Models.Enums;
using Elysio.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Elysio.Services
{
    public class ApprovalTerminationStrategy
    {
        private readonly int _maxIterations;
        private readonly HashSet<string> _approverNames;
        private int _currentIteration = 0;

        public ApprovalTerminationStrategy(int maxIterations, IEnumerable<string> approverNames)
        {
            _maxIterations = maxIterations;
            _approverNames = new HashSet<string>(approverNames);
        }

        public bool ShouldTerminate(string content, string agentName)
        {
            _currentIteration++;

            // Check if max iterations reached
            if (_currentIteration >= _maxIterations)
            {
                return true;
            }

            // Only allow approval from designated approvers
            if (!_approverNames.Contains(agentName))
            {
                return false;
            }

            // Terminate if an approver's message contains "approve"
            return content.Contains("approve", StringComparison.OrdinalIgnoreCase);
        }
    }

    public class AgentsService(
            IOllamaService ollamaService,
            ApplicationDbContext dbContext) : IAgentsService
    {
        private const int MaxIterations = 10;

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

            // Create chat agents and store them with their corresponding entities
            var chatAgents = new List<(IChatCompletionService Service, Agent Entity)>();
            foreach (var agent in agents)
            {
                chatAgents.Add((ollamaService.CreateAgent(agent.Name, agent.Prompt), agent));
            }

            // Create a group chat with the agents
            var chatHistory = ollamaService.CreateGroupChat(chatAgents.Select(a => a.Service), initialMessage);

            // Store initial user message
            await StoreMessage(initialMessage, conversationId, RolesEnum.User);

            // Set up the termination strategy - consider the last agent as the approver
            var approver = agents.Last();
            var terminationStrategy = new ApprovalTerminationStrategy(
                MaxIterations,
                new[] { approver.Name }
            );

            foreach (var (service, entity) in chatAgents)
            {
                string? responseContent = null;

                var response = await service.GetChatMessageContentAsync(chatHistory);

                if (response?.Content != null)
                {
                    responseContent = response.Content;

                    // Store the message in the database
                    await StoreMessage(
                        responseContent,
                        conversationId,
                        RolesEnum.Agent,
                        entity.Id
                    );

                    // Add the response to chat history for context
                    chatHistory.AddAssistantMessage(responseContent);

                    // Yield the response with agent information
                    yield return $"{responseContent}|{RolesEnum.Agent}|{entity.Name}";

                    // Check if we should terminate the conversation
                    if (terminationStrategy.ShouldTerminate(responseContent, entity.Name))
                    {
                        yield break;
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