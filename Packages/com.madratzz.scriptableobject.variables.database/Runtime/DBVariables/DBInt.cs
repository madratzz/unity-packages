using System;
using System.Globalization;
using CustomUtilities.Attributes;
using UnityEngine;


namespace ProjectCore.Variables
{
    [CreateAssetMenu(fileName = "v_", menuName = "ProjectCore/Variables/Int Persistent")]
    public class DBInt : Int, IDBVariable
    {
        [SerializeField] protected string Key;
        
        public string PlayerPrefsKey => Key;

        public void Refresh()
        {
            Load();
        }

        private void OnEnable()
        {
            Load();
        }

        public void SetKey(string key)
        {
            Key = key;
        }

        public string GetKey()
        {
            return Key;
        }

        public override void SetValue(int value)
        {
            base.SetValue(value);
            Save();
        }

        public override void SetValue(Int value)
        {
            base.SetValue(value);
            Save();
        }

        public override void ApplyChange(int amount)
        {
            base.ApplyChange(amount);
            Save();
        }

        public override void ApplyChange(Int amount)
        {
            base.ApplyChange(amount);
            Save();
        }
        
        [Button]
        public virtual void Save()
        {
            DBManager.SetInt(this, Key, Value);
        }
        
        [Button]
        public virtual void Load()
        {
            if (!string.IsNullOrEmpty(Key) && DBManager.HasKey(this, Key))
            {
                Value = DBManager.GetInt(this, Key);
            }
            else
            {
                if (ResetToDefaultOnPlay)
                {
                    Value = DefaultValue;
                }
                else
                {
                    Value = 0;
                }
            }
        }

        void IDBVariable.Update(object value)
        {
            int integer = Value;
            if (value is int)
            {
                integer = Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            else if (value is long)
            {
                integer = Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }

            SetValue(integer);
        }

        object IDBVariable.GetValue()
        {
            return GetValue();
        }
    }
}
