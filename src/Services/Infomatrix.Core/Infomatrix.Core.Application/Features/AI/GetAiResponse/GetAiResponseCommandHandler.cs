using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Shared;

namespace Infomatrix.Core.Application.Features.AI.GetAiResponse;

internal sealed class GetAiResponseCommandHandler
    : ICommandHandler<GetAiResponseCommand, string>
{
    private readonly IAIService _aiService;

    public GetAiResponseCommandHandler(
        IAIService aiService)
    {
        _aiService = aiService;
    }

    public async Task<Result<string>> Handle(
        GetAiResponseCommand request,
        CancellationToken cancellationToken)
    {
        return await _aiService.GetResponseAsync(
            request.UserId,
            request.Prompt,
            null,
            cancellationToken);
    }
}
