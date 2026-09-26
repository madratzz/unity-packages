using System.Collections.Generic;

namespace ProjectCore.Architecture
{
    /// <summary>
    /// Default <see cref="IFlowLogic"/> implementation. Holds a strategy table
    /// from (Context, UICloseReason) tuples to <see cref="FlowIntent"/> and
    /// returns <see cref="FlowIntent.DefaultToGame"/> when no strategy matches.
    ///
    /// Covers the four template screens — MainMenu, Gameplay, Settings and
    /// Store — plus the original Boot and LevelFail routes. Settings and Store
    /// are overlays: their State assets set PausesPreviousState, so
    /// <see cref="UICloseReasons.ResumeGame"/> maps to
    /// <see cref="FlowIntent.ResumePrevious"/> and drops back to whatever was
    /// underneath, while <see cref="UICloseReasons.Home"/> is the explicit
    /// route back to the menu.
    ///
    /// Any (context, reason) pair not listed falls through to
    /// <see cref="FlowIntent.DefaultToGame"/>. Subclass this class to add or
    /// override entries without forking the controller.
    /// </summary>
    public class ApplicationFlowLogic : IFlowLogic
    {
        private readonly Dictionary<(FlowContext, UICloseReasons), FlowIntent> _strategies;

        public ApplicationFlowLogic()
        {
            _strategies = new Dictionary<(FlowContext, UICloseReasons), FlowIntent>();
            InitializeStrategies();
        }

        private void InitializeStrategies()
        {
            Add(FlowContext.Boot,      UICloseReasons.Game, FlowIntent.GoToGame);
            Add(FlowContext.LevelFail, UICloseReasons.Game, FlowIntent.GoToGame);

            // Boot straight to the menu instead of the game. ApplicationFlowController.Boot()
            // still asks for (Boot, Game), so this only fires for a caller that asks for
            // (Boot, Home) explicitly — the FSM's own BootState is what decides the first
            // screen by default.
            Add(FlowContext.Boot, UICloseReasons.Home, FlowIntent.GoToMainMenu);

            // Main menu is the hub: it launches the game and opens the two overlays.
            Add(FlowContext.MainMenu, UICloseReasons.Game,     FlowIntent.GoToGame);
            Add(FlowContext.MainMenu, UICloseReasons.Settings, FlowIntent.OpenSettings);
            Add(FlowContext.MainMenu, UICloseReasons.Store,    FlowIntent.OpenStore);

            // Settings and Store are overlay states (PausesPreviousState on their State
            // assets), so closing them resumes whatever was underneath rather than
            // navigating somewhere new. Home is the explicit "back to menu" escape.
            Add(FlowContext.Settings, UICloseReasons.ResumeGame, FlowIntent.ResumePrevious);
            Add(FlowContext.Settings, UICloseReasons.Home,       FlowIntent.GoToMainMenu);
            Add(FlowContext.Store,    UICloseReasons.ResumeGame, FlowIntent.ResumePrevious);
            Add(FlowContext.Store,    UICloseReasons.Home,       FlowIntent.GoToMainMenu);

            // In-game routes out of gameplay.
            Add(FlowContext.Gameplay, UICloseReasons.Home,     FlowIntent.GoToMainMenu);
            Add(FlowContext.Gameplay, UICloseReasons.Settings, FlowIntent.OpenSettings);
            Add(FlowContext.Gameplay, UICloseReasons.Store,    FlowIntent.OpenStore);

            // Level-fail offers the store (buy a revive) and a way back to the menu.
            Add(FlowContext.LevelFail, UICloseReasons.Store, FlowIntent.OpenStore);
            Add(FlowContext.LevelFail, UICloseReasons.Home,  FlowIntent.GoToMainMenu);
        }

        /// <summary>
        /// Pure function: returns the intent mapped to the given (context,
        /// reason) pair, or <see cref="FlowIntent.DefaultToGame"/> when no
        /// mapping is registered.
        /// </summary>
        public FlowIntent GetDecision(FlowContext context, UICloseReasons reason)
        {
            return _strategies.GetValueOrDefault((context, reason), FlowIntent.DefaultToGame);
        }

        protected void Add(FlowContext ctx, UICloseReasons reason, FlowIntent intent)
        {
            _strategies[(ctx, reason)] = intent;
        }
    }
}
