using CustomUtilities.Attributes;
using UnityEngine;

namespace ProjectCore.Events
{
    public class GameEventRaiser : MonoBehaviour
    {
        [RequireReference]
        [SerializeField] private GameEvent GameEvent;

        public void InvokeEvent()
        {
            if (GameEvent == null)
            {
                Debug.LogError("[GameEventRaiser] GameEvent is not assigned.", this);
                return;
            }

            GameEvent.Invoke();
        }
    }
}
