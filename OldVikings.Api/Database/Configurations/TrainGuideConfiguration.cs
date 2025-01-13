using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OldVikings.Api.Database.Entities;

namespace OldVikings.Api.Database.Configurations;

public class TrainGuideConfiguration : IEntityTypeConfiguration<TrainGuide>
{
    public void Configure(EntityTypeBuilder<TrainGuide> builder)
    {
        builder.HasKey(trainGuide => trainGuide.Id);

        builder.Property(trainGuide => trainGuide.CurrentPlayerIndex).IsRequired();
        builder.Property(trainGuide => trainGuide.LastUpdate).IsRequired();
    }
}