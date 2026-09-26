#if ODIN_INSPECTOR

using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ProjectCore.Variables
{
    public abstract class RuntimeDictionary<TKey, TValue> : SerializedScriptableObject
    {
        public Dictionary<TKey,TValue> KeyValuePairs;

        public void Clear()
        {
            KeyValuePairs.Clear();
        }

        public bool Add(TKey key, TValue value)
        {
            if (KeyValuePairs.ContainsKey(key))
            {
                return false;
            }
            else
            {
                KeyValuePairs.Add(key, value);
                return true;
            }
        }

        public void AddOrUpdate(TKey key, TValue value)
        {
            if (KeyValuePairs.ContainsKey(key))
            {
                KeyValuePairs[key] = value;
            }
            else
            {
                Add(key, value);
            }
        }

        public bool Remove(TKey key)
        {
            if (KeyValuePairs.ContainsKey(key))
            {
                KeyValuePairs.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public TValue this [TKey key]
        {
            get
            {
                return KeyValuePairs[key];
            }
        }

        private void OnEnable()
        {
            KeyValuePairs = new Dictionary<TKey, TValue>();
        }
    }
}
#endif
