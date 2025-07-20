using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TicTacToe.Application.Abstractions.Clock;
using TicTacToe.Application.Abstractions.Data;
using TicTacToe.Application.Abstractions.Services;
using TicTacToe.Application.Options;
using TicTacToe.Domain.Abstractions;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Errors;

namespace TicTacToe.Application.Services;

public class GamesService(
    IApplicationDbContext context,
    IOptionsSnapshot<GameOptions> gameOptions,
    IDateTimeProvider dateTimeProvider)
    : IGamesService
{
    private const int MinimumGameSize = 3;
    private const int MinimumWinLength = 2;
    
    public async Task<Result<int>> CreateGameAsync(CancellationToken ct = default)
    {
        var size = gameOptions.Value.Size;
        var winLength = gameOptions.Value.WinLength;

        if (size < MinimumGameSize || winLength < MinimumWinLength)
        {
            return Result.Failure<int>(GameErrors.InvalidGameOptions);
        }
        
        var game = Game.Create(size, winLength, dateTimeProvider.UtcNow);
        
        await context.Games.AddAsync(game, ct);
        
        await context.SaveChangesAsync(ct);
        
        return Result.Success(game.Id);
    }

    public async Task<Result<Game>> GetGameByIdAsync(int gameId, CancellationToken ct = default)
    {
        if (gameId < 1)
        {
            return Result.Failure<Game>(GameErrors.InvalidGameId);
        }
        
        if (await context.Games
                .Include(x => x.Moves)
                .FirstOrDefaultAsync(x => x.Id == gameId, ct) is not { } game)
        {
            return Result.Failure<Game>(GameErrors.NotFound);
        }

        return Result.Success(game);
    }
}
