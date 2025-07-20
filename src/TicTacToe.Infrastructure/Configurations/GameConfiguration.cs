using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Infrastructure.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .ValueGeneratedOnAdd();

        builder.Property(g => g.Size)
            .IsRequired();

        builder.Property(g => g.WinLength)
            .IsRequired();
        
        builder.Property(g => g.NextSymbol)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(g => g.Winner)
            .HasConversion<int?>();

        builder.Property(g => g.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(g => g.CreatedAt)
            .IsRequired();
        
        builder.Property(g => g.Version)
            .IsConcurrencyToken();
        
        builder.HasMany(g => g.Moves)
            .WithOne()
            .HasForeignKey(m => m.GameId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
