using CustomUtilities.Attributes;
using ProjectCore.Events;
using ProjectCore.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectCore.Architecture
{
    /// <summary>
    /// Two-way binding between a <see cref="Toggle"/> and a ScriptableObject
    /// <see cref="Bool"/>: clicking writes to the variable, and a change from
    /// anywhere else moves the toggle.
    ///
    /// Uses <see cref="Toggle.SetIsOnWithoutNotify"/> for the write-back so the
    /// two directions cannot chase each other — see <see cref="VariableSlider"/>
    /// for the same reasoning.
    ///
    /// <see cref="Variable"/> is typed as <see cref="Bool"/>, so it accepts
    /// <c>Bool</c>, <c>DBBool</c>, <c>BoolWithEvent</c> or <c>DBBoolWithEvent</c>.
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public class VariableToggle : MonoBehaviour
    {
        [Tooltip("Variable this toggle reads and writes. Accepts Bool, DBBool, BoolWithEvent or DBBoolWithEvent.")]
        [RequireReference]
        [SerializeField] private Bool Variable;

        [Tooltip("Raised by the variable when it changes. Leave empty if nothing else writes it.")]
        [SerializeField] private GameEvent ValueChanged;

        private Toggle _toggle;

        private void Awake() => _toggle = GetComponent<Toggle>();

        private void OnEnable()
        {
            if (_toggle == null) _toggle = GetComponent<Toggle>();
            if (_toggle == null) return;

            _toggle.onValueChanged.AddListener(OnToggled);
            if (ValueChanged != null) ValueChanged.Handler += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            if (_toggle != null) _toggle.onValueChanged.RemoveListener(OnToggled);
            if (ValueChanged != null) ValueChanged.Handler -= Refresh;
        }

        /// <summary>Pull the variable's value into the toggle without echoing it back.</summary>
        public void Refresh()
        {
            if (_toggle == null || Variable == null) return;
            _toggle.SetIsOnWithoutNotify(Variable.GetValue());
        }

        private void OnToggled(bool value)
        {
            if (Variable == null || Variable.GetValue() == value) return;
            Variable.SetValue(value);
        }
    }
}
