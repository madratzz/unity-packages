using System;
using System.Globalization;
using CustomUtilities.Attributes;
using UnityEngine;

namespace ProjectCore.Variables
{
    [CreateAssetMenu(fileName = "v_", menuName = "ProjectCore/Variables/Bool Persistent")]
    public class DBBool : Bool, IDBVariable
    {
        [SerializeField] protected string Key;

        public void SetKey(string key)
        {
            Key = key;
        }

        public string GetKey()
        {
            return Key;
        }
        
        private void OnEnable()
        {
            ((IDBVariable)this).Load();
        }

        public override void SetValue(bool value)
        {
            base.SetValue(value);
            ((IDBVariable)this).Save();
        }

        public override void SetValue(Bool value)
        {
            base.SetValue(value);
            ((IDBVariable)this).Save();
        }

        [Button]
        void IDBVariable.Save()
        {
            DBManager.SetBool(this, Key, Value);
        }

        [Button]
        void IDBVariable.Load()
        {
            if (!string.IsNullOrEmpty(Key) && DBManager.HasKey(this, Key))
            {
                Value = DBManager.GetBool(this, Key);
            }
            else
            {
                // Nothing saved yet, so the author's DefaultValue is the only
                // sensible starting point. ResetToDefaultOnPlay decides what to do
                // with a value that HAS been saved; it is not a reason to discard
                // DefaultValue for a hard false on a first run.
                Value = DefaultValue;
            }
        }

        void IDBVariable.Update(object value)
        {
            bool boolean = Value;
            if (value is int)
            {
                boolean = Convert.ToInt32(value, CultureInfo.InvariantCulture) == 1;
            }
            else if (value is long)
            {
                boolean = Convert.ToInt32(value, CultureInfo.InvariantCulture) == 1;
            }
            else if (value is bool)
            {
                boolean = Convert.ToBoolean(value, CultureInfo.InvariantCulture);
            }

            SetValue(boolean);
        }

        object IDBVariable.GetValue()
        {
            return GetValue() ? 1 : 0;
        }
    }
}
