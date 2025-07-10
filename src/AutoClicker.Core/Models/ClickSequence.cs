namespace AutoClicker.Core.Models;

/// <summary>
/// Represents a sequence of click positions with timing and execution settings
/// </summary>
public class ClickSequence
{
    public List<ClickPosition> Positions { get; set; } = new();
    public int DelayMilliseconds { get; set; }
    public bool IsLooping { get; set; }
    public DateTime? ScheduledStartTime { get; set; }
    public string Name { get; set; } = string.Empty;
}
