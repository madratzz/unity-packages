using System.Collections;
using UnityEngine;

namespace ProjectCore.StateMachine
{
    /// <summary>
    /// ScriptableObject base class for state assets. Subclasses override the
    /// lifecycle methods to provide behaviour. All lifecycle methods are
    /// coroutines so they can yield (animation cues, input waits, async
    /// loading, etc.) without blocking the FSM's frame loop.
    /// </summary>
    public class State : ScriptableObject
    {
        [Tooltip("If true, transitioning to this state pushes the current state onto the FSM's pause stack instead of exiting it. The current state can be resumed via FSM.ShouldResumePreviousState().")]
        public bool PausesPreviousState;

        protected IState _Listener;

        public virtual IEnumerator Init(IState listener)
        {
            _Listener = listener;
            yield break;
        }

        public virtual IEnumerator Execute()
        {
            yield break;
        }

        public virtual IEnumerator Tick()
        {
            yield break;
        }

        public virtual IEnumerator Exit()
        {
            yield break;
        }

        public virtual IEnumerator Resume()
        {
            yield break;
        }

        public virtual IEnumerator Pause()
        {
            yield break;
        }

        public virtual IEnumerator Cleanup()
        {
            yield break;
        }
    }
}
