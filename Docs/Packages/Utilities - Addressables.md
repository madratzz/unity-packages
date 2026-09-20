# Utilities - Addressables

← [[Home]] · [[Architecture Overview]]

**Package:** `com.madratzz.utilities.addressables` · [Full README & API](../../Packages/com.madratzz.utilities.addressables/README.md)

Safe coroutine-based Addressables asset loading: `AssetReference` validation, component extraction, contextual error reporting, and leak-free failure handling.

## Depends on

- `com.unity.addressables` (`>= 2.9.1`, external). No dependency on other packages in this repo.

## Used by

Nothing in this repo yet.

## Key types

| Type | Description |
|---|---|
| `AddressablesHelper.Instantiate<T>(assetRef, onSuccess, onFailure?, context?)` | Validates, instantiates, extracts a `T` component, calls `onSuccess`. Releases the instance and calls `onFailure(handle)` on any failure. |
| `AddressablesHelper.InstantiateGameObject(...)` | Same contract, returns the `GameObject` directly. |

## Quick usage

```csharp
yield return AddressablesHelper.Instantiate<MyHud>(
    hudRef,
    onSuccess: (hud, h) => hud.Initialize(levelData),
    onFailure: h => Debug.LogWarning($"HUD load failed: {h.OperationException?.Message}"),
    debugContext: this);
```

Both methods are coroutines — start with `StartCoroutine(...)`. If `onSuccess` throws, the instance is still released (no instance leak on caller bugs).

---
← [[Home]]
