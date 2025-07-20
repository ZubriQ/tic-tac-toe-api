using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Infrastructure.Configurations;

public class MoveConfiguration : IEntityTypeConfiguration<Move>
{
    public void Configure(EntityTypeBuilder<Move> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedOnAdd();

        builder.Property(m => m.GameId)
            .IsRequired();

        builder.Property(m => m.Player)
            .IsRequired();

        builder.Property(m => m.PlayerMove)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.Version)
            .IsConcurrencyToken();

        builder.OwnsOne(
            m => m.Position,
            pos =>
            {
                pos.Property(p => p.Row)
                    .HasColumnName("Row")
                    .IsRequired();

                pos.Property(p => p.Column)
                    .HasColumnName("Column")
                    .IsRequired();
            });
    }
}
