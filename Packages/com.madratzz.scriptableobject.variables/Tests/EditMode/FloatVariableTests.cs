using NUnit.Framework;
using ProjectCore.Variables;
using UnityEngine;

namespace Madratzz.Tests.Variables
{
    public class FloatVariableTests
    {
        private Float _variable;

        [SetUp]
        public void SetUp()
        {
            _variable = ScriptableObject.CreateInstance<Float>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_variable);
        }

        [Test]
        public void SetValue_UpdatesValue()
        {
            _variable.SetValue(2.5f);
            Assert.AreEqual(2.5f, _variable.GetValue(), 1e-6f);
        }

        [Test]
        public void ApplyChange_Accumulates()
        {
            _variable.SetValue(1f);

            _variable.ApplyChange(0.5f);
            _variable.ApplyChange(0.25f);

            Assert.AreEqual(1.75f, _variable.GetValue(), 1e-6f);
        }

        [Test]
        public void ResetToDefaultValue_RestoresDefault()
        {
            _variable.SetDefaultValue(3.5f);
            _variable.SetValue(99f);

            _variable.ResetToDefaultValue();

            Assert.AreEqual(3.5f, _variable.GetValue(), 1e-6f);
        }

        [Test]
        public void ImplicitOperator_ReturnsCurrentValue()
        {
            _variable.SetValue(1.25f);
            float raw = _variable;
            Assert.AreEqual(1.25f, raw, 1e-6f);
        }
    }
}
