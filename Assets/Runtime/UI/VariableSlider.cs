using CustomUtilities.Attributes;
using ProjectCore.Events;
using ProjectCore.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectCore.Architecture
{
    /// <summary>
    /// Two-way binding between a <see cref="Slider"/> and a ScriptableObject
    /// <see cref="Float"/>: dragging writes to the variable, and a change from
    /// anywhere else moves the slider.
    ///
    /// The write-back uses <see cref="Slider.SetValueWithoutNotify"/>, which is
    /// what keeps the two directions from chasing each other — a plain
    /// <c>slider.value =</c> would fire <c>onValueChanged</c>, write the variable
    /// again, raise its event again, and loop.
    ///
    /// <see cref="Variable"/> is typed as <see cref="Float"/>, so it accepts
    /// <c>Float</c> or <c>FloatWithEvent</c>. Without <see cref="ValueChanged"/>
    /// the binding still works one-way (slider writes the variable) and seeds
    /// itself on enable; it just won't follow changes made elsewhere.
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class VariableSlider : MonoBehaviour
    {
        [Tooltip("Variable this slider reads and writes. Accepts Float or FloatWithEvent.")]
        [RequireReference]
        [SerializeField] private Float Variable;

        [Tooltip("Raised by the variable when it changes. Leave empty if nothing else writes it.")]
        [SerializeField] private GameEvent ValueChanged;

        private Slider _slider;

        private void Awake() => _slider = GetComponent<Slider>();

        private void OnEnable()
        {
            if (_slider == null) _slider = GetComponent<Slider>();
            if (_slider == null) return;

            _slider.onValueChanged.AddListener(OnSliderMoved);
            if (ValueChanged != null) ValueChanged.Handler += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            if (_slider != null) _slider.onValueChanged.RemoveListener(OnSliderMoved);
            if (ValueChanged != null) ValueChanged.Handler -= Refresh;
        }

        /// <summary>Pull the variable's value into the slider without echoing it back.</summary>
        public void Refresh()
        {
            if (_slider == null || Variable == null) return;
            _slider.SetValueWithoutNotify(Variable.GetValue());
        }

        private void OnSliderMoved(float value)
        {
            if (Variable == null) return;

            // Guard against a redundant write when the value did not really move:
            // SetValue on a *WithEvent variable raises an event, and doing that on
            // every pixel of drag wakes every listener for nothing.
            if (Mathf.Approximately(Variable.GetValue(), value)) return;

            Variable.SetValue(value);
        }
    }
}
