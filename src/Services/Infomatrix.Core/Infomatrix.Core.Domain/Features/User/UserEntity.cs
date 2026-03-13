using Infomatrix.Core.Domain.Common;

namespace Infomatrix.Core.Domain.Features.User;

public class UserEntity : BaseEntity
{
    public string FirstName { get; protected set; } = string.Empty;

    public string LastName { get; protected set; } = string.Empty;

    public string Email { get; protected set; } = string.Empty;

    protected UserEntity()
    {
    }

    protected UserEntity(
        Guid id,
        string firstName,
        string lastName,
        string email)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}
