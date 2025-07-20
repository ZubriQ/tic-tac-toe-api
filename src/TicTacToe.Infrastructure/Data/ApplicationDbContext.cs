using Microsoft.EntityFrameworkCore;
using TicTacToe.Application.Abstractions.Data;
using TicTacToe.Domain.Entities;
using TicTacToe.Infrastructure.Configurations;

namespace TicTacToe.Infrastructure.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Game> Games => Set<Game>();
    
    public DbSet<Move> Moves => Set<Move>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.TicTacToe);

        modelBuilder.ApplyConfiguration(new GameConfiguration());
        modelBuilder.ApplyConfiguration(new MoveConfiguration());
    }
}
