using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCore.Events
{
    public class GameEventRaiserOnEnable : MonoBehaviour
    {
        [SerializeField] private GameEvent GameEvent;

        private void OnEnable()
        {
            GameEvent.Invoke();
        }
    }
}

