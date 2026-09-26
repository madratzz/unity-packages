using UnityEngine;

namespace ProjectCore.EngineExtensions
{
    public static class UnityExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.TryGetComponent<T>(out var component)
                ? component
                : gameObject.AddComponent<T>();
        }
    }
}
