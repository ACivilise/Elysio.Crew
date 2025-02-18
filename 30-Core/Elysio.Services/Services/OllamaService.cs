using Elysio.Services.Interfaces;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Elysio.Services
{
    public class OllamaService(IChatCompletionService chatCompletionService, Kernel kernel) : IOllamaService
    {
        private readonly Dictionary<string, string> _agentPrompts = new();

        public async Task<string> Chat(string message, ChatHistory? history = null)
        {
            history ??= new ChatHistory("You are a helpful assistant.");
            history.AddUserMessage(message);

            try
            {
                var chatResult = await chatCompletionService.GetChatMessageContentAsync(message);
                return chatResult?.Content ?? string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception($"Chat completion error: {ex.Message}", ex);
            }
        }

        public IChatCompletionService CreateAgent(string name, string instructions)
        {
            var chatHistory = new ChatHistory();
            chatHistory.AddSystemMessage($"Your name is {name}. {instructions}");
            // Store the agent's prompt for later reference
            _agentPrompts[name] = instructions;
            return chatCompletionService;
        }

        public ChatHistory CreateGroupChat(IEnumerable<IChatCompletionService> agents, string initialPrompt)
        {
            var chatHistory = new ChatHistory();
            chatHistory.AddSystemMessage("You are participating in a group chat. Work together to assist the user.");
            chatHistory.AddUserMessage(initialPrompt);
            return chatHistory;
        }

        public string? GetAgentPrompt(string name)
        {
            return _agentPrompts.TryGetValue(name, out var prompt) ? prompt : null;
        }
    }

    public class ChatResult
    {
        public string Content { get; set; } = string.Empty;
        public string AgentName { get; set; } = string.Empty;
    }
}

