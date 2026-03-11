using Infomatrix.Core.Api.Base;
using Infomatrix.Core.Api.Extensions;
using Infomatrix.Core.Api.Features.AI.Requests;
using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Infomatrix.Core.Api.Features.AI;

[Authorize]
[Route("api/ai")]
[ApiController]
public class AIController : BaseController
{
    public AIController(ISender sender)
        : base(sender)
    {
    }

    [HttpPost]
    public async Task<IActionResult> GetAIResponseAsync(
        [FromServices] IAIService aiService,
        [FromBody] AIRequest request,
        CancellationToken cancellationToken)
    {
        var result = await aiService.GetResponseAsync(
            GetUserId(),
            request.Prompt,
            null,
            cancellationToken);

        return result
            .ToActionResult();
    }
}