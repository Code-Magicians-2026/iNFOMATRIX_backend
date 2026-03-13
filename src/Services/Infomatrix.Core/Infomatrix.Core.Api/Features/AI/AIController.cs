using Infomatrix.Core.Api.Base;
using Infomatrix.Core.Api.Extensions;
using Infomatrix.Core.Api.Features.AI.Requests;
using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.DTOs;
using Infomatrix.Core.Application.Features.AI.GetAiResponse;
using Infomatrix.Core.Application.Features.AI.GetVisionAiResponse;
using Microsoft.AspNetCore.Mvc;

namespace Infomatrix.Core.Api.Features.AI;

// TODO: uncomment after testing
//[Authorize]
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
        [FromBody] AIRequest request,
        CancellationToken cancellationToken)
    {
        var command = new GetAiResponseCommand(
            GetUserId(),
            request.Prompt);

        var result = await _sender
            .Send(command, cancellationToken);

        return result
            .ToActionResult();
    }

    [HttpPost("vision")]
    public async Task<IActionResult> GetAIResponseAsync(
        [FromForm] AIRequest request,
        IFormFile file,
        CancellationToken cancellationToken)
    {
        using var stream = file
            .OpenReadStream();

        ImageDto image = new ImageDto(
            stream,
            file.ContentType,
            file.Name);

        var command = new GetVisionAiResponseCommand(
            request.Prompt,
            image);

        var result = await _sender
            .Send(command, cancellationToken);

        return result
            .ToActionResult();
    }
}