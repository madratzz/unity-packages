# SOAP - Variables Database

← [[Home]] · [[Architecture Overview]]

**Package:** `com.madratzz.scriptableobject.variables.database` · [Full README & API](../../Packages/com.madratzz.scriptableobject.variables.database/README.md)

Persistent variants of [[SOAP - Variables]] backed by `PlayerPrefs` — auto-saves on every `SetValue`, no manual save calls.

## Depends on

- [[SOAP - Variables]]
- [[Utilities - Attributes]]

## Used by

- [[SOAP - Event Variables]]
- [[GameFlow (Template Layer)]] *(uses `DBInt` for pause-timestamp persistence)*

## Key types

| Type | Extends | Description |
|---|---|---|
| `DBInt` / `DBFloat` / `DBBool` / `DBString` | `Int` / `Float` / `Bool` / `String` | Auto-saves to `PlayerPrefs` on `SetValue` (and `ApplyChange` for `DBInt`/`DBFloat`). |
| `DBEpochTime` | `DBInt` | Adds `AddDays`/`SubtractDays` helpers. |
| `Database` | `ScriptableObject` | `PlayerPrefs` abstraction layer, swappable for testing. |
| `DBManager` | *(static)* | Registry of active DB variables; JSON import (export is not yet implemented — see the package `CHANGELOG.md`). |

---
← [[Home]]
