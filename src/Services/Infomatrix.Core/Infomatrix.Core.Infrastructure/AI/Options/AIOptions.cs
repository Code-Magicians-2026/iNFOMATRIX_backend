namespace Infomatrix.Core.Infrastructure.AI.Options;

public sealed class AIOptions
{
    public const string SectionName = "AI";
    
    public string DeploymentName { get; init; }

    public string Endpoint { get; init; }

    public string ApiKey { get; init; }
}
