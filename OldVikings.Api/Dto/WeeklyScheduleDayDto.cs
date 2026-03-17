namespace OldVikings.Api.Dto;

public class WeeklyScheduleDayDto
{
    public DateOnly Date { get; set; }

    public string? LeaderPlayer { get; set; }

    public string? VipPlayer { get; set; }
}