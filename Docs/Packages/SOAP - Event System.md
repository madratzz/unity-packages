# SOAP - Event System

← [[Home]] · [[Architecture Overview]]

**Package:** `com.madratzz.scriptableobject.eventsystem.core` · [Full README & API](../../Packages/com.madratzz.scriptableobject.eventsystem.core/README.md)

ScriptableObject-based event bus: parameterless `GameEvent` assets with listener/raiser MonoBehaviour components for decoupled scene communication — no direct references needed between sender and receiver.

## Depends on

- [[Utilities - Attributes]]

## Used by

- [[SOAP - Event System Extensions]]
- [[SOAP - Event Variables]]
- [[SOAP - Time Machine]]
- [[SOAP - Architecture (GameFlow)]] *(unmerged, via [[SOAP - Event System Extensions]])*

## Key types

| Type | Description |
|---|---|
| `GameEvent` | ScriptableObject event asset with `Action`-based handler and `Invoke()`. |
| `GameEventListener` | Subscribes to a `GameEvent`, forwards it to a `UnityEvent`. |
| `GameEventRaiser` | Invokes a stored `GameEvent` via `InvokeEvent()`. |
| `GameEventRaiserOnEnable` | Auto-invokes its `GameEvent` on `OnEnable`. |

## Usage

1. Right-click → Create → **Game Event**.
2. Add `GameEventListener` to the receiver, assign the event and a `UnityEvent` response.
3. Call `myEvent.Invoke()` in code, or add a `GameEventRaiser` to the sender.

---
← [[Home]]
