using OldVikings.Api.Database.Entities;
using OldVikings.Api.Dto;

namespace OldVikings.Api.Interfaces;

public interface IScheduleRepository
{
    Task<WeeklyScheduleDto?> GetCurrentWeekAsync(CancellationToken cancellationToken = default);

    Task<WeeklySchedule?> GetWeeklyAsync(DateOnly weekStarMonday, CancellationToken cancellationToken = default);

    Task<bool> WeekExistsAsync(DateOnly weekStartMonday, CancellationToken cancellationToken = default);

    Task AddScheduleAsync(WeeklySchedule schedule, CancellationToken cancellationToken = default);

    Task AddDayAsync(WeeklyScheduleDay day, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}