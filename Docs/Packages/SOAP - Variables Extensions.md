# SOAP - Variables Extensions

← [[Home]] · [[Architecture Overview]]

**Package:** `com.madratzz.scriptableobject.variables.extensions` · [Full README & API](../../Packages/com.madratzz.scriptableobject.variables.extensions/README.md)

Extended SO variable types: generic arrays, shared Vector2/Vector3 values, and serialized dictionaries (dictionary types require Odin Inspector).

## Depends on

Nothing — this package is standalone. It shares the `ProjectCore.Variables` namespace with [[SOAP - Variables]] and follows the same asset pattern, but uses none of its types; its assembly definitions carry no cross-package references, so it installs on its own.

## Used by

Nothing in this repo yet.

## Key types

| Type | Description |
|---|---|
| `Array<T>` / `ArrayInt` | SO-backed list with add/remove/insert/indexed access. |
| `Vector2Shared` / `Vector3Shared` | SO-backed vectors with get/set and `ApplyChange`. |
| `RuntimeDictionary<TKey, TValue>` / `RuntimeDictionaryIntInt` | SO-backed dictionaries — **compiled only when `ODIN_INSPECTOR` is defined**, via [Odin Inspector](https://odininspector.com/)'s `SerializedScriptableObject`. |

All types except the dictionaries work without Odin.

---
← [[Home]]
