# Utilities - Attributes

← [[Home]] · [[Architecture Overview]]

**Package:** `com.madratzz.utilities.attributes` · [Full README & API](../../Packages/com.madratzz.utilities.attributes/README.md)

Custom inspector attributes with editor drawers — `[InlineEditor]` and `[Button]`. Zero-dependency, so it's safe as a leaf dependency for any other package.

## Depends on

None — zero-dependency leaf package.

## Used by

- [[SOAP - Variables]]
- [[SOAP - Variables Database]]
- [[SOAP - Event System]]
- [[SOAP - Event System Extensions]]

## Key types

| Type | Description |
|---|---|
| `InlineEditorAttribute` | Renders a referenced object (or the target class) inline in the inspector. |
| `ButtonAttribute` | Adds a clickable inspector button that invokes the decorated method. |
| `InlineEditorDrawer` / `ButtonEditor` | Editor drawers backing the two attributes above. |

## Quick usage

```csharp
using CustomUtilities.Attributes;

[InlineEditor(Expanded = false)]
public class MySettings : ScriptableObject { }
```

Namespace: `CustomUtilities.Attributes` (runtime), `CustomEditorUtilities` (editor).

---
← [[Home]]
