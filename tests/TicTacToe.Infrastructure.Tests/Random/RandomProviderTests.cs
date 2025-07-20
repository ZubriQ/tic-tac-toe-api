using FluentAssertions;
using TicTacToe.Infrastructure.Random;

namespace TicTacToe.Infrastructure.Tests.Random;

public class RandomProviderTests
{
    private readonly RandomProvider _sut = new();

    [Fact]
    public void NextDouble_ShouldReturnValueBetweenZeroAndOne()
    {
        // Act
        var value = _sut.NextDouble();

        // Assert
        value.Should().BeGreaterThanOrEqualTo(0.0).And.BeLessThan(1.0);
    }

    [Fact]
    public void NextDouble_ShouldReturnDifferentValuesOnSubsequentCalls()
    {
        // Act
        var value1 = _sut.NextDouble();
        var value2 = _sut.NextDouble();

        // Assert
        value1.Should().NotBe(value2, "случайные значения не должны повторяться подряд");
    }
}
