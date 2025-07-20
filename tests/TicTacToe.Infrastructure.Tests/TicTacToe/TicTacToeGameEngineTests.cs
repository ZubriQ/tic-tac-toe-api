using FluentAssertions;
using Moq;
using TicTacToe.Application.Abstractions.Random;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Enums;
using TicTacToe.Domain.ValueObjects;
using TicTacToe.Infrastructure.TicTacToe;

namespace TicTacToe.Infrastructure.Tests.TicTacToe;

public class TicTacToeGameEngineTests
{
    /// <summary>
    ///     Текущая дата
    /// </summary>
    private readonly DateTimeOffset _now = DateTimeOffset.UtcNow;
    
    /// <summary>
    ///     Тестируемый unit
    /// </summary>
    private readonly TicTacToeGameEngine _sut;
    
    public TicTacToeGameEngineTests()
    {
        var randomMock = new Mock<IRandomProvider>();
        _sut = new TicTacToeGameEngine(randomMock.Object);
    }
    
    [Fact(DisplayName = "Определение победителя при правилах 3х3, 3")]
    public void DetermineWinnerAndUpdateGameStatus_Should_set_winner_When_three_x_in_row()
    {
        // Arrange
        const int size = 3;
        const int winLength = 3;
        
        var game = Game.Create(size, winLength, _now);

        game.Moves.Add(Move.Create(
            gameId: 0,
            moveNumber: 1,
            player: GameSymbol.X,
            playerMove: GameSymbol.X,
            position: new Position(0, 0),
            isFlipped: false,
            createdAt: _now.AddMinutes(1)));

        game.Moves.Add(Move.Create(
            gameId: 0,
            moveNumber: 2,
            player: GameSymbol.X,
            playerMove: GameSymbol.X,
            position: new Position(0, 1),
            isFlipped: false,
            createdAt: _now.AddMinutes(2)));

        var lastMove = Move.Create(
            gameId: 0,
            moveNumber: 3,
            player: GameSymbol.X,
            playerMove: GameSymbol.X,
            position: new Position(0, 2),
            isFlipped: false,
            createdAt: _now.AddMinutes(3));

        game.Moves.Add(lastMove);

        // Act
        _sut.DetermineWinnerAndUpdateGameStatus(game, lastMove);

        // Assert
        game.Status.Should().Be(GameStatus.XPlayerWon);
        game.Winner.Should().Be(GameSymbol.X);
    }
    
    [Fact(DisplayName = "Определение победителя при правилах 6х6, 5")]
    public void DetermineWinnerAndUpdateGameStatus_Should_set_winner_When_five_in_row()
    {
        // Arrange
        const int size = 6;
        const int winLength = 5;
        
        var game = Game.Create(size, winLength, _now);

        for (var i = 0; i < 4; i++)
        {
            game.Moves.Add(Move.Create(
                gameId: 0,
                moveNumber: i + 1,
                player: GameSymbol.O,
                playerMove: GameSymbol.O,
                position: new Position(i, 2),
                isFlipped: false,
                createdAt: _now.AddMinutes(i + 1)));
        }

        var lastMove = Move.Create(
            gameId: 0,
            moveNumber: 5,
            player: GameSymbol.O,
            playerMove: GameSymbol.O,
            position: new Position(4, 2),
            isFlipped: false,
            createdAt: _now.AddMinutes(5));

        game.Moves.Add(lastMove);

        // Act
        _sut.DetermineWinnerAndUpdateGameStatus(game, lastMove);

        // Assert
        game.Status.Should().Be(GameStatus.OPlayerWon);
        game.Winner.Should().Be(GameSymbol.O);
    }
    
    [Fact(DisplayName = "Определение правильного статуса - 'в процессе' - при правилах 10х10, 10")]
    public void DetermineWinnerAndUpdateGameStatus_Should_set_in_progress_When_moves_are_less_than_win_length()
    {
        // Arrange
        const int size = 10;
        const int winLength = 10;
        
        var game = Game.Create(size, winLength, _now);

        for (var i = 0; i < 7; i++)
        {
            game.Moves.Add(Move.Create(
                gameId: 0,
                moveNumber: i + 1,
                player: GameSymbol.X,
                playerMove: GameSymbol.X,
                position: new Position(0, i),
                isFlipped: false,
                createdAt: _now.AddMinutes(i + 1)));
        }

        var lastMove = Move.Create(
            gameId: 0,
            moveNumber: 8,
            player: GameSymbol.X,
            playerMove: GameSymbol.X,
            position: new Position(0, 7),
            isFlipped: false,
            createdAt: _now.AddMinutes(8));

        game.Moves.Add(lastMove);

        // Act
        _sut.DetermineWinnerAndUpdateGameStatus(game, lastMove);

        // Assert
        game.Status.Should().Be(GameStatus.InProgress);
        game.Winner.Should().BeNull();
    }
}
