# SOAP - Event Variables

← [[Home]] · [[Architecture Overview]]

**Package:** `com.madratzz.scriptableobject.event.variables` · [Full README & API](../../Packages/com.madratzz.scriptableobject.event.variables/README.md)

Bridges [[SOAP - Variables]]/[[SOAP - Variables Database]] and [[SOAP - Event System]]: variables that automatically raise a `GameEvent` whenever their value changes — no manual event-raising needed. Ideal for driving UI purely from data changes.

## Depends on

- [[SOAP - Event System]]
- [[SOAP - Variables]]
- [[SOAP - Variables Database]]

## Used by

Nothing in this repo yet.

## Key types

| Type | Extends | Raises event on |
|---|---|---|
| `BoolWithEvent` | `Bool` | `SetValue` |
| `DBBoolWithEvent` | `DBBool` | `SetValue` |
| `DBIntWithEvent` | `DBInt` | `SetValue`, `ApplyChange` |

All three are null-guarded — an unassigned event asset skips the raise instead of throwing.

---
← [[Home]]
