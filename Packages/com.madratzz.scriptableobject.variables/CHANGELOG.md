# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [Unreleased]

### Fixed
- Corrected asmdef `name` from `madratzz.scriptableobjectvariables.runtime` to `madratzz.scriptableobject.variables.runtime` (missing dot).

## [0.0.1] - 2026-04-05

### Added
- Initial release
- `IVariable<T>` interface — typed get/set, default value management, and reset
- `IApplyChange<T>` interface — cumulative value modification
- `Int`, `Float` — ScriptableObject numeric variables implementing both interfaces
- `Bool`, `String` — ScriptableObject variables implementing `IVariable<T>`
