using System.Runtime.InteropServices;
using AutoClicker.Core.Interfaces;
using AutoClicker.Core.Models;

namespace AutoClicker.Core.Services;

/// <summary>
/// Service for simulating mouse clicks
/// </summary>
public class ClickService : IClickService
{
    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

    private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const uint MOUSEEVENTF_LEFTUP = 0x0004;

    public async Task ClickAsync(int x, int y)
    {
        // Move cursor to position
        SetCursorPos(x, y);
        
        // Small delay to ensure cursor has moved
        await Task.Delay(10);
        
        // Simulate mouse down and up
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        await Task.Delay(10);
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
    }

    public async Task ExecuteSequenceAsync(ClickSequence sequence, CancellationToken cancellationToken)
    {
        do
        {
            foreach (var position in sequence.Positions.OrderBy(p => p.Order))
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                await ClickAsync(position.X, position.Y);
                
                if (cancellationToken.IsCancellationRequested)
                    return;

                await Task.Delay(sequence.DelayMilliseconds, cancellationToken);
            }
        } while (sequence.IsLooping && !cancellationToken.IsCancellationRequested);
    }
}
