using DesktopOrganizer.Domain;

namespace DesktopOrganizer.App.Services;

/// <summary>
/// Application service for desktop scanning operations
/// </summary>
public class DesktopScanService
{
    private readonly IDesktopScanService _scanService;
    private readonly IPreferencesRepository _preferencesRepository;

    public DesktopScanService(IDesktopScanService scanService, IPreferencesRepository preferencesRepository)
    {
        _scanService = scanService;
        _preferencesRepository = preferencesRepository;
    }

    public async Task<List<Item>> ScanDesktopAsync()
    {
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        return await ScanDesktopAsync(desktopPath);
    }

    public async Task<List<Item>> ScanDesktopAsync(string desktopPath)
    {
        var allItems = await _scanService.ScanDesktopAsync(desktopPath);
        var preferences = await _preferencesRepository.LoadAsync();

        // Filter items based on preferences
        var filteredItems = allItems.Where(item => !preferences.ShouldIgnoreFile(item)).ToList();

        return filteredItems;
    }

    public async Task<string> GetDesktopJsonAsync()
    {
        var items = await ScanDesktopAsync();
        return await GetDesktopJsonAsync(items);
    }

    public async Task<string> GetDesktopJsonAsync(List<Item> items)
    {
        await Task.CompletedTask;

        var desktopData = new
        {
            scan_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            total_items = items.Count,
            files = items.Where(i => !i.IsDirectory).Select(i => new
            {
                name = i.Name,
                extension = i.Extension,
                size_bytes = i.Size,
                modified = i.ModifiedTime.ToString("yyyy-MM-dd HH:mm:ss"),
                is_shortcut = i.IsShortcut,
                target = i.Target
            }),
            folders = items.Where(i => i.IsDirectory).Select(i => new
            {
                name = i.Name,
                modified = i.ModifiedTime.ToString("yyyy-MM-dd HH:mm:ss")
            })
        };

        return System.Text.Json.JsonSerializer.Serialize(desktopData, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
    }

    public async Task<int> GetItemCountAsync()
    {
        var items = await ScanDesktopAsync();
        return items.Count;
    }

    public async Task<Dictionary<string, int>> GetItemStatisticsAsync()
    {
        var items = await ScanDesktopAsync();

        var stats = new Dictionary<string, int>
        {
            ["Total"] = items.Count,
            ["Files"] = items.Count(i => !i.IsDirectory),
            ["Folders"] = items.Count(i => i.IsDirectory),
            ["Shortcuts"] = items.Count(i => i.IsShortcut)
        };

        // Group by extension
        var extensionGroups = items
            .Where(i => !i.IsDirectory && !string.IsNullOrEmpty(i.Extension))
            .GroupBy(i => i.Extension.ToLowerInvariant())
            .OrderByDescending(g => g.Count())
            .Take(5);

        foreach (var group in extensionGroups)
        {
            stats[group.Key] = group.Count();
        }

        return stats;
    }
}