using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OldVikings.Api.Database.Entities;

namespace OldVikings.Api.Database.Configurations;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.DisplayName).IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.Registered).IsRequired();

        builder.Property(p => p.Approved).IsRequired().HasDefaultValue(true);

        builder.Property(p => p.CreatedAt).IsRequired();
    }
}