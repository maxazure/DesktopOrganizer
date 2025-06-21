namespace DesktopOrganizer.Domain;

/// <summary>
/// Domain interfaces for repository and service contracts
/// </summary>
public interface IDesktopScanService
{
    Task<List<Item>> ScanDesktopAsync(string desktopPath);
    Task<List<Item>> ScanDirectoryAsync(string directoryPath, bool includeSubdirectories = false);
}

public interface IPlanPreviewService
{
    Task<Plan> ParseLLMResponseAsync(string llmResponse, List<Item> desktopItems);
    Task<bool> ValidatePlanAsync(Plan plan, List<Item> desktopItems);
    Task<Plan> AdjustPlanAsync(Plan originalPlan, List<MoveOperation> adjustments);
    Task<Dictionary<string, List<string>>> GetPreviewGroupsAsync(Plan plan);
}

public interface IExecutionService
{
    Task<ExecutionResult> ExecutePlanAsync(Plan plan, string desktopPath, IProgress<ExecutionProgress>? progress = null, CancellationToken cancellationToken = default);
    Task<bool> CanExecutePlanAsync(Plan plan, string desktopPath);
    Task<ExecutionResult> SimulatePlanAsync(Plan plan, string desktopPath);
}

public interface IUndoService
{
    Task<bool> SaveUndoInfoAsync(List<MoveOperation> operations);
    Task<List<MoveOperation>?> GetLastUndoInfoAsync();
    Task<bool> ExecuteUndoAsync(IProgress<ExecutionProgress>? progress = null, CancellationToken cancellationToken = default);
    Task ClearUndoInfoAsync();
    Task<bool> HasUndoDataAsync();
    Task<DateTime?> GetUndoTimestampAsync();
}

public interface IPreferencesRepository
{
    Task<Preferences> LoadAsync();
    Task SaveAsync(Preferences preferences);
    Task<bool> ExistsAsync();
    Task<Preferences> GetDefaultPreferencesAsync();
}

public interface IModelProfileRepository
{
    Task<List<ModelProfile>> LoadAllAsync();
    Task SaveAllAsync(List<ModelProfile> profiles);
    Task<ModelProfile?> GetDefaultAsync();
    Task SetDefaultAsync(string profileId);
    Task<bool> ExistsAsync();
}

public interface ICredentialService
{
    Task<string?> GetApiKeyAsync(string keyRef);
    Task<bool> SaveApiKeyAsync(string keyRef, string apiKey);
    Task<bool> DeleteApiKeyAsync(string keyRef);
    Task<bool> TestApiKeyAsync(string keyRef);
}

/// <summary>
/// Execution progress information
/// </summary>
public class ExecutionProgress
{
    public int TotalOperations { get; init; }
    public int CompletedOperations { get; init; }
    public string? CurrentOperation { get; init; }
    public string? Status { get; init; }
    public double PercentComplete => TotalOperations > 0 ? (double)CompletedOperations / TotalOperations * 100 : 0;

    public ExecutionProgress(int totalOperations, int completedOperations, string? currentOperation = null, string? status = null)
    {
        TotalOperations = totalOperations;
        CompletedOperations = completedOperations;
        CurrentOperation = currentOperation;
        Status = status;
    }
}

/// <summary>
/// Execution result information
/// </summary>
public class ExecutionResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public List<MoveOperation> CompletedOperations { get; init; } = new();
    public List<MoveOperation> FailedOperations { get; init; } = new();
    public TimeSpan Duration { get; init; }

    public ExecutionResult(bool success, string? errorMessage = null, TimeSpan duration = default)
    {
        Success = success;
        ErrorMessage = errorMessage;
        Duration = duration;
    }
}