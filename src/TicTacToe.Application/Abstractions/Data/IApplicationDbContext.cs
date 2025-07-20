using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Game> Games { get; }
    
    DbSet<Move> Moves { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
