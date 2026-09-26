using System.Collections;

namespace ProjectCore.StateMachine
{
    /// <summary>
    /// Interface for external control of an FSM. Allows a non-FSM caller to
    /// request transitions and clean up paused-state chains without touching
    /// the FSM asset directly.
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Queues a transition to <paramref name="state"/> via
        /// <paramref name="transition"/>. The transition is consumed on the
        /// FSM's next Tick.
        /// </summary>
        void TransitionTo(State state, Transition transition);

        /// <summary>
        /// Coroutine that runs the cleanup lifecycle of every state currently
        /// in the pause stack, in reverse-push order. Paused states whose push
        /// occurred while <paramref name="state"/> was current are skipped —
        /// they belong to a different FSM frame. The FSM must be the
        /// <paramref name="state"/>'s current owner (i.e. you are clearing
        /// your own paused stack).
        /// </summary>
        IEnumerator CleanupAllPausedStates(State state);
    }
}
