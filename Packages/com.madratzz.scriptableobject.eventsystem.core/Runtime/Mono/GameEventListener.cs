using CustomUtilities.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectCore.Events
{
    public class GameEventListener : MonoBehaviour
    {
        [RequireReference]
        [SerializeField] private GameEvent GameEvent;
        [SerializeField] private UnityEvent OnGameEventEvent;

        private void OnEnable()
        {
            if (GameEvent == null)
            {
                Debug.LogError("[GameEventListener] GameEvent is not assigned.", this);
                return;
            }

            GameEvent.Handler += OnEvent;
        }

        private void OnDisable()
        {
            if (GameEvent == null)
                return;

            GameEvent.Handler -= OnEvent;
        }

        private void OnEvent()
        {
            OnGameEventEvent.Invoke();
        }
    }
}
