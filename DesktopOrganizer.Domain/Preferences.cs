namespace DesktopOrganizer.Domain;

/// <summary>
/// Represents user organization preferences
/// </summary>
public class Preferences
{
    public Dictionary<string, List<string>> FolderRules { get; init; } = new();
    public List<string> IgnoreExtensions { get; init; } = new();
    public long MaxFileSizeBytes { get; init; } = 1024 * 1024 * 1024; // 1GB default
    public bool AutoCreateFolders { get; init; } = true;
    public bool ConfirmBeforeExecution { get; init; } = true;
    public string Language { get; init; } = "en-US";
    
    // New natural language preference field
    public string NaturalLanguagePreference { get; set; } = string.Empty;
    public string LastUsedTemplate { get; set; } = string.Empty;
    
    public DateTime LastModified { get; set; } = DateTime.Now;

    public Preferences() { }

    public Preferences(Dictionary<string, List<string>>? folderRules = null,
                      List<string>? ignoreExtensions = null,
                      long maxFileSizeBytes = 1024 * 1024 * 1024,
                      bool autoCreateFolders = true,
                      bool confirmBeforeExecution = true,
                      string language = "en-US")
    {
        FolderRules = folderRules ?? new Dictionary<string, List<string>>();
        IgnoreExtensions = ignoreExtensions ?? new List<string>();
        MaxFileSizeBytes = maxFileSizeBytes;
        AutoCreateFolders = autoCreateFolders;
        ConfirmBeforeExecution = confirmBeforeExecution;
        Language = language;
    }

    public void AddFolderRule(string folderName, List<string> extensions)
    {
        FolderRules[folderName] = extensions;
        LastModified = DateTime.Now;
    }

    public void RemoveFolderRule(string folderName)
    {
        FolderRules.Remove(folderName);
        LastModified = DateTime.Now;
    }

    public bool ShouldIgnoreFile(string extension)
    {
        return IgnoreExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
    }

    public bool ShouldIgnoreFile(Item item)
    {
        return item.Size > MaxFileSizeBytes ||
               ShouldIgnoreFile(item.Extension) ||
               item.IsSystemIcon();
    }

    public string ToJsonString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}