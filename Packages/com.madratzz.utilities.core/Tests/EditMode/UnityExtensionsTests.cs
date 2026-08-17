using NUnit.Framework;
using ProjectCore.EngineExtensions;
using UnityEngine;

namespace Madratzz.Tests.Core
{
    public class UnityExtensionsTests
    {
        private GameObject _go;

        [SetUp]
        public void SetUp()
        {
            _go = new GameObject("TestGO");
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_go);
        }

        [Test]
        public void GetOrAddComponent_WhenMissing_AddsAndReturnsComponent()
        {
            // Regression: the archived implementation discarded AddComponent's
            // result and returned null on the add path.
            var rigidbody = _go.GetOrAddComponent<Rigidbody>();

            Assert.IsNotNull(rigidbody);
            Assert.AreSame(_go, rigidbody.gameObject);
        }

        [Test]
        public void GetOrAddComponent_WhenPresent_ReturnsSameInstance()
        {
            var first = _go.GetOrAddComponent<BoxCollider>();
            var second = _go.GetOrAddComponent<BoxCollider>();

            Assert.AreSame(first, second);
            Assert.AreEqual(1, _go.GetComponents<BoxCollider>().Length);
        }
    }
}
