namespace OldVikings.Api.Database.Entities;

public class WeeklySchedule
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public DateOnly WeekStartDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<WeeklyScheduleDay> Days { get; set; } = [];
}