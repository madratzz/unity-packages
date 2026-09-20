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
| `SearchableAssetDrawer` / `SearchableAssetDropdown` / `SearchableAssetFinder` | Registered against `ScriptableObject` itself (`useForChildren: true`), so **every** `ScriptableObject`-typed `[SerializeField]` field in the project — no attribute needed — renders as a button opening an `AdvancedDropdown` (the same searchable tree widget behind "Add Component") listing every project asset of the field's type via `AssetDatabase.FindAssets`, instead of the default object picker. |

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

## Searchable dropdowns for ScriptableObject fields

No attribute, no setup — every `[SerializeField]` field typed as `ScriptableObject` (or any subclass: `GameEvent`, a `Variables` type, a custom asset type, …) automatically gets a searchable-dropdown Inspector button instead of the default object picker, the moment `com.madratzz.utilities.attributes` is in the project. `SearchableAssetDrawer` is registered against the `ScriptableObject` type itself (`[CustomPropertyDrawer(typeof(ScriptableObject), true)]`), not against a marker attribute, so it applies globally and automatically to every derived type — no per-field or per-class opt-in, ever.

Combine with `[RequireReference]` where a field is mandatory — one makes it easy to pick the right asset, the other catches it if nobody did. Requires no third-party dependency; it's built on Unity's own `AdvancedDropdown` (Editor-only, `UnityEditor.IMGUI.Controls`) — evaluated as the free alternative to an Odin Inspector `[ValueDropdown]` before committing to a paid dependency.

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
