namespace OldVikings.Api.Database.Entities;

public class TrainGuide
{
    public Guid Id { get; set; }

    public int CurrentPlayerIndex { get; set; }

    public DateTime LastUpdate { get; set; }
}