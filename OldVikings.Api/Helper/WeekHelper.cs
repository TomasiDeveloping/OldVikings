namespace OldVikings.Api.Helper;

public static class WeekHelper
{
    public static DateOnly GetNextMonday(DateOnly date)
    {
        var daysUntilNextMonday = ((int)DayOfWeek.Monday - (int)date.DayOfWeek + 7) % 7;
        if (daysUntilNextMonday == 0) daysUntilNextMonday = 7;
        return date.AddDays(daysUntilNextMonday);
    }

    public static DateOnly GetCurrentWeekMonday()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var diff = (7 + (int)date.DayOfWeek - (int)DayOfWeek.Monday) % 7;
        return date.AddDays(-diff);
    }
}