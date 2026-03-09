using Infomatrix.Core.Api.Base;
using Infomatrix.Core.Api.Extensions;
using Infomatrix.Core.Api.Features.Users.Mappers;
using Infomatrix.Core.Api.Features.Users.Responses;
using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Features.User.GetById;
using Infomatrix.Core.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Infomatrix.Core.Api.Features.Users;

[Authorize]
[ApiController]
[Route("api/users")]
public class UserController : BaseController
{
    public UserController(ISender sender)
        : base(sender)
    {
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<UserResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);

        var result = await _sender
            .Query(query, cancellationToken);

        return result
            .Map(r => r.ToResponse())
            .ToActionResult();
    }
}
