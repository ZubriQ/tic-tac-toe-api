using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;
using TicTacToe.Application.Abstractions.Clock;
using TicTacToe.Application.Abstractions.Data;
using TicTacToe.Application.Options;
using TicTacToe.Application.Services;
using TicTacToe.Domain.Abstractions;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Enums;

namespace TicTacToe.Application.Tests.Services;

public class GamesServiceTests
{
    /// <summary>
    ///     Текущее время 
    /// </summary>
    private readonly DateTimeOffset _now;
    
    /// <summary>
    ///     Настройки игры
    /// </summary>
    private readonly Mock<IOptionsSnapshot<GameOptions>> _optsMock;
    
    /// <summary>
    ///     Провайдер времени
    /// </summary>
    private readonly Mock<IDateTimeProvider> _clockMock;
    
    /// <summary>
    ///     Контекст БД
    /// </summary>
    private readonly Mock<IApplicationDbContext> _contextMock;

    public GamesServiceTests()
    {
        _now = DateTimeOffset.UtcNow;
        
        var optsValue = new GameOptions { Size = 3, WinLength = 3 };
        _optsMock = new Mock<IOptionsSnapshot<GameOptions>>();
        _optsMock.Setup(o => o.Value).Returns(optsValue);
        
        _clockMock = new Mock<IDateTimeProvider>();
        _clockMock.Setup(c => c.UtcNow).Returns(_now);
        
        _contextMock = new Mock<IApplicationDbContext>();
    }

    [Fact(DisplayName = "CreateGameAsync должен работать корректно с валидными настройками и возвращать game id")]
    public async Task CreateGameAsync_Should_create_and_return_id_When_options_valid()
    {
        // Arrange
        var gamesDbSetMock = new Mock<DbSet<Game>>();

        Game? capturedGame = null;

        gamesDbSetMock
            .Setup(d => d.AddAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()))
            .Callback((Game g, CancellationToken _) => capturedGame = g)
            .ReturnsAsync((Game _, CancellationToken _) => null);
        
        _contextMock.Setup(c => c.Games).Returns(gamesDbSetMock.Object);
        _contextMock
            .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Callback(() =>
            {
                if (capturedGame is not null)
                {
                    TestHelpers.SetPrivateGameId(capturedGame, 333);
                }
            })
            .ReturnsAsync(1);

        var sut = new GamesService(_contextMock.Object, _optsMock.Object, _clockMock.Object);

        // Act
        var result = await sut.CreateGameAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        result.Value.Should().Be(333);

        capturedGame.Should().NotBeNull();
        capturedGame.Size.Should().Be(3);
        capturedGame.WinLength.Should().Be(3);
        capturedGame.CreatedAt.Should().Be(_now);

        gamesDbSetMock.Verify(d => d.AddAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact(DisplayName = "GetGameByIdAsync возвращает Success<Game> когда игра существует")]
    public async Task GetGameByIdAsync_Should_return_game_When_exists()
    {
        // Arrange
        var game = Game.Create(3, 3, _now);
        TestHelpers.SetPrivateGameId(game, 42);

        var move = TestHelpers.CreateMove(
            gameId: 42, 
            moveNumber: 1, 
            player: GameSymbol.X, 
            playerMove: GameSymbol.X, 
            row: 0, 
            col: 0, 
            isFlipped: false, 
            createdAt: _now.AddMinutes(1));
        
        game.Moves.Add(move);

        var gamesData = new List<Game> { game };
        
        _contextMock.Setup(c => c.Games).ReturnsDbSet(gamesData);
        _contextMock.Setup(c => c.Moves).ReturnsDbSet([.. game.Moves]);

        var sut = new GamesService(_contextMock.Object, _optsMock.Object, _clockMock.Object);

        // Act
        var result = await sut.GetGameByIdAsync(42);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        result.Value.Id.Should().Be(42);
        result.Value.Moves.Should().HaveCount(1);
    }
}
