using System;
using CustomUtilities.Attributes;
using UnityEngine;

namespace ProjectCore.Events
{

    [CreateAssetMenu(fileName = "e_", menuName = "ProjectCore/Events/Game Event - Basic")]
    [InlineEditor(Expanded = false)]
    public class GameEvent : ScriptableObject
    {
        public event Action Handler;

        [Button]
        public void Invoke()
        {
            Handler?.Invoke();
        }
    }
}
