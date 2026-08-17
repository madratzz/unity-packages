using NUnit.Framework;
using ProjectCore.Variables;
using UnityEngine;

namespace Madratzz.Tests.Variables
{
    public class ScalarVariableTests
    {
        [Test]
        public void Bool_SetValue_RoundTrips()
        {
            var variable = ScriptableObject.CreateInstance<Bool>();

            variable.SetValue(true);
            Assert.IsTrue(variable.GetValue());

            variable.SetValue(false);
            Assert.IsFalse(variable.GetValue());

            Object.DestroyImmediate(variable);
        }

        [Test]
        public void Bool_ResetToDefaultValue_RestoresDefault()
        {
            var variable = ScriptableObject.CreateInstance<Bool>();
            variable.SetDefaultValue(true);
            variable.SetValue(false);

            variable.ResetToDefaultValue();

            Assert.IsTrue(variable.GetValue());
            Object.DestroyImmediate(variable);
        }

        [Test]
        public void String_SetValue_RoundTrips()
        {
            var variable = ScriptableObject.CreateInstance<String>();

            variable.SetValue("hello");

            Assert.AreEqual("hello", variable.GetValue());
            Object.DestroyImmediate(variable);
        }

        [Test]
        public void String_SetValue_Null_IsStored()
        {
            var variable = ScriptableObject.CreateInstance<String>();
            variable.SetValue("x");

            variable.SetValue((string)null);

            Assert.IsNull(variable.GetValue());
            Object.DestroyImmediate(variable);
        }
    }
}
