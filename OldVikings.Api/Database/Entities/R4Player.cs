namespace OldVikings.Api.Database.Entities;

public class R4Player
{
    public Guid Id { get; set; }

    public int Order { get; set; }

    public required string PlayerName { get; set; }
}