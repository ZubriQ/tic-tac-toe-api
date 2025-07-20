namespace TicTacToe.Application.Abstractions.Clock;

public interface IDateTimeProvider
{
    public DateTimeOffset UtcNow { get; }
}
