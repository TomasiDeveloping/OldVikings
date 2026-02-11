using OldVikings.Api.Database.Entities;

namespace OldVikings.Api.Dto;

public class WeeklyScheduleDto
{
    public Guid Id { get; set; }

    public DateOnly WeekStartDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<WeeklyScheduleDayDto> Days { get; set; } = [];
}