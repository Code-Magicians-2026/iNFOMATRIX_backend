namespace Infomatrix.Contracts.AI;

public sealed record AiVisionRequest(
    string Prompt,
    AiImageDto Image,
    string? SystemPrompt = null,
    Guid? UserId = null,
    Guid? ChatId = null);
