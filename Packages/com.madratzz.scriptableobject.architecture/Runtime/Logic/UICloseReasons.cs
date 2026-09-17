namespace ProjectCore.Architecture
{
    /// <summary>
    /// Reasons a UI view was closed. <see cref="ApplicationFlowLogic"/> uses
    /// these as the second axis of its (Context, Reason) → Intent strategy
    /// table. Values are stable; extend by adding new entries.
    /// </summary>
    public enum UICloseReasons
    {
        None        = 0,
        Home        = 1,
        Game        = 2,
        Settings    = 3,
        ResumeGame  = 4,
        Revive      = 5,
        SkipLevel   = 6
    }
}
