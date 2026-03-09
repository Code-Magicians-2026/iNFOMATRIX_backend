namespace Infomatrix.Core.Domain.Common;

public class DomainException : Exception
{
    public DomainException()
    {
    }

    public DomainException(string message)
        : base(message)
    {
    }

    public static void ThrowIf(bool condition, string message)
    {
        if (condition)
            throw new DomainException(message);
    }
}
