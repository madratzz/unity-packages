# GameFlow (Template Integration Layer)

Top-level GameFlow orchestrator: wires FSM transitions to a pure-function `(FlowContext, UICloseReasons) → FlowIntent` strategy table. This is this repository's `Assets/` template layer, not a standalone package — it depends on the `com.madratzz.scriptableobject.*` packages under `Packages/` and demonstrates them wired into a working boot flow. See `IDEA.md` for the `Packages/` vs. `Assets/` split.

## Overview

`ApplicationBase` owns the FSM and TimeMachine coroutine loops and the application lifecycle events (pause/resume with timestamp persistence). `ApplicationFlowController` translates GameEvent close callbacks into FSM transitions via the decision table — every UI close is a `(context, reason)` lookup, the result is one of four intents (`GoToGame`, `GoToLevelFail`, `ResumePrevious`, `DefaultToGame`).

All wiring is `[SerializeField]` — no VContainer, no Zenject. The default decision logic is `ApplicationFlowLogic`; subclass it and override the strategy table to add contexts (Settings, MainMenu, LevelComplete, …) without forking the controller.

## Types

| Type | Description |
|---|---|
| `ApplicationBase` | MonoBehaviour: owns the FSM + TimeMachine coroutines + app lifecycle events (pause/resume + `AppPausedTime` DBInt timestamp). Place on the persistent application GameObject. |
| `ApplicationFlowController` | MonoBehaviour: routes GameEvent triggers to FSM transitions via the decision table. Reads its FSM from a wired `ApplicationBase` reference rather than holding its own — place on the boot scene's persistent controller. |
| `ApplicationFlowLogic` | Default `IFlowLogic` — strategies `Boot + Game → GoToGame`, `LevelFail + Game → GoToGame`. Subclass to extend. |
| `IFlowLogic` | Pure-function decision contract. |
| `FlowContext` | Enum — current application screen context. |
| `FlowIntent` | Enum — navigation/logic intents. Numeric values are stable. |
| `UICloseReasons` | Enum — reason a UI view was closed. |

## Usage

```csharp
// 1. Add ApplicationBase + ApplicationFlowController to your boot scene
//    (see Assets/Prefabs/ApplicationBase.prefab and ApplicationFlowController.prefab).
// 2. Wire SerializeFields:
//    - ApplicationBase.ApplicationStateMachine: your FiniteStateMachine asset
//      (this is the only FSM reference in the system — the controller reads
//      it from ApplicationBase, it does not have its own FSM field)
//    - ApplicationFlowController.ApplicationBase: reference to that same
//      ApplicationBase
//    - ApplicationTimeMachine: optional TimeMachine asset for the per-second tick loop
//    - AppPaused / AppResumed: GameEvent assets fired on app lifecycle
//    - AppPausedTime: DBInt asset for the pause timestamp
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
        Add(FlowContext.Settings, UICloseReasons.ResumeGame, FlowIntent.ResumePrevious);
    }
}
```

Then on the `ApplicationFlowController` GameObject, enable **UseCustomLogic** and add a `MyFlowLogic` component.

## Catching missing wiring

`ApplicationBase.ApplicationStateMachine` and `ApplicationFlowController.ApplicationBase` are marked `[RequireReference]` (from `com.madratzz.utilities.attributes`) — run **Tools → Validate Required References** to scan every scene and prefab for either one left unassigned, instead of finding out at runtime. `ApplicationFlowController` still fails loudly (`Debug.LogError` + disables itself) if either check the validator can't cover (e.g. a null reference set at runtime).

`ApplicationBase.AppPausedTime` (a `DBInt`, and every other `ScriptableObject`-typed field here — `ApplicationStateMachine`, `ApplicationTimeMachine`, `AppPaused`, `AppResumed`) gets a searchable-dropdown Inspector automatically from `com.madratzz.utilities.attributes` — no attribute needed, it applies to every `ScriptableObject` field project-wide. `AppPausedTime` stays optional (the pause-timestamp write is skipped if unassigned), so unlike `ApplicationStateMachine` it isn't `[RequireReference]`.

## Requirements

The `scriptableobject.*` package family (already embedded under `Packages/`): `eventsystem.extensions`, `statemachine.core`, `time.machine`, `variables`, `variables.database`. `com.madratzz.utilities.attributes` (for `[RequireReference]` and the searchable-dropdown Inspector). No third-party dependencies.

## License

MIT
