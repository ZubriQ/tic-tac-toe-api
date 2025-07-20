using TicTacToe.Application.Dtos;
using TicTacToe.Domain.Abstractions;

namespace TicTacToe.Application.Abstractions.Services;

public interface IMovesService
{
    Task<Result<int>> CreateByGameIdAsync(CreateMoveCommand command, CancellationToken ct = default);
}
