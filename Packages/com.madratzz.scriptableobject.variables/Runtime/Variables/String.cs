using UnityEngine;

namespace ProjectCore.Variables
{
    [CreateAssetMenu(fileName = "v_", menuName = "ProjectCore/Variables/String")]
    public class String : ScriptableObject, IVariable<string>
    {
        [SerializeField] protected string Value;
        [SerializeField] protected string DefaultValue;
        [SerializeField] protected bool ResetToDefaultOnPlay = true;

        private void OnEnable()
        {
            if (ResetToDefaultOnPlay)
            {
                Value = DefaultValue;
            }
        }

        public virtual string GetValue()
        {
            return Value;
        }

        public virtual string GetDefaultValue()
        {
            return DefaultValue;
        }

        public virtual void SetValue(string value)
        {
            Value = value;
        }

        public virtual void SetValue(String value)
        {
            Value = value.Value;
        }

        public virtual void SetDefaultValue(string value)
        {
            DefaultValue = value;
        }

        public virtual void SetDefaultValue(String value)
        {
            DefaultValue = value;
        }

         public virtual void ResetToDefaultValue()
        {
            SetValue(DefaultValue);
        }

        public static implicit operator string(String str)
        {
            return str.Value;
        }
    }
}
