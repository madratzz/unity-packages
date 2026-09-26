# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).
The initial release collects all pre-release changes into a single entry; versions will increment once the package is first published to the Verdaccio registry.

## [0.0.1] - Unreleased

### Added
- Initial release
- `AddressablesHelper` static class with coroutine-based safe asset loading
- `Instantiate<T>` — validates `AssetReference`, instantiates, extracts a typed component, releases the instance and reports on any failure path
- `InstantiateGameObject` — validates `AssetReference`, instantiates, returns the `GameObject` directly with the same release semantics
- Error logs include the calling object as Unity's log context (clicking the log highlights the caller in the Hierarchy)

### Changed
- `onFailure` callback is now `Action<AsyncOperationHandle<GameObject>>` so callers can inspect the handle's status/exception — the contextual error log still fires regardless
- `Instantiate<T>` and `InstantiateGameObject` now wrap `onSuccess` in try/catch: if the success handler throws, the instance is released (no more leak on caller bug)
- `com.unity.addressables` pinned to `2.9.1` (current Unity 6 default)
