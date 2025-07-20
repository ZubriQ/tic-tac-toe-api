using System.Reflection;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Enums;
using TicTacToe.Domain.ValueObjects;

namespace TicTacToe.Application.Tests;

/// <summary>
///     Для DDD-like сущностей с приватными полями
/// </summary>
internal static class TestHelpers
{
    public static int GetPrivateMoveId(Move move)
    {
        var prop = typeof(Move).GetProperty(nameof(Move.Id),
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        
        return (int)prop?.GetValue(move)!;
    }

    public static void SetPrivateMoveId(Move move, int id)
    {
        var prop = typeof(Move).GetProperty(nameof(Move.Id),
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        
        prop?.SetValue(move, id);
    }
    
    public static void SetPrivateGameId(Game game, int id)
    {
        var prop = typeof(Game).GetProperty(nameof(Game.Id),
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        prop?.SetValue(game, id);
    }
    
    public static Move CreateMove(
        int gameId,
        int moveNumber,
        GameSymbol player,
        GameSymbol playerMove,
        int row,
        int col,
        bool isFlipped,
        DateTimeOffset createdAt)
    {
        var move = Move.Create(
            gameId,
            moveNumber,
            player,
            playerMove,
            new Position(row, col),
            isFlipped,
            createdAt);

        var idProp = typeof(Move).GetProperty(nameof(Move.Id),
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        
        idProp?.SetValue(move, moveNumber);

        return move;
    }
}
