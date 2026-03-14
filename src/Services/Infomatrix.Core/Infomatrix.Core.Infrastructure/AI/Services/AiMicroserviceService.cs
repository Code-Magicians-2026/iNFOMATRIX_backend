using Infomatrix.Contracts.AI;
using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Application.DTOs;
using Infomatrix.Core.Shared;
using Refit;

namespace Infomatrix.Core.Infrastructure.AI.Services;

internal sealed class AiMicroserviceService : IAIService, IVisionService
{
    private readonly IAiMicroserviceApi _client;

    public AiMicroserviceService(IAiMicroserviceApi client)
    {
        _client = client;
    }

    public async Task<Result<string>> GetResponseAsync(
        Guid userId,
        string systemMessage,
        string userPrompt,
        Guid? chatId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new AiRequest(userId, userPrompt, systemMessage, chatId);

            var response = string.IsNullOrWhiteSpace(systemMessage)
                ? await _client.GetChatResponseAsync(request, cancellationToken)
                : await _client.GenerateQuestAsync(request, cancellationToken);

            return response.Content;
        }
        catch (ApiException ex)
        {
            return Result.Failure<string>(Error.Unavailable("AI.Service", ex.Message));
        }
    }

    public Task<Result<T>> GetResponseAsync<T>(
        Guid userId,
        string userPrompt,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<string>> GetResponseAsync(
        string systemPrompt,
        string prompt,
        ImageDto image,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new AiVisionRequest(
                prompt,
                await ToAiImageDtoAsync(image),
                systemPrompt);

            var response = string.IsNullOrWhiteSpace(systemPrompt)
                ? await _client.GetVisionResponseAsync(request, cancellationToken)
                : await _client.GenerateQuestWithVisionAsync(request, cancellationToken);

            return response.Content;
        }
        catch (ApiException ex)
        {
            return Result.Failure<string>(Error.Unavailable("AI.Service", ex.Message));
        }
    }

    public async Task<Result<string>> GetResponseAsync(
        string systemPrompt,
        string prompt,
        ImageDto image1,
        ImageDto image2,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _client.GenerateSummaryByVisionAsync(
                new AiVisionCompareRequest(
                    prompt,
                    await ToAiImageDtoAsync(image1),
                    await ToAiImageDtoAsync(image2),
                    systemPrompt),
                cancellationToken);

            return response.Content;
        }
        catch (ApiException ex)
        {
            return Result.Failure<string>(Error.Unavailable("AI.Service", ex.Message));
        }
    }

    private static async Task<AiImageDto> ToAiImageDtoAsync(ImageDto image)
    {
        using var memoryStream = new MemoryStream();
        await image.Data.CopyToAsync(memoryStream);

        return new AiImageDto(
            Convert.ToBase64String(memoryStream.ToArray()),
            image.ContentType,
            image.FileName);
    }
}
