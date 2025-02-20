using Elysio.Services.Interfaces;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Elysio.Services
{
    public class OllamaService(IChatCompletionService chatCompletionService) : IOllamaService
    {
        private readonly Dictionary<string, string> _agentPrompts = new();

        public async Task<string> Chat(string message, ChatHistory? history = null)
        {
            history ??= new ChatHistory("You are a helpful assistant.");
            history.AddUserMessage(message);

            try
            {
                var chatResult = await chatCompletionService.GetChatMessageContentsAsync(history);
                return chatResult?.FirstOrDefault()?.Content ?? string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception($"Chat completion error: {ex.Message}", ex);
            }
        }

        public IChatCompletionService CreateCompletionService(string name, string instructions)
        {
            // Store the agent's prompt for later reference
            _agentPrompts[name] = instructions;

            // Create a wrapper around the chat completion service that includes the agent's context
            return new AgentChatCompletionService(chatCompletionService, name, instructions);
        }

        public string? GetAgentPrompt(string name)
        {
            return _agentPrompts.TryGetValue(name, out var prompt) ? prompt : null;
        }
    }

    public class AgentChatCompletionService : IChatCompletionService
    {
        private readonly IChatCompletionService _innerService;
        private readonly string _agentName;
        private readonly string _instructions;

        public AgentChatCompletionService(IChatCompletionService innerService, string agentName, string instructions)
        {
            _innerService = innerService;
            _agentName = agentName;
            _instructions = instructions;
        }

        public IReadOnlyDictionary<string, object?> Attributes => _innerService.Attributes;

        public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(
            ChatHistory chatHistory,
            PromptExecutionSettings? executionSettings = null,
            Kernel? kernel = null,
            CancellationToken cancellationToken = default)
        {
            // Create a new chat history that includes the agent's context
            var agentHistory = new ChatHistory();
            
            // Add the agent's instructions as a system message
            agentHistory.AddSystemMessage($"You are {_agentName}. {_instructions}");
            
            // Add the existing chat history
            var enumeratedHistory = chatHistory.ToArray();
            foreach (var message in enumeratedHistory)
            {
                if (message.Content != null)
                {
                    agentHistory.AddMessage(message.Role, message.Content);
                }
            }

            // Get the response from the inner service
            var results = await _innerService.GetChatMessageContentsAsync(agentHistory, executionSettings, kernel, cancellationToken);

            // Add agent metadata to the responses
            foreach (var result in results)
            {
                result.Metadata = new Dictionary<string, object?>
                {
                    ["AgentName"] = _agentName
                };
            }

            return results;
        }

        public IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(
            ChatHistory chatHistory,
            PromptExecutionSettings? executionSettings = null,
            Kernel? kernel = null,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("Streaming is not supported by this service");
        }
    }
}

