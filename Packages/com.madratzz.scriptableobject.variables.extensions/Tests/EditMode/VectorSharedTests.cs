using NUnit.Framework;
using ProjectCore.Variables;
using UnityEngine;

namespace Madratzz.Tests.VariablesExtensions
{
    public class VectorSharedTests
    {
        [Test]
        public void Vector3Shared_SetAndGet_RoundTrips()
        {
            var vector = ScriptableObject.CreateInstance<Vector3Shared>();

            vector.SetValue(new Vector3(1, 2, 3));

            Assert.AreEqual(new Vector3(1, 2, 3), vector.GetValue());
            Object.DestroyImmediate(vector);
        }

        [Test]
        public void Vector3Shared_ApplyChange_Accumulates()
        {
            var vector = ScriptableObject.CreateInstance<Vector3Shared>();
            vector.SetValue(Vector3.one);

            vector.ApplyChange(new Vector3(1, 0, 1));

            Assert.AreEqual(new Vector3(2, 1, 2), vector.GetValue());
            Object.DestroyImmediate(vector);
        }

        [Test]
        public void Vector3Shared_ImplicitOperator_ReturnsCurrentValue()
        {
            var vector = ScriptableObject.CreateInstance<Vector3Shared>();
            vector.SetValue(Vector3.up);

            Vector3 raw = vector;

            Assert.AreEqual(Vector3.up, raw);
            Object.DestroyImmediate(vector);
        }

        [Test]
        public void Vector2Shared_SetAndGet_RoundTrips()
        {
            var vector = ScriptableObject.CreateInstance<Vector2Shared>();

            vector.SetValue(new Vector2(4, 5));

            Assert.AreEqual(new Vector2(4, 5), vector.GetValue());
            Object.DestroyImmediate(vector);
        }
    }
}
