using CustomUtilities.Attributes;
using UnityEngine;

namespace ProjectCore.Variables
{
    [CreateAssetMenu(fileName = "v_", menuName = "ProjectCore/Variables/String Persistent")]
    public class DBString : String, IDBVariable
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
            Load();
        }

        public void Refresh()
        {
            Load();
        }
        
        public override void SetValue(string value)
        {
            base.SetValue(value);
            Save();
        }

        public override void SetValue(String value)
        {
            base.SetValue(value);
            Save();
        }

        [Button]
        public virtual void Save()
        {
            DBManager.SetString(this, Key, Value);
        }

        [Button]
        public virtual void Load()
        {
            if (!string.IsNullOrEmpty(Key) && DBManager.HasKey(this, Key))
            {
                Value = DBManager.GetString(this, Key);
            }
            else
            {
                // Nothing saved yet, so the author's DefaultValue is the only
                // sensible starting point. ResetToDefaultOnPlay decides what to do
                // with a value that HAS been saved; it is not a reason to discard
                // DefaultValue for a hard string.Empty on a first run.
                Value = DefaultValue;
            }
        }

        void IDBVariable.Update(object value)
        {
            if (value is string)
            {
                SetValue((string)value);
            }
        }

        object IDBVariable.GetValue()
        {
            return GetValue();
        }
    }
}
