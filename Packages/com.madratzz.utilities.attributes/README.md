# Inspector Attributes

Custom inspector attributes with editor drawers. Zero-dependency — safe as a leaf dependency for any package.

## Runtime Attributes

| Type | Description |
|------|-------------|
| `InlineEditorAttribute` | Renders a referenced object (or the target ScriptableObject class) inline in the inspector, with optional expanded state. Usable on fields, properties, classes, and structs. |
| `ButtonAttribute` | Adds a clickable inspector button that invokes the decorated method. Optional custom button name and height. |

## Editor Drawers

| Type | Description |
|------|-------------|
| `InlineEditorDrawer` | Property drawer backing `InlineEditorAttribute` |
| `ButtonEditor` | Inspector button rendering for `ButtonAttribute` methods |
| `EditorHelperMethods` | Shared editor GUI helpers used by the drawers |

## Usage

```csharp
using CustomUtilities.Attributes;

public class Int : ScriptableObject
{
    // Renders this asset's inspector inline when referenced as a field
}

[InlineEditor(Expanded = false)]
public class MySettings : ScriptableObject { }
```

Namespace: `CustomUtilities.Attributes` (runtime), `CustomEditorUtilities` (editor).
