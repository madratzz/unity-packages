# Variable Database

Persistent ScriptableObject variables backed by PlayerPrefs, providing typed DB variables for int, float, bool, string, and epoch time with JSON export support.

## Overview

`DB*` types extend the base Variable types and auto-save to `PlayerPrefs` on every `SetValue` call — no manual save calls required. `DBManager` maintains a static registry of all active DB variables and supports JSON import/export for backup, restore, or cross-device transfer. `Database` is a ScriptableObject wrapper around `PlayerPrefs` that makes the persistence layer swappable for testing.

## Types

| Type | Extends | Description |
|------|---------|-------------|
| `DBInt` | `Int` | Persistent integer — auto-saves on `SetValue` and `ApplyChange` |
| `DBFloat` | `Float` | Persistent float — auto-saves on `SetValue` and `ApplyChange` |
| `DBBool` | `Bool` | Persistent boolean — auto-saves on `SetValue` |
| `DBString` | `String` | Persistent string — auto-saves on `SetValue` |
| `DBEpochTime` | `DBInt` | Persistent epoch timestamp with `AddDays` / `SubtractDays` helpers |
| `Database` | `ScriptableObject` | PlayerPrefs abstraction layer for testability |
| `DBManager` | *(static)* | Registry of active DB variables with JSON import/export |

## Installation

Install via the Unity Package Manager pointing to your Verdaccio registry.

## License

MIT
