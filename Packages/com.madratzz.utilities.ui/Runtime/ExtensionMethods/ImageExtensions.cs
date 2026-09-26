using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ExtensionMethods
{
    public static class ImageExtensions
    {
        public static void SetAlpha(this Image image, float alpha)
        {
            var currentColor = image.color;
            currentColor.a = alpha;
            image.color = currentColor;
        }

        public static void SetAlpha(this MaskableGraphic uiElement, float alpha)
        {
            var currentColor = uiElement.color;
            currentColor.a = alpha;
            uiElement.color = currentColor;
        }

        public static void SetAlpha(this CanvasGroup canvasGroup, float alpha)
        {
            canvasGroup.alpha = alpha;
        }

    }
}