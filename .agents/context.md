# Active Project Context

Last updated: 2026-09-20

## Project Summary

`unity-packages` is a Unity project hosting a collection of personal Unity game-development packages, and — via its `Assets/` template/integration layer — a clonable SOAP-based project template built on top of them. Packages are distributed independently (intended Verdaccio registry); the `Assets/` layer is not a package, it's cloned with the repo. See `IDEA.md`.

## Current Goals

- Maintain a durable, repository-local agent-context system without placing operational context in Unity's `Assets/` tree.
- Provide a root `AGENTS.md` guide so AI agents and automation harnesses start from one consistent workflow.
- Bootstrap reusable Unity package foundations and define package boundaries as implementation work begins.
- Build out the `Assets/` GameFlow template layer (boot/game scenes, prefabs, wired ScriptableObject assets) as a working demonstration of the package family end-to-end.

## Current Architecture / Structure

- `AGENTS.md` — repository-wide operating guide for AI agents and automation harnesses.
- `IDEA.md` — high-level project intent; defines the `Packages/` (reusable, Verdaccio-distributed) vs. `Assets/` (clonable template/integration layer) split.
- `Assets/` — the project template layer: `Assets/Runtime` + `Assets/Tests` hold the GameFlow application code (`ApplicationBase`, `ApplicationFlowController`, decision-table logic — see `Assets/Runtime/README.md`) that wires the package family together; `Assets/Prefabs` and `Assets/Scenes` (`BootstrapScene`, `GameScene`) are the scaffolding; `Assets/GameEvents` holds the `GameEvent` assets the flow raises/listens to (currently empty, not yet populated). This code was previously a standalone package (`com.madratzz.scriptableobject.architecture`) under `Packages/`; moved into `Assets/` deliberately (2026-09-20) as the template's integration layer rather than a reusable package.
- `Packages/` — Unity Package Manager manifest and lockfile; the manifest includes URP, Input System, AI Navigation, Test Framework, and Unity AI packages. Sixteen custom embedded packages live under `Packages/`: the `com.madratzz.utilities.*` family (`attributes`, `core`, `coroutines`, `ui`, `addressables`, `buildautomation`, `unity.alwaysstartfromscenezero`), `com.madratzz.platform.device`, and the `com.madratzz.scriptableobject.*` (SOAP) family (`variables`, `variables.database`, `variables.extensions`, `eventsystem.core`, `eventsystem.extensions`, `event.variables`, `statemachine.core`, `time.machine`). Utility packages use `com.madratzz.*` assembly names; scriptableobject packages use `madratzz.scriptableobject.*`; namespaces remain the archived `ProjectCore.*` / `CustomUtilities.*` / `ExtensionMethods` mix (cleanup deferred).
- `ProjectSettings/` — Unity project configuration.
- `.agents/` — concise, active context for agents.
- `.archive/` — dated historical context, organized by logs, memory, learnings, context snapshots, and agent files.

## Important Decisions

- `AGENTS.md` is the root entry point for repository-wide AI-agent and automation-harness workflow; `.agents/` supplies current project context.
- Active agent material is maintained in `.agents/`; detailed historical material is retained in `.archive/` rather than deleted.
- The context system uses Markdown, relative links, ISO dates in content, DD-MM-YY archive filenames, and reverse-chronological indexes.
- Context files must never include credentials or other sensitive values; use placeholders if redaction is required.
- (2026-09-20, user decision) This repo is both a package library and a clonable template: reusable SOAP/utility packages stay under `Packages/` as before; the GameFlow application layer lives in `Assets/` as template/integration glue, not as a package. The formerly-hollow `Packages/com.madratzz.scriptableobject.architecture/` (package.json with no code, after the code moved to `Assets/`) was deleted rather than kept as a misleading stub.

## Active Constraints

- The project currently uses Unity `6000.3.24f1` (Unity 6.3) and URP `17.3.0`.
- Keep AI-agent operational files outside Unity runtime folders and avoid generated folders such as `Library/`, `Temp/`, `Logs/`, and `UserSettings/`.
- Maintain context changes idempotently: update existing sections carefully, archive before major restructuring, and never overwrite archive files without explicit user direction.
- Keep active files concise and retain holistic archive summaries and pointers after archival.

## Current Open Questions

- What Verdaccio publishing conventions should the 5 packages still without EditMode tests (`utilities.attributes`, `utilities.core`, `utilities.coroutines`, `utilities.ui`, `platform.device`) adopt before first publish? (11 of 16 packages already have a `Tests/` folder.)
- What Verdaccio package naming, versioning, publishing, and access conventions should this repository adopt?
- Should `Assets/GameEvents/` (currently empty) hold hand-authored `GameEvent` assets checked into the repo, or should the template ship with none and expect consumers to create their own?

## Archive Summary

No archived project-context snapshots exist yet. This active file records the initial repository baseline and setup decisions.

## Archive Pointers

- [Archived Context Index](../.archive/context/INDEX.md)
