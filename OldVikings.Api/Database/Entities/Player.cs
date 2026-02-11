namespace OldVikings.Api.Database.Entities;

public class Player
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public required string DisplayName { get; set; }

    public bool Registered { get; set; } = false;

    public bool Approved { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public PoolLeader?  PoolLeader { get; set; }

    public PoolVip? PoolVip { get; set; }
}