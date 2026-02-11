using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OldVikings.Api.Database.Entities;

namespace OldVikings.Api.Database.Configurations;

public class PoolVipConfiguration : IEntityTypeConfiguration<PoolVip>
{
    public void Configure(EntityTypeBuilder<PoolVip> builder)
    {
        builder.HasKey(pv => pv.PlayerId);

        builder.Property(pv => pv.IsAvailable)
            .IsRequired();

        builder.Property(pv => pv.ForcePick)
            .IsRequired();

        builder.Property(pv => pv.BlockNextCycle)
            .IsRequired();

        builder.Property(pv => pv.UpdatedAt)
            .IsRequired();

        builder.HasOne(pv => pv.Player)
            .WithOne(p => p.PoolVip)
            .HasForeignKey<PoolVip>(pv => pv.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}