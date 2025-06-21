namespace DesktopOrganizer.Domain;

/// <summary>
/// Represents a preference template for natural language organization preferences
/// </summary>
public class PreferenceTemplate
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsBuiltIn { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime LastModified { get; set; } = DateTime.Now;

    public PreferenceTemplate() { }

    public PreferenceTemplate(string name, string description, string content, bool isBuiltIn = false)
    {
        Name = name;
        Description = description;
        Content = content;
        IsBuiltIn = isBuiltIn;
    }

    public string ToJsonString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}