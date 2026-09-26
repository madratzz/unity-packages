using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCore.StateMachine
{
    /// <summary>
    /// ScriptableObject finite state machine. The caller drives it by running
    /// <see cref="Tick"/> as a coroutine (typically via
    /// <c>CoroutineHandler</c> from <c>com.madratzz.utilities.coroutines</c>):
    ///
    /// <code>CoroutineHandler.StartStaticCoroutine(fsm.Tick());</code>
    ///
    /// The FSM does not run itself — it's a ScriptableObject and cannot host
    /// coroutines on its own. The caller is responsible for starting and
    /// stopping the loop.
    /// </summary>
    [CreateAssetMenu(fileName = "StateMachine", menuName = "ProjectCore/State Machine/Basic FSM")]
    public class FiniteStateMachine : ScriptableObject, IState
    {
        [SerializeField] private State BootState;

        [System.NonSerialized] protected State CurrentState;
        [System.NonSerialized] protected State PreviousState;
        [System.NonSerialized] protected Transition CurrentTransition;
        [System.NonSerialized] protected bool ResumePreviousState;

        [System.NonSerialized] private readonly Stack<State> PausedStates = new Stack<State>();

        /// <summary>The state currently executing.</summary>
        public State RunningState => CurrentState;

        /// <summary>True while a state is on the pause stack.</summary>
        public bool IsStatePaused(State state) => PausedStates.Contains(state);

        void IState.TransitionTo(State state, Transition transition)
        {
            Transition(transition);
        }

        IEnumerator IState.CleanupAllPausedStates(State state)
        {
            if (state != CurrentState)
                yield break;

            while (PausedStates.Count > 0)
            {
                yield return PausedStates.Pop().Cleanup();
            }
        }

        /// <summary>
        /// On the next Tick, exit the current state and resume the most recently
        /// paused one. No-op when nothing is paused.
        /// </summary>
        public void ShouldResumePreviousState()
        {
            if (PausedStates.Count == 0)
                return;

            ResumePreviousState = true;
        }

        /// <summary>
        /// Queues a transition to fire on the next Tick. Ignored if another
        /// transition is already queued or the supplied transition is null /
        /// has no ToState.
        /// </summary>
        public void Transition(Transition transition)
        {
            if (CurrentTransition != null)
                return;
            if (transition == null || transition.ToState == null)
                return;

            CurrentTransition = transition;
        }

        /// <summary>
        /// FSM tick loop. The caller drives it as a coroutine. Each frame the
        /// FSM (a) consumes any queued transition, or resumes a paused state if
        /// requested, then (b) yields once to the caller's coroutine before
        /// running the current state's Tick.
        ///
        /// Lifecycle order:
        ///   Boot: BootState.Init → BootState.Execute (once) → loop
        ///   Transition: CurrentState.Exit (or Pause) → Transition.Execute →
        ///               NewState.Init → NewState.Execute (once) → loop
        ///   Each frame: CurrentState.Tick → yield
        /// </summary>
        public IEnumerator Tick()
        {
            // Boot: only on first Tick when no state is current.
            if (CurrentState == null)
            {
                CurrentState = BootState;
                if (CurrentState == null)
                    yield break;

                yield return CurrentState.Init(this);
                yield return CurrentState.Execute();
            }

            while (true)
            {
                if (CurrentTransition != null && CurrentTransition.ToState != null)
                {
                    var nextState = CurrentTransition.ToState;

                    if (nextState.PausesPreviousState)
                    {
                        PausedStates.Push(CurrentState);
                        yield return CurrentState.Pause();
                    }
                    else
                    {
                        yield return CurrentState.Exit();
                    }

                    yield return CurrentTransition.Execute();

                    SetState(nextState);
                    yield return CurrentState.Init(this);
                    yield return CurrentState.Execute();

                    CurrentTransition = null;
                }
                else if (ResumePreviousState && PausedStates.Count > 0)
                {
                    var resumed = PausedStates.Pop();
                    if (resumed != null)
                    {
                        yield return CurrentState.Exit();
                        SetState(resumed);
                        yield return CurrentState.Resume();
                    }
                    ResumePreviousState = false;
                }

                if (CurrentState != null)
                {
                    yield return CurrentState.Tick();
                }

                yield return null;
            }
        }

        private void SetState(State state)
        {
            PreviousState = CurrentState;
            CurrentState = state;
        }
    }
}
