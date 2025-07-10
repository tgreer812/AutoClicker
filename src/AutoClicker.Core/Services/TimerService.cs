using AutoClicker.Core.Interfaces;

namespace AutoClicker.Core.Services;

/// <summary>
/// Service for timer and scheduling functionality
/// </summary>
public class TimerService : ITimerService
{
    private int _serverTimeOffsetHours = 0;
    
    public int ServerTimeOffsetHours 
    { 
        get => _serverTimeOffsetHours; 
        set => _serverTimeOffsetHours = value; 
    }

    public DateTime GetCurrentTime(bool useServerTime = false)
    {
        var currentTime = DateTime.Now;
        return useServerTime ? currentTime.AddHours(_serverTimeOffsetHours) : currentTime;
    }

    public TimeSpan GetTimeUntilStart(DateTime scheduledTime, bool useServerTime = false)
    {
        var currentTime = GetCurrentTime(useServerTime);
        return scheduledTime - currentTime;
    }

    public async Task ScheduleActionAsync(DateTime scheduledTime, Action action, bool useServerTime = false, CancellationToken cancellationToken = default)
    {
        // TODO: Implement scheduled action execution
        throw new NotImplementedException();
    }
}
