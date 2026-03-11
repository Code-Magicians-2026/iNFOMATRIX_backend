using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Shared;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

namespace Infomatrix.Core.Infrastructure.AI;

internal class AIService : IAIService
{
    private readonly IChatCompletionService _chatCompletion;
    private readonly Kernel _kernel;

    public AIService(
        IChatCompletionService chatCompletion,
        Kernel kernel)
    {
        _chatCompletion = chatCompletion;
        _kernel = kernel;
    }

    public async Task<Result<string>> GetResponseAsync(
        Guid userId,
        string userPrompt,
        Guid? chatId,
        CancellationToken cancellationToken = default)
    {
        var executionSettings = new AzureOpenAIPromptExecutionSettings()
        {
        };

        var response = await _chatCompletion.GetChatMessageContentAsync(
            userPrompt,
            executionSettings,
            _kernel,
            cancellationToken);

        return response.Content;
    }

    public Task<Result<T>> GetResponseAsync<T>(
        Guid userId,
        string userPrompt,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
