using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CustomUtilities.Attributes;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CustomEditorUtilities
{
    // Scans every scene and prefab under Assets/ for [RequireReference] fields
    // that are unassigned. Run from the menu for a manual check, or via
    // -batchmode -executeMethod CustomEditorUtilities.RequiredReferenceValidator.ValidateProjectCI
    // to fail a CI build on missing wiring.
    public static class RequiredReferenceValidator
    {
        private const BindingFlags FieldFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        [MenuItem("Tools/Validate Required References")]
        public static void ValidateProjectMenuItem()
        {
            List<string> issues = ValidateProject();
            if (issues.Count == 0)
                Debug.Log("[RequiredReferenceValidator] No missing required references found.");
            else
                Debug.LogError($"[RequiredReferenceValidator] {issues.Count} missing required reference(s) found — see errors above.");
        }

        public static void ValidateProjectCI()
        {
            List<string> issues = ValidateProject();
            EditorApplication.Exit(issues.Count == 0 ? 0 : 1);
        }

        public static List<string> ValidateProject()
        {
            var issues = new List<string>();
            issues.AddRange(ValidateScenes());
            issues.AddRange(ValidatePrefabs());
            return issues;
        }

        private static List<string> ValidateScenes()
        {
            var issues = new List<string>();

            foreach (string path in FindAssetPaths("t:Scene"))
            {
                var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

                foreach (GameObject root in scene.GetRootGameObjects())
                    issues.AddRange(ValidateGameObject(root, path));

                EditorSceneManager.CloseScene(scene, true);
            }

            return issues;
        }

        private static List<string> ValidatePrefabs()
        {
            var issues = new List<string>();

            foreach (string path in FindAssetPaths("t:Prefab"))
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab != null)
                    issues.AddRange(ValidateGameObject(prefab, path));
            }

            return issues;
        }

        private static IEnumerable<string> FindAssetPaths(string filter)
        {
            return AssetDatabase.FindAssets(filter, new[] { "Assets" })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Distinct();
        }

        private static List<string> ValidateGameObject(GameObject root, string assetPath)
        {
            var issues = new List<string>();

            foreach (Component component in root.GetComponentsInChildren<Component>(true))
            {
                if (component == null)
                    continue; // missing script

                foreach (FieldInfo field in component.GetType().GetFields(FieldFlags))
                {
                    if (field.GetCustomAttribute<RequireReferenceAttribute>() == null)
                        continue;

                    object value = field.GetValue(component);
                    bool isMissing = value == null || (value is Object unityObject && unityObject == null);
                    if (!isMissing)
                        continue;

                    string message = $"[RequiredReferenceValidator] {assetPath} :: {GetHierarchyPath(component.transform)} :: " +
                                      $"{component.GetType().Name}.{field.Name} is required but unassigned.";
                    Debug.LogError(message);
                    issues.Add(message);
                }
            }

            return issues;
        }

        private static string GetHierarchyPath(Transform transform)
        {
            var names = new List<string>();
            for (Transform t = transform; t != null; t = t.parent)
                names.Insert(0, t.name);

            return string.Join("/", names);
        }
    }
}
