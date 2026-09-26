# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).
The initial release collects all pre-release changes into a single entry; versions will increment once the package is first published to the Verdaccio registry.

## [0.0.1] - Unreleased

### Added
- Initial release
- `Array<T>`, `ArrayInt` — generic and concrete SO-backed list types with add/remove/insert support
- `Vector2Shared`, `Vector3Shared` — SO-backed shared vector values with `ApplyChange` support
- `RuntimeDictionary<TKey,TValue>`, `RuntimeDictionaryIntInt` — SO-backed serialized dictionaries (requires Odin Inspector; guarded by `ODIN_INSPECTOR` scripting define)
- README documenting Odin Inspector requirement for `RuntimeDictionary` types

### Changed
- `Vector3Shared` moved into the `ProjectCore.Variables` namespace (was global) — consistent with `Vector2Shared` and the rest of the package; update any `using` directives in consuming code
- Runtime asmdef no longer references the retired `com.madratzz.utilities.extensions.runtime` assembly (the package only needs `madratzz.scriptableobject.variables.runtime`)

### Fixed
- `Array<T>.list` is now initialized inline — previously null on runtime-created assets, causing `NullReferenceException` on the first `Add`/`Remove` (caught by the new EditMode tests)
