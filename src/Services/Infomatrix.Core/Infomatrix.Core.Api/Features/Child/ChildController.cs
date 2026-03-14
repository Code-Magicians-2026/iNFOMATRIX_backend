using Infomatrix.Core.Api.Base;
using Infomatrix.Core.Api.Extensions;
using Infomatrix.Core.Api.Features.Auth.Requests;
using Infomatrix.Core.Api.Features.Child.Mappers;
using Infomatrix.Core.Api.Features.Child.Requests;
using Infomatrix.Core.Api.Features.Child.Responses;
using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Features.Auth.RegisterChild;
using Infomatrix.Core.Infrastructure.Persistence;
using Infomatrix.Core.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infomatrix.Core.Api.Features.Child;

[Authorize]
[Route("api/children")]
[ApiController]
public class ChildController : BaseController
{
    private readonly AppDbContext _dbContext;

    public ChildController(ISender sender, AppDbContext dbContext)
        : base(sender)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterChildAsync(
        [FromBody] RegisterChildRequest request,
        CancellationToken cancellationToken)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);

        var command = new RegisterChildCommand(
            request.FirstName,
            request.LastName,
            email,
            request.Password,
            request.FamilyId);

        var result = await _sender.Send(command, cancellationToken);

        return result
            .Map(childDto => childDto.ToResponse())
            .ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetChildren(CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var parent = await _dbContext.Parents
            .Include(p => p.Family)
            .ThenInclude(f => f.Children)
            .FirstOrDefaultAsync(p => p.Id == userId, cancellationToken);

        if (parent == null)
        {
            return Forbid("User is not a parent.");
        }

        if (parent.Family == null)
        {
            return NotFound("Family not found for the current user.");
        }

        var children = parent.Family.Children.Select(c => new ChildResponse(
            c.Id,
            c.FirstName,
            c.LastName,
            c.Email,
            c.Experience
        )).ToList();

        return Ok(children);
    }
}
