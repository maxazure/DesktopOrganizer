using DesktopOrganizer.Domain;

namespace DesktopOrganizer.Infrastructure;

/// <summary>
/// Implements file organization execution functionality
/// </summary>
public class ExecutionEngine : IExecutionService
{
    public async Task<ExecutionResult> ExecutePlanAsync(Plan plan, string desktopPath,
        IProgress<ExecutionProgress>? progress = null, CancellationToken cancellationToken = default)
    {
        var startTime = DateTime.Now;
        var completedOperations = new List<MoveOperation>();
        var failedOperations = new List<MoveOperation>();

        try
        {
            // Create folders first
            await CreateFoldersAsync(plan.NewFolders, desktopPath, cancellationToken);

            // Execute move operations
            for (int i = 0; i < plan.MoveOperations.Count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var operation = plan.MoveOperations[i];
                progress?.Report(new ExecutionProgress(
                    plan.MoveOperations.Count,
                    i,
                    operation.Item,
                    "Moving files..."));

                try
                {
                    await ExecuteMoveOperationAsync(operation, desktopPath);
                    completedOperations.Add(operation);
                }
                catch (Exception ex)
                {
                    failedOperations.Add(operation);
                    Console.WriteLine($"Failed to move {operation.Item}: {ex.Message}");
                }
            }

            progress?.Report(new ExecutionProgress(
                plan.MoveOperations.Count,
                plan.MoveOperations.Count,
                null,
                "Complete"));

            var duration = DateTime.Now - startTime;
            return new ExecutionResult(
                failedOperations.Count == 0,
                failedOperations.Any() ? $"Failed to move {failedOperations.Count} items" : null,
                duration)
            {
                CompletedOperations = completedOperations,
                FailedOperations = failedOperations
            };
        }
        catch (OperationCanceledException)
        {
            // Rollback completed operations
            await RollbackOperationsAsync(completedOperations);
            throw;
        }
        catch (Exception ex)
        {
            await RollbackOperationsAsync(completedOperations);
            var duration = DateTime.Now - startTime;
            return new ExecutionResult(false, ex.Message, duration)
            {
                CompletedOperations = completedOperations,
                FailedOperations = failedOperations
            };
        }
    }

    public async Task<bool> CanExecutePlanAsync(Plan plan, string desktopPath)
    {
        return await Task.Run(() =>
        {
            if (!Directory.Exists(desktopPath))
                return false;

            foreach (var operation in plan.MoveOperations)
            {
                var sourcePath = Path.Combine(desktopPath, operation.Item);
                if (!File.Exists(sourcePath) && !Directory.Exists(sourcePath))
                    return false;
            }

            return true;
        });
    }

    public async Task<ExecutionResult> SimulatePlanAsync(Plan plan, string desktopPath)
    {
        return await Task.Run(() =>
        {
            var errors = new List<string>();

            foreach (var operation in plan.MoveOperations)
            {
                var sourcePath = Path.Combine(desktopPath, operation.Item);
                var targetFolderPath = Path.Combine(desktopPath, operation.TargetFolder);
                var destinationPath = Path.Combine(targetFolderPath, operation.Item);

                if (!File.Exists(sourcePath) && !Directory.Exists(sourcePath))
                {
                    errors.Add($"Source not found: {operation.Item}");
                }

                if (File.Exists(destinationPath) || Directory.Exists(destinationPath))
                {
                    errors.Add($"Destination already exists: {destinationPath}");
                }
            }

            return new ExecutionResult(
                errors.Count == 0,
                errors.Any() ? string.Join("; ", errors) : null);
        });
    }

    private static async Task CreateFoldersAsync(List<string> folderNames, string desktopPath,
        CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            foreach (var folderName in folderNames)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var folderPath = Path.Combine(desktopPath, folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
            }
        }, cancellationToken);
    }

    private static async Task ExecuteMoveOperationAsync(MoveOperation operation, string desktopPath)
    {
        await Task.Run(() =>
        {
            var sourcePath = Path.Combine(desktopPath, operation.Item);
            var targetFolderPath = Path.Combine(desktopPath, operation.TargetFolder);
            var destinationPath = Path.Combine(targetFolderPath, operation.Item);

            // Ensure target folder exists
            if (!Directory.Exists(targetFolderPath))
            {
                Directory.CreateDirectory(targetFolderPath);
            }

            // Handle name conflicts
            destinationPath = GetUniqueDestinationPath(destinationPath);

            // Update operation with actual paths
            operation.SourcePath = sourcePath;
            operation.DestinationPath = destinationPath;

            try
            {
                // Move file or directory
                if (File.Exists(sourcePath))
                {
                    File.Move(sourcePath, destinationPath);
                }
                else if (Directory.Exists(sourcePath))
                {
                    Directory.Move(sourcePath, destinationPath);
                }
                else
                {
                    throw new FileNotFoundException($"Source not found: {sourcePath}");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to execute operation - {sourcePath} -> {destinationPath}: {ex.Message}", ex);
            }
        });
    }

    private static string GetUniqueDestinationPath(string originalPath)
    {
        if (!File.Exists(originalPath) && !Directory.Exists(originalPath))
            return originalPath;

        var directory = Path.GetDirectoryName(originalPath) ?? string.Empty;
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(originalPath);
        var extension = Path.GetExtension(originalPath);

        var counter = 1;
        string newPath;
        do
        {
            var newName = $"{nameWithoutExtension} ({counter}){extension}";
            newPath = Path.Combine(directory, newName);
            counter++;
        }
        while (File.Exists(newPath) || Directory.Exists(newPath));

        return newPath;
    }

    private static async Task RollbackOperationsAsync(List<MoveOperation> operations)
    {
        await Task.Run(() =>
        {
            // Reverse the order for rollback
            foreach (var operation in operations.AsEnumerable().Reverse())
            {
                try
                {
                    if (!string.IsNullOrEmpty(operation.SourcePath) &&
                        !string.IsNullOrEmpty(operation.DestinationPath))
                    {
                        if (File.Exists(operation.DestinationPath))
                        {
                            File.Move(operation.DestinationPath, operation.SourcePath);
                        }
                        else if (Directory.Exists(operation.DestinationPath))
                        {
                            Directory.Move(operation.DestinationPath, operation.SourcePath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Rollback failed for {operation.Item}: {ex.Message}");
                }
            }
        });
    }
}