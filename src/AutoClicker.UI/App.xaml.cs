using System.Windows;
using AutoClicker.Core.Interfaces;
using AutoClicker.Core.Services;
using AutoClicker.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AutoClicker.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Configure services
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            // Create and show main window
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register core services
            services.AddSingleton<IClickService, ClickService>();
            services.AddSingleton<IHotkeyService, HotkeyService>();
            services.AddSingleton<ITimerService, TimerService>();
            services.AddSingleton<IConfigurationService, ConfigurationService>();

            // Register ViewModels
            services.AddTransient<MainWindowViewModel>();

            // Register Views
            services.AddTransient<MainWindow>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }
}

