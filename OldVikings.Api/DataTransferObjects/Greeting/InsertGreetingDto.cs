namespace OldVikings.Api.DataTransferObjects.Greeting;

public class InsertGreetingDto
{

    public int ServerNumber { get; set; }

    public required string AllianceName { get; set; }

    public string PlayerName { get; set; } = string.Empty;
}