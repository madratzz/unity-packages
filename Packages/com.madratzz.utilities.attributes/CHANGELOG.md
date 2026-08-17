# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [Unreleased]

### Changed
- `Singleton<T>.Instance` now uses `FindFirstObjectByType<T>()` on Unity 2023.1+ to silence the `FindObjectOfType<T>()` deprecation warning; the auto-create fallback for missing instances is preserved to keep existing call-sites working.
- TextMeshProExtensions is now hard-required: the package declares `com.unity.ugui 2.0.0` as a dependency (which ships TextMeshPro in Unity 6), the runtime asmdef references the `Unity.TextMeshPro` assembly, and the file no longer needs a `TMP_PRESENT` guard.
- Reviewed and validated package configuration for Verdaccio distribution.

## [0.0.1] - 2026-04-05

### Added
- Initial release
- `Singleton<T>`, `SingletonPersistent<T>` — generic MonoBehaviour singleton base classes
- `CoroutineHandler` — `SingletonPersistent` providing static coroutine management for non-MonoBehaviour callers
- `TimeCounter` — coroutine-based timer with pause/play/speed control and formatted output
- `CoroutineDelay`, `CoroutineCondition` — serializable coroutine sequence helpers
- `RectTransformExtensions` — anchor, pivot, size, and position manipulation helpers
- `ScrollRectExtensions` — snap-to-child utilities for `ScrollRect`
- `ImageExtensions` — `SetAlpha` for `Image`, `MaskableGraphic`, and `CanvasGroup`
- `TextMeshProExtensions` — `SetOpacity` for `TextMeshProUGUI`
- `DateTimeExtensions` — epoch conversion, range checks, elapsed time, and duration formatting
- `UnityExtensions` — `GetOrAddComponent<T>` extension
- `[ButtonAttribute]` / `[InlineEditorAttribute]` — custom inspector attributes
- `ButtonEditor` — renders `[ButtonAttribute]` methods as inspector buttons on MonoBehaviours and ScriptableObjects
- `InlineEditorDrawer` — renders `[InlineEditorAttribute]` fields with inline SO editing
- `EditorHelperMethods.FindAllScriptableObjectsOfType<T>` — asset discovery utility
- `UnitySerializedDictionary<TKey,TValue>` — inspector-serializable dictionary base class
- `DoNotDestroyGameObjectOnLoad` — MonoBehaviour helper for persistent scene objects
