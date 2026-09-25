using NUnit.Framework;
using ProjectCore.Events;
using ProjectCore.Variables;
using UnityEngine;

namespace Madratzz.Tests.EventVariables
{
    public class EventVariableTests
    {
        private GameEvent _valueChanged;

        [SetUp]
        public void SetUp()
        {
            _valueChanged = ScriptableObject.CreateInstance<GameEvent>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_valueChanged);
        }

        private void Wire<T>(T variable) where T : ScriptableObject
        {
            // ValueChanged is [SerializeField] protected on the event-variables;
            // assign via SerializedObject to avoid reflection drift.
            var so = new UnityEditor.SerializedObject(variable);
            so.FindProperty("ValueChanged").objectReferenceValue = _valueChanged;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        [Test]
        public void BoolWithEvent_SetValue_RaisesValueChanged()
        {
            var variable = ScriptableObject.CreateInstance<BoolWithEvent>();
            Wire(variable);
            int calls = 0;
            _valueChanged.Handler += () => calls++;

            variable.SetValue(true);

            Assert.AreEqual(1, calls);
            Assert.IsTrue(variable.GetValue());
            Object.DestroyImmediate(variable);
        }

        [Test]
        public void BoolWithEvent_SetValue_WithNoEventAssigned_DoesNotThrow()
        {
            var variable = ScriptableObject.CreateInstance<BoolWithEvent>();

            Assert.DoesNotThrow(() => variable.SetValue(true));
            Object.DestroyImmediate(variable);
        }

        [Test]
        public void DBIntWithEvent_SetValue_RaisesValueChanged()
        {
            var variable = ScriptableObject.CreateInstance<DBIntWithEvent>();
            variable.SetKey("evint_" + System.Guid.NewGuid().ToString("N"));
            Wire(variable);
            int calls = 0;
            _valueChanged.Handler += () => calls++;

            variable.SetValue(5);

            Assert.AreEqual(1, calls);
            Object.DestroyImmediate(variable);
        }

        [Test]
        public void DBIntWithEvent_ApplyChange_RaisesValueChanged()
        {
            var variable = ScriptableObject.CreateInstance<DBIntWithEvent>();
            variable.SetKey("evint_" + System.Guid.NewGuid().ToString("N"));
            Wire(variable);
            int calls = 0;
            _valueChanged.Handler += () => calls++;

            variable.ApplyChange(3);

            Assert.AreEqual(1, calls);
            Object.DestroyImmediate(variable);
        }

        [Test]
        public void DBBoolWithEvent_SetValue_RaisesValueChanged()
        {
            var variable = ScriptableObject.CreateInstance<DBBoolWithEvent>();
            variable.SetKey("evbool_" + System.Guid.NewGuid().ToString("N"));
            Wire(variable);
            int calls = 0;
            _valueChanged.Handler += () => calls++;

            variable.SetValue(true);

            Assert.AreEqual(1, calls);
            Object.DestroyImmediate(variable);
        }

        [Test]
        public void AddListener_UnassignedEvent_DoesNotThrow()
        {
            var variable = ScriptableObject.CreateInstance<BoolWithEvent>();

            Assert.DoesNotThrow(() => variable.AddListener(() => { }));
            Assert.DoesNotThrow(() => variable.RemoveListener(() => { }));
            Object.DestroyImmediate(variable);
        }

        [Test]
        public void IntWithEvent_SetValue_RaisesValueChanged()
        {
            var variable = ScriptableObject.CreateInstance<IntWithEvent>();
            Wire(variable);
            int calls = 0;
            _valueChanged.Handler += () => calls++;

            variable.SetValue(7);

            Assert.AreEqual(1, calls);
            Assert.AreEqual(7, variable.GetValue());
            Object.DestroyImmediate(variable);
        }

        [Test]
        public void IntWithEvent_ApplyChange_RaisesValueChanged()
        {
            // Incrementing is the common path for a score; without the
            // ApplyChange override the value would move without notifying.
            var variable = ScriptableObject.CreateInstance<IntWithEvent>();
            Wire(variable);
            variable.SetValue(10);
            int calls = 0;
            _valueChanged.Handler += () => calls++;

            variable.ApplyChange(5);

            Assert.AreEqual(1, calls);
            Assert.AreEqual(15, variable.GetValue());
            Object.DestroyImmediate(variable);
        }

        [Test]
        public void IntWithEvent_WithNoEventAssigned_DoesNotThrow()
        {
            var variable = ScriptableObject.CreateInstance<IntWithEvent>();

            Assert.DoesNotThrow(() => variable.SetValue(3));
            Assert.DoesNotThrow(() => variable.ApplyChange(1));
            Assert.AreEqual(4, variable.GetValue());
            Object.DestroyImmediate(variable);
        }

        [Test]
        public void FloatWithEvent_SetValue_RaisesValueChanged()
        {
            var variable = ScriptableObject.CreateInstance<FloatWithEvent>();
            Wire(variable);
            int calls = 0;
            _valueChanged.Handler += () => calls++;

            variable.SetValue(0.35f);

            Assert.AreEqual(1, calls);
            Assert.AreEqual(0.35f, variable.GetValue(), 0.0001f);
            Object.DestroyImmediate(variable);
        }

        [Test]
        public void FloatWithEvent_ApplyChange_RaisesValueChanged()
        {
            var variable = ScriptableObject.CreateInstance<FloatWithEvent>();
            Wire(variable);
            variable.SetValue(0.5f);
            int calls = 0;
            _valueChanged.Handler += () => calls++;

            variable.ApplyChange(0.25f);

            Assert.AreEqual(1, calls);
            Assert.AreEqual(0.75f, variable.GetValue(), 0.0001f);
            Object.DestroyImmediate(variable);
        }

        [Test]
        public void FloatWithEvent_WithNoEventAssigned_DoesNotThrow()
        {
            var variable = ScriptableObject.CreateInstance<FloatWithEvent>();

            Assert.DoesNotThrow(() => variable.SetValue(0.2f));
            Assert.DoesNotThrow(() => variable.ApplyChange(0.1f));
            Assert.AreEqual(0.3f, variable.GetValue(), 0.0001f);
            Object.DestroyImmediate(variable);
        }
    }
}
