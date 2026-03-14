using Infomatrix.Contracts.AI;
using Refit;

namespace Infomatrix.Core.Infrastructure.AI.Services;

public interface IAiMicroserviceApi
{
    [Post("/api/ai/chat")]
    Task<AiResponse> GetChatResponseAsync([Body] AiRequest request, CancellationToken cancellationToken = default);

    [Post("/api/ai/vision")]
    Task<AiResponse> GetVisionResponseAsync([Body] AiVisionRequest request, CancellationToken cancellationToken = default);

    [Post("/api/ai/quest")]
    Task<AiResponse> GenerateQuestAsync([Body] AiRequest request, CancellationToken cancellationToken = default);

    [Post("/api/ai/quest-vision")]
    Task<AiResponse> GenerateQuestWithVisionAsync([Body] AiVisionRequest request, CancellationToken cancellationToken = default);

    [Post("/api/ai/summary-vision")]
    Task<AiResponse> GenerateSummaryByVisionAsync([Body] AiVisionCompareRequest request, CancellationToken cancellationToken = default);
}
