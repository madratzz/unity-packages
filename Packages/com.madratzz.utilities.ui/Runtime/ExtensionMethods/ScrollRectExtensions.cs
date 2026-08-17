using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ExtensionMethods
{
    public static class ScrollRectExtensions
    {
        public static Vector2 GetSnapToPositionToBringChildIntoView(this ScrollRect scroller, RectTransform child)
        {
            Canvas.ForceUpdateCanvases();
            Vector2 viewportLocalPosition = scroller.viewport.localPosition;
            Vector2 childLocalPosition = child.localPosition;
            Vector2 result = new Vector2(
                0 - (viewportLocalPosition.x + childLocalPosition.x),
                0 - (viewportLocalPosition.y + childLocalPosition.y)
            );

            scroller.content.anchoredPosition = result;
            
            return result;
        }

        /// <summary>
        /// Snaps the Scroll View to the Child Rect i.e. moves to that child position
        /// </summary>
        /// <param name="scroller">Ref Scroll Rect</param>
        /// <param name="child">Child Rect in the Scroll Rect</param>
        /// <param name="offsetX">Offset in XPos</param>
        /// <param name="offsetY">Offset in YPos</param>
        public static void SnapTo(this ScrollRect scroller, RectTransform child, float offsetX = 0, float offsetY = 0)
        {
            Canvas.ForceUpdateCanvases();

            var contentPos = (Vector2) scroller.transform.InverseTransformPoint(scroller.content.position);
            var childPos = (Vector2) scroller.transform.InverseTransformPoint(child.position);
            var endPos = contentPos - childPos;
            // If no horizontal scroll, then don't change contentPos.x
            if (!scroller.horizontal) endPos.x = contentPos.x + offsetX;
            // If no vertical scroll, then don't change contentPos.y
            if (!scroller.vertical) endPos.y = contentPos.y + offsetY;

            endPos.x += offsetX;
            endPos.y += offsetY;
            scroller.content.anchoredPosition = endPos;
        }

        /// <summary>
        /// Returns the Position of the child in the ScrollRect
        /// </summary>
        /// <param name="scroller">Ref Scroll Rect</param>
        /// <param name="child">Child Rect in the Scroll Rect</param>
        /// <param name="offsetX">Offset in XPos</param>
        /// <param name="offsetY">Offset in YPos</param>
        /// <returns></returns>
        public static Vector2 GetSnapPosition(this ScrollRect scroller, RectTransform child, float offsetX = 0,
                                              float offsetY = 0)
        {
            Canvas.ForceUpdateCanvases();

            var contentPos = (Vector2) scroller.transform.InverseTransformPoint(scroller.content.position);
            var childPos = (Vector2) scroller.transform.InverseTransformPoint(child.position);
            var endPos = contentPos - childPos;
            // If no horizontal scroll, then don't change contentPos.x
            if (!scroller.horizontal) endPos.x = contentPos.x + offsetX;
            // If no vertical scroll, then don't change contentPos.y
            if (!scroller.vertical) endPos.y = contentPos.y + offsetY;

            endPos.x += offsetX;
            endPos.y += offsetY;

            return endPos;
        }
    }
}
