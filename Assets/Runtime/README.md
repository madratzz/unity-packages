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

The template ships a **working** four-screen flow — **MainMenu, Gameplay, Settings, Store** — under `Assets/StateMachine`, `Assets/GameEvents`, `Assets/Variables` and `Assets/UI`. Press Play in `BootstrapScene` and the main menu appears; the buttons navigate. The FSM boots into `MainMenuState`.

### How a screen gets on screen

| Type | Role |
|---|---|
| `UIViewState` | A `State` that owns one screen. Instantiates its `ViewPrefab` on `Execute`, `Hide` on `Pause`, `Show` on `Resume`, destroys on `Exit`. |
| `GameState` | `UIViewState` + a scene. Loads `GameScene` **additively** before showing its view, and unloads it on `Exit`. `GameplayState` uses this. |
| `UIView` | Sits on the screen prefab. Exposes `Show`/`Hide`, and `Close(int reason)` which raises its `ClosedEvent` with a `UICloseReasons` value. |
| `VariableLabel` | Writes a ScriptableObject `Int` into a UI `Text`, refreshing when the variable's `ValueChanged` event fires. |

### Gameplay: scene + HUD

`GameplayState` is a `GameState`, so entering it loads `Assets/Scenes/GameScene.unity` additively and puts `GameHudView` over it — a transparent HUD (top stat bar, compact buttons bottom-right), not a full-screen menu, so the game shows through.

The load is **additive, never single**: `ApplicationBase` and `ApplicationFlowController` live in the boot scene and are not `DontDestroyOnLoad`, so a single-mode load would destroy the FSM owner mid-transition and strand the flow. `GameScene` must therefore stay in Build Settings (it is at index 1; `BootstrapScene` stays at 0 so the *always start from scene zero* utility still boots correctly).

`GameScene` has no camera or `AudioListener` of its own — the boot scene's are the persistent pair. A second set would render twice and log *"there are 2 audio listeners in the scene"*.

Opening Settings or Store over gameplay hides the HUD but leaves `GameScene` loaded and running underneath; closing returns to it untouched.

### The HUD reads live variables

The HUD's two labels are bound to ScriptableObject variables through `VariableLabel`, so they are not static text:

| Label | Variable | Refreshes on |
|---|---|---|
| `SCORE {0}` | `v_Score` (`IntWithEvent`, session-only) | `e_ScoreChanged` |
| `COINS {0}` | `v_Coins` (`DBIntWithEvent`, persisted) | `e_CoinsChanged` |

Anything that calls `SetValue` or `ApplyChange` on those variables repaints the HUD — no polling, no `Update`, no reference from the variable back to the UI:

```csharp
[SerializeField] private Int Score;   // drop v_Score in
Score.ApplyChange(100);               // HUD updates
```

`VariableLabel.Variable` is typed as `Int`, so it accepts `Int`, `DBInt`, `IntWithEvent` and `DBIntWithEvent` alike. The `ValueChanged` event is wired on both the variable and the label rather than the label reaching into the variable, so a plain `Int` someone else raises an event for works too. Leave the event empty for a read-once value: the label still shows the right number on enable, it just won't follow later changes.

A view never decides where to go next — it only reports *why* it closed. The controller turns that `(context, reason)` pair into the next transition, so screens stay ignorant of each other.

Wire a Button's `onClick` to `UIView.Close` with the reason as the int argument (`Home` 1, `Game` 2, `Settings` 3, `ResumeGame` 4, `Store` 7).

Because `Pause`/`Resume` hide and re-show rather than destroy and rebuild, an overlay is cheap: opening Settings over Gameplay leaves the gameplay screen alive and merely hidden, and closing it restores exactly what was there.

The placeholder prefabs in `Assets/UI` use **legacy `UnityEngine.UI.Text`, not TextMeshPro** — TMP's essential resources are not imported in this project, so TMP labels would render as missing-font boxes. Swap them once you import TMP.

`BootstrapScene` carries an `EventSystem` with an **`InputSystemUIInputModule`**; this project is set to the new Input System only (`activeInputHandler: 1`), where the legacy `StandaloneInputModule` does nothing and every button would be dead.

`Tools → Project Bootstrap → Build Screen Views` regenerates the four prefabs and re-wires the states. It is re-runnable and overwrites rather than duplicates.

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
