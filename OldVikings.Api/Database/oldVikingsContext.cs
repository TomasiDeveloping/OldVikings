using Microsoft.EntityFrameworkCore;
using OldVikings.Api.Database.Entities;

namespace OldVikings.Api.Database;

public class OldVikingsContext(DbContextOptions<OldVikingsContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OldVikingsContext).Assembly);
    }

    public DbSet<Greeting> Greetings { get; set; }
}