# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [0.0.2] - 2026-08-17

### Security
- Removed hardcoded keystore credentials (`sgs123`) from `BuilderConfig` defaults and deleted the committed `BuilderConfig.asset` — credentials now default to empty and must be provided via an uncommitted `Resources/BuilderConfig` asset or Player Settings

### Fixed
- `Builder` no longer throws `NullReferenceException` on any build: the static `BuilderConfig` field was never assigned; `BuilderConfig.LoadDefault()` now loads from `Resources/BuilderConfig` with a blank-instance fallback
- Keystore credentials are only applied for Android builds (were also set on iOS)
- `CreateAssetMenu` restored on `BuilderConfig` (was commented out, making the config asset uncreatable from the menu)

### Changed
- `TryGenerateVersionCode` and `GetEnabledScenePaths` extracted as internal seams for EditMode tests (no behavior change)

## [0.0.1] - 2026-04-05

### Added
- Initial release
- `BuilderConfig` ScriptableObject — stores Android keystore path and credentials
- `Builder` editor utility — `Build/` menu items for iOS, Android APK, and Android AAB targets
