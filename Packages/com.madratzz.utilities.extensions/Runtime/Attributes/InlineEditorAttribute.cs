using System;
using UnityEngine;

namespace CustomUtilities.Attributes
{
    // Updated to allow usage on Fields, Properties, AND Classes/Structs
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct)]
    public class InlineEditorAttribute : PropertyAttribute
    {
        public bool Expanded = false;

        public InlineEditorAttribute(bool expanded = false)
        {
            this.Expanded = expanded;
        }
    }
}