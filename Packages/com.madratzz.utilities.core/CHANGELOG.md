# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [0.0.1] - 2026-08-17

### Added
- Initial release — split out of `com.madratzz.utilities.extensions`
- `Singleton<T>`, `SingletonPersistent<T>` — generic MonoBehaviour singleton base classes (legacy interop; see README for new-code guidance)
- `UnitySerializedDictionary<TKey,TValue>` — inspector-serializable dictionary base class
- `DoNotDestroyGameObjectOnLoad` — MonoBehaviour helper for persistent scene objects
- `DateTimeExtensions` — epoch conversion, range checks, elapsed time, and duration formatting
- `Utilities` — string-to-number parsing (`ToInt`/`ToFloat`/`ToDouble`/`ToBool`) and epoch/`mm:ss` time helpers

### Fixed
- `UnityExtensions.GetOrAddComponent<T>` no longer returns `null` on the add path — it now uses `TryGetComponent` and returns the added component; constraint widened from `MonoBehaviour` to `Component`

### Removed
- `Utilities.GetDeviceId` — broken on iOS (referenced undefined symbols) and replaced by `com.madratzz.platform.device`'s `DeviceIdentity`
- `Utilities.ParseImageName`, `Utilities.ParseDialogues` — game-specific helpers that did not belong in a shared package
