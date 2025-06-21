using DesktopOrganizer.Domain;
using System.Text.Json;

namespace DesktopOrganizer.App.Services;

/// <summary>
/// Application service for undo operations
/// </summary>
public class UndoService : IUndoService
{
    private readonly string _undoFilePath;
    private readonly IExecutionService _executionService;

    public UndoService(IExecutionService executionService)
    {
        _executionService = executionService;
        var tempPath = Path.GetTempPath();
        _undoFilePath = Path.Combine(tempPath, "DesktopOrganizer_Undo.json");
    }

    public async Task<bool> SaveUndoInfoAsync(List<MoveOperation> operations)
    {
        try
        {
            var undoData = new UndoData
            {
                Timestamp = DateTime.Now,
                Operations = operations.Select(op => new UndoOperation
                {
                    OriginalPath = op.SourcePath ?? string.Empty,
                    CurrentPath = op.DestinationPath ?? string.Empty,
                    ItemName = op.Item,
                    TargetFolder = op.TargetFolder
                }).ToList()
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(undoData, options);
            await File.WriteAllTextAsync(_undoFilePath, json);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving undo info: {ex.Message}");
            return false;
        }
    }

    public async Task<List<MoveOperation>?> GetLastUndoInfoAsync()
    {
        try
        {
            if (!File.Exists(_undoFilePath))
                return null;

            var json = await File.ReadAllTextAsync(_undoFilePath);
            var undoData = JsonSerializer.Deserialize<UndoData>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (undoData?.Operations == null)
                return null;

            return undoData.Operations.Select(op => new MoveOperation(
                item: op.ItemName,
                targetFolder: Path.GetFileName(Path.GetDirectoryName(op.OriginalPath)) ?? string.Empty,
                sourcePath: op.CurrentPath, // Source and destination are swapped for undo
                destinationPath: op.OriginalPath
            )).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading undo info: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> ExecuteUndoAsync(IProgress<ExecutionProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var undoOperations = await GetLastUndoInfoAsync();
            if (undoOperations == null || !undoOperations.Any())
            {
                return false;
            }

            // Create a reverse plan
            var undoPlan = new Plan(
                newFolders: new List<string>(), // No new folders needed for undo
                moveOperations: undoOperations,
                modelUsed: "Undo Operation"
            );

            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var result = await _executionService.ExecutePlanAsync(undoPlan, desktopPath, progress, cancellationToken);

            if (result.Success)
            {
                // Clear undo info after successful undo
                await ClearUndoInfoAsync();
            }

            return result.Success;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing undo: {ex.Message}");
            return false;
        }
    }

    public async Task ClearUndoInfoAsync()
    {
        try
        {
            if (File.Exists(_undoFilePath))
            {
                await Task.Run(() => File.Delete(_undoFilePath));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error clearing undo info: {ex.Message}");
        }
    }

    public async Task<bool> HasUndoDataAsync()
    {
        try
        {
            if (!File.Exists(_undoFilePath))
                return false;

            var undoData = await GetLastUndoInfoAsync();
            return undoData?.Any() == true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<DateTime?> GetUndoTimestampAsync()
    {
        try
        {
            if (!File.Exists(_undoFilePath))
                return null;

            var json = await File.ReadAllTextAsync(_undoFilePath);
            var undoData = JsonSerializer.Deserialize<UndoData>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return undoData?.Timestamp;
        }
        catch
        {
            return null;
        }
    }
}

/// <summary>
/// Data structure for undo information storage
/// </summary>
internal class UndoData
{
    public DateTime Timestamp { get; set; }
    public List<UndoOperation> Operations { get; set; } = new();
}

/// <summary>
/// Individual undo operation data
/// </summary>
internal class UndoOperation
{
    public string OriginalPath { get; set; } = string.Empty;
    public string CurrentPath { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string TargetFolder { get; set; } = string.Empty;
}