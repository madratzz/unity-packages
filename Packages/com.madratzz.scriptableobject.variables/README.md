# Variables

ScriptableObject variables for int, float, bool, and string with typed get/set interfaces, default value reset, and cumulative change support.

## Overview

Each variable type is a ScriptableObject asset that stores a runtime value alongside a default value. Because values live in assets rather than MonoBehaviours, they survive scene loads and can be shared freely across systems without direct references. `Int` and `Float` additionally support cumulative changes via `ApplyChange`, making them well-suited for currency, scores, and health values.

## Types

| Type | Interfaces | Description |
|------|-----------|-------------|
| `Int` | `IVariable<int>`, `IApplyChange<int>` | Integer variable with `ApplyChange(amount)` for additive modification |
| `Float` | `IVariable<float>`, `IApplyChange<float>` | Float variable with `ApplyChange(amount)` for additive modification |
| `Bool` | `IVariable<bool>` | Boolean variable |
| `String` | `IVariable<string>` | String variable |

## Interfaces

| Interface | Members |
|-----------|---------|
| `IVariable<T>` | `GetValue`, `GetDefaultValue`, `SetValue`, `SetDefaultValue`, `ResetToDefaultValue` |
| `IApplyChange<T>` | `ApplyChange(T amount)` |

## Installation

Install via the Unity Package Manager pointing to your Verdaccio registry.

## License

MIT
