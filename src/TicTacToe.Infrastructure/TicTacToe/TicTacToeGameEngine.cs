using TicTacToe.Application.Abstractions.Random;
using TicTacToe.Application.Abstractions.TicTacToe;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Enums;
using TicTacToe.Domain.Extensions;

namespace TicTacToe.Infrastructure.TicTacToe;

public class TicTacToeGameEngine(IRandomProvider randomProvider) : ITicTacToeGameEngine
{
    private const double FlipChance = 0.10;
    private const int FlipPerNumberMoves = 3;
    
    private static readonly (int dr, int dc)[] Directions =
    [
        (0, 1),
        (1, 0),
        (1, 1),
        (1, -1)
    ];
    
    private GameSymbol _symbol;
    private Game _game;
    private int _size;
    private int _need;
    private int _row0;
    private int _col0;
    
    public void DetermineWinnerAndUpdateGameStatus(Game game, Move lastMove)
    {
        InitializeVariables(game, lastMove);

        var winner = DetermineWinner();
        if (winner is not null)
        {
            game.SetWinner(winner.Value);
            return;
        }

        var moveCount = game.Moves.Count;
        if (moveCount >= game.Size * game.Size)
        {
            game.SetStatusDraw();
            return;
        }

        game.SetStatusInProgress();
    }

    private void InitializeVariables(Game game, Move lastMove)
    {
        _symbol = lastMove.PlayerMove;
        _game = game;
        _size = game.Size;
        _need = game.WinLength;
        _row0 = lastMove.Position.Row;
        _col0 = lastMove.Position.Column;
    }

    private GameSymbol? DetermineWinner()
    {
        var cells = new HashSet<(int r,int c)>();
        FillMap(cells);

        if (IsWinningMove(cells))
        {
            return _symbol;
        }

        return null;
    }
    
    private void FillMap(HashSet<(int r, int c)> cells)
    {
        foreach (var m in _game.Moves)
        {
            if (m.PlayerMove == _symbol)
            {
                cells.Add((m.Position.Row, m.Position.Column));
            }
        }
    }

    private bool IsWinningMove(HashSet<(int r, int c)> occupied)
    {
        foreach (var (dr, dc) in Directions)
        {
            if (LineSpan(occupied, _row0, _col0, dr, dc) >= _need)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    ///     Длина непрерывной линии через (row,col) по (dr,dc), включая исходную.
    /// </summary>
    private int LineSpan(HashSet<(int r, int c)> occupied, int row, int col, int dr, int dc)
    {
        return 1
               + CountForward(occupied, row, col, dr, dc)
               + CountForward(occupied, row, col, -dr, -dc);
    }

    /// <summary>
    ///     Сколько занятых подряд от (row,col) в направлении (dr,dc); без исходной.
    /// </summary>
    private int CountForward(HashSet<(int r, int c)> occupied, int row, int col, int dr, int dc)
    {
        var r = row + dr;
        var c = col + dc;
        var count = 0;

        while (r >= 0 && r < _size && c >= 0 && c < _size && occupied.Contains((r, c)))
        {
            count++;
            r += dr;
            c += dc;
        }
        
        return count;
    }
    
    public int TryFlip(Game game, out bool isFlipped, out GameSymbol placedSymbol)
    {
        var currentSymbol = game.NextSymbol;
        
        var moveNumber = game.Moves.Count + 1;
        var shouldFlip = moveNumber % FlipPerNumberMoves == 0;
        isFlipped = shouldFlip && randomProvider.NextDouble() < FlipChance;
        
        placedSymbol = isFlipped ? currentSymbol.Flip() : currentSymbol;
        
        return moveNumber;
    }
}
