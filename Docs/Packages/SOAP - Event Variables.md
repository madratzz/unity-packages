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
| `IntWithEvent` | `Int` | `SetValue`, `ApplyChange` |
| `DBBoolWithEvent` | `DBBool` | `SetValue` |
| `DBIntWithEvent` | `DBInt` | `SetValue`, `ApplyChange` |

All four are null-guarded — an unassigned event asset skips the raise instead of throwing.

The `Int` variants raise on `ApplyChange` as well as `SetValue`: incrementing is the common path for a counter, and would otherwise move the value without notifying anyone.

`IntWithEvent` is the session-only counterpart to `DBIntWithEvent`, added 2026-09-26 — the set previously had `BoolWithEvent` for plain `Bool` but nothing for plain `Int`. The `Assets/` template's HUD uses both: `v_Score` (`IntWithEvent`) and `v_Coins` (`DBIntWithEvent`) drive its labels through `VariableLabel` — see [[GameFlow (Template Layer)]].

---
← [[Home]]
