using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Application.DTOs;
using Infomatrix.Core.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Infomatrix.Core.Infrastructure.AI;

public class VisionService : IVisionService
{
    private readonly IChatCompletionService _chatService;
    private readonly ILogger<VisionService> _logger;

    public VisionService(
        IChatCompletionService chatService,
        ILogger<VisionService> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }

    public async Task<Result<string>> GetResponseAsync(
        string systemPrompt,
        string prompt,
        ImageDto image,
        CancellationToken cancellationToken)
    {
        var chat = new ChatHistory();

        chat.AddSystemMessage(systemPrompt);

        var message = new ChatMessageContent(
            AuthorRole.User,
            prompt);

        using var memoryStream = new MemoryStream();

        await image.Data.CopyToAsync(memoryStream);
        byte[] imageBytes = memoryStream.ToArray();

        message.Items.Add(
            new ImageContent(
                new ReadOnlyMemory<byte>(imageBytes),
                image.ContentType));

        chat.Add(message);

        var response = await _chatService
            .GetChatMessageContentAsync(chat);

        return response.Content;
    }

    public async Task<Result<string>> GetResponseAsync(
        string systemPrompt,
        string prompt,
        ImageDto image1,
        ImageDto image2,
        CancellationToken cancellationToken)
    {
        var chat = new ChatHistory();

        chat.AddSystemMessage(systemPrompt);

        var message = new ChatMessageContent(
            AuthorRole.User,
            prompt);

        using var memoryStream = new MemoryStream();

        var imageContent1 = await CreateImageContentAsync(
            image1.Data,
            image1.ContentType);

        var imageContent2 = await CreateImageContentAsync(
            image2.Data,
            image2.ContentType);

        message.Items.Add(imageContent1);
        message.Items.Add(imageContent2);

        chat.Add(message);

        var response = await _chatService
            .GetChatMessageContentAsync(chat);

        return response.Content;
    }

    private async Task<ImageContent> CreateImageContentAsync(
        Stream imageStream,
        string contentType)
    {
        using var ms = new MemoryStream();
        await imageStream.CopyToAsync(ms);

        return new ImageContent(
            new ReadOnlyMemory<byte>(ms.ToArray()),
            contentType);
    }
}
