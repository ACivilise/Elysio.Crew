using Elysio.Services.Interfaces;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Elysio.Services
{
    public class OllamaService(IChatCompletionService chatCompletionService) : IOllamaService
    {
        async Task IOllamaService.Chat()
        {
            var chatHistory = new ChatHistory("You are a helpful assistant that knows about AI.");
            chatHistory.AddUserMessage("Hi, I'm looking for book suggestions");
            var reply = await chatCompletionService.GetChatMessageContentAsync(chatHistory);
        }
    }
}