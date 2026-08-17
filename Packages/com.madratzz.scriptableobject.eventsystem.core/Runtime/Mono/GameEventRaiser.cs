using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCore.Events
{
    public class GameEventRaiser : MonoBehaviour
    {
        [SerializeField] private GameEvent GameEvent;

        public void InvokeEvent()
        {
            GameEvent.Invoke();
        }
    }
}
