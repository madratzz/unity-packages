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
- [[GameFlow (Template Layer)]] *(via [[SOAP - Event System Extensions]])*

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

## ⚠️ The failure mode that actually bites: mismatched, not missing

All three components' `GameEvent` field is `[RequireReference]`-marked ([[Utilities - Attributes]]'s `RequiredReferenceValidator` catches an *unassigned* one across every scene/prefab), and an unassigned field now logs an error instead of throwing. Neither catches the sharper bug: a raiser and listener each pointing at a *different*, both-valid `GameEvent` asset. That's silent — no error, nothing fires — since there's no way to know two distinct assets weren't meant to be distinct. When a `GameEvent`-driven feature "does nothing," check that every raiser and listener for it reference the exact same asset before anything else.

---
← [[Home]]
