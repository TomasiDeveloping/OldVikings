namespace OldVikings.Api.DataTransferObjects.R4Roles;

public class R4RoleDto
{
    public Guid Id { get; set; }

    public required string RoleName { get; set; }

    public required string PlayerName { get; set; }

    public required string ImageUrl { get; set; }
}