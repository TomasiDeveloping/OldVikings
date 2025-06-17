using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OldVikings.Api.Database.Entities;

namespace OldVikings.Api.Database.Configurations;

public class R4PlayerConfiguration : IEntityTypeConfiguration<R4Player>
{
    public void Configure(EntityTypeBuilder<R4Player> builder)
    {
        builder.HasKey(r4Player => r4Player.Id);

        builder.Property(r4Player => r4Player.PlayerName).IsRequired().HasMaxLength(250);
    }
}