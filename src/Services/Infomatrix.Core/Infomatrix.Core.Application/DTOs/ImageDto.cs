namespace Infomatrix.Core.Application.DTOs;

public sealed record ImageDto(
    Stream Data,
    string ContentType,
    string FileName);
