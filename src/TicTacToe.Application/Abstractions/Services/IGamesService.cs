using TicTacToe.Domain.Abstractions;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Abstractions.Services;

public interface IGamesService
{
    Task<Result<int>> CreateGameAsync(CancellationToken ct = default);
    
    Task<Result<Game>> GetGameByIdAsync(int gameId, CancellationToken ct = default);
}
