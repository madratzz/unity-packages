# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).
The initial release collects all pre-release changes into a single entry; versions will increment once the package is first published to the Verdaccio registry.

## [0.0.1] - Unreleased

### Added
- Initial release
- `GameEvent` ScriptableObject — parameterless event asset with `Action`-based handler and `Invoke` method
- `GameEventListener` MonoBehaviour — subscribes to a `GameEvent` and forwards calls to a `UnityEvent`
- `GameEventRaiser` MonoBehaviour — invokes a stored `GameEvent` via `InvokeEvent`
- `GameEventRaiserOnEnable` MonoBehaviour — automatically invokes its `GameEvent` on `OnEnable`

### Changed
- Dependency moved from the retired `com.madratzz.utilities.extensions` to `com.madratzz.utilities.attributes`

### Fixed
- Removed unused `madratzz.scriptableobjectvariables.runtime` reference from runtime asmdef
- Corrected asmdef `name` from `madratzz.scriptableobjecteventsystem.runtime` to `madratzz.scriptableobject.eventsystem.runtime` (missing dot)
