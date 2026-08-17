# Variable Extensions

Extended ScriptableObject variable types including generic arrays, shared Vector2/Vector3 values, and serialized dictionaries (dictionary types require Odin Inspector).

## Overview

Extends the SO Variable system with collection and vector types that follow the same ScriptableObject asset pattern as the base Variables package.

## Types

| Type | Description |
|------|-------------|
| `Array<T>` | Abstract generic SO-backed list with add, remove, insert, and indexed access |
| `ArrayInt` | Concrete `Array<int>` asset |
| `Vector2Shared` | SO-backed `Vector2` with get/set and `ApplyChange` |
| `Vector3Shared` | SO-backed `Vector3` with get/set and `ApplyChange` |
| `RuntimeDictionary<TKey, TValue>` | Abstract SO-backed dictionary *(requires Odin Inspector)* |
| `RuntimeDictionaryIntInt` | Concrete `RuntimeDictionary<int, int>` asset *(requires Odin Inspector)* |

## Requirements

### Odin Inspector (for dictionary types)

`RuntimeDictionary` and `RuntimeDictionaryIntInt` are compiled only when the `ODIN_INSPECTOR` scripting define symbol is present. They inherit from Sirenix's `SerializedScriptableObject` to allow Unity to serialize generic `Dictionary<TKey, TValue>` fields in the Inspector.

To use these types:
1. Import [Odin Inspector](https://odininspector.com/) into your project.
2. Ensure the `ODIN_INSPECTOR` define is set — Odin sets this automatically on import.

All other types (`Array<T>`, `ArrayInt`, `Vector2Shared`, `Vector3Shared`) work without Odin.

## Installation

Install via the Unity Package Manager pointing to your Verdaccio registry.

## License

MIT
