using TicTacToe.Domain.Abstractions;

namespace TicTacToe.Domain.Errors;

public static class MoveErrors
{
    public static readonly Error InvalidGameId = Error.Problem(
        "Moves.InvalidGameId",
        "Invalid game id");
    
    public static readonly Error GameNotFound = Error.Problem(
        "Moves.GameNotFound",
        "Game not found");
    
    public static readonly Error GameIsFinished = Error.Problem(
        "Moves.GameIsFinished",
        "Game is already finished");
    
    public static readonly Error AlreadyOccupied = Error.Problem(
        "Moves.AlreadyOccupied",
        "The squire is already occupied");
    
    public static readonly Error RowOrColumnOutOfBounds = Error.Problem(
        "Moves.RowOrColumnOutOfBounds",
        "Row or column out of bounds");
}
