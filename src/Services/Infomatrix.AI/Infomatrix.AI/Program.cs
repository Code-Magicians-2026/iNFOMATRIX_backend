using Infomatrix.AI.Options;
using Infomatrix.Contracts.AI;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddOptions<AIOptions>()
    .Bind(builder.Configuration.GetSection(AIOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<IOptions<AIOptions>>().Value;
    var kernelBuilder = Kernel.CreateBuilder();

    kernelBuilder.AddAzureOpenAIChatCompletion(
        options.DeploymentName,
        options.Endpoint,
        options.ApiKey);

    return kernelBuilder.Build();
});

builder.Services.AddSingleton(sp =>
{
    var kernel = sp.GetRequiredService<Kernel>();
    return kernel.GetRequiredService<IChatCompletionService>();
});

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "AI service is working");

var ai = app.MapGroup("/api/ai");

ai.MapPost("/chat", async (
    AiRequest request,
    IChatCompletionService chatService,
    CancellationToken cancellationToken) =>
{
    var chat = new ChatHistory();

    if (!string.IsNullOrWhiteSpace(request.SystemPrompt))
    {
        chat.AddSystemMessage(request.SystemPrompt);
    }

    chat.AddUserMessage(request.Prompt);

    var response = await ExecuteWithRetryAsync(
        ct => chatService.GetChatMessageContentAsync(chat, cancellationToken: ct),
        cancellationToken);

    return Results.Ok(new AiResponse(response.Content ?? string.Empty));
});

ai.MapPost("/vision", async (
    AiVisionRequest request,
    IChatCompletionService chatService,
    CancellationToken cancellationToken) =>
{
    var response = await GetVisionResponseAsync(request.SystemPrompt, request.Prompt, request.Image, chatService, cancellationToken);
    return Results.Ok(new AiResponse(response));
});

ai.MapPost("/quest", async (
    AiRequest request,
    IChatCompletionService chatService,
    CancellationToken cancellationToken) =>
{
    var chat = new ChatHistory();

    if (!string.IsNullOrWhiteSpace(request.SystemPrompt))
    {
        chat.AddSystemMessage(request.SystemPrompt);
    }

    chat.AddUserMessage(request.Prompt);

    var response = await ExecuteWithRetryAsync(
        ct => chatService.GetChatMessageContentAsync(chat, cancellationToken: ct),
        cancellationToken);

    return Results.Ok(new AiResponse(response.Content ?? string.Empty));
});

ai.MapPost("/quest-vision", async (
    AiVisionRequest request,
    IChatCompletionService chatService,
    CancellationToken cancellationToken) =>
{
    var response = await GetVisionResponseAsync(request.SystemPrompt, request.Prompt, request.Image, chatService, cancellationToken);
    return Results.Ok(new AiResponse(response));
});

ai.MapPost("/summary-vision", async (
    AiVisionCompareRequest request,
    IChatCompletionService chatService,
    CancellationToken cancellationToken) =>
{
    var chat = new ChatHistory();

    if (!string.IsNullOrWhiteSpace(request.SystemPrompt))
    {
        chat.AddSystemMessage(request.SystemPrompt);
    }

    var message = new ChatMessageContent(AuthorRole.User, request.Prompt);
    message.Items.Add(ToImageContent(request.BeforeImage));
    message.Items.Add(ToImageContent(request.AfterImage));

    chat.Add(message);

    var response = await ExecuteWithRetryAsync(
        ct => chatService.GetChatMessageContentAsync(chat, cancellationToken: ct),
        cancellationToken);

    return Results.Ok(new AiResponse(response.Content ?? string.Empty));
});

app.Run();

static async Task<string> GetVisionResponseAsync(
    string? systemPrompt,
    string prompt,
    AiImageDto image,
    IChatCompletionService chatService,
    CancellationToken cancellationToken)
{
    var chat = new ChatHistory();

    if (!string.IsNullOrWhiteSpace(systemPrompt))
    {
        chat.AddSystemMessage(systemPrompt);
    }

    var message = new ChatMessageContent(AuthorRole.User, prompt);
    message.Items.Add(ToImageContent(image));

    chat.Add(message);

    var response = await ExecuteWithRetryAsync(
        ct => chatService.GetChatMessageContentAsync(chat, cancellationToken: ct),
        cancellationToken);

    return response.Content ?? string.Empty;
}

static async Task<T> ExecuteWithRetryAsync<T>(
    Func<CancellationToken, Task<T>> operation,
    CancellationToken cancellationToken)
{
    const int maxAttempts = 3;

    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        try
        {
            return await operation(cancellationToken);
        }
        catch (Exception ex) when (IsTransient(ex) && attempt < maxAttempts)
        {
            var delay = TimeSpan.FromMilliseconds(250 * attempt);
            await Task.Delay(delay, cancellationToken);
        }
    }

    return await operation(cancellationToken);
}

static bool IsTransient(Exception ex) =>
    ex is HttpRequestException
    or TaskCanceledException
    or Microsoft.SemanticKernel.HttpOperationException;

static ImageContent ToImageContent(AiImageDto image)
{
    var base64 = image.Base64Data;

    const string marker = ";base64,";
    var markerIndex = base64.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
    if (markerIndex >= 0)
    {
        base64 = base64[(markerIndex + marker.Length)..];
    }

    var bytes = Convert.FromBase64String(base64);
    var contentType = string.IsNullOrWhiteSpace(image.ContentType)
        ? "image/jpeg"
        : image.ContentType;

    return new ImageContent(new ReadOnlyMemory<byte>(bytes), contentType);
}

public partial class Program;
