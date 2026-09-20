# SOAP - State Machine

← [[Home]] · [[Architecture Overview]]

**Package:** `com.madratzz.scriptableobject.statemachine.core` · [Full README & API](../../Packages/com.madratzz.scriptableobject.statemachine.core/README.md)

ScriptableObject-based finite state machine with coroutine-driven state lifecycle (`Init`, `Execute`, `Tick`, `Exit`, `Pause`, `Resume`) and declarative `Transition` assets. States and transitions are data-driven, inspector-configurable assets.

## Depends on

None declared.

## Used by

- [[GameFlow (Template Layer)]]

## Key types

| Type | Description |
|---|---|
| `FiniteStateMachine` | `Tick()` drives the loop, `Transition(Transition)` queues a transition, `ShouldResumePreviousState()` resumes the most recently paused state. |
| `State` | Override `Init`/`Execute`/`Tick`/`Exit`/`Pause`/`Resume`/`Cleanup` (all coroutines). |
| `Transition` | Links a source state to a target with an optional `Execute` coroutine. |
| `IState` | External control: `TransitionTo`, `CleanupAllPausedStates`. |

## ⚠️ The FSM does not self-drive

It's a ScriptableObject and cannot host coroutines on its own — the caller must start and stop `Tick()`, typically via [[Utilities - Coroutines]]:

```csharp
CoroutineHandler.StartStaticCoroutine(fsm.Tick());
```

[[GameFlow (Template Layer)]] used to have a bug here — two components each holding an unlinked `FiniteStateMachine` reference, with only one actually ticking it — fixed by having one component read the FSM from the other rather than duplicating the reference. `ApplicationBase.ApplicationStateMachine` is now the single source of truth, marked `[RequireReference]` so an unassigned one is caught by [[Utilities - Attributes]]'s validator.

## Lifecycle order

```
Boot (first Tick):       BootState.Init → BootState.Execute → loop
Transition (each frame): CurrentState.Exit (or Pause) → Transition.Execute → NextState.Init → NextState.Execute → loop
Resume (each frame):     CurrentState.Exit → Popped.Resume → loop
Every frame:              CurrentState.Tick → yield
```

`PausesPreviousState` on the target `State` controls exit-vs-pause during a transition; paused states live on a stack, resumed in reverse-push order.

---
← [[Home]]
