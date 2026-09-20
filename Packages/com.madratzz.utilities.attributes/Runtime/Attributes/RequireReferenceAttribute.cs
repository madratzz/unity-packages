using System;
using UnityEngine;

namespace CustomUtilities.Attributes
{
    // Marks a [SerializeField] reference as required. RequiredReferenceValidator
    // (Editor) scans scenes and prefabs for fields carrying this attribute and
    // reports any that are unassigned.
    [AttributeUsage(AttributeTargets.Field)]
    public class RequireReferenceAttribute : PropertyAttribute
    {
    }
}
