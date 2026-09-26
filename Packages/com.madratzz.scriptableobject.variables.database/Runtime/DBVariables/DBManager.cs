using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCore.Variables
{
    public class DBManager
    {
        private static Database _database;
        private static Database Database
        {
            get
            {
                if (_database == null)
                {
                    _database = ScriptableObject.CreateInstance<Database>();
                }
                return _database;
            }
        }

        private static Dictionary<string, IDBVariable> _dbVariables = new Dictionary<string, IDBVariable>();

        public static void LoadJsonData(Dictionary<string, object> jsonDict)
        {
            try
            {
                foreach (var item in jsonDict)
                {
                    if (_dbVariables.ContainsKey(item.Key))
                    {
                        IDBVariable dBVariable = _dbVariables[item.Key];
                        dBVariable.Update(item.Value);
                    }
                    else
                    {
                        SaveUnknownVariable(item.Key, item.Value);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error saving data : " + e.Message);
            }
        }

        public static bool HasKey(IDBVariable dBVariable, string key)
        {
            TrackVariable(dBVariable, key);
            return Database.HasKey(key);
        }

        public static void SetInt(IDBVariable dBVariable, string key, int value)
        {
            TrackVariable(dBVariable, key);
            Database.SetInt(key, value);
        }

        public static int GetInt(IDBVariable dBVariable, string key)
        {
            TrackVariable(dBVariable, key);
            return Database.GetInt(key);
        }

        public static void SetFloat(IDBVariable dBVariable, string key, float value)
        {
            TrackVariable(dBVariable, key);
            Database.SetFloat(key, value);
        }

        public static float GetFloat(IDBVariable dBVariable, string key)
        {
            TrackVariable(dBVariable, key);
            return Database.GetFloat(key);
        }

        public static void SetBool(IDBVariable dBVariable, string key, bool value)
        {
            TrackVariable(dBVariable, key);
            Database.SetBool(key, value);
        }

        public static bool GetBool(IDBVariable dBVariable, string key)
        {
            TrackVariable(dBVariable, key);
            return Database.GetBool(key);
        }

        public static void SetString(IDBVariable dBVariable, string key, string value)
        {
            TrackVariable(dBVariable, key);
            Database.SetString(key, value);
        }

        public static string GetString(IDBVariable dBVariable, string key)
        {
            TrackVariable(dBVariable, key);
            return Database.GetString(key);
        }

        private static void TrackVariable(IDBVariable dBVariable, string key)
        {
            if (dBVariable != null && !_dbVariables.ContainsKey(key))
            {
                _dbVariables.Add(key, dBVariable);
            }
        }

        private static void SaveUnknownVariable(string key, object value)
        {
            if (value is int)
            {
                SetInt(null, key, (int)value);
            }
            else if (value is long)
            {
                SetInt(null, key, Convert.ToInt32(value));
            }
            else if (value is float)
            {
                SetFloat(null, key, (float)(value));
            }
            else if (value is double)
            {
                SetFloat(null, key, (float)((double)value));
            }
            else if (value is string)
            {
                SetString(null, key, (string)value);
            }
            else if (value is bool)
            {
                SetInt(null, key, ((bool)value) ? 1 : 0);
            }
            else
            {
                Debug.LogError("[ERR][DBM] Error saving data " + key);
            }
        }
    }
}