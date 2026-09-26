using System.Collections;
using UnityEngine;

namespace ProjectCore.StateMachine
{
    [CreateAssetMenu(fileName = "Transition", menuName = "ProjectCore/State Machine/Transitions/Basic Transition")]
    public class Transition : ScriptableObject
    {
        [Tooltip("State the FSM transitions into when this transition fires.")]
        public State ToState;

        public virtual IEnumerator Execute()
        {
            yield break;
        }
    }
}
