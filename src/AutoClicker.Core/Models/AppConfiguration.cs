using System.Collections.Generic;

namespace AutoClicker.Core.Models
{
    /// <summary>
    /// Application configuration settings
    /// </summary>
    public class AppConfiguration
    {
        public List<ClickSequence> SavedSequences { get; set; } = new List<ClickSequence>();
        public Keybindings Keybindings { get; set; } = new Keybindings();
        public int TimeOffsetHours { get; set; } = 0;
        public int DefaultDelayMs { get; set; } = 50;
        public bool UseServerTime { get; set; } = false;
        public WindowSettings WindowSettings { get; set; } = new WindowSettings();
    }

    /// <summary>
    /// Keybinding configuration
    /// </summary>
    public class Keybindings
    {
        public string Record { get; set; } = "[";
        public string Start { get; set; } = "]";
        public string Stop { get; set; } = "\\";
        public string Clear { get; set; } = "Delete";
    }

    /// <summary>
    /// Window position and size settings
    /// </summary>
    public class WindowSettings
    {
        public double Left { get; set; } = 100;
        public double Top { get; set; } = 100;
        public double Width { get; set; } = 800;
        public double Height { get; set; } = 600;
    }
}
