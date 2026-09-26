# Coroutine Utilities

Static coroutine runner for non-MonoBehaviour code.

## Contents

| Type | Description |
|------|-------------|
| `CoroutineHandler` | `SingletonPersistent` providing static `StartCoroutine` / `StopCoroutine` for non-MonoBehaviour callers, `AfterWait` delays, `WaitLoop` condition loops, and per-delay update loops (`DoUpdate`/`RemoveUpdate`). |
| `TimeCounter` | Coroutine-based countdown/countup timer with pause, play, speed control, and formatted output. |
| `CoroutineSequence` / `CoroutineDelay` / `CoroutineCondition` | Serializable sequence steps: execute an action after a delay or when a condition becomes true. |
| `CoroutineStatic` | `MonoBehaviour` extension methods mirroring `AfterWait`. |

## Note

`CoroutineHandler` extends `SingletonPersistent<T>` from `com.madratzz.utilities.core` — a legacy
pattern retained for compatibility. See that package's README for guidance on new code.

Namespace: `CustomUtilities`. Depends on `com.madratzz.utilities.core`.
