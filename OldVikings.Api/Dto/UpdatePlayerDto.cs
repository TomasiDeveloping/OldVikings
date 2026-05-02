namespace OldVikings.Api.Dto;

public class UpdatePlayerDto
{
    public Guid Id { get; set; }

    public required string DisplayName { get; set; }

    public bool Registered { get; set; }

    public bool Approved { get; set; }
}