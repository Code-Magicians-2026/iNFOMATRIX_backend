using Infomatrix.Core.Application.DTOs;
using Infomatrix.Core.Shared;

namespace Infomatrix.Core.Application.Abstractions.Services;

public interface IVisionService
{
    Task<Result<string>> GetResponseAsync(
        string prompt,
        ImageDto image,
        CancellationToken cancellationToken);
}
