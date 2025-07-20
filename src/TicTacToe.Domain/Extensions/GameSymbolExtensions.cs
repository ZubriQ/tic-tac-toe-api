using TicTacToe.Domain.Enums;

namespace TicTacToe.Domain.Extensions;

public static class GameSymbolExtensions
{
    public static GameSymbol Flip(this GameSymbol s) => s switch
    {
        GameSymbol.X => GameSymbol.O,
        GameSymbol.O => GameSymbol.X,
        _ => s
    };
}
