# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [0.0.1] - 2026-08-17

### Added
- Initial release — split out of `com.madratzz.utilities.extensions`
- `CoroutineHandler` — `SingletonPersistent` static coroutine runner for non-MonoBehaviour callers, with `AfterWait` delays, `WaitLoop` condition loops, and per-delay update loops
- `TimeCounter` — coroutine-based timer with pause/play/speed control and formatted output
- `CoroutineSequence`, `CoroutineDelay`, `CoroutineCondition` — serializable coroutine sequence steps
- `CoroutineStatic` — MonoBehaviour extension methods mirroring `AfterWait`

### Changed
- Now depends on `com.madratzz.utilities.core` for `SingletonPersistent<T>` instead of shipping the singleton classes itself
