namespace Infomatrix.Core.Shared;

public enum ErrorType
{
    BadRequest = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    Unauthorized = 4,
    Forbidden = 5,
    Failure = 6,
    TooManyRequests = 7,
    Unavailable = 8,
}
