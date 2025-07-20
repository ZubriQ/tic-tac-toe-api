using FluentAssertions;
using TicTacToe.Domain.Enums;
using TicTacToe.Domain.Extensions;

namespace TicTacToe.Domain.Tests.Extensions;

public class GameSymbolExtensionsTests
{

    [Fact(DisplayName = "Расширение смены знака X - O, и наоборот")]
    public void Game_symbol_flip_extension_method_Should_work_correctly()
    {
        // Arrange
        var gameSymbol1 = GameSymbol.X;
        var gameSymbol2 = GameSymbol.O;
        
        // Act
        gameSymbol1 = gameSymbol1.Flip();
        gameSymbol2 = gameSymbol2.Flip();
        
        // Assert
        gameSymbol1.Should().Be(GameSymbol.O);
        gameSymbol2.Should().Be(GameSymbol.X);
    }
}
