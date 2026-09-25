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
| `FloatWithEvent` | `Float` | `SetValue`, `ApplyChange` |
| `DBBoolWithEvent` | `DBBool` | `SetValue` |
| `DBIntWithEvent` | `DBInt` | `SetValue`, `ApplyChange` |

All five are null-guarded — an unassigned event asset skips the raise instead of throwing.

The numeric variants raise on `ApplyChange` as well as `SetValue`: incrementing is the common path for a counter, and would otherwise move the value without notifying anyone.

`IntWithEvent` and `FloatWithEvent` were added 2026-09-26 — the set previously had `BoolWithEvent` for plain `Bool` and `DBIntWithEvent` for persistent `Int`, but nothing for a plain session-only `Int` or any `Float` at all. The `Assets/` template uses four of the five: `v_Score` (`IntWithEvent`) and `v_Coins` (`DBIntWithEvent`) drive the HUD labels, while `v_MusicVolume` / `v_SfxVolume` (`FloatWithEvent`) and `v_VibrationEnabled` (`DBBoolWithEvent`) drive the Settings sliders and toggle — see [[GameFlow (Template Layer)]].

---
← [[Home]]
