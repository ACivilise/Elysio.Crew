using Microsoft.SemanticKernel.ChatCompletion;

namespace Elysio.Services.Interfaces
{
    public interface IOllamaService
    {
        Task<string> Chat(string message, ChatHistory? history = null);
        IChatCompletionService CreateAgent(string name, string instructions);
        ChatHistory CreateGroupChat(IEnumerable<IChatCompletionService> agents, string initialPrompt);
        string? GetAgentPrompt(string name);
    }
}