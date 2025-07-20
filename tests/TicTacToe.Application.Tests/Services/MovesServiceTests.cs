using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;
using TicTacToe.Application.Abstractions.Clock;
using TicTacToe.Application.Abstractions.Data;
using TicTacToe.Application.Abstractions.Random;
using TicTacToe.Application.Abstractions.TicTacToe;
using TicTacToe.Application.Dtos;
using TicTacToe.Application.Services;
using TicTacToe.Domain.Abstractions;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Enums;

namespace TicTacToe.Application.Tests.Services;

public class MovesServiceTests
{
    [Fact(DisplayName = "CreateByGameIdAsync создает ход и возвращает move id")]
    public async Task CreateByGameIdAsync_Should_create_move_When_valid()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        var game = Game.Create(3, 3, now);
        TestHelpers.SetPrivateGameId(game, 10);
        
        var gamesData = new List<Game> { game };

        var contextMock = new Mock<IApplicationDbContext>();
        contextMock.Setup(c => c.Games).ReturnsDbSet(gamesData);
        contextMock.Setup(c => c.Moves).ReturnsDbSet([]);

        contextMock
            .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Callback(() =>
            {
                var nextMoveId = 1;
                foreach (var move in game.Moves)
                {
                    if (TestHelpers.GetPrivateMoveId(move) == 0)
                    {
                        TestHelpers.SetPrivateMoveId(move, nextMoveId++);
                    }
                }
            })
            .ReturnsAsync(1);

        var clockMock = new Mock<IDateTimeProvider>();
        clockMock.Setup(c => c.UtcNow).Returns(now.AddMinutes(1));

        var randomMock = new Mock<IRandomProvider>();
        randomMock.Setup(r => r.NextDouble()).Returns(0.99);

        var engineMock = new Mock<ITicTacToeGameEngine>();
        engineMock
            .Setup(e => e.DetermineWinnerAndUpdateGameStatus(
                It.IsAny<Game>(), It.IsAny<Move>()))
            .Verifiable();

        var sut = new MovesService(
            contextMock.Object,
            clockMock.Object,
            randomMock.Object,
            engineMock.Object);

        var command = new CreateMoveCommand(
            GameId: game.Id,
            Row: 0,
            Column: 0);

        // Act
        var result = await sut.CreateByGameIdAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue("Успешный ход на успешных данных");
        result.Error.Should().Be(Error.None);

        result.Value.Should().Be(1);

        game.Moves.Should().HaveCount(1);
        var move = Assert.Single(game.Moves);

        move.GameId.Should().Be(game.Id);
        move.MoveNumber.Should().Be(1);
        move.Player.Should().Be(GameSymbol.X);
        move.PlayerMove.Should().Be(GameSymbol.X);
        move.Position.Row.Should().Be(0);
        move.Position.Column.Should().Be(0);
        move.IsFlipped.Should().BeFalse();

        contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        engineMock.Verify(e => e.DetermineWinnerAndUpdateGameStatus(game, move), Times.Once);
    }
}
