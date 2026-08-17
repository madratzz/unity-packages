namespace ProjectCore.Architecture
{
    /// <summary>
    /// A navigation or logic intent the application flow resolves to a
    /// specific <see cref="ApplicationFlowController"/> command. Numeric values
    /// are stable; never renumber existing entries.
    /// </summary>
    public enum FlowIntent
    {
        /// <summary>Default/uninitialized sentinel — never returned by a wired strategy.</summary>
        None = 0,

        /// <summary>Fallback when no strategy matches the (Context, Reason) tuple.</summary>
        DefaultToGame = 1,

        // Navigation (100 range)
        GoToGame       = 101,
        GoToLevelFail  = 102,
        OpenSettings   = 103,

        // Logic actions (200 range)
        ResumePrevious = 200
    }
}
