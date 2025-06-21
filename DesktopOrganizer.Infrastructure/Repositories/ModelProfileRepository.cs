using DesktopOrganizer.Domain;
using System.Text.Json;

namespace DesktopOrganizer.Infrastructure.Repositories;

/// <summary>
/// File-based model profile repository implementation
/// </summary>
public class ModelProfileRepository : IModelProfileRepository
{
    private readonly string _modelsPath;

    public ModelProfileRepository()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appDataPath, "DesktopOrganizer");
        Directory.CreateDirectory(appFolder);
        _modelsPath = Path.Combine(appFolder, "models.json");
    }

    public async Task<List<ModelProfile>> LoadAllAsync()
    {
        if (!await ExistsAsync())
        {
            return await GetDefaultProfilesAsync();
        }

        try
        {
            var json = await File.ReadAllTextAsync(_modelsPath);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
            var profiles = JsonSerializer.Deserialize<List<ModelProfile>>(json, options);
            return profiles ?? await GetDefaultProfilesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading model profiles: {ex.Message}");
            return await GetDefaultProfilesAsync();
        }
    }

    public async Task SaveAllAsync(List<ModelProfile> profiles)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(profiles, options);
            await File.WriteAllTextAsync(_modelsPath, json);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to save model profiles: {ex.Message}", ex);
        }
    }

    public async Task<ModelProfile?> GetDefaultAsync()
    {
        var profiles = await LoadAllAsync();
        return profiles.FirstOrDefault(p => p.IsDefault) ?? profiles.FirstOrDefault();
    }

    public async Task SetDefaultAsync(string profileId)
    {
        var profiles = await LoadAllAsync();

        // Remove default from all profiles
        foreach (var profile in profiles)
        {
            profile.IsDefault = false;
        }

        // Set the specified profile as default
        var targetProfile = profiles.FirstOrDefault(p => p.Id == profileId);
        if (targetProfile != null)
        {
            targetProfile.IsDefault = true;
        }

        await SaveAllAsync(profiles);
    }

    public async Task<bool> ExistsAsync()
    {
        return await Task.FromResult(File.Exists(_modelsPath));
    }

    private async Task<List<ModelProfile>> GetDefaultProfilesAsync()
    {
        await Task.CompletedTask;

        return new List<ModelProfile>
        {
            new ModelProfile(
                name: "DeepSeek Chat",
                provider: "DeepSeek",
                baseUrl: "https://api.deepseek.com",
                modelId: "deepseek-chat",
                keyRef: "deepseek_main",
                timeoutSeconds: 60,
                isDefault: true
            ),
            new ModelProfile(
                name: "OpenAI GPT-4",
                provider: "OpenAI",
                baseUrl: "https://api.openai.com",
                modelId: "gpt-4",
                keyRef: "openai_main",
                timeoutSeconds: 60,
                isDefault: false
            ),
            new ModelProfile(
                name: "Claude 3 Sonnet",
                provider: "Anthropic",
                baseUrl: "https://api.anthropic.com",
                modelId: "claude-3-sonnet-20240229",
                keyRef: "anthropic_main",
                timeoutSeconds: 60,
                isDefault: false
            )
        };
    }
}