namespace Infomatrix.Contracts.AI;

public sealed record AiVisionCompareRequest(
    string Prompt,
    AiImageDto BeforeImage,
    AiImageDto AfterImage,
    string? SystemPrompt = null,
    Guid? UserId = null,
    Guid? ChatId = null);
