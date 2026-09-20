# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).
The initial release collects all pre-release changes into a single entry; versions will increment once the package is first published to the Verdaccio registry.

## [0.0.1] - Unreleased

### Added
- Initial release — top-level GameFlow orchestrator that wires FSM transitions to a pure-function (Context, Reason) → Intent strategy table
- `FlowContext` enum — current application screen context (`Boot`, `LevelFail`, `None`)
- `FlowIntent` enum — navigation and logic intents (`GoToGame`, `GoToLevelFail`, `ResumePrevious`, `DefaultToGame`, `None`)
- `UICloseReasons` enum — reason a UI view was closed (`Home`, `Game`, `Settings`, `ResumeGame`, `Revive`, `SkipLevel`, `None`)
- `IFlowLogic` — pure-function decision contract; implementations are deterministic and side-effect free
- `ApplicationFlowLogic` — default `IFlowLogic` with strategies `Boot + Game → GoToGame` and `LevelFail + Game → GoToGame`; subclass and use the protected `Add` hook to register more strategies without forking the controller
- `ApplicationBase` — MonoBehaviour that owns the FSM and TimeMachine coroutine loops and the application lifecycle events (pause/resume with timestamp persistence); SerializeField-wired, no DI
- `ApplicationFlowController` — MonoBehaviour that routes GameEvent triggers to `FlowIntent` decisions and fires FSM transitions; SerializeField-wired, defaults to `ApplicationFlowLogic` (override via `UseCustomLogic` + a component on the GameObject)

### Changed
- Base port on the asteroids-demo's working `_Game/Core/` rather than the archived sample's older revision — the demo is a third-generation cleanup of the same code with a smaller intent map and dead UI features stripped
- `ApplicationFlowLogic`'s default strategies use the demo's slimmed 2-entry table instead of the archive's 11-entry table (Settings, MainMenu, LevelComplete contexts commented out in the demo)
- All `[SerializeField] private` fields on `ApplicationBase` and `ApplicationFlowController` renamed to PascalCase (`applicationBase` → `ApplicationBase`, `gameStateTransition` → `GameStateTransition`, `gotoGame` → `GotoGame`, `useCustomLogic` → `UseCustomLogic`, etc.) to match the rest of the ported `scriptableobject.*` package family's convention — resolves this package's contribution to the project's open `[SerializeField]` casing question.

### Fixed
- `ApplicationBase` and `ApplicationFlowController` no longer require VContainer — `[Inject] Construct(...)` removed; SerializeField wiring is the only path. Keeps the package DI-free.
- `ApplicationFlowController.PerformTransition` no longer checks for `UIViewTransition` — that type belongs to a `ProjectCore.UI` subsystem not in this package. The check was dead code without the supporting types.
- `Camera.main` lookup in `ApplicationFlowController.Awake` removed (was paired with the removed `UIViewTransition` branch).
- **Duplicate, unlinked FSM references removed**: `ApplicationFlowController` no longer has its own `FiniteStateMachine` `[SerializeField]` — it now reads `ApplicationBase.StateMachine` through a new `[SerializeField] ApplicationBase` reference. Previously the two components could be wired to different FSM assets (or one left unassigned) with no error, silently breaking transitions.
- `ApplicationBase.Awake()` no longer falls back to `Resources.Load<FiniteStateMachine>("StateMachine")` — that implicit magic-string lookup violated this repo's explicit-wiring rule and could silently resolve to the wrong asset. An unassigned `ApplicationStateMachine` now logs a warning instead.
- README's "extend the decision table" example called `FlowIntent.GoToMainMenu`, which doesn't exist on the enum — replaced with `FlowContext.Settings, UICloseReasons.ResumeGame, FlowIntent.ResumePrevious`, matching the actual `ApplicationFlowLogicTests` example.
- Removed a redundant self-referencing `using ProjectCore.Architecture;` in `IFlowLogic.cs` (the file is itself declared inside that namespace).

### Removed
- No samples, no prefabs, no scene assets — the package ships framework primitives only. The asteroids-demo project is the working integration example.
