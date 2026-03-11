using Infomatrix.Core.Shared;

namespace Infomatrix.Core.Application.Abstractions.Services;

public interface IAIService
{
    Task<Result<string>> GetResponseAsync(
        Guid userId,
        string userPrompt,
        Guid? chatId,
        CancellationToken cancellationToken = default);

    Task<Result<T>> GetResponseAsync<T>(
        Guid userId,
        string userPrompt,
        CancellationToken cancellationToken = default);
}
