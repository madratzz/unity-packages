# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).
The initial release collects all pre-release changes into a single entry; versions will increment once the package is first published to the Verdaccio registry.

## [0.0.1] - Unreleased

### Added
- Initial release
- `FiniteStateMachine` ScriptableObject — coroutine-driven FSM runner; call `Tick()` to drive the loop (e.g. via `CoroutineHandler.StartStaticCoroutine(fsm.Tick())`)
- `State` ScriptableObject base — override `Init`/`Execute`/`Tick`/`Exit`/`Pause`/`Resume`/`Cleanup`
- `Transition` ScriptableObject — links a source state to a target state with an optional execution coroutine
- `IState` interface — external control via `TransitionTo` and `CleanupAllPausedStates`

### Changed
- State lifecycle methods now carry `<summary>` XML documentation matching the package README
- `PausedStates` is a `readonly` field initialised inline (equivalent behavior, clearer intent)

### Fixed
- `Tick()` re-runs `Init`/`Execute` after every transition (not just the boot path) — the archived code transitioned to the new state but never called `Init` on it, so transition targets ran `Tick` without ever being initialized
- `Tick()` null-guards `CurrentState` before calling `Tick()` on it — the resume-pop path could leave a null state if the stack contained a destroyed state
- `Tick()` null-guards the popped state on resume — `Stack.Pop` can return null if the popped state was destroyed
- `Transition()` rejects null transitions and transitions with null `ToState` early (was a single `&&` chain, now explicit)
- `CleanupAllPausedStates` skips cleanup when called for a non-current state — was a footgun: any caller would empty the stack regardless of ownership
- Corrected asmdef `name` from `madratzz.scriptableobjectstatemachinecore.runtime` to `madratzz.scriptableobject.statemachine.core.runtime` (missing dots)
