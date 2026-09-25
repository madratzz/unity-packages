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

Both prefabs ship pre-wired to the assets described under **Shipped screen flow** below, so the list here is what to change when you point them at your own assets rather than a set-up checklist.

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

To extend the decision table, subclass and call `Add` — it writes through an indexer, so it both adds new pairs and overrides shipped ones:

```csharp
public class MyFlowLogic : ApplicationFlowLogic
{
    public MyFlowLogic()
    {
        Add(FlowContext.LevelFail, UICloseReasons.Revive,    FlowIntent.GoToGame);   // new
        Add(FlowContext.MainMenu,  UICloseReasons.Game,      FlowIntent.OpenStore);  // override
    }
}
```

Then on the `ApplicationFlowController` GameObject, enable **UseCustomLogic** and add a `MyFlowLogic` component.

## Shipped screen flow

The template ships assets for four screens — **MainMenu, Gameplay, Settings, Store** — under `Assets/StateMachine`, `Assets/GameEvents` and `Assets/Variables`, and both prefabs come pre-wired to them. The FSM boots into `MainMenuState`.

`SettingsState` and `StoreState` set `PausesPreviousState`, so they overlay whatever is running: closing one with `UICloseReasons.ResumeGame` resolves to `ResumePrevious` and drops back underneath, while `UICloseReasons.Home` routes to the menu.

| Context | Close reason | Intent |
|---|---|---|
| `Boot` | `Game` / `Home` | `GoToGame` / `GoToMainMenu` |
| `MainMenu` | `Game` / `Settings` / `Store` | `GoToGame` / `OpenSettings` / `OpenStore` |
| `Gameplay` | `Home` / `Settings` / `Store` | `GoToMainMenu` / `OpenSettings` / `OpenStore` |
| `Settings`, `Store` | `ResumeGame` / `Home` | `ResumePrevious` / `GoToMainMenu` |
| `LevelFail` | `Game` / `Store` / `Home` | `GoToGame` / `OpenStore` / `GoToMainMenu` |

Anything unlisted falls through to `DefaultToGame`.

A view reports its dismissal by raising the matching `e_*ViewClosed` `GameEventWithInt` with a `UICloseReasons` value as the payload; the `e_Goto*` plain `GameEvent`s are direct commands that skip the table. `ApplicationFlowController.Boot()` still asks for `(Boot, Game)`, so it jumps straight to gameplay — the FSM's `BootState` is what puts the menu first. Change `Boot()` to `(Boot, Home)` if you want the boot hook itself to land on the menu.

There is deliberately **no** `LevelFailState` or `ToLevelFail` transition: `LevelFail` predates these four screens and its `LevelFailTransition` field is left unassigned until you author that screen.

## Catching missing wiring

`ApplicationBase.ApplicationStateMachine` and `ApplicationFlowController.ApplicationBase` are marked `[RequireReference]` (from `com.madratzz.utilities.attributes`) — run **Tools → Validate Required References** to scan every scene and prefab for either one left unassigned, instead of finding out at runtime. `ApplicationFlowController` still fails loudly (`Debug.LogError` + disables itself) if either check the validator can't cover (e.g. a null reference set at runtime).

`ApplicationBase.AppPausedTime` (a `DBInt`, and every other `ScriptableObject`-typed field here — `ApplicationStateMachine`, `ApplicationTimeMachine`, `AppPaused`, `AppResumed`) gets a searchable-dropdown Inspector automatically from `com.madratzz.utilities.attributes` — no attribute needed, it applies to every `ScriptableObject` field project-wide. `AppPausedTime` stays optional (the pause-timestamp write is skipped if unassigned), so unlike `ApplicationStateMachine` it isn't `[RequireReference]`.

## Requirements

The `scriptableobject.*` package family (already embedded under `Packages/`): `eventsystem.extensions`, `statemachine.core`, `time.machine`, `variables`, `variables.database`. `com.madratzz.utilities.attributes` (for `[RequireReference]` and the searchable-dropdown Inspector). No third-party dependencies.

## License

MIT
