# Utilities - UI

← [[Home]] · [[Architecture Overview]]

**Package:** `com.madratzz.utilities.ui` · [Full README & API](../../Packages/com.madratzz.utilities.ui/README.md)

uGUI and TextMeshPro extension methods: alpha/opacity helpers, RectTransform anchoring, ScrollRect snapping, coordinate conversion. This is the only `utilities.*` package that depends on `com.unity.ugui` — depend on it only from packages that actually render UI.

## Depends on

- `com.unity.ugui` (`2.0.0`, external — provides TextMeshPro in Unity 6). No dependency on other packages in this repo.

## Used by

Nothing in this repo yet.

## Key types

| Type | Members |
|---|---|
| `ImageExtensions` | `SetAlpha` for `Image`, `MaskableGraphic`, `CanvasGroup`. |
| `TextMeshProExtensions` | `SetOpacity` for `TextMeshProUGUI`. |
| `RectTransformExtensions` | `AnchorToCorners`, `SetPivotAndAnchors`, size/position helpers. |
| `ScrollRectExtensions` | `SnapTo`, `GetSnapToPositionToBringChildIntoView`. |
| `RectTransformUtilities.SwitchToRectTransform` | Converts anchored position between two RectTransforms. |

Namespaces: `ExtensionMethods` (extension methods), `CustomUtilities` (`RectTransformUtilities`).

---
← [[Home]]
