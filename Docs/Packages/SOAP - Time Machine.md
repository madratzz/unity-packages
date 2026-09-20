# SOAP - Time Machine

← [[Home]] · [[Architecture Overview]]

**Package:** `com.madratzz.scriptableobject.time.machine` · [Full README & API](../../Packages/com.madratzz.scriptableobject.time.machine/README.md)

ScriptableObject timer that fires a `GameEvent` on a fixed interval, enabling time-driven logic without direct MonoBehaviour coupling. Its tick loop runs through `CoroutineHandler`, so it survives scene loads and needs no MonoBehaviour owner.

## Depends on

- [[SOAP - Event System]]
- [[Utilities - Coroutines]]
- [[Utilities - Core]]

## Used by

- [[GameFlow (Template Layer)]] *(drives the app-level per-second tick loop)*

## Key types

| Type | Description |
|---|---|
| `TimeMachine` | `StartTicking()` / `StopTicking()`; safe to call redundantly. |

**Inspector options:** **Tick Interval** (default 1s), **Use Real Time** (off = scaled time, pauses at `Time.timeScale == 0`; on = unscaled wall-clock, survives pause).

## Usage

1. Assign a `GameEvent` to the **Tick Event** field.
2. Call `StartTicking()`/`StopTicking()`, or hook to a `GameEventRaiserOnEnable`.
3. Add a `GameEventListener` for per-tick callbacks.

## Sample

**Package Manager → Time Machine → Samples → Import** gives a pre-configured `TimeMachine` and `e_TimeMachineTick` event asset. See [[Getting Started]]#5. Trying a sample.

---
← [[Home]]
