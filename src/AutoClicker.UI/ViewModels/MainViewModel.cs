using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoClicker.Core.Models;
using AutoClicker.Core.Interfaces;
using AutoClicker.UI.Commands;
using System.Runtime.InteropServices;
using System;
using System.Threading;
using System.Linq;

namespace AutoClicker.UI.ViewModels;

/// <summary>
/// Main ViewModel for the AutoClicker application
/// </summary>
public class MainViewModel : ViewModelBase
{
    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int X;
        public int Y;
    }

    private readonly IClickService _clickService;
    private readonly IHotkeyService _hotkeyService;
    private readonly ITimerService _timerService;
    private readonly IConfigurationService _configurationService;

    private int _delayMilliseconds = 50;
    private bool _isLooping;
    private bool _useServerTime;
    private DateTime? _scheduledStartTime;
    private string _status = "Ready";
    private bool _isRunning;
    private CancellationTokenSource? _cancellationTokenSource;

    public MainViewModel(IClickService clickService, IHotkeyService hotkeyService, ITimerService timerService, IConfigurationService configurationService)
    {
        _clickService = clickService;
        _hotkeyService = hotkeyService;
        _timerService = timerService;
        _configurationService = configurationService;
        
        Positions = new ObservableCollection<ClickPosition>();
        
        // Initialize commands
        RecordPositionCommand = new RelayCommand(ExecuteRecordPosition);
        StartSequenceCommand = new RelayCommand(ExecuteStartSequence, CanExecuteStartSequence);
        StopSequenceCommand = new RelayCommand(ExecuteStopSequence, CanExecuteStopSequence);
        ClearPositionsCommand = new RelayCommand(ExecuteClearPositions, CanExecuteClearPositions);
        RemovePositionCommand = new RelayCommand(ExecuteRemovePosition);
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

    public bool IsRunning
    {
        get => _isRunning;
        set => SetProperty(ref _isRunning, value);
    }

    // Commands
    public ICommand RecordPositionCommand { get; }
    public ICommand StartSequenceCommand { get; }
    public ICommand StopSequenceCommand { get; }
    public ICommand ClearPositionsCommand { get; }
    public ICommand RemovePositionCommand { get; }

    private void ExecuteRecordPosition(object? parameter)
    {
        if (GetCursorPos(out POINT point))
        {
            var position = new ClickPosition
            {
                X = point.X,
                Y = point.Y,
                Order = Positions.Count + 1,
                Label = $"Position {Positions.Count + 1}"
            };
            
            Positions.Add(position);
            Status = $"Recorded position at ({point.X}, {point.Y})";
        }
    }

    private async void ExecuteStartSequence(object? parameter)
    {
        if (Positions.Count == 0)
        {
            Status = "No positions recorded";
            return;
        }

        _cancellationTokenSource = new CancellationTokenSource();
        IsRunning = true;
        Status = "Running sequence...";

        var sequence = new ClickSequence
        {
            Positions = Positions.ToList(),
            DelayMilliseconds = DelayMilliseconds,
            IsLooping = IsLooping,
            ScheduledStartTime = ScheduledStartTime
        };

        try
        {
            await _clickService.ExecuteSequenceAsync(sequence, _cancellationTokenSource.Token);
            Status = "Sequence completed";
        }
        catch (OperationCanceledException)
        {
            Status = "Sequence stopped";
        }
        finally
        {
            IsRunning = false;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }

    private bool CanExecuteStartSequence(object? parameter)
    {
        return !IsRunning && Positions.Count > 0;
    }

    private void ExecuteStopSequence(object? parameter)
    {
        _cancellationTokenSource?.Cancel();
        Status = "Stopping sequence...";
    }

    private bool CanExecuteStopSequence(object? parameter)
    {
        return IsRunning;
    }

    private void ExecuteClearPositions(object? parameter)
    {
        Positions.Clear();
        Status = "All positions cleared";
    }

    private bool CanExecuteClearPositions(object? parameter)
    {
        return Positions.Count > 0 && !IsRunning;
    }

    private void ExecuteRemovePosition(object? parameter)
    {
        if (parameter is ClickPosition position)
        {
            Positions.Remove(position);
            
            // Reorder remaining positions
            int order = 1;
            foreach (var pos in Positions)
            {
                pos.Order = order++;
                pos.Label = $"Position {pos.Order}";
            }
            
            Status = $"Removed position";
        }
    }
}
