using Infomatrix.Core.Application.Abstractions.Messaging;

namespace Infomatrix.Core.Application.Features.AI.GetAiResponse;

public sealed record GetAiResponseCommand(
    Guid UserId, 
    string Prompt)
    : ICommand<string>;
