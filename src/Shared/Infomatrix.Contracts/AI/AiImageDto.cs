namespace Infomatrix.Contracts.AI;

public sealed record AiImageDto(
    string Base64Data,
    string ContentType,
    string FileName);
