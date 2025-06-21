namespace DesktopOrganizer.Domain;

/// <summary>
/// Represents an organization plan for desktop items
/// </summary>
public class Plan
{
    public List<string> NewFolders { get; init; } = new();
    public List<MoveOperation> MoveOperations { get; init; } = new();
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public string? ModelUsed { get; init; }
    
    // New folder structure for enhanced UI
    public List<PlanFolder>? Folders { get; set; } = new();

    public Plan() { }

    public Plan(List<string> newFolders, List<MoveOperation> moveOperations, string? modelUsed = null)
    {
        NewFolders = newFolders ?? new List<string>();
        MoveOperations = moveOperations ?? new List<MoveOperation>();
        ModelUsed = modelUsed;
    }

    public bool IsValid()
    {
        return MoveOperations.Any() &&
               MoveOperations.All(op => !string.IsNullOrWhiteSpace(op.Item) &&
                                       !string.IsNullOrWhiteSpace(op.TargetFolder));
    }

    public int TotalItemsToMove => MoveOperations.Count;

    public Dictionary<string, List<string>> GroupByFolder()
    {
        return MoveOperations
            .GroupBy(op => op.TargetFolder)
            .ToDictionary(g => g.Key, g => g.Select(op => op.Item).ToList());
    }

    public IEnumerable<MoveOperation> GetMoveOperations()
    {
        return MoveOperations ?? new List<MoveOperation>();
    }
}

/// <summary>
/// Represents a single file/folder move operation
/// </summary>
public class MoveOperation
{
    public string Item { get; init; } = string.Empty;
    public string TargetFolder { get; init; } = string.Empty;
    public string? SourcePath { get; set; }
    public string? DestinationPath { get; set; }

    public MoveOperation() { }

    public MoveOperation(string item, string targetFolder, string? sourcePath = null, string? destinationPath = null)
    {
        Item = item;
        TargetFolder = targetFolder;
        SourcePath = sourcePath;
        DestinationPath = destinationPath;
    }

    public override string ToString() => $"{Item} → {TargetFolder}";
}

/// <summary>
/// Represents a folder in the organization plan with detailed information
/// </summary>
public class PlanFolder
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<string>? Files { get; set; } = new();
    public bool KeepOnDesktop { get; set; } = false;
    public string? Path { get; set; }
}

/// <summary>
/// Represents a new folder to be created
/// </summary>
public class NewFolder
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string? Description { get; set; }
}