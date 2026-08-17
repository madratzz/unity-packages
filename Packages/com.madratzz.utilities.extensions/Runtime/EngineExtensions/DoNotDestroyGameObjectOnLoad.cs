using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCore.EngineExtensions
{
    public class DoNotDestroyGameObjectOnLoad : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}