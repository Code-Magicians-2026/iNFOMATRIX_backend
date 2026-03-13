using Infomatrix.Core.Api.Base;
using Infomatrix.Core.Api.Extensions;
using Infomatrix.Core.Api.Features.Auth.Requests;
using Infomatrix.Core.Api.Features.Child.Mappers;
using Infomatrix.Core.Api.Features.Child.Requests;
using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Features.Auth.RegisterChild;
using Infomatrix.Core.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Infomatrix.Core.Api.Features.Child;

[Authorize]
[Route("api/children")]
[ApiController]
public class ChildController : BaseController
{
    public ChildController(ISender sender)
        : base(sender)
    {
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterChildAsync(
        [FromBody] RegisterChildRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterChildCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            request.FamilyId);

        var result = await _sender.Send(command, cancellationToken);

        return result
            .Map(childDto => childDto.ToResponse())
            .ToActionResult();
    }
}
