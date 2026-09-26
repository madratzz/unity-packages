using CustomUtilities.Attributes;
using UnityEngine;

namespace ProjectCore.Events
{
    public class GameEventRaiserOnEnable : MonoBehaviour
    {
        [RequireReference]
        [SerializeField] private GameEvent GameEvent;

        private void OnEnable()
        {
            if (GameEvent == null)
            {
                Debug.LogError("[GameEventRaiserOnEnable] GameEvent is not assigned.", this);
                return;
            }

            GameEvent.Invoke();
        }
    }
}

