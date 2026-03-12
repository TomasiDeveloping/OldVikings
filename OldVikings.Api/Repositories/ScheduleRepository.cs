using Microsoft.EntityFrameworkCore;
using OldVikings.Api.Database;
using OldVikings.Api.Database.Entities;
using OldVikings.Api.Dto;
using OldVikings.Api.Helper;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Repositories;

public class ScheduleRepository(OldVikingsContext dbContext) : IScheduleRepository
{
    public async Task<List<WeeklyScheduleDto>> GetHistoryAsync(int page = 1, int pageSize = 10, string? playerName = null, int? year = null,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.WeeklySchedules.AsQueryable();

        if (!string.IsNullOrWhiteSpace(playerName))
        {
            var lowerName = playerName.ToLower();
            query = query.Where(s => s.Days.Any(d =>
                (d.LeaderPlayer.DisplayName.ToLower().Contains(lowerName) ||
                 d.VipPlayer.DisplayName.ToLower().Contains(lowerName))));
        }

        if (year.HasValue)
        {
            query = query.Where(s => s.WeekStartDate.Year == year.Value);
        }

        var weeklyScheduleDtos = await query
            .OrderByDescending(s => s.WeekStartDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new WeeklyScheduleDto
            {
                Id = s.Id,
                CreatedAt = s.CreatedAt,
                WeekStartDate = s.WeekStartDate,
                Days = s.Days
                    .OrderBy(d => d.Date) // Mo -> So
                    .Select(d => new WeeklyScheduleDayDto
                    {
                        Date = d.Date,
                        LeaderPlayer = d.LeaderPlayer.DisplayName,
                        VipPlayer = d.VipPlayer.DisplayName
                    })
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return weeklyScheduleDtos;
    }

    public async Task<WeeklyScheduleDto?> GetCurrentWeekAsync(CancellationToken cancellationToken = default)
    {
        var currentWeekStart = WeekHelper.GetCurrentWeekMonday();

        var dto = await dbContext.WeeklySchedules
            .Where(s => s.WeekStartDate == currentWeekStart)
            .Select(s => new WeeklyScheduleDto
            {
                Id = s.Id,
                CreatedAt = s.CreatedAt,
                WeekStartDate = s.WeekStartDate,
                Days = s.Days
                    .OrderBy(d => d.Date) // Mo -> So
                    .Select(d => new WeeklyScheduleDayDto
                    {
                        Date = d.Date,
                        LeaderPlayer = d.LeaderPlayer.DisplayName,
                        VipPlayer = d.VipPlayer.DisplayName
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        return dto;
    }

    public async Task<WeeklyScheduleDto?> GetWeekAfterNextAsync(DateOnly date, CancellationToken cancellationToken = default)
    {
        var nextMonday = WeekHelper.GetNextMonday(date);

        var dto = await dbContext.WeeklySchedules
            .Where(s => s.WeekStartDate == nextMonday)
            .Select(s => new WeeklyScheduleDto
            {
                Id = s.Id,
                CreatedAt = s.CreatedAt,
                WeekStartDate = s.WeekStartDate,
                Days = s.Days
                    .OrderBy(d => d.Date) // Mo -> So
                    .Select(d => new WeeklyScheduleDayDto
                    {
                        Date = d.Date,
                        LeaderPlayer = d.LeaderPlayer.DisplayName,
                        VipPlayer = d.VipPlayer.DisplayName
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        return dto;

    }

    public async Task<WeeklySchedule?> GetWeeklyAsync(DateOnly weekStarMonday, CancellationToken cancellationToken = default)
    {
        return await dbContext.WeeklySchedules
            .Include(s => s.Days)
            .FirstOrDefaultAsync(s => s.WeekStartDate == weekStarMonday, cancellationToken);
    }

    public async Task<bool> WeekExistsAsync(DateOnly weekStartMonday, CancellationToken cancellationToken = default)
    {
        return await dbContext.WeeklySchedules.AnyAsync(s => s.WeekStartDate == weekStartMonday, cancellationToken);
    }

    public async Task AddScheduleAsync(WeeklySchedule schedule, CancellationToken cancellationToken = default)
    {
        await dbContext.WeeklySchedules.AddAsync(schedule, cancellationToken);
    }

    public async Task AddDayAsync(WeeklyScheduleDay day, CancellationToken cancellationToken = default)
    {
        await dbContext.WeeklyScheduleDays.AddAsync(day, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}