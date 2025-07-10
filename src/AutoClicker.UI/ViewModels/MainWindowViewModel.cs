using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AutoClicker.Core.Interfaces;
using AutoClicker.Core.Models;
using AutoClicker.UI.Commands;

namespace AutoClicker.UI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
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
        private AppConfiguration _configuration;
        private CancellationTokenSource? _cancellationTokenSource;

        private string _status = "Ready";
        private bool _isRunning;
        private int _delayMilliseconds = 50;
        private bool _isLooping;

        public ObservableCollection<ClickPosition> Positions { get; }
        public ICommand RecordPositionCommand { get; }
        public ICommand StartSequenceCommand { get; }
        public ICommand StopSequenceCommand { get; }
        public ICommand ClearPositionsCommand { get; }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged();
            }
        }

        public int DelayMilliseconds
        {
            get => _delayMilliseconds;
            set
            {
                _delayMilliseconds = value;
                OnPropertyChanged();
            }
        }

        public bool IsLooping
        {
            get => _isLooping;
            set
            {
                _isLooping = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel(IClickService clickService, IHotkeyService hotkeyService, 
            ITimerService timerService, IConfigurationService configurationService)
        {
            _clickService = clickService;
            _hotkeyService = hotkeyService;
            _timerService = timerService;
            _configurationService = configurationService;
            
            _configuration = _configurationService.GetDefaultConfiguration();
            
            RecordPositionCommand = new RelayCommand(_ => RecordPosition());
            StartSequenceCommand = new RelayCommand(_ => StartSequence(), _ => !IsRunning && Positions.Count > 0);
            StopSequenceCommand = new RelayCommand(_ => StopSequence(), _ => IsRunning);
            ClearPositionsCommand = new RelayCommand(_ => ClearPositions(), _ => !IsRunning && Positions.Count > 0);
            
            Positions = new ObservableCollection<ClickPosition>();
            
            // Load configuration and register hotkeys
            _ = LoadConfigurationAsync();
        }

        private async Task LoadConfigurationAsync()
        {
            _configuration = await _configurationService.LoadConfigurationAsync();
            RegisterHotkeys();
        }

        private void RegisterHotkeys()
        {
            _hotkeyService.UnregisterAllHotkeys();
            
            // Register hotkeys with UI thread dispatching
            _hotkeyService.RegisterHotkey(_configuration.Keybindings.Record, 
                () => Application.Current.Dispatcher.BeginInvoke(() => RecordPositionCommand.Execute(null)));
            
            _hotkeyService.RegisterHotkey(_configuration.Keybindings.Start, 
                () => Application.Current.Dispatcher.BeginInvoke(() => StartSequenceCommand.Execute(null)));
            
            _hotkeyService.RegisterHotkey(_configuration.Keybindings.Stop, 
                () => Application.Current.Dispatcher.BeginInvoke(() => StopSequenceCommand.Execute(null)));
            
            _hotkeyService.RegisterHotkey(_configuration.Keybindings.Clear, 
                () => Application.Current.Dispatcher.BeginInvoke(() => ClearPositionsCommand.Execute(null)));
        }

        private void RecordPosition()
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

        private async void StartSequence()
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
                Name = "Current Sequence",
                Positions = new List<ClickPosition>(Positions),
                DelayMilliseconds = DelayMilliseconds,
                IsLooping = IsLooping
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

        private void StopSequence()
        {
            _cancellationTokenSource?.Cancel();
            Status = "Stopping sequence...";
        }

        private void ClearPositions()
        {
            Positions.Clear();
            Status = "All positions cleared";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}