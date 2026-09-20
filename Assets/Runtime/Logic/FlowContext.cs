namespace ProjectCore.Architecture
{
    /// <summary>
    /// Where in the application flow a decision is being requested from. The
    /// strategy table in <see cref="ApplicationFlowLogic"/> maps
    /// (Context + UICloseReasons) → FlowIntent.
    /// </summary>
    public enum FlowContext
    {
        None      = 0,
        Boot      = 1,
        MainMenu  = 2,
        LevelFail = 3,
        Settings  = 4
    }
}
