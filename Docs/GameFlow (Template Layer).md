# GameFlow (Template Layer)

← [[Home]] · [[Architecture Overview]]

**Location:** `Assets/Runtime` (+ `Assets/Tests`, `Assets/Prefabs`, `Assets/Scenes`) — **not a package.** This is the repository's `Assets/` template/integration layer, not one of the `Packages/com.madratzz.*` family. See [[Architecture Overview]] and `IDEA.md` for the `Packages/` vs. `Assets/` split. Full README: [`Assets/Runtime/README.md`](../Assets/Runtime/README.md).

Top-level GameFlow orchestrator: wires FSM transitions to a pure-function `(FlowContext, UICloseReasons) → FlowIntent` decision table. Depends only on the `scriptableobject.*` package family (plus `utilities.attributes` for wiring validation) — no DI container, no third-party deps.

`ApplicationBase` owns the FSM and TimeMachine coroutine loops plus app lifecycle events (pause/resume with timestamp persistence). `ApplicationFlowController` translates `GameEvent` close callbacks into FSM transitions via the decision table — every UI close is a `(context, reason)` lookup returning one of `GoToGame`, `GoToLevelFail`, `ResumePrevious`, `DefaultToGame`.

## Depends on

- [[SOAP - Event System Extensions]]
- [[SOAP - State Machine]]
- [[SOAP - Time Machine]]
- [[SOAP - Variables]]
- [[SOAP - Variables Database]]
- [[Utilities - Attributes]] *(for `[RequireReference]` — see below)*

## Used by

Nothing — this is the top of the dependency graph (see [[Architecture Overview]]).

## Key types

| Type | Description |
|---|---|
| `ApplicationBase` | MonoBehaviour: owns the FSM + TimeMachine coroutines + app lifecycle events. Place on the persistent application GameObject. |
| `ApplicationFlowController` | MonoBehaviour: routes `GameEvent` triggers to FSM transitions via the decision table. Reads its FSM from a wired `ApplicationBase` reference rather than holding its own. |
| `ApplicationFlowLogic` | Default `IFlowLogic` — subclass and override the strategy table to add contexts without forking the controller. |
| `IFlowLogic` | Pure-function decision contract. |
| `FlowContext` / `FlowIntent` / `UICloseReasons` | Enums for screen context, navigation intent, and UI-close reason. |

## Usage

```csharp
// 1. Add ApplicationBase + ApplicationFlowController to your boot scene
//    (see Assets/Prefabs/ApplicationBase.prefab and ApplicationFlowController.prefab).
// 2. Wire SerializeFields: ApplicationStateMachine, ApplicationTimeMachine,
//    AppPaused/AppResumed GameEvents, AppPausedTime (DBInt), transitions +
//    GameEvents on the controller, and ApplicationFlowController.ApplicationBase
//    (the same ApplicationBase from step 1).
// 3. Hook ApplicationFlowController.Boot() to a startup event
//    (e.g. GameEventRaiserOnEnable).
```

To extend the decision table, subclass `ApplicationFlowLogic`, add entries in the constructor, then enable **UseCustomLogic** on the controller and attach your subclass.

## Catching missing wiring

`ApplicationBase.ApplicationStateMachine` and `ApplicationFlowController.ApplicationBase` are marked `[RequireReference]` ([[Utilities - Attributes]]) — run **Tools → Validate Required References** to catch either left unassigned across every scene and prefab, instead of finding out at runtime. This doesn't catch *mismatched* wiring (e.g. two different, both-valid `FiniteStateMachine` assets) — see [[SOAP - Event System]]'s note on the same limitation for `GameEvent`s.

## Requirements

`scriptableobject.*` family: `eventsystem.extensions`, `statemachine.core`, `time.machine`, `variables`, `variables.database`. `utilities.attributes` (for `[RequireReference]`). No third-party dependencies.

---
← [[Home]]
