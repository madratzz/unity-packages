# SOAP - Event System Extensions

← [[Home]] · [[Architecture Overview]]

**Package:** `com.madratzz.scriptableobject.eventsystem.extensions` · [Full README & API](../../Packages/com.madratzz.scriptableobject.eventsystem.extensions/README.md)

Typed generic extensions to [[SOAP - Event System]]: parameterised events for primitives, sprites, and Vector3, plus return-value events.

## Depends on

- [[SOAP - Event System]]
- [[Utilities - Attributes]]

## Used by

- [[SOAP - Architecture (GameFlow)]] *(unmerged)*

## Key types

| Type | Description |
|---|---|
| `GameEventWithParam<T>` | Generic single-parameter event. |
| `GameEventWithParam<T,U,V>` | Generic three-parameter event. |
| `GameEventWithReturn<T>` | Return-value event — returns `default(T)` when unsubscribed. |
| `GameEventWithString` / `Int` / `Float` / `Bool` / `Sprite` | Concrete `GameEventWithParam<T>` types. |
| `GameEventWithIntStringBool` | Concrete `GameEventWithParam<int, string, bool>`. |
| `GameEventReturnsVector3` | `GameEventWithReturn<Vector3?>` with exception handling. |

---
← [[Home]]
