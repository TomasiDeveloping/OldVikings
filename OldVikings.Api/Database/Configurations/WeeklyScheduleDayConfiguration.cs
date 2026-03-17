using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OldVikings.Api.Database.Entities;

namespace OldVikings.Api.Database.Configurations;

public class WeeklyScheduleDayConfiguration : IEntityTypeConfiguration<WeeklyScheduleDay>
{
    public void Configure(EntityTypeBuilder<WeeklyScheduleDay> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Date)
            .IsRequired();

        builder.HasOne(d => d.LeaderPlayer)
            .WithMany()
            .HasForeignKey(d => d.LeaderPlayerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(d => d.VipPlayer)
            .WithMany()
            .HasForeignKey(d => d.VipPlayerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(d => d.Schedule)
            .WithMany(p => p.Days)
            .HasForeignKey(d => d.ScheduleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.ScheduleId, x.Date }).IsUnique();
    }
}