using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCore.Variables
{
    public class Database : ScriptableObject
    {
        public virtual bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public virtual void Save()
        {
            PlayerPrefs.Save();
        }

        public virtual void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            Save();
        }

        public virtual int GetInt(string key)
        {
            return PlayerPrefs.GetInt(key);
        }

        public virtual void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public virtual float GetFloat(string key)
        {
            return PlayerPrefs.GetFloat(key);
        }

        public virtual void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
            Save();
        }

        public virtual bool GetBool(string key)
        {
            return PlayerPrefs.GetInt(key) == 1;
        }

        public virtual void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public virtual string GetString(string key)
        {
            return PlayerPrefs.GetString(key);
        }
    }
}