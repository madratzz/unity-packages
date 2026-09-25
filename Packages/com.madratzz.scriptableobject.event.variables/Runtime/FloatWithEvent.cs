using System;
using ProjectCore.Events;
using UnityEngine;

namespace ProjectCore.Variables
{
    /// <summary>
    /// A <see cref="Float"/> that raises a <see cref="GameEvent"/> whenever its
    /// value changes — the float counterpart to <see cref="IntWithEvent"/>.
    ///
    /// Typical use is a setting (volume, sensitivity) that several listeners
    /// react to: an audio mixer, a slider showing the current value, a
    /// persistence layer.
    /// </summary>
    [CreateAssetMenu(fileName = "v_", menuName = "ProjectCore/Variables/Float With Event")]
    public class FloatWithEvent : Float
    {
        [SerializeField] protected GameEvent ValueChanged;

        public override void SetValue(float value)
        {
            base.SetValue(value);
            RaiseValueChanged();
        }

        public override void SetValue(Float value)
        {
            base.SetValue(value);
            RaiseValueChanged();
        }

        public override void ApplyChange(float amount)
        {
            base.ApplyChange(amount);
            RaiseValueChanged();
        }

        public override void ApplyChange(Float amount)
        {
            base.ApplyChange(amount);
            RaiseValueChanged();
        }

        public void AddListener(Action callback)
        {
            if (ValueChanged != null)
                ValueChanged.Handler += callback;
        }

        public void RemoveListener(Action callback)
        {
            if (ValueChanged != null)
                ValueChanged.Handler -= callback;
        }

        private void RaiseValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged.Invoke();
        }
    }
}
