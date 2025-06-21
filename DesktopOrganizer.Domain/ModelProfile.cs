namespace DesktopOrganizer.Domain;

/// <summary>
/// Represents an LLM provider configuration profile
/// </summary>
public class ModelProfile
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string Name { get; init; } = string.Empty;
    public string Provider { get; init; } = string.Empty; // DeepSeek, OpenAI, Anthropic
    public string BaseUrl { get; init; } = string.Empty;
    public string ModelId { get; init; } = string.Empty;
    public string KeyRef { get; init; } = string.Empty; // Reference to Credential Manager
    public int TimeoutSeconds { get; init; } = 60;
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.Now;

    public ModelProfile() { }

    public ModelProfile(string name, string provider, string baseUrl, string modelId,
                       string keyRef, int timeoutSeconds = 60, bool isDefault = false)
    {
        Name = name;
        Provider = provider;
        BaseUrl = baseUrl;
        ModelId = modelId;
        KeyRef = keyRef;
        TimeoutSeconds = timeoutSeconds;
        IsDefault = isDefault;
    }

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Name) &&
               !string.IsNullOrWhiteSpace(Provider) &&
               !string.IsNullOrWhiteSpace(BaseUrl) &&
               !string.IsNullOrWhiteSpace(ModelId) &&
               !string.IsNullOrWhiteSpace(KeyRef) &&
               TimeoutSeconds > 0;
    }

    public string GetCredentialTarget() => $"DesktopOrganizer_{KeyRef}";

    public override string ToString() => $"{Name} ({Provider})";
}