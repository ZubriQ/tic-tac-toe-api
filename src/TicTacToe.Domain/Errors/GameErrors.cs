using TicTacToe.Domain.Abstractions;

namespace TicTacToe.Domain.Errors;

public static class GameErrors
{
    public static readonly Error InvalidGameOptions = Error.Problem(
        "Games.InvalidGameOptions",
        "Invalid game size or win length condition");
    
    public static readonly Error InvalidGameId = Error.Problem(
        "Games.InvalidGameId",
        "Invalid game id");
    
    public static readonly Error NotFound = Error.Problem(
        "Games.NotFound",
        "Game not found");
}
