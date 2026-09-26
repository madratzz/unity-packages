using CustomUtilities.Attributes;
using UnityEngine;

namespace ProjectCore.Variables
{
    [CreateAssetMenu(fileName = "v_", menuName = "ProjectCore/Variables/Float Persistent")]
    public class DBFloat : Float, IDBVariable
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

        public override void SetValue(float value)
        {
            base.SetValue(value);
            Save();
        }

        public override void SetValue(Float value)
        {
            base.SetValue(value);
            Save();
        }

        public override void ApplyChange(float amount)
        {
            base.ApplyChange(amount);
            Save();
        }

        public override void ApplyChange(Float amount)
        {
            base.ApplyChange(amount);
            Save();
        }

        [Button]
        public virtual void Save()
        {
            DBManager.SetFloat(this, Key, Value);
        }

        [Button]
        public virtual void Load()
        {
            if (!string.IsNullOrEmpty(Key) && DBManager.HasKey(this, Key))
            {
                Value = DBManager.GetFloat(this, Key);
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
            if (value is float)
            {
                SetValue((float)(value));
            }
            else if (value is double)
            {
                SetValue((float)((double)value));
            }
        }

        object IDBVariable.GetValue()
        {
            return GetValue();
        }
    }
}