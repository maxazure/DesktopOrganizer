namespace DesktopOrganizer.Domain;

/// <summary>
/// Represents a file or folder on the desktop
/// </summary>
public class Item
{
    public string Name { get; init; } = string.Empty;
    public string FullPath { get; init; } = string.Empty;
    public string Extension { get; init; } = string.Empty;
    public long Size { get; init; }
    public DateTime ModifiedTime { get; init; }
    public bool IsDirectory { get; init; }
    public bool IsShortcut { get; init; }
    public string? Target { get; init; } // For shortcuts

    public Item() { }

    public Item(string name, string fullPath, string extension, long size, 
                DateTime modifiedTime, bool isDirectory, bool isShortcut = false, string? target = null)
    {
        Name = name;
        FullPath = fullPath;
        Extension = extension;
        Size = size;
        ModifiedTime = modifiedTime;
        IsDirectory = isDirectory;
        IsShortcut = isShortcut;
        Target = target;
    }

    public bool IsSystemIcon()
    {
        var systemIcons = new[] { "Recycle Bin", "This PC", "Network", "Control Panel" };
        return systemIcons.Any(icon => Name.Contains(icon, StringComparison.OrdinalIgnoreCase));
    }

    public override string ToString() => $"{Name} ({(IsDirectory ? "Folder" : Extension)})";
}