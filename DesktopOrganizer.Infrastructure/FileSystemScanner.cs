using DesktopOrganizer.Domain;

namespace DesktopOrganizer.Infrastructure;

/// <summary>
/// Implements desktop file system scanning functionality
/// </summary>
public class FileSystemScanner : IDesktopScanService
{
    public async Task<List<Item>> ScanDesktopAsync(string desktopPath)
    {
        return await ScanDirectoryAsync(desktopPath, false);
    }

    public async Task<List<Item>> ScanDirectoryAsync(string directoryPath, bool includeSubdirectories = false)
    {
        var items = new List<Item>();

        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");
        }

        await Task.Run(() =>
        {
            try
            {
                var searchOption = includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                var entries = Directory.EnumerateFileSystemEntries(directoryPath, "*", searchOption);

                foreach (var entry in entries)
                {
                    try
                    {
                        var item = CreateItemFromPath(entry);
                        if (item != null)
                        {
                            items.Add(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error but continue scanning
                        Console.WriteLine($"Error scanning {entry}: {ex.Message}");
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new InvalidOperationException($"Access denied to directory: {directoryPath}", ex);
            }
        });

        return items;
    }

    private static Item? CreateItemFromPath(string path)
    {
        try
        {
            var name = Path.GetFileName(path);
            var isDirectory = Directory.Exists(path);
            var extension = isDirectory ? string.Empty : Path.GetExtension(path);
            var size = 0L;
            var modifiedTime = DateTime.MinValue;
            var isShortcut = false;
            string? target = null;

            if (isDirectory)
            {
                var dirInfo = new DirectoryInfo(path);
                modifiedTime = dirInfo.LastWriteTime;
            }
            else
            {
                var fileInfo = new FileInfo(path);
                size = fileInfo.Length;
                modifiedTime = fileInfo.LastWriteTime;
                isShortcut = extension.Equals(".lnk", StringComparison.OrdinalIgnoreCase);

                if (isShortcut)
                {
                    target = GetShortcutTarget(path);
                }
            }

            return new Item(name, path, extension, size, modifiedTime, isDirectory, isShortcut, target);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static string? GetShortcutTarget(string shortcutPath)
    {
        try
        {
            // Simple implementation - for full functionality would need Windows Shell APIs
            // This is a basic placeholder that could be enhanced
            return shortcutPath;
        }
        catch
        {
            return null;
        }
    }
}