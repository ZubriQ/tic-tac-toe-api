using TicTacToe.Application.Abstractions.Random;

namespace TicTacToe.Infrastructure.Random;

public sealed class RandomProvider : IRandomProvider
{
    private readonly System.Random _rng = new();
    
    public double NextDouble() => _rng.NextDouble();
}
