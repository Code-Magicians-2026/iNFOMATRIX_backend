namespace Infomatrix.Core.Api.Features.Child.Responses;

public record ChildResponse(
    Guid Id,
    string FirstName,
    string LastName,
    int Experience);
