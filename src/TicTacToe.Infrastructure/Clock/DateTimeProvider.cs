using TicTacToe.Application.Abstractions.Clock;

namespace TicTacToe.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
