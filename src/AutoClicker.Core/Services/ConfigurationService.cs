using System.Text.Json;
using AutoClicker.Core.Interfaces;
using AutoClicker.Core.Models;

namespace AutoClicker.Core.Services;

/// <summary>
/// Service for loading and saving application configuration
/// </summary>
public class ConfigurationService : IConfigurationService
{
    private readonly string _configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AutoClicker", "config.json");

    public async Task<AppConfiguration> LoadConfigurationAsync()
    {
        // TODO: Implement configuration loading from JSON file
        throw new NotImplementedException();
    }

    public async Task SaveConfigurationAsync(AppConfiguration configuration)
    {
        // TODO: Implement configuration saving to JSON file
        throw new NotImplementedException();
    }

    public AppConfiguration GetDefaultConfiguration()
    {
        return new AppConfiguration
        {
            Sequences = new List<ClickSequence>(),
            Keybindings = new Dictionary<string, string>
            {
                { "record", "[" },
                { "start", "]" },
                { "stop", "\\" },
                { "clear", "Delete" }
            },
            TimeOffset = 0,
            DefaultDelayMs = 50
        };
    }
}
