using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OldVikings.Api.Database.Entities;

namespace OldVikings.Api.Database.Configurations;

public class R4RolesConfiguration : IEntityTypeConfiguration<R4Roles>
{
    public void Configure(EntityTypeBuilder<R4Roles> builder)
    {
        builder.HasKey(role => role.Id);
        builder.Property(role => role.RoleName).HasMaxLength(250).IsRequired();
        builder.Property(role => role.PlayerName).HasMaxLength(250).IsRequired();
        builder.Property(role => role.ImageUrl).HasMaxLength(250).IsRequired();
    }
}