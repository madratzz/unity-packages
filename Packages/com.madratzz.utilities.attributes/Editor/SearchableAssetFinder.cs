using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
// Disambiguate: `using System` + `using UnityEngine` make bare `Object` a CS0104
// ambiguity between System.Object and UnityEngine.Object.
using Object = UnityEngine.Object;

namespace CustomEditorUtilities
{
    // Pure asset lookup, kept separate from SearchableAssetDropdown's UI so it
    // can be exercised directly by EditMode tests (see AssemblyInfo.cs).
    internal static class SearchableAssetFinder
    {
        internal static List<Object> FindAssets(Type assetType)
        {
            return AssetDatabase.FindAssets($"t:{assetType.Name}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Distinct()
                .OrderBy(path => path, StringComparer.Ordinal)
                .Select(path => AssetDatabase.LoadAssetAtPath(path, assetType))
                .Where(asset => asset != null)
                .ToList();
        }
    }
}
