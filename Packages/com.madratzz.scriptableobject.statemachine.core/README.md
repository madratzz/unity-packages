# State Machine

ScriptableObject-based finite state machine with coroutine-driven state lifecycle methods and declarative Transition assets.

## Overview

States and transitions are ScriptableObject assets, so the entire FSM graph is data-driven and inspector-configurable. `FiniteStateMachine` exposes a `Tick()` coroutine that the caller drives (typically via `CoroutineHandler` from `com.madratzz.utilities.coroutines`):

```csharp
CoroutineHandler.StartStaticCoroutine(fsm.Tick());
```

The FSM does **not** self-drive — it is a ScriptableObject and cannot host coroutines on its own. The caller is responsible for starting and stopping the loop.

## Types

| Type | Description |
|------|-------------|
| `FiniteStateMachine` | ScriptableObject FSM runner — `Tick()` drives the loop, `Transition(Transition)` queues a transition, `ShouldResumePreviousState()` exits the current state and resumes the most recently paused one |
| `State` | ScriptableObject base class — override `Init`, `Execute`, `Tick`, `Exit`, `Pause`, `Resume`, `Cleanup` (all coroutines) |
| `Transition` | ScriptableObject linking a source state to a target state with an optional `Execute` coroutine |
| `IState` | Interface for external FSM control: `TransitionTo`, `CleanupAllPausedStates` |

## Lifecycle Order

```
Boot (first Tick):            BootState.Init → BootState.Execute → loop
Transition (each frame):      CurrentState.Exit (or Pause) → Transition.Execute
                              → NextState.Init → NextState.Execute → loop
Resume (each frame):          CurrentState.Exit → Popped.Resume → loop
Every frame:                  CurrentState.Tick → yield
```

`PausesPreviousState` (on the target `State`) controls whether the FSM exits or pauses the source state during a transition. Paused states live on a stack and are resumed in reverse-push order via `ShouldResumePreviousState()`.

## Installation

Install via the Unity Package Manager pointing to your Verdaccio registry.

## License

MIT
