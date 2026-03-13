namespace Infomatrix.Core.Api.Features.Child.Requests;

public sealed record RegisterChildRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    Guid FamilyId);
