using DesktopOrganizer.Domain;
using System.Text.Json;

namespace DesktopOrganizer.Infrastructure.Repositories;

/// <summary>
/// File-based preferences repository implementation
/// </summary>
public class PreferencesRepository : IPreferencesRepository
{
    private readonly string _preferencesPath;

    public PreferencesRepository()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appDataPath, "DesktopOrganizer");
        Directory.CreateDirectory(appFolder);
        _preferencesPath = Path.Combine(appFolder, "preferences.json");
    }

    public async Task<Preferences> LoadAsync()
    {
        if (!await ExistsAsync())
        {
            return await GetDefaultPreferencesAsync();
        }

        try
        {
            var json = await File.ReadAllTextAsync(_preferencesPath);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
            var preferences = JsonSerializer.Deserialize<Preferences>(json, options);
            return preferences ?? await GetDefaultPreferencesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading preferences: {ex.Message}");
            return await GetDefaultPreferencesAsync();
        }
    }

    public async Task SaveAsync(Preferences preferences)
    {
        try
        {
            preferences.LastModified = DateTime.Now;

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(preferences, options);
            await File.WriteAllTextAsync(_preferencesPath, json);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to save preferences: {ex.Message}", ex);
        }
    }

    public async Task<bool> ExistsAsync()
    {
        return await Task.FromResult(File.Exists(_preferencesPath));
    }

    public async Task<Preferences> GetDefaultPreferencesAsync()
    {
        await Task.CompletedTask;

        var defaultFolderRules = new Dictionary<string, List<string>>
        {
            ["Documents"] = new() { ".pdf", ".doc", ".docx", ".txt", ".rtf", ".odt" },
            ["Pictures"] = new() { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg", ".ico" },
            ["Videos"] = new() { ".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv" },
            ["Music"] = new() { ".mp3", ".wav", ".flac", ".aac", ".ogg", ".wma" },
            ["Archives"] = new() { ".zip", ".rar", ".7z", ".tar", ".gz", ".bz2" },
            ["Executables"] = new() { ".exe", ".msi", ".deb", ".dmg", ".pkg" },
            ["Spreadsheets"] = new() { ".xls", ".xlsx", ".csv", ".ods" },
            ["Presentations"] = new() { ".ppt", ".pptx", ".odp" }
        };

        var defaultIgnoreExtensions = new List<string>
        {
            ".tmp", ".log", ".cache", ".bak"
        };

        return new Preferences(
            folderRules: defaultFolderRules,
            ignoreExtensions: defaultIgnoreExtensions,
            maxFileSizeBytes: 1024 * 1024 * 1024, // 1GB
            autoCreateFolders: true,
            confirmBeforeExecution: true,
            language: "en-US"
        );
    }
}