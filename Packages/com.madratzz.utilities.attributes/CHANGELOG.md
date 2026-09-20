# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).
The initial release collects all pre-release changes into a single entry; versions will increment once the package is first published to the Verdaccio registry.

## [0.0.1] - Unreleased

### Added
- Initial release — split out of `com.madratzz.utilities.extensions` into a zero-dependency leaf package
- `InlineEditorAttribute` — renders a target ScriptableObject (or referenced object) inline in the inspector; usable on fields, properties, classes, and structs
- `ButtonAttribute` — invokes a method from a clickable inspector button, with optional name and height
- `InlineEditorDrawer` — property drawer backing `InlineEditorAttribute`
- `ButtonEditor` — inspector button rendering for `[Button]` methods
- `EditorHelperMethods` — shared editor GUI helpers used by the drawers
- `RequireReferenceAttribute` + `RequiredReferenceValidator` — marker attribute for required `[SerializeField]` references, and an Editor scanner (menu item + `-executeMethod` CI entry point) that reports every scene/prefab field carrying it that's unassigned. Added in response to real bugs found in this project where an unassigned or mismatched `GameEvent`/`FiniteStateMachine` reference silently broke a feature with no error.
- First EditMode tests for this package (`Tests/EditMode/RequiredReferenceValidatorTests.cs`), covering `RequiredReferenceValidator`'s scan against constructed test GameObjects — unassigned/assigned/unmarked fields, and that the reported hierarchy path is correct for a nested GameObject. Exercises `ValidateGameObject` directly via an `internal` seam (`Editor/AssemblyInfo.cs`'s `InternalsVisibleTo`, matching the pattern already used in `com.madratzz.utilities.buildautomation`).
- `SearchableAssetDrawer`/`SearchableAssetDropdown`/`SearchableAssetFinder` — draws every `ScriptableObject`-typed `[SerializeField]` field as a searchable `AdvancedDropdown` over every project asset of the field's type, instead of the default object picker. Registered against the `ScriptableObject` type itself (`[CustomPropertyDrawer(typeof(ScriptableObject), true)]`, not a marker attribute), so it applies automatically to every field of every `ScriptableObject` subclass, project-wide, with nothing to add per field or per class. No third-party dependency (built on Unity's own `AdvancedDropdown`, the widget behind "Add Component") — evaluated as the free alternative to an Odin Inspector `[ValueDropdown]` before committing to a paid dependency. `SearchableAssetFinder`'s lookup logic is covered by `Tests/EditMode/SearchableAssetFinderTests.cs` (the dropdown UI itself isn't unit-testable).
