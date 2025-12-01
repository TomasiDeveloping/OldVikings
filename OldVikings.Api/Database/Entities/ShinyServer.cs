namespace OldVikings.Api.Database.Entities;

public class ShinyServer
{
    public Guid Id { get; set; }

    public int ServerNumber { get; set; }

    public DateOnly FirstShinyDate { get; set; }
}