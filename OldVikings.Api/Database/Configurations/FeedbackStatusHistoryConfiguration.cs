using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OldVikings.Api.Database.Entities;

namespace OldVikings.Api.Database.Configurations;

public class FeedbackStatusHistoryConfiguration : IEntityTypeConfiguration<FeedbackStatusHistory>
{
    public void Configure(EntityTypeBuilder<FeedbackStatusHistory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.DiscordUserName).HasMaxLength(64).IsRequired();
        builder.Property(x => x.Note).HasMaxLength(300).IsRequired(false);

        builder.Property(x => x.ChangedAtUtc).IsRequired();

        builder.Property(x => x.DiscordUserId)
            .HasConversion(
                v => checked((long)v),
                v => checked((ulong)v))
            .HasColumnType("bigint");

        builder.HasOne(x => x.FeedbackItem)
            .WithMany()
            .HasForeignKey(x => x.FeedbackItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.FeedbackItemId, x.ChangedAtUtc });
    }
}