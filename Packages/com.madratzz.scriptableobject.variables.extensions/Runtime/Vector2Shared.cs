using UnityEngine;

namespace ProjectCore.Variables
{
    [CreateAssetMenu(fileName = "v_", menuName = "ProjectCore/Variables/Shared Vector2")]
    public class Vector2Shared : ScriptableObject
    {
        [SerializeField] protected Vector2 Value;
        [SerializeField] protected Vector2 DefaultValue;
        [SerializeField] protected bool ResetToDefaultOnPlay = true;

        private void OnEnable()
        {
            if (ResetToDefaultOnPlay)
            {
                Value = DefaultValue;
            }
        }

        public Vector2 GetValue()
        {
            return Value;
        }

        public virtual void SetValue(Vector2 value)
        {
            Value = value;
        }

        public virtual void SetValue(Vector2Shared value)
        {
            Value = value.Value;
        }

        public virtual void ApplyChange(Vector2 amount)
        {
            Value += amount;
        }

        public virtual void ApplyChange(Vector2Shared amount)
        {
            Value += amount.Value;
        }

        public static implicit operator Vector2(Vector2Shared vectorValue)
        {
            return vectorValue.Value;
        }
    }
}
