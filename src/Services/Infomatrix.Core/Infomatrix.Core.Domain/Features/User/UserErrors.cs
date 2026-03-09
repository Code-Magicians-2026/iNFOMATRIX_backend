using Infomatrix.Core.Shared;

namespace Infomatrix.Core.Domain.Features.User;

public static class UserErrors
{
    public static Error NotFound(Guid id) => Error.NotFound(
        "UserInfo.NotFound",
        $"User info with id = '{id}' was not found");

    public static Error EmailNotFound(string email) => Error.NotFound(
        "UserInfo.EmailNotFound",
        $"User info with email = '{email}' was not found");

    public static Error AlreadyExists(string email) => Error.Conflict(
        "UserInfo.AlreadyExists",
        $"User info with email = '{email}' already exists");

    public static Error TimezoneNull(Guid id) => Error.Validation(
        "UserInfo.TimezoneNull",
        $"Timezone for user info with id = '{id}' is null");
}
