using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OldVikings.Api.Database.Entities;

namespace OldVikings.Api.Database.Configurations;

public class ShinyServerConfiguration : IEntityTypeConfiguration<ShinyServer>
{
    public void Configure(EntityTypeBuilder<ShinyServer> builder)
    {
        builder.HasKey(server => server.Id);

        builder.Property(server => server.ServerNumber).IsRequired();

        builder.Property(server => server.FirstShinyDate).IsRequired();

    }
}