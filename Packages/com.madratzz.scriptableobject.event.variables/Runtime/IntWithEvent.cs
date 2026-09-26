using System;
using ProjectCore.Events;
using UnityEngine;

namespace ProjectCore.Variables
{
    /// <summary>
    /// A non-persistent <see cref="Int"/> that raises a <see cref="GameEvent"/>
    /// whenever its value changes. The session-only counterpart to
    /// <see cref="DBIntWithEvent"/>, mirroring <see cref="BoolWithEvent"/>.
    ///
    /// <see cref="ApplyChange"/> is overridden as well as <see cref="SetValue"/>,
    /// because incrementing a score is the common path and would otherwise
    /// change the value without notifying anyone.
    /// </summary>
    [CreateAssetMenu(fileName = "v_", menuName = "ProjectCore/Variables/Int With Event")]
    public class IntWithEvent : Int
    {
        [SerializeField] protected GameEvent ValueChanged;

        public override void SetValue(int value)
        {
            base.SetValue(value);
            RaiseValueChanged();
        }

        public override void SetValue(Int value)
        {
            base.SetValue(value);
            RaiseValueChanged();
        }

        public override void ApplyChange(int amount)
        {
            base.ApplyChange(amount);
            RaiseValueChanged();
        }

        public override void ApplyChange(Int amount)
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
