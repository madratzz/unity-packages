# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).
The initial release collects all pre-release changes into a single entry; versions will increment once the package is first published to the Verdaccio registry.

## [0.0.1] - Unreleased

### Added
- Initial release
- `GameEventWithParam<T>` — generic single-parameter event base class
- `GameEventWithParam<T,U,V>` — generic three-parameter event base class
- `GameEventWithReturn<T>` — generic return-value event base class
- Concrete types: `GameEventWithString`, `GameEventWithInt`, `GameEventWithFloat`, `GameEventWithBool`, `GameEventWithSprite`, `GameEventWithIntStringBool`
- `GameEventReturnsVector3` — return-value event for optional `Vector3` with exception handling

### Changed
- Dependency moved from the retired `com.madratzz.utilities.extensions` to `com.madratzz.utilities.attributes`
- `GameEventWithReturn<T>.Raise()` now returns `default(T)` when no handler is subscribed instead of throwing `NullReferenceException`; `GameEventReturnsVector3`'s swallow-all try/catch override removed as redundant

### Fixed
- `GameEventWithIntStringBool` asset menu name now reads "Game Event With Int, String, Bool" (the Bool parameter was missing)
