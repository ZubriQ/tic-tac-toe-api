using System.Text.Json.Serialization;

namespace TicTacToe.Api.Filters;

[method: JsonConstructor]
internal sealed class IdempotentResponse(int statusCode, object? value)
{
    public int StatusCode { get; } = statusCode;
    public object? Value { get; } = value;
}
