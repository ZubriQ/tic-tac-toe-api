using FluentAssertions;
using TicTacToe.Domain.Abstractions;

namespace TicTacToe.Domain.Tests.Abstractions;

public class ResultTests
{
    [Fact(DisplayName = "Success<T> не должно содержать ошибки")]
    public void Success_Generic_ShouldContainValue()
    {
        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().Be(Error.None);
    }

    [Fact(DisplayName = "Failure<T> не может содержать значение")]
    public void Failure_Generic_Value_Access_ShouldThrow()
    {
        // Arrange
        var error = new Error("Test.Failure", "failure", ErrorType.Problem);

        // Act
        var result = Result.Failure<int>(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
        
        var act = () => { _ = result.Value; };
        act.Should().Throw<InvalidOperationException>();
    }
}
