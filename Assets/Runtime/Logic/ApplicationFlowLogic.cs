using System.Collections.Generic;

namespace ProjectCore.Architecture
{
    /// <summary>
    /// Default <see cref="IFlowLogic"/> implementation. Holds a strategy table
    /// from (Context, UICloseReason) tuples to <see cref="FlowIntent"/> and
    /// returns <see cref="FlowIntent.DefaultToGame"/> when no strategy matches.
    ///
    /// Default wiring matches the asteroids-demo reference game:
    ///   - <see cref="FlowContext.Boot"/> + <see cref="UICloseReasons.Game"/> → <see cref="FlowIntent.GoToGame"/>
    ///   - <see cref="FlowContext.LevelFail"/> + <see cref="UICloseReasons.Game"/> → <see cref="FlowIntent.GoToGame"/>
    ///
    /// Subclass or replace this class to add more contexts (e.g. MainMenu,
    /// Settings, LevelComplete) without forking the controller.
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
            Add(FlowContext.Boot,     UICloseReasons.Game, FlowIntent.GoToGame);
            Add(FlowContext.LevelFail, UICloseReasons.Game, FlowIntent.GoToGame);
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
