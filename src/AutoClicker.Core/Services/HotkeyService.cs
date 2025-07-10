using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AutoClicker.Core.Interfaces;

namespace AutoClicker.Core.Services
{
    /// <summary>
    /// Service for managing global hotkeys
    /// </summary>
    public class HotkeyService : IHotkeyService, IDisposable
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        
        private LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;
        private readonly Dictionary<Keys, Action> _hotkeyActions = new Dictionary<Keys, Action>();
        private readonly Dictionary<string, Keys> _hotkeyMappings = new Dictionary<string, Keys>();

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public HotkeyService()
        {
            _proc = HookCallback;
        }

        public bool RegisterHotkey(string key, Action action)
        {
            try
            {
                Debug.WriteLine($"Registering hotkey: {key}");
                var keyCode = ParseKey(key);
                if (keyCode != Keys.None)
                {
                    _hotkeyActions[keyCode] = action;
                    _hotkeyMappings[key] = keyCode;
                    Debug.WriteLine($"Mapped key '{key}' to {keyCode}");
                    
                    // Start the hook if it's not already running
                    if (_hookID == IntPtr.Zero)
                    {
                        StartHook();
                        if (_hookID == IntPtr.Zero)
                        {
                            Debug.WriteLine("Failed to start keyboard hook");
                            return false;
                        }
                        Debug.WriteLine($"Keyboard hook started successfully: {_hookID}");
                    }
                    return true;
                }
                Debug.WriteLine($"Failed to parse key: {key}");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error registering hotkey: {ex.Message}");
                return false;
            }
        }

        public bool UnregisterHotkey(string key)
        {
            try
            {
                if (_hotkeyMappings.TryGetValue(key, out var keyCode))
                {
                    _hotkeyActions.Remove(keyCode);
                    _hotkeyMappings.Remove(key);
                    
                    // Stop the hook if no hotkeys are registered
                    if (_hotkeyActions.Count == 0 && _hookID != IntPtr.Zero)
                    {
                        StopHook();
                    }
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public void UnregisterAllHotkeys()
        {
            _hotkeyActions.Clear();
            _hotkeyMappings.Clear();
            StopHook();
        }

        private void StartHook()
        {
            _hookID = SetHook(_proc);
            if (_hookID == IntPtr.Zero)
            {
                var error = Marshal.GetLastWin32Error();
                Debug.WriteLine($"Failed to set hook. Error code: {error}");
            }
        }

        private void StopHook()
        {
            if (_hookID != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hookID);
                _hookID = IntPtr.Zero;
            }
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule? curModule = curProcess.MainModule)
            {
                IntPtr handle = IntPtr.Zero;
                
                if (curModule != null)
                {
                    handle = GetModuleHandle(curModule.ModuleName);
                }
                
                if (handle == IntPtr.Zero)
                {
                    // Try alternative method - pass null for current process
                    handle = Marshal.GetHINSTANCE(typeof(HotkeyService).Module);
                }
                
                var hookId = SetWindowsHookEx(WH_KEYBOARD_LL, proc, handle, 0);
                if (hookId == IntPtr.Zero)
                {
                    var error = Marshal.GetLastWin32Error();
                    Debug.WriteLine($"SetWindowsHookEx failed with error: {error}");
                }
                
                return hookId;
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;
                
                Debug.WriteLine($"Key pressed: {key} (vkCode: {vkCode})");
                
                if (_hotkeyActions.TryGetValue(key, out var action))
                {
                    Debug.WriteLine($"Executing action for key: {key}");
                    try
                    {
                        // Execute the action directly - let the UI layer handle threading
                        action?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error executing hotkey action: {ex.Message}");
                    }
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private Keys ParseKey(string key)
        {
            switch (key.ToUpper())
            {
                case "[":
                case "OEMBRACKETLEFT":
                    return Keys.OemOpenBrackets;
                case "]":
                case "OEMBRACKETRIGHT":
                    return Keys.OemCloseBrackets;
                case "\\":
                case "OEMBACKSLASH":
                    return Keys.OemBackslash;
                case "DELETE":
                case "DEL":
                    return Keys.Delete;
                case "SPACE":
                    return Keys.Space;
                case "ENTER":
                    return Keys.Enter;
                case "TAB":
                    return Keys.Tab;
                case "ESC":
                case "ESCAPE":
                    return Keys.Escape;
                default:
                    // Try to parse single character keys
                    if (key.Length == 1)
                    {
                        char c = char.ToUpper(key[0]);
                        if (c >= 'A' && c <= 'Z')
                        {
                            return (Keys)c;
                        }
                        if (c >= '0' && c <= '9')
                        {
                            return (Keys)c;
                        }
                    }
                    return Keys.None;
            }
        }

        public void Dispose()
        {
            UnregisterAllHotkeys();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}

