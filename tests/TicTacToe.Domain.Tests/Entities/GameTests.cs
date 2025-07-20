using FluentAssertions;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Enums;

namespace TicTacToe.Domain.Tests.Entities;

public class GameTests
{
    /// <summary>
    ///     Игра
    /// </summary>
    private readonly Game _game = Game.Create(3, 3, DateTimeOffset.UtcNow);

    [Fact(DisplayName = "Обновление статуса игры на ничья")]
    public void Game_set_status_draw_Should_work_correctly()
    {
        // Act
        _game.SetStatusDraw();
        
        // Assert
        _game.Status.Should().Be(GameStatus.Draw);
    }
    
    [Fact(DisplayName = "Обновление статуса игры на в процессе")]
    public void Game_set_status_in_progress_Should_work_correctly()
    {
        // Act
        _game.SetStatusInProgress();
        
        // Assert
        _game.Status.Should().Be(GameStatus.InProgress);
    }
    
    [Fact(DisplayName = "Обновление победителя должно работать успешно")]
    public void Game_set_winner_Should_work_correctly()
    {
        // Act
        _game.SetWinner(GameSymbol.O);
        
        // Assert
        _game.Winner.Should().Be(GameSymbol.O);
    }
}
