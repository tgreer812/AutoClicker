namespace AutoClicker.Core.Models;

/// <summary>
/// Represents a single click position with coordinates and metadata
/// </summary>
public class ClickPosition
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Order { get; set; }
    public string Label { get; set; } = string.Empty;
}
