extern alias CoreInfra;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Moq;
using Xunit;

using Infomatrix.Core.Application.DTOs;
using VisionService = CoreInfra::Infomatrix.Core.Infrastructure.AI.VisionService;
using CacheService = CoreInfra::Infomatrix.Core.Infrastructure.Cache.CacheService;

namespace Infomatrix.AI.Tests;

public class CoreServicesUnitTests
{
    [Fact]
    public async Task CacheService_SetAsync_Should_Use_Provided_Expiry()
    {
        var cache = new Mock<IDistributedCache>();
        var sut = new CacheService(cache.Object);
        var expiry = TimeSpan.FromMinutes(5);

        await sut.SetAsync("key", new { Name = "value" }, expiry);

        cache.Verify(c => c.SetAsync(
                "key",
                It.IsAny<byte[]>(),
                It.Is<DistributedCacheEntryOptions>(o => o.AbsoluteExpirationRelativeToNow == expiry),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task VisionService_GetResponseAsync_SingleImage_Should_Send_One_Image()
    {
        var chatService = new CapturingChatCompletionService("vision-ok");
        var sut = new VisionService(chatService, NullLogger<VisionService>.Instance);

        await using var imageStream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
        var image = new ImageDto(imageStream, "image/png", "sample.png");

        var result = await sut.GetResponseAsync("system", "prompt", image, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("vision-ok", result.Value);
        Assert.NotNull(chatService.LastChatHistory);
        Assert.Equal(2, chatService.LastChatHistory!.Count);
        Assert.Equal(1, chatService.LastChatHistory[1].Items.OfType<ImageContent>().Count());
    }

    [Fact]
    public async Task VisionService_GetResponseAsync_TwoImages_Should_Send_Two_Images()
    {
        var chatService = new CapturingChatCompletionService("compare-ok");
        var sut = new VisionService(chatService, NullLogger<VisionService>.Instance);

        await using var before = new MemoryStream(new byte[] { 1, 2, 3 });
        await using var after = new MemoryStream(new byte[] { 4, 5, 6 });

        var image1 = new ImageDto(before, "image/png", "before.png");
        var image2 = new ImageDto(after, "image/png", "after.png");

        var result = await sut.GetResponseAsync("system", "prompt", image1, image2, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("compare-ok", result.Value);
        Assert.NotNull(chatService.LastChatHistory);
        Assert.Equal(2, chatService.LastChatHistory!.Count);
        Assert.Equal(2, chatService.LastChatHistory[1].Items.OfType<ImageContent>().Count());
    }

    private sealed class CapturingChatCompletionService : IChatCompletionService
    {
        private readonly string _response;

        public CapturingChatCompletionService(string response)
        {
            _response = response;
        }

        public ChatHistory? LastChatHistory { get; private set; }

        public IReadOnlyDictionary<string, object?> Attributes => new Dictionary<string, object?>();

        public Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(
            ChatHistory chatHistory,
            PromptExecutionSettings? executionSettings = null,
            Kernel? kernel = null,
            CancellationToken cancellationToken = default)
        {
            LastChatHistory = chatHistory;

            return Task.FromResult<IReadOnlyList<ChatMessageContent>>(
                new[] { new ChatMessageContent(AuthorRole.Assistant, _response) });
        }

        public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(
            ChatHistory chatHistory,
            PromptExecutionSettings? executionSettings = null,
            Kernel? kernel = null,
            [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            yield break;
        }
    }
}
