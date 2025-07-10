using AutoClicker.Core.Interfaces;
using AutoClicker.Core.Models;

namespace AutoClicker.Core.Services;

/// <summary>
/// Service for simulating mouse clicks
/// </summary>
public class ClickService : IClickService
{
    public Task ClickAsync(int x, int y)
    {
        // TODO: Implement Windows API SendInput for mouse clicking
        throw new NotImplementedException();
    }

    public Task ExecuteSequenceAsync(ClickSequence sequence, CancellationToken cancellationToken)
    {
        // TODO: Implement sequence execution with delays and randomization
        throw new NotImplementedException();
    }
}
