using UnityEngine;

namespace ProjectCore.Events
{
    public delegate T GameEventHandlerWithReturn<T>();

    public class GameEventWithReturn<T> : ScriptableObject
    {
        public event GameEventHandlerWithReturn<T> Handler;

        /// <summary>
        /// Raises the event and returns the handler's result. Returns
        /// <c>default(T)</c> when no handler is subscribed instead of throwing.
        /// </summary>
        public virtual T Raise()
        {
            return Handler != null ? Handler.Invoke() : default;
        }
    }
}
