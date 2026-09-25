using CustomUtilities.Attributes;
using ProjectCore.Events;
using UnityEngine;

namespace ProjectCore.Architecture
{
    /// <summary>
    /// A screen. Lives on a UI prefab that <see cref="UIViewState"/> instantiates
    /// when the FSM enters that state, and destroys when it leaves.
    ///
    /// The view never navigates. It reports *why* it was dismissed by raising
    /// <see cref="ClosedEvent"/> with a <see cref="UICloseReasons"/> value, and
    /// <see cref="ApplicationFlowController"/> turns that (context, reason) pair
    /// into the next transition via the decision table. That keeps every screen
    /// ignorant of what follows it.
    ///
    /// Wire a Button to <see cref="Close"/> and set the int argument to the
    /// <see cref="UICloseReasons"/> you want (Game = 2, Settings = 3,
    /// ResumeGame = 4, Store = 7, Home = 1).
    /// </summary>
    public class UIView : MonoBehaviour
    {
        [Tooltip("Raised with a UICloseReasons value when this view is dismissed. The controller listens and resolves the next transition.")]
        [RequireReference]
        [SerializeField] private GameEventWithInt ClosedEvent;

        /// <summary>Re-show this view after it was hidden by an overlay.</summary>
        public void Show() => gameObject.SetActive(true);

        /// <summary>Hide without destroying — used when an overlay state pauses this one.</summary>
        public void Hide() => gameObject.SetActive(false);

        /// <summary>
        /// Report dismissal. Hook to a Button's onClick with the reason as the
        /// int argument. Takes an int rather than the enum so the Inspector can
        /// serialize the argument on a UnityEvent.
        /// </summary>
        public void Close(int reason)
        {
            if (ClosedEvent == null)
            {
                Debug.LogError($"[UIView] '{name}' has no ClosedEvent assigned — dismissal with reason {reason} goes nowhere.", this);
                return;
            }

            ClosedEvent.Invoke(reason);
        }

        /// <summary>Typed convenience for calls from code.</summary>
        public void Close(UICloseReasons reason) => Close((int)reason);
    }
}
