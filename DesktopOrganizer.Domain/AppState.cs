namespace DesktopOrganizer.Domain;

/// <summary>
/// Represents the current state of the Desktop Organizer application
/// </summary>
public enum AppState
{
    /// <summary>
    /// Application is ready to accept user preferences
    /// </summary>
    Ready,

    /// <summary>
    /// AI is processing the organization plan
    /// </summary>
    Processing,

    /// <summary>
    /// Preview is ready, user can adjust and execute the plan
    /// </summary>
    PreviewReady,

    /// <summary>
    /// Executing the organization plan
    /// </summary>
    Executing,

    /// <summary>
    /// Organization completed, undo operation available
    /// </summary>
    Completed
}