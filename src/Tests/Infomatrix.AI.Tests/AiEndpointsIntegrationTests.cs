using System.Net.Http.Json;
using Infomatrix.Contracts.AI;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Xunit;

namespace Infomatrix.AI.Tests;

public class AiEndpointsIntegrationTests
{
    [Fact]
    public async Task Chat_Should_Return_Response()
    {
        var chatService = new ScriptedChatCompletionService((_, _) =>
            new ChatMessageContent(AuthorRole.Assistant, "ok"));

        await using var factory = CreateFactory(chatService);
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/ai/chat", new AiRequest(Guid.NewGuid(), "hello"));

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<AiResponse>();
        Assert.NotNull(payload);
        Assert.Equal("ok", payload.Content);
    }

    [Fact]
    public async Task Vision_Should_Accept_DataUri_Base64()
    {
        var chatService = new ScriptedChatCompletionService((_, _) =>
            new ChatMessageContent(AuthorRole.Assistant, "vision-ok"));

        await using var factory = CreateFactory(chatService);
        using var client = factory.CreateClient();

        var imageBase64 = "data:image/png;base64," + Convert.ToBase64String(new byte[] { 1, 2, 3, 4 });

        var request = new AiVisionRequest(
            "describe",
            new AiImageDto(imageBase64, "image/png", "sample.png"));

        var response = await client.PostAsJsonAsync("/api/ai/vision", request);

        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadFromJsonAsync<AiResponse>();
        Assert.NotNull(payload);
        Assert.Equal("vision-ok", payload.Content);
    }

    [Fact]
    public async Task Chat_Should_Retry_On_Transient_Error_Up_To_3_Attempts()
    {
        var chatService = new ScriptedChatCompletionService((attempt, _) =>
        {
            if (attempt < 3)
            {
                throw new HttpRequestException("transient");
            }

            return new ChatMessageContent(AuthorRole.Assistant, "retried-ok");
        });

        await using var factory = CreateFactory(chatService);
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/ai/chat", new AiRequest(Guid.NewGuid(), "hello"));

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<AiResponse>();
        Assert.NotNull(payload);
        Assert.Equal("retried-ok", payload.Content);
        Assert.Equal(3, chatService.Calls);
    }

    private static WebApplicationFactory<Program> CreateFactory(IChatCompletionService chatService)
    {
        return new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((_, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["AI:DeploymentName"] = "test",
                        ["AI:Endpoint"] = "https://test.openai.azure.com/",
                        ["AI:ApiKey"] = "test-key"
                    });
                });

                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<IChatCompletionService>();
                    services.AddSingleton(chatService);
                });
            });
    }

    private sealed class ScriptedChatCompletionService : IChatCompletionService
    {
        private readonly Func<int, ChatHistory, ChatMessageContent> _handler;

        public ScriptedChatCompletionService(Func<int, ChatHistory, ChatMessageContent> handler)
        {
            _handler = handler;
        }

        public int Calls { get; private set; }

        public IReadOnlyDictionary<string, object?> Attributes => new Dictionary<string, object?>();

        public Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(
            ChatHistory chatHistory,
            PromptExecutionSettings? executionSettings = null,
            Kernel? kernel = null,
            CancellationToken cancellationToken = default)
        {
            Calls++;
            var result = _handler(Calls, chatHistory);
            return Task.FromResult<IReadOnlyList<ChatMessageContent>>(new[] { result });
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
