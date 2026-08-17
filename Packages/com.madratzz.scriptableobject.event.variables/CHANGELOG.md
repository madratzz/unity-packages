# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [0.0.2] - 2026-08-17

### Fixed
- `BoolWithEvent`, `DBBoolWithEvent`, `DBIntWithEvent` no longer throw `NullReferenceException` when the `ValueChanged` event asset is unassigned — event raise and `AddListener`/`RemoveListener` are now null-guarded
- Corrected asmdef `name` from `madratzz.scriptableobjecteventvariables.runtime` to `madratzz.scriptableobject.event.variables.runtime` (missing dot)

## [0.0.1] - 2026-04-05

### Added
- Initial release
- `BoolWithEvent` — `Bool` variable that raises a `GameEvent` on value change
- `DBBoolWithEvent` — `DBBool` variable that raises a `GameEvent` on value change
- `DBIntWithEvent` — `DBInt` variable that raises a `GameEvent` on value change or `ApplyChange`
