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
- All three `GameEvent` fields now get a searchable-dropdown Inspector automatically when `com.madratzz.utilities.attributes` is in the project — its `SearchableAssetDrawer` is registered against `ScriptableObject` itself, so this needs no attribute or other change here; noted for visibility since it changes the Inspector for these fields.

### Fixed
- Removed unused `madratzz.scriptableobjectvariables.runtime` reference from runtime asmdef
- Corrected asmdef `name` from `madratzz.scriptableobjecteventsystem.runtime` to `madratzz.scriptableobject.eventsystem.runtime` (missing dot)
- `GameEventListener.OnEnable`/`OnDisable`, `GameEventRaiser.InvokeEvent`, and `GameEventRaiserOnEnable.OnEnable` no longer throw `NullReferenceException` when their `GameEvent` field is unassigned — they now log a `Debug.LogError` and no-op instead. All three fields are marked `[RequireReference]` (from `com.madratzz.utilities.attributes`) so `RequiredReferenceValidator` catches an unassigned one before it ever runs.
- Added `Tests/EditMode/GameEventMonoTests.cs` — regression coverage for the null-guard fix above on all three MonoBehaviours (`LogAssert.Expect` + `Assert.DoesNotThrow`), plus a sanity check that a correctly-assigned `GameEventRaiser` still raises its event.
