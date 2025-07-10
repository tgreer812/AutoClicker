using AutoClicker.Core.Models;

namespace AutoClicker.Core.Interfaces;

/// <summary>
/// Interface for configuration persistence
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// Loads configuration from storage
    /// </summary>
    Task<AppConfiguration> LoadConfigurationAsync();
    
    /// <summary>
    /// Saves configuration to storage
    /// </summary>
    Task SaveConfigurationAsync(AppConfiguration configuration);
    
    /// <summary>
    /// Gets the default configuration
    /// </summary>
    AppConfiguration GetDefaultConfiguration();
}
