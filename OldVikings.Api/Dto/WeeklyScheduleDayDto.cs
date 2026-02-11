using OldVikings.Api.Database.Entities;

namespace OldVikings.Api.Dto;

public class WeeklyScheduleDayDto
{
    public DateOnly Date { get; set; }

    public string LeaderPlayer { get; set; } = null!;

    public string VipPlayer { get; set; } = null!;
}