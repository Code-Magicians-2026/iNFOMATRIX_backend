using Infomatrix.Core.Domain.Common;

namespace Infomatrix.Core.Domain.Features.User;

public class UserEntity : BaseEntity
{
    public string FullName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    private UserEntity()
    {
    }

    private UserEntity(
        Guid id,
        string fullName,
        string email)
        : base(id)
    {
        FullName = fullName;
        Email = email;
    }

    public static UserEntity Create(
        Guid id,
        string fullName,
        string email)
    {
        UserException
            .ThrowIfFullNameInvalid(fullName);

        UserException
            .ThrowIfEmailInvalid(email);

        return new UserEntity(id, fullName, email);
    }

    public void Update(string fullName)
    {
        UserException
            .ThrowIfFullNameInvalid(fullName);

        FullName = fullName;
    }
}
