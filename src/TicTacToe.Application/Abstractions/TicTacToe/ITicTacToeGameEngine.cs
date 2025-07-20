using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Enums;

namespace TicTacToe.Application.Abstractions.TicTacToe;

public interface ITicTacToeGameEngine
{
    void DetermineWinnerAndUpdateGameStatus(Game game, Move lastMove);

    int TryFlip(Game game, out bool isFlipped, out GameSymbol placedSymbol);
}
