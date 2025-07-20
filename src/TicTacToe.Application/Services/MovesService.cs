using Microsoft.EntityFrameworkCore;
using TicTacToe.Application.Abstractions.Clock;
using TicTacToe.Application.Abstractions.Data;
using TicTacToe.Application.Abstractions.Random;
using TicTacToe.Application.Abstractions.Services;
using TicTacToe.Application.Abstractions.TicTacToe;
using TicTacToe.Application.Dtos;
using TicTacToe.Domain.Abstractions;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Enums;
using TicTacToe.Domain.Errors;
using TicTacToe.Domain.Extensions;
using TicTacToe.Domain.ValueObjects;

namespace TicTacToe.Application.Services;

public class MovesService(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IRandomProvider randomProvider,
    ITicTacToeGameEngine gameEngine)
    : IMovesService
{
    private const int ProcEachTurn = 3;
    private const double FlipChance = 0.10;

    public async Task<Result<int>> CreateByGameIdAsync(CreateMoveCommand command, CancellationToken ct = default)
    {
        if (command.GameId < 1)
        {
            return Result.Failure<int>(MoveErrors.InvalidGameId);
        }

        var game = await context.Games
            .Include(x => x.Moves)
            .FirstOrDefaultAsync(x => x.Id == command.GameId, ct);

        if (game is null)
        {
            return Result.Failure<int>(MoveErrors.GameNotFound);
        }
        
        if (game.IsOutOfBounds(command.Row, command.Column))
        {
            return Result.Failure<int>(MoveErrors.RowOrColumnOutOfBounds);
        }

        if (game.IsFinished())
        {
            return Result.Failure<int>(MoveErrors.GameIsFinished);
        }

        if (IsAlreadyOccupied(command, game))
        {
            return Result.Failure<int>(MoveErrors.AlreadyOccupied);
        }

        var moveNumber = TryFlip(game, out var isFlipped, out var placedSymbol);

        var move = Move.Create(
            game.Id, 
            moveNumber, 
            game.NextSymbol,
            placedSymbol, 
            new Position(command.Row, command.Column),
            isFlipped,
            dateTimeProvider.UtcNow);
        
        game.Moves.Add(move);
        
        game.AdvanceTurn();
        
        gameEngine.DetermineWinnerAndUpdateGameStatus(game, move);
        
        await context.SaveChangesAsync(ct);
        
        return Result.Success(move.Id);
    }

    private static bool IsAlreadyOccupied(CreateMoveCommand command, Game game)
    {
        return game.Moves.Any(m => 
            m.Position.Row == command.Row &&
            m.Position.Column == command.Column);
    }
    
    private int TryFlip(Game game, out bool isFlipped, out GameSymbol placedSymbol)
    {
        var currentSymbol = game.NextSymbol;
        
        var moveNumber = game.Moves.Count + 1;
        var shouldFlip = moveNumber % ProcEachTurn == 0;
        isFlipped = shouldFlip && randomProvider.NextDouble() < FlipChance;
        
        placedSymbol = isFlipped ? currentSymbol.Flip() : currentSymbol;
        
        return moveNumber;
    }
}
