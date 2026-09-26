# SOAP - Variables

← [[Home]] · [[Architecture Overview]]

**Package:** `com.madratzz.scriptableobject.variables` · [Full README & API](../../Packages/com.madratzz.scriptableobject.variables/README.md)

ScriptableObject variables for int/float/bool/string with typed get/set interfaces, default-value reset, and cumulative change support. Values live in assets, not MonoBehaviours, so they survive scene loads and can be shared without direct references — the foundation of the SOAP pattern (see [[Architecture Overview]]).

## Depends on

- [[Utilities - Attributes]]

## Used by

- [[SOAP - Variables Database]]
- [[SOAP - Event Variables]]
- [[GameFlow (Template Layer)]]

## Key types

| Type | Interfaces | Description |
|---|---|---|
| `Int` | `IVariable<int>`, `IApplyChange<int>` | `ApplyChange(amount)` for additive modification. |
| `Float` | `IVariable<float>`, `IApplyChange<float>` | Same, for floats. |
| `Bool` | `IVariable<bool>` | Boolean variable. |
| `String` | `IVariable<string>` | String variable. |

`IVariable<T>`: `GetValue`, `GetDefaultValue`, `SetValue`, `SetDefaultValue`, `ResetToDefaultValue`.

---
← [[Home]]
