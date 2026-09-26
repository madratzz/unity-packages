# UI Extensions

uGUI and TextMeshPro extension methods. This is the only `com.madratzz.utilities.*` package that
depends on `com.unity.ugui` / TextMeshPro — depend on it only from packages that actually render UI.

## Contents

| Type | Members |
|------|---------|
| `ImageExtensions` | `SetAlpha` for `Image`, `MaskableGraphic`, `CanvasGroup` |
| `TextMeshProExtensions` | `SetOpacity` for `TextMeshProUGUI` |
| `RectTransformExtensions` | `AnchorToCorners`, `SetPivotAndAnchors`, size and position helpers |
| `ScrollRectExtensions` | `SnapTo`, `GetSnapToPositionToBringChildIntoView` |
| `RectTransformUtilities` | `SwitchToRectTransform` — convert anchored position between two RectTransforms |

Namespaces: `ExtensionMethods` (extension methods), `CustomUtilities` (`RectTransformUtilities`).

Depends on `com.unity.ugui 2.0.0` (which provides TextMeshPro in Unity 6).
