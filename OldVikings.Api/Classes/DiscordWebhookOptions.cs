namespace OldVikings.Api.Classes;

public class DiscordWebhookOptions
{
    public required string ApiKey { get; set; }

    public required string TestUrl { get; set; }

    public required string EventChannelUrl { get; set; }

    public required string DesertStormChannelUrl { get; set; }

    public required string TrainChannelUrl { get; set; }

    public required string ShinyTaskChannelUrl { get; set; }
}