using System.Text.Json;
using AutoClicker.Core.Interfaces;
using AutoClicker.Core.Models;

namespace AutoClicker.Core.Services;

/// <summary>
/// Service for managing application configuration
/// </summary>
public class ConfigurationService : IConfigurationService
{
    private readonly string _configPath;
    private AppConfiguration _configuration;

    public ConfigurationService()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var autoClickerPath = Path.Combine(appDataPath, "AutoClicker");
        Directory.CreateDirectory(autoClickerPath);
        
        _configPath = Path.Combine(autoClickerPath, "config.json");
        _configuration = new AppConfiguration();
    }

    public async Task<AppConfiguration> LoadConfigurationAsync()
    {
        try
        {
            if (File.Exists(_configPath))
            {
                var json = await File.ReadAllTextAsync(_configPath);
                _configuration = JsonSerializer.Deserialize<AppConfiguration>(json) ?? new AppConfiguration();
            }
        }
        catch
        {
            _configuration = new AppConfiguration();
        }
        
        return _configuration;
    }

    public async Task SaveConfigurationAsync(AppConfiguration configuration)
    {
        _configuration = configuration;
        var json = JsonSerializer.Serialize(configuration, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_configPath, json);
    }

    public void SaveSequence(ClickSequence sequence)
    {
        var existingIndex = _configuration.SavedSequences.FindIndex(s => s.Name == sequence.Name);
        if (existingIndex >= 0)
        {
            _configuration.SavedSequences[existingIndex] = sequence;
        }
        else
        {
            _configuration.SavedSequences.Add(sequence);
        }
    }

    public List<ClickSequence> GetSavedSequences()
    {
        return _configuration.SavedSequences.ToList();
    }

    public AppConfiguration GetDefaultConfiguration()
    {
        return new AppConfiguration
        {
            Keybindings = new Keybindings
            {
                Record = "[",
                Start = "]",
                Stop = "\\",
                Clear = "Delete"
            },
            TimeOffsetHours = 0,
            DefaultDelayMs = 50,
            UseServerTime = false,
            SavedSequences = new List<ClickSequence>()
        };
    }
}
