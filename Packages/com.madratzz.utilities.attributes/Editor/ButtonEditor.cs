using System.Linq;
using System.Reflection;
using CustomUtilities.Attributes;
using UnityEditor;
using UnityEngine;

namespace CustomEditorUtilities
{
// 1. Target all MonoBehaviours
    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class MonoBehaviourButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the normal inspector
            DrawDefaultInspector();

            // Draw the buttons below
            ButtonEditorLogic.DrawButtons(target);
        }
    }

// 2. Target all ScriptableObjects
    [CustomEditor(typeof(ScriptableObject), true), CanEditMultipleObjects]
    public class ScriptableObjectButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the normal inspector
            DrawDefaultInspector();

            // Draw the buttons below
            ButtonEditorLogic.DrawButtons(target);
        }
    }

// Shared Logic to keep things clean
    public static class ButtonEditorLogic
    {
        public static void DrawButtons(Object target)
        {
            // Loop through all methods in the target class
            var methods = target.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0);

            if (methods.Any())
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Buttons", EditorStyles.boldLabel);

                foreach (var method in methods)
                {
                    var attr = (ButtonAttribute)method.GetCustomAttributes(typeof(ButtonAttribute), true)[0];
                    string buttonName = string.IsNullOrEmpty(attr.ButtonName)
                        ? ObjectNames.NicifyVariableName(method.Name)
                        : attr.ButtonName;
                    float height = attr.Height > 0 ? attr.Height : EditorGUIUtility.singleLineHeight * 1.5f;

                    if (GUILayout.Button(buttonName, GUILayout.Height(height)))
                    {
                        method.Invoke(target, null);
                    }
                }
            }
        }
    }
}