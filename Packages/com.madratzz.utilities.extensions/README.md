# Extension Methods

Runtime and editor utilities including UI extension methods, DateTime helpers, coroutine management, singleton base classes, serialized dictionary, and custom inspector attributes.

## Overview

A general-purpose utility package providing the foundational building blocks used across the other `com.madratzz` packages. Includes singleton patterns, a static coroutine manager, a rich set of UI and DateTime extension methods, and custom inspector attributes for MonoBehaviours and ScriptableObjects.

## Singletons

| Type | Description |
|------|-------------|
| `Singleton<T>` | Generic MonoBehaviour singleton with lazy instance creation |
| `SingletonPersistent<T>` | Extends `Singleton<T>` with `DontDestroyOnLoad` |

## Coroutine Utilities

| Type | Description |
|------|-------------|
| `CoroutineHandler` | `SingletonPersistent` providing static `StartCoroutine` / `StopCoroutine` for non-MonoBehaviour callers |
| `TimeCounter` | Coroutine-based countdown/countup timer with pause, play, and speed control |
| `CoroutineDelay` | Serializable sequence: executes an action after a configurable delay |
| `CoroutineCondition` | Serializable sequence: executes an action when a condition becomes true |

## UI Extension Methods

| Type | Members |
|------|---------|
| `RectTransformExtensions` | `AnchorToCorners`, `SetPivotAndAnchors`, size and position helpers |
| `ScrollRectExtensions` | `SnapTo`, `GetSnapToPositionToBringChildIntoView` |
| `ImageExtensions` | `SetAlpha` for `Image`, `MaskableGraphic`, `CanvasGroup` |
| `TextMeshProExtensions` | `SetOpacity` for `TextMeshProUGUI` (requires `com.unity.ugui >= 2.0.0`, which ships TextMeshPro in Unity 6) |

## General Extension Methods

| Type | Members |
|------|---------|
| `DateTimeExtensions` | `IsInRange`, `IsLessThan`, `IsGreaterThan`, epoch conversion, `Elapsed`, `GetRemainingTime`, duration formatting |
| `UnityExtensions` | `GetOrAddComponent<T>` |

## Inspector Attributes

| Attribute | Description |
|-----------|-------------|
| `[ButtonAttribute]` | Marks a method to appear as a button in the Inspector (works on MonoBehaviours and ScriptableObjects) |
| `[InlineEditorAttribute]` | Renders a ScriptableObject field with inline editing and foldout in the Inspector |

## Editor Utilities

| Type | Members |
|------|---------|
| `EditorHelperMethods` | `FindAllScriptableObjectsOfType<T>` — finds all SO assets of a given type in the project |

## Other

| Type | Description |
|------|-------------|
| `UnitySerializedDictionary<TKey,TValue>` | Abstract inspector-serializable dictionary base class |
| `DoNotDestroyGameObjectOnLoad` | MonoBehaviour that calls `DontDestroyOnLoad` on `Awake` |

## Installation

Install via the Unity Package Manager pointing to your Verdaccio registry.

## License

MIT
