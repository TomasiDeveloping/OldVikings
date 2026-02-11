using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OldVikings.Api.Database.Entities;

namespace OldVikings.Api.Database.Configurations;

public class PoolLeaderConfiguration : IEntityTypeConfiguration<PoolLeader>
{
    public void Configure(EntityTypeBuilder<PoolLeader> builder)
    {
        builder.HasKey(pl => pl.PlayerId);

        builder.Property(pl => pl.IsAvailable)
            .IsRequired();

        builder.Property(pl => pl.ForcePick)
            .IsRequired();

        builder.Property(pl => pl.BlockNextCycle)
            .IsRequired();

        builder.Property(pl => pl.UpdatedAt)
            .IsRequired();

        builder.HasOne(pl => pl.Player)
            .WithOne(p => p.PoolLeader)
            .HasForeignKey<PoolLeader>(pl => pl.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}