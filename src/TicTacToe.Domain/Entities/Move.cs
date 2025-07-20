using TicTacToe.Domain.Enums;
using TicTacToe.Domain.ValueObjects;

namespace TicTacToe.Domain.Entities;

public sealed class Move
{
    private Move(
        int gameId, 
        int moveNumber, 
        GameSymbol player, 
        GameSymbol playerMove, 
        Position position,
        bool isFlipped,
        DateTimeOffset createdAt)
    {
        GameId = gameId;
        MoveNumber = moveNumber;
        Player = player;
        PlayerMove = playerMove;
        Position = position;
        IsFlipped = isFlipped;
        CreatedAt = createdAt;
    }

    /// <summary>
    ///     Required by EF Core
    /// </summary>
    private Move()
    {
        
    }
    
    public int Id { get; private set; }
    
    public int GameId { get; private set; }
    
    public int MoveNumber { get; private set; }
    
    public GameSymbol Player { get; private set; }
    
    public GameSymbol PlayerMove { get; private set; }
    
    public Position Position { get; private set; }
    
    public bool IsFlipped { get; private set; }
    
    public DateTimeOffset CreatedAt { get; private set; }
    
    public int Version { get; private set; }

    public static Move Create(
        int gameId, 
        int moveNumber, 
        GameSymbol player, 
        GameSymbol playerMove, 
        Position position,
        bool isFlipped,
        DateTimeOffset createdAt)
    {
        return new Move(gameId, moveNumber, player, playerMove, position, isFlipped, createdAt);
    }
}
