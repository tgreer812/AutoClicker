using AutoClicker.Core.Interfaces;

namespace AutoClicker.Core.Services;

/// <summary>
/// Service for managing global hotkeys
/// </summary>
public class HotkeyService : IHotkeyService
{
    public bool RegisterHotkey(string key, Action callback)
    {
        // TODO: Implement Windows API hotkey registration
        throw new NotImplementedException();
    }

    public bool UnregisterHotkey(string key)
    {
        // TODO: Implement hotkey unregistration
        throw new NotImplementedException();
    }

    public void UnregisterAllHotkeys()
    {
        // TODO: Implement cleanup of all hotkeys
        throw new NotImplementedException();
    }
}
