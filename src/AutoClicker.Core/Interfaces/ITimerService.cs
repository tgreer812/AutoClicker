namespace AutoClicker.Core.Interfaces;

/// <summary>
/// Interface for timer and scheduling functionality
/// </summary>
public interface ITimerService
{
    /// <summary>
    /// Gets the current time with optional server offset
    /// </summary>
    DateTime GetCurrentTime(bool useServerTime = false);
    
    /// <summary>
    /// Calculates time until scheduled start
    /// </summary>
    TimeSpan GetTimeUntilStart(DateTime scheduledTime, bool useServerTime = false);
    
    /// <summary>
    /// Schedules an action to run at a specific time
    /// </summary>
    Task ScheduleActionAsync(DateTime scheduledTime, Action action, bool useServerTime = false, CancellationToken cancellationToken = default);
}
