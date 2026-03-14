using Infomatrix.Core.Api.Base;
using Infomatrix.Core.Api.Features.Family.Requests;
using Infomatrix.Core.Api.Features.Family.Responses;
using Infomatrix.Core.Api.Features.Child.Responses;
using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Domain.Features.Family;
using Infomatrix.Core.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infomatrix.Core.Api.Features.Family;

[ApiController]
[Route("api/families")]
[Authorize]
public class FamilyController : BaseController
{
    private readonly AppDbContext _dbContext;

    public FamilyController(ISender sender, AppDbContext dbContext) : base(sender)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<IActionResult> CreateFamily([FromBody] CreateFamilyRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Family name cannot be empty.");
        }

        var family = FamilyEntity.Create(request.Name);
        var parent = await _dbContext.Parents.FindAsync(GetUserId());

        _dbContext.Families.Add(family);
        parent.JoinFamily(family.Id);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new FamilyResponse(family.Id, family.Name);

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetFamily(CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var parent = await _dbContext.Parents
            .Include(p => p.Family)
            .FirstOrDefaultAsync(p => p.Id == userId, cancellationToken);

        if (parent?.Family != null)
        {
            return Ok(new FamilyResponse(parent.Family.Id, parent.Family.Name));
        }

        var child = await _dbContext.Children
            .Include(c => c.Family)
            .FirstOrDefaultAsync(c => c.Id == userId, cancellationToken);

        if (child?.Family != null)
        {
            return Ok(new FamilyResponse(child.Family.Id, child.Family.Name));
        }

        return NotFound("Family not found for the current user.");
    }

    [HttpGet("children")]
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
            c.Experience
        )).ToList();

        return Ok(children);
    }
}
