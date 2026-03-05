using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OldVikings.Api.Database.Entities;

namespace OldVikings.Api.Database.Configurations;

public class FeedbackItemConfiguration : IEntityTypeConfiguration<FeedbackItem>
{
    public void Configure(EntityTypeBuilder<FeedbackItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(120);
        builder.Property(x => x.Message).HasMaxLength(4000).IsRequired();

        builder.Property(x => x.DisplayName).HasMaxLength(64);

        builder.Property(x => x.StatusMessage).HasMaxLength(300);

        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.UpdatedAtUtc).IsRequired(false);

        builder.Property(x => x.IsAnonymous).IsRequired();
        builder.Property(x => x.VotesCount).IsRequired();

        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.Category).IsRequired();
        builder.Property(x => x.Visibility).IsRequired();

        builder.Property(x => x.UpdatedByDiscordName).HasMaxLength(64).IsRequired(false);

        builder.Property(x => x.DiscordChannelId)
            .HasConversion<long?>(
                v => v.HasValue ? checked((long)v.Value) : null,
                v => v.HasValue ? checked((ulong)v.Value) : null)
            .HasColumnType("bigint")
            .IsRequired(false);

        builder.Property(x => x.DiscordMessageId)
            .HasConversion<long?>(
                v => v.HasValue ? checked((long)v.Value) : null,
                v => v.HasValue ? checked((ulong)v.Value) : null)
            .HasColumnType("bigint")
            .IsRequired(false);

        builder.Property(x => x.DiscordThreadId)
            .HasConversion<long?>(
                v => v.HasValue ? checked((long)v.Value) : null,
                v => v.HasValue ? checked((ulong)v.Value) : null)
            .HasColumnType("bigint")
            .IsRequired(false);

        builder.Property(x => x.UpdatedByDiscordUserId)
            .HasConversion<long?>(
                v => v.HasValue ? checked((long)v.Value) : null,
                v => v.HasValue ? checked((ulong)v.Value) : null)
            .HasColumnType("bigint")
            .IsRequired(false);

        builder.HasIndex(x => new { x.DiscordChannelId, x.DiscordMessageId });
    }
}