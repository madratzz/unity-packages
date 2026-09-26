using System;
using ProjectCore.Events;
using UnityEngine;

namespace ProjectCore.Variables
{
    [CreateAssetMenu(fileName = "v_", menuName = "ProjectCore/Variables/Int Persistent With Event")]
    public class DBIntWithEvent : DBInt
    {
        [SerializeField] protected GameEvent ValueChanged;
        
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