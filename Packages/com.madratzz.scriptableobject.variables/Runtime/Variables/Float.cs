using UnityEngine;

namespace ProjectCore.Variables
{
    [CreateAssetMenu(fileName ="v_", menuName = "ProjectCore/Variables/Float")]
    public class Float : ScriptableObject, IVariable<float>, IApplyChange<float>
    {
        [SerializeField] protected float Value;
        [SerializeField] protected float DefaultValue;
        [SerializeField] protected bool ResetToDefaultOnPlay = true;

        private void OnEnable()
        {
            if (ResetToDefaultOnPlay)
            {
                Value = DefaultValue;
            }
        }

        public float GetValue()
        {
            return Value;
        }

        public void ResetToDefaultValue()
        {
            SetValue(DefaultValue);
        }

        public float GetDefaultValue()
        {
            return DefaultValue;
        }

        public virtual void SetValue(float value)
        {
            Value = value;
        }

        public virtual void SetValue(Float value)
        {
            Value = value.Value;
        }

        public virtual void SetDefaultValue(float value)
        {
            DefaultValue = value;
        }

        public virtual void SetDefaultValue(Float value)
        {
            DefaultValue = value;
        }

        public virtual void ApplyChange(float amount)
        {
            Value += amount;
        }

        public virtual void ApplyChange(Float amount)
        {
            Value += amount.Value;
        }
        

        public static implicit operator float(Float floatingPoint)
        {
            return floatingPoint.Value;
        }
    }
}