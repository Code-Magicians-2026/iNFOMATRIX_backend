using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Application.DTOs.Quest;
using Infomatrix.Core.Shared;
using Infomatrix.Core.Shared.Extensions;
using System.Text.Json;

namespace Infomatrix.Core.Application.Features.AI.GenerateQuest;

public sealed record GenerateQuestCommand(
    Guid UserId,
    string UserPrompt)
    : ICommand<QuestPlanDto>;

internal sealed class GenerateQuestCommandHandler
    : ICommandHandler<GenerateQuestCommand, QuestPlanDto>
{
    private readonly IAIService _aiService;

    public GenerateQuestCommandHandler(IAIService aiService)
    {
        _aiService = aiService;
    }

    public async Task<Result<QuestPlanDto>> Handle(
        GenerateQuestCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _aiService.GetResponseAsync(
            command.UserId,
            GetSystemMessage(),
            command.UserPrompt,
            null,
            cancellationToken);

        if (result.IsFailure)
        {
            return result.MapFailure<QuestPlanDto>();
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };

        var quest = JsonSerializer
            .Deserialize<QuestPlanDto>(result.Value, options);

        return quest;
    }

    private static string GetSystemMessage()
    {
        return
            $$"""
            **Role**: You are a world-class Game Designer and Narrative Architect. Your goal is to transform parental instructions and provided photos into immersive "Epic Quests" for children.

            **Core Directive**: 
            The next user message will contain a **task description (prompt)** and potentially an **image**. You must:
            1. Scan the image for specific objects, colors, and layout.
            2. Combine the parent's text with visual evidence to create a personalized quest plan.
            3. Decompose the goal into logical steps based on the requested `intensity`.

            **1. Visual Intelligence**:
            - Reference specific items from the photo (e.g., "the red sneakers near the door" or "the building blocks on the rug").
            - Use these visual anchors to make the quest feel real and undeniable.

            **2. Narrative Themes**:
            Rotate between: Space Exploration, Medieval Fantasy, Cyberpunk/Tech, or Wilderness Expedition. Use thematic jargon (e.g., "Containment breach" for a mess, "Power crystals" for toys).

            **3. Content Guidelines**:
            - **Language**: All user-facing strings MUST be in **English**.
            - **Tone**: Heroic/Encouraging for the child; Professional/Structured for the parent.
            - **Actionable**: Every quest description must start with a concrete physical action.

            **4. Intensity Logic**:
            - **Low**: 1-2 quests. Core task only.
            - **Medium**: 3-4 quests. Phased approach.
            - **High**: 5+ quests. Granular steps including prep and verification.

            **5. JSON RULES (System.Text.Json)**:
            - Output a **single JSON object** only. 
            - Root: { "title", "summary", "childMessage", "quests": [], "totalEstimatedMinutes" }.
            - No markdown formatting (no ```json).
            - No text before or after the JSON.
            - Escape all internal quotes with backslashes (\").

            **6. Example Structure**:
            {
              "title": "Operation: Midnight Galaxy",
              "summary": "Visual scan completed. Organizing the sleeping pod area.",
              "childMessage": "Commander! Scanners detected debris in the landing zone [the rug]. Clear it to proceed!",
              "quests": [
                {
                  "title": "Scrap Metal Retrieval",
                  "description": "Pick up the scattered blocks [visible near the bed] and return them to the bin.",
                  "difficulty": "medium",
                  "rewardXp": 120,
                  "estimatedMinutes": 10
                }
              ],
              "totalEstimatedMinutes": 10
            }
            """;
    }
}