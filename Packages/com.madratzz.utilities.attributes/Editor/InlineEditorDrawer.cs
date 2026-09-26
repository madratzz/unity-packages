using System;
using UnityEngine;
using UnityEditor;
using CustomUtilities.Attributes;

namespace CustomEditorUtilities
{
    [CustomPropertyDrawer(typeof(InlineEditorAttribute))]
public class InlineEditorDrawer : PropertyDrawer
{
    private Editor _cachedEditor;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // 1. Draw the standard object field
        Rect objectFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(objectFieldRect, property, label);

        // 2. Check if we have a valid object
        if (property.objectReferenceValue != null)
        {
            // SMART CHECK: Look for attribute on the Field first, then the Class type
            InlineEditorAttribute inlineAttr = GetAttribute(property);

            // Only draw inline if we found the attribute!
            if (inlineAttr != null)
            {
                DrawInlineEditor(position, property, inlineAttr);
            }
        }
        else
        {
            CleanupEditor();
        }

        EditorGUI.EndProperty();
    }

    // Helper to find the attribute on Field OR Class
    private InlineEditorAttribute GetAttribute(SerializedProperty property)
    {
        // 1. Check if the attribute is on the Field (standard)
        if (attribute is InlineEditorAttribute attr) return attr;

        // 2. If not, check if the attribute is on the Class Type of the object
        if (property.objectReferenceValue != null)
        {
            Type objectType = property.objectReferenceValue.GetType();
            var classAttr = Attribute.GetCustomAttribute(objectType, typeof(InlineEditorAttribute)) as InlineEditorAttribute;
            return classAttr;
        }

        return null;
    }

    private void DrawInlineEditor(Rect position, SerializedProperty property, InlineEditorAttribute attr)
    {
        EditorGUI.indentLevel++;
        
        // Foldout
        Rect foldoutRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, "Edit Content", true);

        if (property.isExpanded)
        {
            // Create Editor
            if (_cachedEditor == null || _cachedEditor.target != property.objectReferenceValue)
            {
                Editor.CreateCachedEditor(property.objectReferenceValue, null, ref _cachedEditor);
            }

            // Draw Editor Background
            GUI.Box(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 5, position.width, position.height - EditorGUIUtility.singleLineHeight - 5), GUIContent.none, GUI.skin.box);

            // Draw Editor
            GUILayout.BeginArea(new Rect(position.x + 10, position.y + EditorGUIUtility.singleLineHeight * 2, position.width - 10, position.height));
            _cachedEditor.OnInspectorGUI();
            GUILayout.EndArea();
        }
        
        EditorGUI.indentLevel--;
    }

    // Standard height calculation (same as before, simplified for brevity)
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Use the same helper to check if we should calculate height
        if (property.objectReferenceValue == null || GetAttribute(property) == null) 
            return EditorGUIUtility.singleLineHeight;

        float totalHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.singleLineHeight + 5; // Header + Foldout

        if (property.isExpanded)
        {
            // Estimate height (Simulated)
            SerializedObject so = new SerializedObject(property.objectReferenceValue);
            SerializedProperty iter = so.GetIterator();
            bool enterChildren = true;
            while (iter.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (iter.propertyPath == "m_Script") continue;
                totalHeight += EditorGUI.GetPropertyHeight(iter, true) + EditorGUIUtility.standardVerticalSpacing;
            }
            totalHeight += 20; // Padding
        }
        return totalHeight;
    }

    private void CleanupEditor()
    {
        if (_cachedEditor != null)
        {
            UnityEngine.Object.DestroyImmediate(_cachedEditor);
            _cachedEditor = null;
        }
    }
}
}