namespace OldVikings.Api.Database.Entities;

public class R4Roles
{
    public Guid Id { get; set; }

    public required string RoleName { get; set; }

    public required string PlayerName { get; set; }

    public required string ImageUrl { get; set; }
}