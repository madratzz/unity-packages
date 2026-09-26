using UnityEngine;

namespace ProjectCore.Variables
{
    [CreateAssetMenu(fileName = "v_", menuName = "ProjectCore/Variables/Bool")]
    public class Bool : ScriptableObject, IVariable<bool>
    {
        [SerializeField] protected bool Value;
        [SerializeField] protected bool DefaultValue;
        [SerializeField] protected bool ResetToDefaultOnPlay = true;

        private void OnEnable()
        {
            if (ResetToDefaultOnPlay)
            {
                Value = DefaultValue;
            }
        }

        public virtual bool GetValue()
        {
            return Value;
        }

        public virtual bool GetDefaultValue()
        {
            return DefaultValue;
        }

        public void SetDefaultValue(bool value)
        {
            DefaultValue = value;
        }

        public virtual void SetValue(bool value)
        {
            Value = value;
        }

        public virtual void SetValue(Bool value)
        {
            Value = value.Value;
        }

        public virtual void ResetToDefaultValue()
        {
            SetValue(DefaultValue);
        }

        public void ApplyChange(bool amount)
        {
            throw new System.NotImplementedException();
        }

        public static implicit operator bool(Bool boolean)
        {
            return boolean.Value;
        }
    }
}
