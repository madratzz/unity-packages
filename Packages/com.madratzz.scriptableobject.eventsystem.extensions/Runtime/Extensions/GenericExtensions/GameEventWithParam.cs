using CustomUtilities.Attributes;
using UnityEngine;

namespace ProjectCore.Events
{
    public delegate void GameEventHandlerWithParam<T>(T t);
    public delegate void GameEventHandlerWithParam<T,U,V>(T t, U u,V v);

    public class GameEventWithParam<T> : ScriptableObject
    {
        public event GameEventHandlerWithParam<T> Handler;
        
        [Button]
        public virtual void Invoke(T t)
        {
            Handler?.Invoke(t);
        }
    }
    
    
    public class GameEventWithParam<T,U,V> : ScriptableObject
    {
        public event GameEventHandlerWithParam<T,U,V> Handler;
        
        [Button]
        public virtual void Invoke(T t, U u,V v)
        {
            Handler?.Invoke(t,u,v);
        }
    }
}
