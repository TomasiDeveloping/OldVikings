namespace OldVikings.Api.Database.Entities;

public class PoolLeader
{
    public Guid PlayerId  { get; set; }

    public bool IsAvailable { get; set; } = true;

    public bool ForcePick { get; set; } = false;

    public bool BlockNextCycle { get; set; } = false;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Player Player { get; set; } = null!;
}