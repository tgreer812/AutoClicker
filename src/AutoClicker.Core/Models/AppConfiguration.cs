namespace AutoClicker.Core.Models;

/// <summary>
/// Application configuration settings
/// </summary>
public class AppConfiguration
{
    public List<ClickSequence> Sequences { get; set; } = new();
    public Dictionary<string, string> Keybindings { get; set; } = new();
    public int TimeOffset { get; set; }
    public int DefaultDelayMs { get; set; } = 50;
    public WindowSettings? WindowSettings { get; set; }
}

/// <summary>
/// Window position and size settings
/// </summary>
public class WindowSettings
{
    public double Left { get; set; }
    public double Top { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
}
