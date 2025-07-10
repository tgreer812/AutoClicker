namespace AutoClicker.Core.Interfaces;

/// <summary>
/// Interface for mouse click simulation functionality
/// </summary>
public interface IClickService
{
    /// <summary>
    /// Simulates a mouse click at the specified coordinates
    /// </summary>
    Task ClickAsync(int x, int y);
    
    /// <summary>
    /// Executes a sequence of clicks with specified delays
    /// </summary>
    Task ExecuteSequenceAsync(Models.ClickSequence sequence, CancellationToken cancellationToken);
}
