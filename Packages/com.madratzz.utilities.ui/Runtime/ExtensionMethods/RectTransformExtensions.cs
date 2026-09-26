using System;
using UnityEngine;

namespace ExtensionMethods
{
  public static class RectTransformExtensions
  {
    public static void AnchorToCorners(this RectTransform transform)
    {
      if (transform == null)
        throw new ArgumentNullException("transform");

      if (transform.parent == null)
        return;

      var parent = transform.parent.GetComponent<RectTransform>();

      var rect = parent.rect;
      Vector2 newAnchorsMin = new Vector2(transform.anchorMin.x + transform.offsetMin.x / rect.width,
        transform.anchorMin.y + transform.offsetMin.y / rect.height);

      Vector2 newAnchorsMax = new Vector2(transform.anchorMax.x + transform.offsetMax.x / rect.width,
        transform.anchorMax.y + transform.offsetMax.y / rect.height);

      transform.anchorMin = newAnchorsMin;
      transform.anchorMax = newAnchorsMax;
      transform.offsetMin = transform.offsetMax = new Vector2(0, 0);
    }

    public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec)
    {
      trans.pivot = aVec;
      trans.anchorMin = aVec;
      trans.anchorMax = aVec;
    }

    public static Vector2 GetSize(this RectTransform trans)
    {
      return trans.rect.size;
    }

    public static float GetWidth(this RectTransform trans)
    {
      return trans.rect.width;
    }

    public static float GetHeight(this RectTransform trans)
    {
      return trans.rect.height;
    }

    public static void SetSize(this RectTransform trans, Vector2 newSize)
    {
      Vector2 oldSize = trans.rect.size;
      Vector2 deltaSize = newSize - oldSize;
      Vector2 pivot;
      trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * (pivot = trans.pivot).x, deltaSize.y * pivot.y);
      trans.offsetMax = trans.offsetMax +
                        new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - pivot.y));
    }

    public static void SetWidth(this RectTransform trans, float newSize)
    {
      SetSize(trans, new Vector2(newSize, trans.rect.size.y));
    }

    public static void SetHeight(this RectTransform trans, float newSize)
    {
      SetSize(trans, new Vector2(trans.rect.size.x, newSize));
    }

    public static void SetBottomLeftPosition(this RectTransform trans, Vector2 newPos)
    {
      trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width),
        newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    public static void SetTopLeftPosition(this RectTransform trans, Vector2 newPos)
    {
      trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width),
        newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static void SetBottomRightPosition(this RectTransform trans, Vector2 newPos)
    {
      trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width),
        newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos)
    {
      trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width),
        newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static void SetAnchoredPositionX(this RectTransform trans, float targetX)
    {
      var position = trans.anchoredPosition;
      position.x = targetX;
      trans.anchoredPosition = position;
    }
    public static void SetAnchoredPositionY(this RectTransform trans, float targetY)
    {
      var position = trans.anchoredPosition;
      position.y = targetY;
      trans.anchoredPosition = position;
    }
    public static void SetAnchoredPositionXY(this RectTransform trans, Vector2 pos)
    {
      var position = trans.anchoredPosition;
      position.x = pos.x;
      position.y = pos.y;
      trans.anchoredPosition = position;
    }

    public static void SetScale(this RectTransform trans, float scale)
    {
      var localScale = trans.localScale;
      localScale.x = scale;
      localScale.y = scale;
      localScale.z = scale;
      trans.localScale = localScale;
    }

    public static void SetAnchorAndPositionAsStretched(this RectTransform rectTransform)
    {
      rectTransform.anchorMax = new Vector2(1, 1);
      rectTransform.anchorMin = new Vector2(0, 0);
      rectTransform.pivot = new Vector2(0.5f, 0.5f);
      rectTransform.rect.Set(0,0,0,0);
      rectTransform.localScale = Vector3.one;
      rectTransform.anchoredPosition = Vector2.zero;
      rectTransform.localPosition = Vector3.zero;
      rectTransform.sizeDelta = Vector2.zero;
    }
  }
}
