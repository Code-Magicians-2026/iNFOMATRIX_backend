using Infomatrix.Core.Api.Base;
using Infomatrix.Core.Api.Extensions;
using Infomatrix.Core.Api.Features.AI.Requests;
using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.DTOs;
using Infomatrix.Core.Application.Features.AI.GenerateConclusion;
using Infomatrix.Core.Application.Features.AI.GenerateQuest;
using Infomatrix.Core.Application.Features.AI.GenerateQuestWithImage;
using Infomatrix.Core.Application.Features.AI.GetAiResponse;
using Infomatrix.Core.Application.Features.AI.GetVisionAiResponse;
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

    //[HttpPost]
    //public async Task<IActionResult> GetAIResponseAsync(
    //    [FromBody] AIRequest request,
    //    CancellationToken cancellationToken)
    //{
    //    var command = new GenerateQuestCommand(
    //        GetUserId(),
    //        request.Prompt);

    //    var result = await _sender
    //        .Send(command, cancellationToken);

    //    return result
    //        .ToActionResult();
    //}

    //[HttpPost("vision")]
    //public async Task<IActionResult> GetAIResponseAsync(
    //    [FromForm] AIRequest request,
    //    IFormFile file,
    //    CancellationToken cancellationToken)
    //{
    //    using var stream = file
    //        .OpenReadStream();

    //    ImageDto image = new ImageDto(
    //        stream,
    //        file.ContentType,
    //        file.Name);

    //    var command = new GenerateQuestWithImageCommand(
    //        GetUserId(),
    //        request.Prompt,
    //        image);

    //    var result = await _sender
    //        .Send(command, cancellationToken);

    //    return result
    //        .ToActionResult();
    //}

    [HttpPost("quest")]
    public async Task<IActionResult> GenerateQuestAsync(
        [FromForm] AIRequest request,
        CancellationToken cancellationToken)
    {
        var command = new GenerateQuestCommand(
            GetUserId(),
            request.Prompt);

        var result = await _sender
            .Send(command, cancellationToken);

        return result.ToActionResult();
    }

    [HttpPost("quest-vision")]
    public async Task<IActionResult> GenerateQuestAsync(
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

        var command = new GenerateQuestWithImageCommand(
            GetUserId(),
            request.Prompt,
            image);

        var result = await _sender
            .Send(command, cancellationToken);

        return result.ToActionResult();
    }

    [HttpPost("summary-vision")]
    public async Task<IActionResult> GenerateSummaryByPhotos(
        IFormFile image1,
        IFormFile image2,
        CancellationToken cancellationToken)
    {
        if (image1 == null || image2 == null)
        {
            return BadRequest("Both 'Before' and 'After' images are required.");
        }

        using var stream1 = image1.OpenReadStream();
        using var stream2 = image2.OpenReadStream();

        var beforeImage = new ImageDto(stream1, image1.ContentType, image1.FileName);
        var afterImage = new ImageDto(stream2, image2.ContentType, image2.FileName);

        var command = new CheckQuestByImagesCommand(
            GetUserId(),
            beforeImage,
            afterImage);

        var result = await _sender.Send(command, cancellationToken);

        return result.ToActionResult();
    }

    public sealed record QuestPlanResponse(
        string Title,
        string Summary,
        string ChildMessage,
        IReadOnlyCollection<QuestItemResponse> Quests,
        int TotalEstimatedMinutes);

    public sealed record QuestItemResponse(
        string Title,
        string Description,
        string Difficulty,
        int RewardXp,
        int EstimatedMinutes);
}