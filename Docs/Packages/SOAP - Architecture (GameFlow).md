# SOAP - Architecture (GameFlow)

← [[Home]] · [[Architecture Overview]]

**Package:** `com.madratzz.scriptableobject.architecture` · full README: [`Packages/com.madratzz.scriptableobject.architecture/README.md` on `feature/architecture`](https://github.com/madratzz/unity-packages/blob/feature/architecture/Packages/com.madratzz.scriptableobject.architecture/README.md)

> ⚠️ **Status: unmerged.** This package exists only on the `feature/architecture` branch, not on `development`. It does not resolve in a normal checkout of this repo — check out that branch (or wait for its PR) to use it.

Top-level GameFlow orchestrator: wires FSM transitions to a pure-function `(FlowContext, UICloseReasons) → FlowIntent` decision table. Depends only on the rest of the `scriptableobject.*` family — no DI container, no third-party deps.

`ApplicationBase` owns the FSM and TimeMachine coroutine loops plus app lifecycle events (pause/resume with timestamp persistence). `ApplicationFlowController` translates `GameEvent` close callbacks into FSM transitions via the decision table — every UI close is a `(context, reason)` lookup returning one of `GoToGame`, `GoToLevelFail`, `ResumePrevious`, `DefaultToGame`.

## Depends on

- [[SOAP - Event System Extensions]]
- [[SOAP - State Machine]]
- [[SOAP - Time Machine]]
- [[SOAP - Variables]]
- [[SOAP - Variables Database]]

## Used by

Nothing — this is the top of the dependency graph (see [[Architecture Overview]]).

## Key types

| Type | Description |
|---|---|
| `ApplicationBase` | MonoBehaviour: owns the FSM + TimeMachine coroutines + app lifecycle events. Place on the persistent application GameObject. |
| `ApplicationFlowController` | MonoBehaviour: routes `GameEvent` triggers to FSM transitions via the decision table. Place on the boot scene's persistent controller. |
| `ApplicationFlowLogic` | Default `IFlowLogic` — subclass and override the strategy table to add contexts without forking the controller. |
| `IFlowLogic` | Pure-function decision contract. |
| `FlowContext` / `FlowIntent` / `UICloseReasons` | Enums for screen context, navigation intent, and UI-close reason. |

## Usage

```csharp
// 1. Add ApplicationBase + ApplicationFlowController to your boot scene.
// 2. Wire SerializeFields: applicationStateMachine, applicationTimeMachine,
//    appPaused/appResumed GameEvents, appPausedTime (DBInt), transitions + GameEvents.
// 3. Hook ApplicationFlowController.Boot() to a startup event
//    (e.g. GameEventRaiserOnEnable).
```

To extend the decision table, subclass `ApplicationFlowLogic`, add entries in the constructor, then enable **useCustomLogic** on the controller and attach your subclass.

## ⚠️ Known issues (from code review, 2026-09-20)

- **Duplicate, unlinked FSM references.** `ApplicationBase` and `ApplicationFlowController` each hold their own `FiniteStateMachine` `[SerializeField]`, but only `ApplicationBase` ticks it. If a scene wires them to different assets — or wires the controller's and leaves the base's unassigned — `Transition()` silently queues transitions on an FSM that never ticks. **When wiring this package, double-check both fields point to the *same* `FiniteStateMachine` asset.**
- **Implicit `Resources.Load` fallback.** `ApplicationBase.Awake()` falls back to `Resources.Load<FiniteStateMachine>("StateMachine")` if the field is unassigned — a magic-string lookup that violates this repo's explicit-wiring rule (see [[Architecture Overview]]). Wire the field explicitly rather than relying on the fallback.
- **README example bug.** The "extend the decision table" example in the package README calls `FlowIntent.GoToMainMenu`, which doesn't exist in the `FlowIntent` enum — don't copy it verbatim; use one of the enum's actual values (`None`, `DefaultToGame`, `GoToGame`, `GoToLevelFail`, `OpenSettings`, `ResumePrevious`).

## Requirements

`scriptableobject.*` family only: `eventsystem.extensions`, `statemachine.core`, `time.machine`, `variables`, `variables.database`. No third-party dependencies.

---
← [[Home]]
