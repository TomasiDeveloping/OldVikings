namespace OldVikings.Api.Dto.Train;

public class TrainConductorDto
{
    public Guid PlayerId { get; set; }

    public required string PlayerName { get; set; }

    public bool IsAvailable { get; set; }

    public bool BlockNextCycle { get; set; }

    public bool ForcePick { get; set; }

    public DateTime UpdatedAt { get; set; }
}