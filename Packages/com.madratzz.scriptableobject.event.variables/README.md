# Event Variables

ScriptableObject variables that automatically raise a GameEvent whenever their value changes, bridging the Variable and Event System packages.

## Overview

Event Variables are thin wrappers over the base and DB variable types that add a `GameEvent` field. Whenever `SetValue` (or `ApplyChange`) is called, the stored `GameEvent` is invoked automatically — no manual event-raising needed. This is ideal for driving UI updates or triggering game logic purely from data changes.

## Types

| Type | Extends | Raises event on |
|------|---------|----------------|
| `BoolWithEvent` | `Bool` | `SetValue` |
| `DBBoolWithEvent` | `DBBool` | `SetValue` |
| `DBIntWithEvent` | `DBInt` | `SetValue`, `ApplyChange` |

## Installation

Install via the Unity Package Manager pointing to your Verdaccio registry.

## License

MIT
