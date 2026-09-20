# Utilities - Core

← [[Home]] · [[Architecture Overview]]

**Package:** `com.madratzz.utilities.core` · [Full README & API](../../Packages/com.madratzz.utilities.core/README.md)

Core runtime utilities with zero package dependencies: legacy singleton base classes, a Unity-serializable dictionary, engine extension methods, and string/DateTime parsing helpers.

## Depends on

None — zero-dependency leaf package.

## Used by

- [[Utilities - Coroutines]]
- [[SOAP - Time Machine]]

## Key types

| Type | Description |
|---|---|
| `Singleton<T>` / `SingletonPersistent<T>` | Generic MonoBehaviour singleton bases. **Legacy interop only** — see below. |
| `UnitySerializedDictionary<TKey, TValue>` | Dictionary subclass Unity can serialize. |
| `UnityExtensions.GetOrAddComponent<T>` | Returns the existing component or adds and returns a new one. |
| `DateTimeExtensions` | Range checks, epoch conversion, duration formatting. |
| `Utilities` | String-to-number parsing, epoch/`mm:ss` helpers. |

## ⚠️ Legacy singletons

`Singleton<T>` / `SingletonPersistent<T>` exist only for compatibility with the archived package family (notably `CoroutineHandler` in [[Utilities - Coroutines]]). **For new code, prefer explicit SOAP wiring** or constructor/VContainer injection — see [[Architecture Overview]]. Do not build new systems that reach through static singleton instances.

Namespaces: `CustomUtilities`, `ProjectCore.EngineExtensions`, `ExtensionMethods`.

---
← [[Home]]
