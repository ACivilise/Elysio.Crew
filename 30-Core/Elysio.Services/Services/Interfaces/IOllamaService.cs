using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Elysio.Services.Interfaces
{
    public interface IOllamaService
    {
        Task<string> Chat(string message, ChatHistory? history = null);
        IChatCompletionService CreateCompletionService(string name, string instructions);
        string? GetAgentPrompt(string name);
    }
}