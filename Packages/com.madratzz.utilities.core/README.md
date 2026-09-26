# Core Utilities

Core runtime utilities with zero package dependencies.

## Contents

| Type | Description |
|------|-------------|
| `Singleton<T>` | Generic MonoBehaviour singleton with lazy instance creation. **Legacy interop** — see below. |
| `SingletonPersistent<T>` | Extends `Singleton<T>` with `DontDestroyOnLoad`. **Legacy interop** — see below. |
| `UnitySerializedDictionary<TKey, TValue>` | `Dictionary` subclass that Unity can serialize (via `ISerializationCallbackReceiver`). |
| `UnityExtensions` | `GetOrAddComponent<T>` — returns the existing component or adds and returns a new one. |
| `DoNotDestroyGameObjectOnLoad` | MonoBehaviour that calls `DontDestroyOnLoad` on itself in `Awake`. |
| `DateTimeExtensions` | `IsInRange`, `IsLessThan`, `IsGreaterThan`, epoch conversion, duration formatting. |
| `Utilities` | String-to-number parsing (`ToInt`/`ToFloat`/`ToDouble`/`ToBool`) and epoch/`mm:ss` time helpers. |

## Note on Singletons (legacy)

`Singleton<T>` and `SingletonPersistent<T>` exist for compatibility with the archived
`com.madratzz` package family (notably `CoroutineHandler` in `com.madratzz.utilities.coroutines`).

**For new code, prefer explicit wiring** per this project's architecture: ScriptableObject
references (SOAP) for shared state, or constructor/VContainer injection for genuine services.
Do not build new systems that reach through static singleton instances.

Namespaces: `CustomUtilities`, `ProjectCore.EngineExtensions`, `ExtensionMethods`.
