# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [Unreleased]

### Fixed
- Corrected asmdef `name` typo `madratzz.scriptableoject.variables.database` → `madratzz.scriptableobject.variables.database.runtime`.

## [0.0.1] - 2026-04-05

### Added
- Initial release
- `Database` ScriptableObject — PlayerPrefs abstraction layer
- `IDBVariable` interface — contract for persistent variable operations (Save, Load, Update)
- `DBInt`, `DBFloat`, `DBBool`, `DBString` — persistent typed variables with auto-save on `SetValue`
- `DBEpochTime` — persistent int variable with epoch time arithmetic helpers
- `DBManager` — static registry with JSON import/export for all active DB variables

### Fixed
- Save and Load methods
