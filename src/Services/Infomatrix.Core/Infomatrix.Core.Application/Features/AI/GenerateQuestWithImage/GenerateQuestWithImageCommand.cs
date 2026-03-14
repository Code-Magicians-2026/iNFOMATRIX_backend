using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Application.DTOs;
using Infomatrix.Core.Application.DTOs.Quest;
using Infomatrix.Core.Shared;
using Infomatrix.Core.Shared.Extensions;
using System.Text.Json;

namespace Infomatrix.Core.Application.Features.AI.GenerateQuestWithImage;

public sealed record GenerateQuestWithImageCommand(
    Guid UserId,
    string UserPrompt,
    ImageDto Image)
    : ICommand<QuestPlanDto>;

internal sealed class GenerateQuestWithImageCommandHandler
    : ICommandHandler<GenerateQuestWithImageCommand, QuestPlanDto>
{
    private readonly IVisionService _visionService;

    public GenerateQuestWithImageCommandHandler(IVisionService visionService)
    {
        _visionService = visionService;
    }

    public async Task<Result<QuestPlanDto>> Handle(
        GenerateQuestWithImageCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _visionService.GetResponseAsync(
            GetSystemMessage(),
            command.UserPrompt,
            command.Image,
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
            **Role**: You are a world-class Game Designer and Narrative Architect specialized in gamifying real-world productivity for children. Your objective is to transform mundane parental instructions and **provided photos** into immersive, high-stakes "Epic Quests."

            **Core Directive**: 
            Analyze the provided parental instruction (`prompt`) and the **uploaded image**. Use the image to identify specific objects, clutter, or areas that need attention. Decompose the visual and textual data into a series of logical, actionable, and gamified steps (`quests`) based on the requested `intensity`.

            **1. Visual Integration**:
            - Observe specific details in the photo (e.g., colors of toys, specific furniture, types of clutter).
            - Incorporate these visual details into the quest descriptions to make them feel "real" and personalized (e.g., "Rescue the blue dinosaur trapped under the chair").

            **2. Narrative Frameworks**:
            Rotate between themes for each response: Space Exploration, Medieval Fantasy, Cyberpunk/Tech, or Wilderness Expedition. Use immersive terminology (e.g., "Debris" instead of "Trash", "Energy Cells" instead of "Batteries").

            **3. Content Guidelines**:
            - **Output Language**: All user-facing strings (`title`, `summary`, `childMessage`, `quests.title`, `quests.description`) MUST be in **English**.
            - **Tone**: Heroic and encouraging for the child; professional and organized for the parent.
            - **Precision**: Every quest `description` must begin with a clear, real-world physical action based on visual evidence from the photo.

            **4. Intensity & Complexity Logic**:
            - **Low**: 1-2 quests. Focus on the most obvious visual mess.
            - **Medium**: 3-4 quests. Logical breakdown including sorting and organizing.
            - **High**: 5+ quests. Granular decomposition including deep cleaning or multi-stage organization.

            **5. CRITICAL JSON RULES (System.Text.Json Compatible)**:
            - Output MUST be a **single JSON object** as the root.
            - Do NOT use markdown code fences (e.g., no ```json).
            - Do NOT include any text, filler, or explanations before or after the JSON.
            - Properly escape any internal quotes using backslashes (`\"`).
            - No trailing commas.

            **6. Output Example (based on a photo of a messy bedroom)**:
            {
              "title": "Operation: Bedrock Base Restoration",
              "summary": "Full organization of the sleeping quarters and toy retrieval based on visual scan.",
              "childMessage": "Commander! Scanners show a massive debris field near the sleeping pods. Clear the sector to ensure a safe landing tonight!",
              "quests": [
                {
                  "title": "Artifact Extraction",
                  "description": "Pick up the scattered building blocks [visible near the rug] and return them to the containment unit.",
                  "difficulty": "medium",
                  "rewardXp": 100,
                  "estimatedMinutes": 10
                },
                {
                  "title": "Soft-Unit Relocation",
                  "description": "Gather the plush creatures from the floor and arrange them on the main docking station (the bed).",
                  "difficulty": "easy",
                  "rewardXp": 40,
                  "estimatedMinutes": 5
                }
              ],
              "totalEstimatedMinutes": 15
            }
            """;
    }
}