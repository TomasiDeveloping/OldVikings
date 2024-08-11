namespace OldVikings.Api.DataTransferObjects.Greeting;

public class GreetingDto
{
    public Guid Id { get; set; }

    public int ServerNumber { get; set; }

    public required string AllianceName { get; set; }

    public string PlayerName { get; set; } = string.Empty;

    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}