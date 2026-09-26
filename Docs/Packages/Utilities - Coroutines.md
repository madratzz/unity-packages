# Utilities - Coroutines

← [[Home]] · [[Architecture Overview]]

**Package:** `com.madratzz.utilities.coroutines` · [Full README & API](../../Packages/com.madratzz.utilities.coroutines/README.md)

Static coroutine runner for non-MonoBehaviour code — delayed actions, condition waits, update loops, and timer sequences.

## Depends on

- [[Utilities - Core]] — `CoroutineHandler` extends `SingletonPersistent<T>` from this package (legacy pattern, kept for compatibility).

## Used by

- [[SOAP - Time Machine]]

## Key types

| Type | Description |
|---|---|
| `CoroutineHandler` | `SingletonPersistent` exposing static `StartCoroutine`/`StopCoroutine`, `AfterWait` delays, `WaitLoop` condition loops, per-delay update loops. |
| `TimeCounter` | Coroutine-based countdown/countup timer with pause/play/speed control. |
| `CoroutineSequence` / `CoroutineDelay` / `CoroutineCondition` | Serializable sequence steps for delay- or condition-driven actions. |
| `CoroutineStatic` | MonoBehaviour extension methods mirroring `AfterWait`. |

## Quick usage

```csharp
CoroutineHandler.StartStaticCoroutine(fsm.Tick());
```

Namespace: `CustomUtilities`.

---
← [[Home]]
