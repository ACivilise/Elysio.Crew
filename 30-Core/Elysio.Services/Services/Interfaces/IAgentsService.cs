using Microsoft.SemanticKernel.ChatCompletion;

namespace Elysio.Services.Interfaces
{
    public interface IAgentsService
    {
        IAsyncEnumerable<string> ChatWithAgents(string initialMessage, Guid conversationId);
    }
}