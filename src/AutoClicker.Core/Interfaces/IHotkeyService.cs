namespace AutoClicker.Core.Interfaces;

/// <summary>
/// Interface for global hotkey management
/// </summary>
public interface IHotkeyService
{
    /// <summary>
    /// Registers a global hotkey
    /// </summary>
    bool RegisterHotkey(string key, Action callback);
    
    /// <summary>
    /// Unregisters a global hotkey
    /// </summary>
    bool UnregisterHotkey(string key);
    
    /// <summary>
    /// Unregisters all hotkeys
    /// </summary>
    void UnregisterAllHotkeys();
}
