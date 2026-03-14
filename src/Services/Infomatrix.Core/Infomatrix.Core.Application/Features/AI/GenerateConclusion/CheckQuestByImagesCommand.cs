using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Application.DTOs;
using Infomatrix.Core.Application.DTOs.Quest;
using Infomatrix.Core.Shared;
using Infomatrix.Core.Shared.Extensions;
using System.Text.Json;

namespace Infomatrix.Core.Application.Features.AI.GenerateConclusion;

public sealed record QuestConclusionDto(
    string Message);

public sealed record CheckQuestByImagesCommand(
    Guid UserId,
    ImageDto PreviousImage,
    ImageDto CurrentImage)
    : ICommand<QuestConclusionDto>;

internal sealed class CheckQuestByImagesCommandHandler
    : ICommandHandler<CheckQuestByImagesCommand, QuestConclusionDto>
{
    private readonly IVisionService _visionService;

    public CheckQuestByImagesCommandHandler(IVisionService visionService)
    {
        _visionService = visionService;
    }

    public async Task<Result<QuestConclusionDto>> Handle(
        CheckQuestByImagesCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _visionService.GetResponseAsync(
            GetSystemMessage(),
            GetUserPrompt(),
            command.PreviousImage,
            command.CurrentImage,
            cancellationToken);

        if (result.IsFailure)
        {
            return result.MapFailure<QuestConclusionDto>();
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };

        var conclusion = JsonSerializer
            .Deserialize<QuestConclusionDto>(result.Value, options);

        return conclusion;
    }

    private static string GetSystemMessage()
    {
        return
        $$"""
        **Role**: You are the "Oracle of Completion," a legendary Quest Inspector who verifies the heroic deeds of young warriors. Your task is to compare two visual scans (images) to determine if a quest has been fulfilled.

        **Core Directive**: 
        Analyze two images: 
        1. **Initial State (Before)**: The mess/task as it was.
        2. **Current State (After)**: The result of the child's work.
        
        Compare these against the original instructions (`prompt`). Determine if the task is fully completed, partially done, or failed. Use the image delta to identify if objects from the first photo are cleared or organized in the second.

        **Output Rules**:
        - Output MUST be a **single JSON object** with exactly one property: "message".
        - The "message" string must be in **English**.
        - Maintain an epic, heroic, and encouraging tone.
        - Do NOT use markdown code fences.
        - Do NOT include any text outside the JSON.

        **JSON Schema**:
        {
          "message": "Your heroic narrative feedback and assessment of the task completion here."
        }
        """;
    }

    private static string GetUserPrompt()
    {
        return "Task: Clean the play area. Compare the 'Before' image (photo 1) with the 'After' image (photo 2). Did I succeed?";
    }
}