namespace OldVikings.Api.Classes;

public class DiscordBotOptions
{
    public required string Token { get; set; }

    public required string FeedbackChannelId { get; set; }

    public required string PublicKey { get; set; }

    public required string DiscordChannelBaseUrl { get; set; }
}