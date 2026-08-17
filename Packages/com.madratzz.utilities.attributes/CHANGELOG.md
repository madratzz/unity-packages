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
