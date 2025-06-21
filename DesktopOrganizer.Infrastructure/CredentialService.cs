using CredentialManagement;
using DesktopOrganizer.Domain;

namespace DesktopOrganizer.Infrastructure;

/// <summary>
/// Windows Credential Manager implementation for API key storage
/// </summary>
public class CredentialService : ICredentialService
{
    private const string CredentialPrefix = "DesktopOrganizer_";

    public async Task<string?> GetApiKeyAsync(string keyRef)
    {
        return await Task.Run(() =>
        {
            try
            {
                var credential = new Credential
                {
                    Target = GetCredentialTarget(keyRef),
                    Type = CredentialType.Generic
                };

                if (credential.Load())
                {
                    return credential.Password;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving API key for {keyRef}: {ex.Message}");
                return null;
            }
        });
    }

    public async Task<bool> SaveApiKeyAsync(string keyRef, string apiKey)
    {
        return await Task.Run(() =>
        {
            try
            {
                var credential = new Credential
                {
                    Target = GetCredentialTarget(keyRef),
                    Username = "DesktopOrganizer",
                    Password = apiKey,
                    Type = CredentialType.Generic,
                    PersistanceType = PersistanceType.LocalComputer
                };

                return credential.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving API key for {keyRef}: {ex.Message}");
                return false;
            }
        });
    }

    public async Task<bool> DeleteApiKeyAsync(string keyRef)
    {
        return await Task.Run(() =>
        {
            try
            {
                var credential = new Credential
                {
                    Target = GetCredentialTarget(keyRef),
                    Type = CredentialType.Generic
                };

                return credential.Delete();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting API key for {keyRef}: {ex.Message}");
                return false;
            }
        });
    }

    public async Task<bool> TestApiKeyAsync(string keyRef)
    {
        var apiKey = await GetApiKeyAsync(keyRef);
        return !string.IsNullOrWhiteSpace(apiKey);
    }

    private static string GetCredentialTarget(string keyRef)
    {
        return $"{CredentialPrefix}{keyRef}";
    }
}