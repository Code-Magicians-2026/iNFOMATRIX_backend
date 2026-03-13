using Azure.Messaging;
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
        string prompt,
        ImageDto image,
        CancellationToken cancellationToken)
    {
        var chat = new ChatHistory();

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
}
