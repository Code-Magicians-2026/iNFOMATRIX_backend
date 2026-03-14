namespace Infomatrix.Contracts.AI;

public sealed record AiRequest(
    Guid? UserId,
    string Prompt,
    string? SystemPrompt = null,
    Guid? ChatId = null);
