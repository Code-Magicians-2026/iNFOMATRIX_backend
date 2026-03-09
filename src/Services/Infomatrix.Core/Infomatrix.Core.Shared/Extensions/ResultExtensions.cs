namespace Infomatrix.Core.Shared.Extensions;

public static class ResultExtensions
{
    public static Result<TResult> Map<TSource, TResult>(
        this Result<TSource> result,
        Func<TSource, TResult> mapper)
    {
        return result.IsSuccess
            ? Result.Success(mapper(result.Value))
            : Result.Failure<TResult>(result.Error);
    }

    public static Result<TOut> MapFailure<TOut>(this Result result)
    {
        return result.IsFailure
            ? Result.Failure<TOut>(result.Error)
            : throw new InvalidOperationException("Cannot map success to failure.");
    }
}
