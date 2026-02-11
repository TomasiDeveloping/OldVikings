namespace OldVikings.Api.Dto;

public class PlayerDto
{
    public Guid Id { get; set; }

    public required string DisplayName { get; set; }

    public bool Registered { get; set; } = false;

    public bool Approved { get; set; } = true;
}