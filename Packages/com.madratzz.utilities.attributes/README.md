# Inspector Attributes

Custom inspector attributes with editor drawers. Zero-dependency — safe as a leaf dependency for any package.

## Runtime Attributes

| Type | Description |
|------|-------------|
| `InlineEditorAttribute` | Renders a referenced object (or the target ScriptableObject class) inline in the inspector, with optional expanded state. Usable on fields, properties, classes, and structs. |
| `ButtonAttribute` | Adds a clickable inspector button that invokes the decorated method. Optional custom button name and height. |
| `RequireReferenceAttribute` | Marks a `[SerializeField]` reference field as required, for `RequiredReferenceValidator` to check. |

## Editor Drawers

| Type | Description |
|------|-------------|
| `InlineEditorDrawer` | Property drawer backing `InlineEditorAttribute` |
| `ButtonEditor` | Inspector button rendering for `ButtonAttribute` methods |
| `EditorHelperMethods` | Shared editor GUI helpers used by the drawers |
| `RequiredReferenceValidator` | Scans every scene and prefab under `Assets/` for `[RequireReference]` fields that are unassigned and logs an error per hit. Run via **Tools → Validate Required References**, or in CI with `-batchmode -executeMethod CustomEditorUtilities.RequiredReferenceValidator.ValidateProjectCI` (exits non-zero on any missing reference). |

## Marking a field as required

```csharp
using CustomUtilities.Attributes;

public class MyController : MonoBehaviour
{
    [RequireReference]
    [SerializeField] private SomeScriptableObject Dependency;
}
```

The attribute itself has no runtime behavior — it's a marker `RequiredReferenceValidator` reflects over. Only mark fields the component genuinely cannot function without (things that already `NullReferenceException` or hard-fail when unassigned); leave truly optional fields unmarked.

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
