namespace OldVikings.Api.Interfaces;

public interface IJwtService
{
    string GenerateTokenAsync(string role);
}