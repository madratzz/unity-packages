# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).
The initial release collects all pre-release changes into a single entry; versions will increment once the package is first published to the Verdaccio registry.

## [0.0.1] - Unreleased

### Added
- Initial release
- `IVariable<T>` interface — typed get/set, default value management, and reset
- `IApplyChange<T>` interface — cumulative value modification
- `Int`, `Float` — ScriptableObject numeric variables implementing both interfaces
- `Bool`, `String` — ScriptableObject variables implementing `IVariable<T>`

### Changed
- Dependency moved from the retired `com.madratzz.utilities.extensions` to `com.madratzz.utilities.attributes` (zero-dependency attributes package); the runtime asmdef reference updated accordingly

### Fixed
- Corrected asmdef `name` from `madratzz.scriptableobjectvariables.runtime` to `madratzz.scriptableobject.variables.runtime` (missing dot)
