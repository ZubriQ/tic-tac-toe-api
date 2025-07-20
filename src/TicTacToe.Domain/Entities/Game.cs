using TicTacToe.Domain.Enums;
using TicTacToe.Domain.Extensions;

namespace TicTacToe.Domain.Entities;

public sealed class Game
{
    private Game(int size, int winLength, DateTimeOffset createdAt)
    {
        Size = size;
        WinLength = winLength;
        NextSymbol =  GameSymbol.X;
        Status = GameStatus.Created;
        CreatedAt = createdAt;
        Moves = [];
    }

    /// <summary>
    ///     Required by EF Core
    /// </summary>
    private Game()
    {
        
    }
    
    public int Id { get; private set; }
    
    public int Size { get; private set; }
    
    public int WinLength { get; private set; }
    
    public GameSymbol NextSymbol { get; private set; }
    
    public GameSymbol? Winner { get; private set; }
    
    public GameStatus Status { get; private set; }
    
    public DateTimeOffset CreatedAt { get; private set; }
    
    public int Version { get; private set; }
    
    public ICollection<Move> Moves { get; private set; }

    public static Game Create(int size, int winLength, DateTimeOffset createdAt)
    {
        return new Game(size, winLength, createdAt);
    }

    public void AdvanceTurn()
    {
        NextSymbol = NextSymbol.Flip();
    }

    public bool IsFinished()
    {
        return Status is GameStatus.Draw or GameStatus.XPlayerWon or GameStatus.OPlayerWon;
    }

    public bool IsOutOfBounds(int row, int column)
    {
        return row < 0 || row >= Size ||
               column < 0 || column >= Size;
    }
    
    public void SetStatusInProgress()
    {
        if (Status is GameStatus.Created)
        {
            Status = GameStatus.InProgress;
        }
    }

    public void SetStatusDraw()
    {
        Status = GameStatus.Draw;
        Winner = null;
    }

    public void SetWinner(GameSymbol symbol)
    {
        Winner = symbol;
        
        Status = symbol switch
        {
            GameSymbol.X => GameStatus.XPlayerWon,
            GameSymbol.O => GameStatus.OPlayerWon,
            _ => Status
        };
    }
}
