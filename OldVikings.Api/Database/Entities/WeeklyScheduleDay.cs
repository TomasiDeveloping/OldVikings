namespace OldVikings.Api.Database.Entities;

public class WeeklyScheduleDay
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public Guid ScheduleId { get; set; }

    public DateOnly Date { get; set; }

    public Guid LeaderPlayerId { get; set; }

    public Guid VipPlayerId { get; set; }

    public WeeklySchedule Schedule { get; set; } = null!;

    public Player LeaderPlayer { get; set; } = null!;

    public Player VipPlayer { get; set; } = null!;
}