# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [0.0.1] - 2026-08-17

### Added
- Initial release — split out of `com.madratzz.utilities.extensions`; the only `utilities.*` package depending on `com.unity.ugui`/TextMeshPro
- `ImageExtensions` — `SetAlpha` for `Image`, `MaskableGraphic`, and `CanvasGroup`
- `TextMeshProExtensions` — `SetOpacity` for `TextMeshProUGUI`
- `RectTransformExtensions` — anchor, pivot, size, and position manipulation helpers
- `ScrollRectExtensions` — snap-to-child utilities for `ScrollRect`
- `RectTransformUtilities.SwitchToRectTransform` — converts anchored position between two RectTransforms (moved from the old shared `Utilities` class)
