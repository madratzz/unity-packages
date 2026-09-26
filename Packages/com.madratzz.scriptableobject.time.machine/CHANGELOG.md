# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).
The initial release collects all pre-release changes into a single entry; versions will increment once the package is first published to the Verdaccio registry.

## [0.0.1] - Unreleased

### Added
- Initial release
- `TimeMachine` ScriptableObject — coroutine-based interval tick system
- `StartTicking()` / `StopTicking()` — the SO starts and stops its own loop via `CoroutineHandler` (survives scene loads, no MonoBehaviour ownership needed); safe to call redundantly
- `TickInterval` (default 1s) and `UseRealTime` (scaled vs unscaled time) Inspector options
- `TimeMachine Sample Assets` — pre-configured `TimeMachine` and `e_TimeMachineTick` assets (under `Samples~/`)

### Changed
- Now depends on `com.madratzz.utilities.coroutines` for the persistent coroutine runner (in addition to `scriptableobject.eventsystem.core`)
- Example assets (`e_TimeMachineTick.asset`, `TimeMachine.asset`) ship under `Runtime/ExampleAssets/` so they're available immediately when the package is imported — they reference types via fileID, not GUID, so they survive fresh imports unchanged

### Fixed
- `TickEvent.Invoke()` no longer throws `NullReferenceException` when the tick event asset is unassigned — the tick loop runs and skips the raise
