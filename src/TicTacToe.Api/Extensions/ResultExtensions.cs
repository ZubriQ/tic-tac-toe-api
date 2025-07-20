using TicTacToe.Domain.Abstractions;

namespace TicTacToe.Api.Extensions;

public static class ResultExtensions
{
    public static TOut Match<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> onSuccess,
        Func<Result<TIn>, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
    }
}
