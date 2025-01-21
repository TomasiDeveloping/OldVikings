namespace OldVikings.Api.DataTransferObjects.TrainGuide;

public class TrainGuideDto
{
    public required string CurrentPlayer { get; set; }

    public required string NextPlayer { get; set; }

    public required string NextButOnePlayer { get; set; }
}