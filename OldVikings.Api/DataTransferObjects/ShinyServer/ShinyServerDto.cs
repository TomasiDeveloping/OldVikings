namespace OldVikings.Api.DataTransferObjects.ShinyServer;

public class ShinyServerDto
{
    public Guid Id { get; set; }

    public int ServerNumber { get; set; }

    public DateOnly FirstShinyDate { get; set; }
}