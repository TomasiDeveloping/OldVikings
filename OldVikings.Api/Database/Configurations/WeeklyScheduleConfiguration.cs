using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OldVikings.Api.Database.Entities;

namespace OldVikings.Api.Database.Configurations;

public class WeeklyScheduleConfiguration : IEntityTypeConfiguration<WeeklySchedule>
{
    public void Configure(EntityTypeBuilder<WeeklySchedule> builder)
    {
        builder.HasKey(ws => ws.Id);

        builder.Property(ws => ws.WeekStartDate)
            .IsRequired();

        builder.Property(ws => ws.CreatedAt)
            .IsRequired();

        builder.HasIndex(ws => ws.WeekStartDate).IsUnique();

        builder.Property(ws => ws.WeekStartDate).HasConversion(
            v => v.ToDateTime(TimeOnly.MinValue),
            v => DateOnly.FromDateTime(v));
    }
}