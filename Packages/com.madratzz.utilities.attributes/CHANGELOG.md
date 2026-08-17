# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [0.0.1] - 2026-08-17

### Added
- Initial release — split out of `com.madratzz.utilities.extensions` into a zero-dependency leaf package
- `InlineEditorAttribute` — renders a target ScriptableObject (or referenced object) inline in the inspector; usable on fields, properties, classes, and structs
- `ButtonAttribute` — invokes a method from a clickable inspector button, with optional name and height
- `InlineEditorDrawer` — property drawer backing `InlineEditorAttribute`
- `ButtonEditor` — inspector button rendering for `[Button]` methods
- `EditorHelperMethods` — shared editor GUI helpers used by the drawers
