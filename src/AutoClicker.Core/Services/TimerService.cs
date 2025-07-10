using AutoClicker.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoClicker.Core.Services
{
    /// <summary>
    /// Service for managing timing and scheduling
    /// </summary>
    public class TimerService : ITimerService
    {
        private System.Timers.Timer? _timer;
        private System.Timers.Timer? _countdownTimer;
        private DateTime? _scheduledTime;
        private Action? _callback;
        private int _timeOffsetHours;

        public event EventHandler<TimeSpan>? CountdownUpdated;

        public void ScheduleExecution(DateTime scheduledTime, Action callback)
        {
            _scheduledTime = scheduledTime;
            _callback = callback;

            var delay = scheduledTime - DateTime.Now;
            if (delay.TotalMilliseconds > 0)
            {
                _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                _timer.Elapsed += OnTimerElapsed;
                _timer.AutoReset = false;
                _timer.Start();

                // Start countdown updates
                _countdownTimer = new System.Timers.Timer(100); // Update every 100ms
                _countdownTimer.Elapsed += (s, e) =>
                {
                    var remaining = scheduledTime - DateTime.Now;
                    if (remaining.TotalMilliseconds <= 0)
                    {
                        _countdownTimer.Stop();
                        _countdownTimer.Dispose();
                        _countdownTimer = null;
                    }
                    else
                    {
                        CountdownUpdated?.Invoke(this, remaining);
                    }
                };
                _countdownTimer.Start();
            }
            else
            {
                // Execute immediately if scheduled time has passed
                callback?.Invoke();
            }
        }

        public async Task ScheduleActionAsync(DateTime scheduledTime, Action action, bool useServerTime, CancellationToken cancellationToken)
        {
            var targetTime = useServerTime ? ConvertToLocalTime(scheduledTime, _timeOffsetHours) : scheduledTime;
            var delay = targetTime - DateTime.Now;
            
            if (delay.TotalMilliseconds > 0)
            {
                try
                {
                    await Task.Delay(delay, cancellationToken);
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        action?.Invoke();
                    }
                }
                catch (TaskCanceledException)
                {
                    // Task was cancelled, no action needed
                }
            }
            else
            {
                // Execute immediately if time has passed
                action?.Invoke();
            }
        }

        public void CancelScheduledExecution()
        {
            _timer?.Stop();
            _timer?.Dispose();
            _timer = null;
            
            _countdownTimer?.Stop();
            _countdownTimer?.Dispose();
            _countdownTimer = null;
            
            _scheduledTime = null;
            _callback = null;
        }

        public DateTime ConvertToServerTime(DateTime localTime, int offsetHours)
        {
            _timeOffsetHours = offsetHours;
            return localTime.AddHours(offsetHours - DateTimeOffset.Now.Offset.Hours);
        }

        public DateTime ConvertToLocalTime(DateTime serverTime, int offsetHours)
        {
            _timeOffsetHours = offsetHours;
            return serverTime.AddHours(DateTimeOffset.Now.Offset.Hours - offsetHours);
        }

        public TimeSpan GetTimeUntilStart(DateTime scheduledTime, bool useServerTime)
        {
            var targetTime = useServerTime ? ConvertToLocalTime(scheduledTime, _timeOffsetHours) : scheduledTime;
            var timeUntilStart = targetTime - DateTime.Now;
            return timeUntilStart.TotalMilliseconds > 0 ? timeUntilStart : TimeSpan.Zero;
        }

        public DateTime GetCurrentTime(bool useServerTime)
        {
            return useServerTime ? ConvertToServerTime(DateTime.Now, _timeOffsetHours) : DateTime.Now;
        }

        private void OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            _callback?.Invoke();
            _timer?.Dispose();
            _timer = null;
        }
    }
}
