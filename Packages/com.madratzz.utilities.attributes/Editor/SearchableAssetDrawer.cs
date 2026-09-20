using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace CustomEditorUtilities
{
    // Registered against ScriptableObject itself (useForChildren: true), so
    // every ScriptableObject-typed [SerializeField] in the project gets this
    // treatment automatically — no per-field attribute needed. See
    // SearchableAssetFinder for the asset lookup and SearchableAssetDropdown
    // for the AdvancedDropdown UI.
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
    public class SearchableAssetDrawer : PropertyDrawer
    {
        private readonly AdvancedDropdownState _dropdownState = new AdvancedDropdownState();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            position = EditorGUI.PrefixLabel(position, label);

            string buttonLabel = property.objectReferenceValue != null
                ? property.objectReferenceValue.name
                : "None";

            if (EditorGUI.DropdownButton(position, new GUIContent(buttonLabel), FocusType.Keyboard))
            {
                SerializedProperty capturedProperty = property.Copy();
                var dropdown = new SearchableAssetDropdown(_dropdownState, ResolveElementType(fieldInfo.FieldType), selected =>
                {
                    capturedProperty.serializedObject.Update();
                    capturedProperty.objectReferenceValue = selected;
                    capturedProperty.serializedObject.ApplyModifiedProperties();
                });
                dropdown.Show(position);
            }
        }

        // property.objectReferenceValue's field can be a raw type, an array,
        // or a List<T> — the dropdown always needs the element type, not the
        // collection type, to search for the right assets.
        private static Type ResolveElementType(Type fieldType)
        {
            if (fieldType.IsArray)
                return fieldType.GetElementType();

            if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(System.Collections.Generic.List<>))
                return fieldType.GetGenericArguments()[0];

            return fieldType;
        }
    }
}
