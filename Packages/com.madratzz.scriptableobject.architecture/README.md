# Architecture

Top-level GameFlow orchestrator: wires FSM transitions to a pure-function `(FlowContext, UICloseReasons) → FlowIntent` strategy table. Depends only on the rest of the `scriptableobject.*` package family — no DI container, no third-party deps.

## Overview

`ApplicationBase` owns the FSM and TimeMachine coroutine loops and the application lifecycle events (pause/resume with timestamp persistence). `ApplicationFlowController` translates GameEvent close callbacks into FSM transitions via the decision table — every UI close is a `(context, reason)` lookup, the result is one of four intents (`GoToGame`, `GoToLevelFail`, `ResumePrevious`, `DefaultToGame`).

All wiring is `[SerializeField]` — no VContainer, no Zenject. The default decision logic is `ApplicationFlowLogic`; subclass it and override the strategy table to add contexts (Settings, MainMenu, LevelComplete, …) without forking the controller.

## Types

| Type | Description |
|---|---|
| `ApplicationBase` | MonoBehaviour: owns the FSM + TimeMachine coroutines + app lifecycle events (pause/resume + `appPausedTime` DBInt timestamp). Place on the persistent application GameObject. |
| `ApplicationFlowController` | MonoBehaviour: routes GameEvent triggers to FSM transitions via the decision table. Place on the boot scene's persistent controller. |
| `ApplicationFlowLogic` | Default `IFlowLogic` — strategies `Boot + Game → GoToGame`, `LevelFail + Game → GoToGame`. Subclass to extend. |
| `IFlowLogic` | Pure-function decision contract. |
| `FlowContext` | Enum — current application screen context. |
| `FlowIntent` | Enum — navigation/logic intents. Numeric values are stable. |
| `UICloseReasons` | Enum — reason a UI view was closed. |

## Usage

```csharp
// 1. Add ApplicationBase + ApplicationFlowController to your boot scene.
// 2. Wire SerializeFields:
//    - applicationStateMachine: your FiniteStateMachine asset (or Resources/StateMachine)
//    - applicationTimeMachine: optional TimeMachine asset for the per-second tick loop
//    - appPaused / appResumed: GameEvent assets fired on app lifecycle
//    - appPausedTime: DBInt asset for the pause timestamp
//    - transitions + GameEvents on the controller
// 3. Hook ApplicationFlowController.Boot() to a startup event (e.g. GameEventRaiserOnEnable).
```

To extend the decision table:

```csharp
public class MyFlowLogic : ApplicationFlowLogic
{
    public MyFlowLogic()
    {
        Add(FlowContext.MainMenu, UICloseReasons.Game, FlowIntent.GoToGame);
        Add(FlowContext.MainMenu, UICloseReasons.Settings, FlowIntent.OpenSettings);
        Add(FlowContext.Settings, UICloseReasons.Home, FlowIntent.GoToMainMenu);
    }
}
```

Then on the `ApplicationFlowController` GameObject, enable **useCustomLogic** and add a `MyFlowLogic` component.

## Requirements

The `scriptableobject.*` package family: `eventsystem.extensions`, `statemachine.core`, `time.machine`, `variables`, `variables.database`. **No third-party deps.**

## Installation

Install via the Unity Package Manager pointing to your Verdaccio registry.

## License

MIT
