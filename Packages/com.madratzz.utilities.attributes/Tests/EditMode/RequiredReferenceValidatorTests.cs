using System.Collections.Generic;
using CustomEditorUtilities;
using CustomUtilities.Attributes;
using NUnit.Framework;
using UnityEngine;

namespace Madratzz.Tests.UtilitiesAttributes
{
    public class RequiredReferenceValidatorTests
    {
        private class RequiredRefHolder : MonoBehaviour
        {
            [RequireReference]
            [SerializeField] public ScriptableObject Dependency;
        }

        private class UnmarkedRefHolder : MonoBehaviour
        {
            [SerializeField] public ScriptableObject Dependency;
        }

        private readonly List<Object> _spawned = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            foreach (Object obj in _spawned)
                if (obj != null)
                    Object.DestroyImmediate(obj);

            _spawned.Clear();
        }

        private GameObject SpawnGameObject(string name = "Test")
        {
            var go = new GameObject(name);
            _spawned.Add(go);
            return go;
        }

        private ScriptableObject SpawnScriptableObject()
        {
            var so = ScriptableObject.CreateInstance<ScriptableObject>();
            _spawned.Add(so);
            return so;
        }

        [Test]
        public void UnassignedRequiredReference_IsReported()
        {
            GameObject go = SpawnGameObject();
            go.AddComponent<RequiredRefHolder>();

            List<string> issues = RequiredReferenceValidator.ValidateGameObject(go, "test-asset-path");

            Assert.AreEqual(1, issues.Count);
            StringAssert.Contains("RequiredRefHolder.Dependency", issues[0]);
        }

        [Test]
        public void AssignedRequiredReference_IsNotReported()
        {
            GameObject go = SpawnGameObject();
            RequiredRefHolder holder = go.AddComponent<RequiredRefHolder>();
            holder.Dependency = SpawnScriptableObject();

            List<string> issues = RequiredReferenceValidator.ValidateGameObject(go, "test-asset-path");

            Assert.IsEmpty(issues);
        }

        [Test]
        public void UnassignedField_WithoutAttribute_IsNotReported()
        {
            GameObject go = SpawnGameObject();
            go.AddComponent<UnmarkedRefHolder>();

            List<string> issues = RequiredReferenceValidator.ValidateGameObject(go, "test-asset-path");

            Assert.IsEmpty(issues);
        }

        [Test]
        public void UnassignedRequiredReference_OnChildObject_IncludesHierarchyPath()
        {
            GameObject parent = SpawnGameObject("Parent");
            GameObject child = SpawnGameObject("Child");
            child.transform.SetParent(parent.transform);
            child.AddComponent<RequiredRefHolder>();

            List<string> issues = RequiredReferenceValidator.ValidateGameObject(parent, "test-asset-path");

            Assert.AreEqual(1, issues.Count);
            StringAssert.Contains("Parent/Child", issues[0]);
        }
    }
}
