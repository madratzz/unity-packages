# Addressables Helper

Safe coroutine-based Addressables asset loading utilities with `AssetReference` validation, component extraction, and contextual error reporting.

## Overview

`AddressablesHelper` is a static utility class that wraps the Addressables API in coroutines with built-in validation, component extraction, and structured error messages. Every method validates the `AssetReference` before attempting to load, surfaces clear error logs (including the calling object as Unity's log context — clicking the log highlights the caller in the Hierarchy), and guarantees no instance leaks even when the success handler throws.

## Methods

| Method | Description |
|---|---|
| `Instantiate<T>(assetRef, onSuccess, onFailure?, context?)` | Validates, instantiates, extracts a component of type `T`, calls `onSuccess(component, handle)`. On any failure path the instance is released and `onFailure(handle)` is invoked with the handle so the caller can inspect status/exception |
| `InstantiateGameObject(assetRef, onSuccess, onFailure?, context?)` | Validates, instantiates, returns the `GameObject` directly via `onSuccess(gameObject, handle)` — same release semantics as `Instantiate<T>` |

Both methods are coroutines — start them with `StartCoroutine(AddressablesHelper.Instantiate<T>(...))`.

## Failure handling

The `onFailure` callback receives the `AsyncOperationHandle<GameObject>` so you can read `handle.Status` and `handle.OperationException`:

```csharp
yield return AddressablesHelper.Instantiate<MyHud>(
    hudRef,
    onSuccess: (hud, h) => hud.Initialize(levelData),
    onFailure: h => Debug.LogWarning($"HUD load failed: {h.OperationException?.Message}"),
    debugContext: this);
```

If the success handler throws, the instance is released and the exception is rethrown via `Debug.LogException` with the same `debugContext` — no instance leaks on caller bugs.

## Requirements

Requires **Addressables** (`com.unity.addressables >= 2.9.1`).

## Installation

Install via the Unity Package Manager pointing to your Verdaccio registry.

## License

MIT
