using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OldVikings.Api.Database.Entities;

namespace OldVikings.Api.Database.Configurations;

public class GreetingsConfiguration : IEntityTypeConfiguration<Greeting>
{
    public void Configure(EntityTypeBuilder<Greeting> builder)
    {
        builder.HasKey(greeting => greeting.Id);
        builder.Property(greeting => greeting.AllianceName).HasMaxLength(150).IsRequired();
        builder.Property(greeting => greeting.PlayerName).HasMaxLength(200).IsRequired(false);
        builder.Property(greeting => greeting.ServerNumber).IsRequired();
    }
}