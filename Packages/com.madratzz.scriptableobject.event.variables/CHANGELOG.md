# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).
The initial release collects all pre-release changes into a single entry; versions will increment once the package is first published to the Verdaccio registry.

## [0.0.1] - Unreleased

### Added
- Initial release
- `BoolWithEvent` — `Bool` variable that raises a `GameEvent` on value change
- `DBBoolWithEvent` — `DBBool` variable that raises a `GameEvent` on value change
- `DBIntWithEvent` — `DBInt` variable that raises a `GameEvent` on value change or `ApplyChange`

### Fixed
- `BoolWithEvent`, `DBBoolWithEvent`, `DBIntWithEvent` no longer throw `NullReferenceException` when the `ValueChanged` event asset is unassigned — event raise and `AddListener`/`RemoveListener` are now null-guarded
- Corrected asmdef `name` from `madratzz.scriptableobjecteventvariables.runtime` to `madratzz.scriptableobject.event.variables.runtime` (missing dot)
