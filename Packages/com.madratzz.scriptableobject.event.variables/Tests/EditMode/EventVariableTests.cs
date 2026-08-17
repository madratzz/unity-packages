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
    }
}
