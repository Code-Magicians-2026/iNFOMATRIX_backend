using Infomatrix.Core.Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Infomatrix.Core.Api.Base;

public abstract class BaseController : ControllerBase
{
    protected readonly ISender _sender;

    protected BaseController(ISender sender)
        : base()
    {
        _sender = sender;
    }

    protected Guid GetUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var id))
        {
            throw new InvalidOperationException("User Id claim is missing or invalid.");
        }

        return id;
    }
}
