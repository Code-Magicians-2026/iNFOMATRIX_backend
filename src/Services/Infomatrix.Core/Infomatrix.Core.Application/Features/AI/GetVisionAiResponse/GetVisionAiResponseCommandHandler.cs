using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Shared;

namespace Infomatrix.Core.Application.Features.AI.GetVisionAiResponse;

internal sealed class GetVisionAiResponseCommandHandler
    : ICommandHandler<GetVisionAiResponseCommand, string>
{
    private readonly IVisionService _visionService;

    public GetVisionAiResponseCommandHandler(
        IVisionService visionService)
    {
        _visionService = visionService;
    }

    public async Task<Result<string>> Handle(
        GetVisionAiResponseCommand request,
        CancellationToken cancellationToken)
    {
        return await _visionService.GetResponseAsync(
            request.Prompt,
            request.Image,
            cancellationToken);
    }
}
