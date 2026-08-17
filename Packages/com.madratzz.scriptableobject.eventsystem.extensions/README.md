# Event System Extensions

Typed generic extensions to the Event System package, adding parameterised events for primitives, sprites, and Vector3, plus return-value events.

## Overview

Extends the core `GameEvent` system with generic base classes for events that carry data. `GameEventWithParam<T>` passes a value to all subscribers; `GameEventWithReturn<T>` collects a return value from a single subscriber. All concrete types are ready-to-use ScriptableObject assets.

## Types

| Type | Description |
|------|-------------|
| `GameEventWithParam<T>` | Generic single-parameter event — subscribers receive one value |
| `GameEventWithParam<T,U,V>` | Generic three-parameter event — subscribers receive three values |
| `GameEventWithReturn<T>` | Generic return-value event — raises and returns a value from the subscriber |
| `GameEventWithString` | Concrete `GameEventWithParam<string>` |
| `GameEventWithInt` | Concrete `GameEventWithParam<int>` |
| `GameEventWithFloat` | Concrete `GameEventWithParam<float>` |
| `GameEventWithBool` | Concrete `GameEventWithParam<bool>` |
| `GameEventWithSprite` | Concrete `GameEventWithParam<Sprite>` |
| `GameEventWithIntStringBool` | Concrete `GameEventWithParam<int, string, bool>` |
| `GameEventReturnsVector3` | `GameEventWithReturn<Vector3?>` with exception handling |

## Installation

Install via the Unity Package Manager pointing to your Verdaccio registry.

## License

MIT
