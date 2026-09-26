using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CustomEditorUtilities
{
    public class EditorHelperMethods
    {
#if UNITY_EDITOR

        public static Dictionary<string, T> FindAllScriptableObjectsOfType<T>() where T : ScriptableObject 
        {
            Dictionary<string, T> scriptableObjectsDict = new Dictionary<string, T>();

            // Find all asset GUIDs for ScriptableObjects of the specified type
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");

            foreach (string guid in guids)
            {
                // Get the asset path from the GUID
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                // Load the asset at the specified path and cast it to the desired type
                T obj = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (obj != null && !scriptableObjectsDict.ContainsKey(assetPath))
                {
                    scriptableObjectsDict.Add(assetPath, obj);
                }
                else if (obj != null)
                {
                    Debug.LogWarning($"Duplicate asset found at path: {assetPath} - Skipping duplicate entry.");
                }
            }

            return scriptableObjectsDict;
        }
#endif
    }
}