using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoClicker.Core.Models;
using AutoClicker.Core.Interfaces;

namespace AutoClicker.UI.ViewModels;

/// <summary>
/// Main ViewModel for the AutoClicker application
/// </summary>
public class MainViewModel : ViewModelBase
{
    private readonly IClickService _clickService;
    private readonly IHotkeyService _hotkeyService;
    private readonly ITimerService _timerService;
    private readonly IConfigurationService _configurationService;

    private int _delayMilliseconds = 50;
    private bool _isLooping;
    private bool _useServerTime;
    private DateTime? _scheduledStartTime;
    private string _status = "Ready";

    public MainViewModel(IClickService clickService, IHotkeyService hotkeyService, ITimerService timerService, IConfigurationService configurationService)
    {
        _clickService = clickService;
        _hotkeyService = hotkeyService;
        _timerService = timerService;
        _configurationService = configurationService;
        
        Positions = new ObservableCollection<ClickPosition>();
        
        // TODO: Initialize commands and load configuration
    }

    public ObservableCollection<ClickPosition> Positions { get; }

    public int DelayMilliseconds
    {
        get => _delayMilliseconds;
        set => SetProperty(ref _delayMilliseconds, value);
    }

    public bool IsLooping
    {
        get => _isLooping;
        set => SetProperty(ref _isLooping, value);
    }

    public bool UseServerTime
    {
        get => _useServerTime;
        set => SetProperty(ref _useServerTime, value);
    }

    public DateTime? ScheduledStartTime
    {
        get => _scheduledStartTime;
        set => SetProperty(ref _scheduledStartTime, value);
    }

    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    // TODO: Implement ICommand properties for UI binding
    // public ICommand RecordPositionCommand { get; }
    // public ICommand StartSequenceCommand { get; }
    // public ICommand StopSequenceCommand { get; }
    // public ICommand ClearPositionsCommand { get; }
}
