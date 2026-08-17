# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [0.0.2] - 2026-08-17

### Removed
- `DBManager.GetJsonData()` — stub that silently returned `null`; JSON export will return as a real implementation in a future release

### Changed
- Dependency moved from the retired `com.madratzz.utilities.extensions` to `com.madratzz.utilities.attributes`

### Fixed
- Corrected asmdef `name` typo `madratzz.scriptableoject.variables.database` → `madratzz.scriptableobject.variables.database.runtime`

## [0.0.1] - 2026-04-05

### Added
- Initial release
- `Database` ScriptableObject — PlayerPrefs abstraction layer
- `IDBVariable` interface — contract for persistent variable operations (Save, Load, Update)
- `DBInt`, `DBFloat`, `DBBool`, `DBString` — persistent typed variables with auto-save on `SetValue`
- `DBEpochTime` — persistent int variable with epoch time arithmetic helpers
- `DBManager` — static registry with JSON import for all active DB variables

### Fixed
- Save and Load methods
