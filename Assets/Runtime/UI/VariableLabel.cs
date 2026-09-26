using CustomUtilities.Attributes;
using ProjectCore.Events;
using ProjectCore.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectCore.Architecture
{
    /// <summary>
    /// Writes a ScriptableObject <see cref="Int"/> into a UI <see cref="Text"/>,
    /// refreshing whenever <see cref="ValueChanged"/> is raised.
    ///
    /// The field is typed as <see cref="Int"/> rather than a concrete subclass so
    /// it accepts the whole family — <c>Int</c>, <c>DBInt</c>, <c>IntWithEvent</c>,
    /// <c>DBIntWithEvent</c> — and the label does not care which it was given.
    ///
    /// <see cref="ValueChanged"/> is the same <see cref="GameEvent"/> asset the
    /// variable raises, wired on both sides rather than the label reaching into
    /// the variable: that keeps this working with a plain <c>Int</c> that someone
    /// else raises an event for, and avoids a cast to a *WithEvent type.
    ///
    /// With no event assigned the label still shows the correct value on enable,
    /// it just will not follow later changes — deliberate, so a static readout is
    /// not forced to poll every frame.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class VariableLabel : MonoBehaviour
    {
        [Tooltip("Variable to display. Accepts Int, DBInt, IntWithEvent or DBIntWithEvent.")]
        [RequireReference]
        [SerializeField] private Int Variable;

        [Tooltip("Raised by the variable when it changes. Leave empty for a value that is only read once, on enable.")]
        [SerializeField] private GameEvent ValueChanged;

        [Tooltip("Composite format string; {0} is the value. e.g. \"SCORE  {0}\"")]
        [SerializeField] private string Format = "{0}";

        private Text _label;
        private int _lastRendered;
        private bool _rendered;

        private void Awake() => _label = GetComponent<Text>();

        private void OnEnable()
        {
            if (ValueChanged != null) ValueChanged.Handler += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            if (ValueChanged != null) ValueChanged.Handler -= Refresh;
        }

        /// <summary>Re-read the variable and repaint. Safe to call directly.</summary>
        public void Refresh()
        {
            if (_label == null) _label = GetComponent<Text>();
            if (_label == null || Variable == null) return;

            var value = Variable.GetValue();

            // Formatting allocates a string, so skip the work when nothing moved.
            if (_rendered && value == _lastRendered) return;

            _lastRendered = value;
            _rendered = true;
            _label.text = string.Format(Format, value);
        }
    }
}
