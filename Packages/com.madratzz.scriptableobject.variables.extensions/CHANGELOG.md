# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [Unreleased]

No pending changes.

## [0.0.1] - 2026-04-05

### Added
- Initial release
- `Array<T>`, `ArrayInt` — generic and concrete SO-backed list types with add/remove/insert support
- `Vector2Shared`, `Vector3Shared` — SO-backed shared vector values with `ApplyChange` support
- `RuntimeDictionary<TKey,TValue>`, `RuntimeDictionaryIntInt` — SO-backed serialized dictionaries (requires Odin Inspector; guarded by `ODIN_INSPECTOR` scripting define)
- README documenting Odin Inspector requirement for `RuntimeDictionary` types
