using Infomatrix.Core.Domain.Common;

namespace Infomatrix.Core.Domain.Features.User;

public class UserException : DomainException
{
    public UserException(string message)
        : base(message)
    {
    }

    public static void ThrowIfFullNameInvalid(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new UserException("Full name cannot be null or empty.");

        if (fullName.Length > 100)
            throw new UserException("Full name cannot exceed 100 characters.");
    }

    public static void ThrowIfEmailInvalid(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new UserException("Email cannot be null or empty.");

        if (!email.Contains('@') || email.StartsWith('@') || email.EndsWith('@'))
            throw new UserException("The provided email address is not in a valid format.");

        if (email.Length > 255)
            throw new UserException("Email cannot exceed 255 characters.");
    }
}
