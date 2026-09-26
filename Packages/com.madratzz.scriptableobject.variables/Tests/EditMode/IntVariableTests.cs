using NUnit.Framework;
using ProjectCore.Variables;
using UnityEngine;

namespace Madratzz.Tests.Variables
{
    public class IntVariableTests
    {
        private Int _variable;

        [SetUp]
        public void SetUp()
        {
            _variable = ScriptableObject.CreateInstance<Int>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_variable);
        }

        [Test]
        public void GetValue_DefaultsToSerializedDefault()
        {
            // CreateInstance triggers OnEnable; ResetToDefaultOnPlay=true syncs Value to DefaultValue
            Assert.AreEqual(_variable.GetDefaultValue(), _variable.GetValue());
        }

        [Test]
        public void SetValue_UpdatesValue()
        {
            _variable.SetValue(42);
            Assert.AreEqual(42, _variable.GetValue());
        }

        [Test]
        public void SetValue_FromAnotherVariable_CopiesValue()
        {
            var other = ScriptableObject.CreateInstance<Int>();
            other.SetValue(7);

            _variable.SetValue(other);

            Assert.AreEqual(7, _variable.GetValue());
            Object.DestroyImmediate(other);
        }

        [Test]
        public void ResetToDefaultValue_RestoresDefault()
        {
            _variable.SetDefaultValue(10);
            _variable.SetValue(99);

            _variable.ResetToDefaultValue();

            Assert.AreEqual(10, _variable.GetValue());
        }

        [Test]
        public void ApplyChange_Accumulates()
        {
            _variable.SetValue(5);

            _variable.ApplyChange(3);
            _variable.ApplyChange(4);

            Assert.AreEqual(12, _variable.GetValue());
        }

        [Test]
        public void ApplyChange_NegativeAmount_Subtracts()
        {
            _variable.SetValue(10);

            _variable.ApplyChange(-4);

            Assert.AreEqual(6, _variable.GetValue());
        }

        [Test]
        public void ImplicitOperator_ReturnsCurrentValue()
        {
            _variable.SetValue(17);
            int raw = _variable;
            Assert.AreEqual(17, raw);
        }
    }
}
