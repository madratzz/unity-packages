using System;
using ProjectCore.Events;
using UnityEngine;

namespace ProjectCore.Variables
{
    [CreateAssetMenu(fileName = "v_", menuName = "ProjectCore/Variables/Bool With Event")]
    public class BoolWithEvent : Bool
    {
        [SerializeField] protected GameEvent ValueChanged;

        public override void SetValue(bool value)
        {
            base.SetValue(value);
            RaiseValueChanged();
        }

        public override void SetValue(Bool value)
        {
            base.SetValue(value);
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
