using System.Collections.Generic;
using System.Linq;
using CustomEditorUtilities;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Madratzz.Tests.UtilitiesAttributes
{
    public class SearchableAssetFinderTests
    {
        private const string TempFolder = "Assets/_SearchableAssetFinderTests_Temp";

        private class TestSearchableAsset : ScriptableObject
        {
        }

        private readonly List<string> _createdPaths = new List<string>();

        [SetUp]
        public void SetUp()
        {
            if (!AssetDatabase.IsValidFolder(TempFolder))
                AssetDatabase.CreateFolder("Assets", "_SearchableAssetFinderTests_Temp");
        }

        [TearDown]
        public void TearDown()
        {
            foreach (string path in _createdPaths)
                AssetDatabase.DeleteAsset(path);
            _createdPaths.Clear();

            if (AssetDatabase.IsValidFolder(TempFolder))
                AssetDatabase.DeleteAsset(TempFolder);
        }

        private void CreateTestAsset(string name)
        {
            var asset = ScriptableObject.CreateInstance<TestSearchableAsset>();
            string path = $"{TempFolder}/{name}.asset";
            AssetDatabase.CreateAsset(asset, path);
            _createdPaths.Add(path);
        }

        [Test]
        public void FindAssets_ReturnsEveryCreatedAssetOfType()
        {
            CreateTestAsset("A");
            CreateTestAsset("B");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            List<Object> found = SearchableAssetFinder.FindAssets(typeof(TestSearchableAsset));

            Assert.AreEqual(2, found.Count);
            CollectionAssert.AreEquivalent(new[] { "A", "B" }, found.Select(a => a.name));
        }

        [Test]
        public void FindAssets_NoAssetsOfType_ReturnsEmpty()
        {
            List<Object> found = SearchableAssetFinder.FindAssets(typeof(TestSearchableAsset));

            Assert.IsEmpty(found);
        }
    }
}
