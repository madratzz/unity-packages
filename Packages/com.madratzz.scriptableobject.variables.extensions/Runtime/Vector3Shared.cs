using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "v_", menuName = "ProjectCore/Variables/Shared Vector3")]
public class Vector3Shared : ScriptableObject
{
    [SerializeField] protected Vector3 Value;
    [SerializeField] protected Vector3 DefaultValue;
    [SerializeField] protected bool ResetToDefaultOnPlay = true;

    private void OnEnable()
    {
        if (ResetToDefaultOnPlay)
        {
            Value = DefaultValue;
        }
    }

    public Vector3 GetValue()
    {
        return Value;
    }

    public virtual void SetValue(Vector3 value)
    {
        Value = value;
    }

    public virtual void SetValue(Vector3Shared value)
    {
        Value = value.Value;
    }

    public virtual void ApplyChange(Vector3 amount)
    {
        Value += amount;
    }

    public virtual void ApplyChange(Vector3Shared amount)
    {
        Value += amount.Value;
    }

    public static implicit operator Vector3(Vector3Shared vectorValue)
    {
        return vectorValue.Value;
    }
}
