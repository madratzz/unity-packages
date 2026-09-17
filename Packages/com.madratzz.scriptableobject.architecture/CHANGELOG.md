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
- `ApplicationFlowController` — MonoBehaviour that routes GameEvent triggers to `FlowIntent` decisions and fires FSM transitions; SerializeField-wired, defaults to `ApplicationFlowLogic` (override via `useCustomLogic` + a component on the GameObject)

### Changed
- Base port on the asteroids-demo's working `_Game/Core/` rather than the archived sample's older revision — the demo is a third-generation cleanup of the same code with a smaller intent map and dead UI features stripped
- `ApplicationFlowLogic`'s default strategies use the demo's slimmed 2-entry table instead of the archive's 11-entry table (Settings, MainMenu, LevelComplete contexts commented out in the demo)

### Fixed
- `ApplicationBase` and `ApplicationFlowController` no longer require VContainer — `[Inject] Construct(...)` removed; SerializeField wiring is the only path. Keeps the package DI-free.
- `ApplicationFlowController.PerformTransition` no longer checks for `UIViewTransition` — that type belongs to a `ProjectCore.UI` subsystem not in this package. The check was dead code without the supporting types.
- `Camera.main` lookup in `ApplicationFlowController.Awake` removed (was paired with the removed `UIViewTransition` branch).

### Removed
- No samples, no prefabs, no scene assets — the package ships framework primitives only. The asteroids-demo project is the working integration example.
