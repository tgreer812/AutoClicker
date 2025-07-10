using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutoClicker.Core.Services;
using AutoClicker.UI.ViewModels;

namespace AutoClicker.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // Initialize services
        var clickService = new ClickService();
        var hotkeyService = new HotkeyService();
        var timerService = new TimerService();
        var configurationService = new ConfigurationService();
        
        // Set DataContext
        DataContext = new MainViewModel(clickService, hotkeyService, timerService, configurationService);
    }
}