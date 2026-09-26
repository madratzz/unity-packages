using System.Collections.Generic;
using System.Linq;
using CustomEditorUtilities;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Madratzz.Tests.UtilitiesAttributes
{
    // The fixture type lives in com.madratzz.utilities.attributes.fixtures, NOT in this
    // assembly: Unity's `t:<TypeName>` asset search does not index types declared in test
    // assemblies, so declaring it here would make every lookup return nothing regardless of
    // which assets exist.
    public class SearchableAssetFinderTests
    {
        private const string TempFolder = "Assets/_SearchableAssetFinderTests_Temp";

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
            var asset = ScriptableObject.CreateInstance<SearchableAssetFinderTestAsset>();
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

            List<Object> found = SearchableAssetFinder.FindAssets(typeof(SearchableAssetFinderTestAsset));
            List<string> foundPaths = found.Select(AssetDatabase.GetAssetPath).ToList();

            // A subset check rather than an exact count, so assets of this type living
            // elsewhere in the project cannot break the test. It asserts that every asset
            // created here is found, and that nothing of another type comes back.
            CollectionAssert.IsSubsetOf(_createdPaths, foundPaths);
            CollectionAssert.AllItemsAreInstancesOfType(found, typeof(SearchableAssetFinderTestAsset));
        }

        [Test]
        public void FindAssets_NoAssetsOfType_ReturnsEmpty()
        {
            List<Object> found = SearchableAssetFinder.FindAssets(typeof(SearchableAssetFinderTestAsset));

            Assert.IsEmpty(found);
        }
    }
}