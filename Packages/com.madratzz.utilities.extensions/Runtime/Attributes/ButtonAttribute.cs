using System;
using UnityEngine;

namespace CustomUtilities.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ButtonAttribute : PropertyAttribute
    {
        public string ButtonName;
        public float Height;

        public ButtonAttribute(string buttonName = null, float height = 0)
        {
            this.ButtonName = buttonName;
            this.Height = height;
        }
    }
}