using UnityEngine;
using UnityEngine.Events;

namespace ProjectCore.Events
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEvent GameEvent;
        [SerializeField] private UnityEvent OnGameEventEvent;

        private void OnEnable()
        {
            GameEvent.Handler += OnEvent;
        }

        private void OnDisable()
        {
            GameEvent.Handler -= OnEvent;
        }

        private void OnEvent()
        {
            OnGameEventEvent.Invoke();
        }
    }
}
