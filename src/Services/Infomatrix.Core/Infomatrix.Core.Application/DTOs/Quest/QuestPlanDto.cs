namespace Infomatrix.Core.Application.DTOs.Quest;

public sealed record QuestPlanDto(
    string Title,
    string Summary,
    string ChildMessage,
    IReadOnlyCollection<QuestItemDto> Quests,
    int TotalEstimatedMinutes);

public sealed record QuestItemDto(
    string Title,
    string Description,
    string Difficulty,
    int RewardXp,
    int EstimatedMinutes);
