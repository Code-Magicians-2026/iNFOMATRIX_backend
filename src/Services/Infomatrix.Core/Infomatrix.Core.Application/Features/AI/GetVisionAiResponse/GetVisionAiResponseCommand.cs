using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.DTOs;

namespace Infomatrix.Core.Application.Features.AI.GetVisionAiResponse;

public sealed record GetVisionAiResponseCommand(
    string Prompt, 
    ImageDto Image) : ICommand<string>;
