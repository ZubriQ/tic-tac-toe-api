namespace TicTacToe.Application.Dtos;

public record CreateMoveCommand(
    int GameId,
    int Row,
    int Column);
