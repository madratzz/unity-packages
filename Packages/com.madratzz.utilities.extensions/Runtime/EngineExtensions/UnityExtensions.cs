using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCore.EngineExtensions
{
    public static class UnityExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                gameObject.AddComponent<T>();
            }
            return component;
        }
    }
}