# Active Project Context

Last updated: 2026-09-25

## Project Summary

`unity-packages` is a Unity project hosting a collection of personal Unity game-development packages, and — via its `Assets/` template/integration layer — a clonable SOAP-based project template built on top of them. Packages are distributed independently (intended Verdaccio registry); the `Assets/` layer is not a package, it's cloned with the repo. See `IDEA.md`.

## Current Goals

- Maintain a durable, repository-local agent-context system without placing operational context in Unity's `Assets/` tree.
- Provide a root `AGENTS.md` guide so AI agents and automation harnesses start from one consistent workflow.
- Bootstrap reusable Unity package foundations and define package boundaries as implementation work begins.
- Build out the `Assets/` GameFlow template layer (boot/game scenes, prefabs, wired ScriptableObject assets) as a working demonstration of the package family end-to-end.

## Current Architecture / Structure

- `AGENTS.md` — repository-wide operating guide for AI agents and automation harnesses (canonical policy source, including version-control/branch/PR policy and version naming).
- `CLAUDE.md` — short pointer to `AGENTS.md` for Claude Code; never duplicates policy.
- `IDEA.md` — high-level project intent; defines the `Packages/` (reusable, Verdaccio-distributed) vs. `Assets/` (clonable template/integration layer) split.
- `Assets/` — the project template layer: `Assets/Runtime` + `Assets/Tests` hold the GameFlow application code (`ApplicationBase`, `ApplicationFlowController`, decision-table logic — see `Assets/Runtime/README.md`) that wires the package family together; `Assets/Prefabs` and `Assets/Scenes` (`BootstrapScene`, `GameScene`) are the scaffolding; `Assets/GameEvents` holds the `GameEvent` assets the flow raises/listens to (currently empty, not yet populated). This code was previously a standalone package (`com.madratzz.scriptableobject.architecture`) under `Packages/`; moved into `Assets/` deliberately (2026-09-20) as the template's integration layer rather than a reusable package — it is not a 17th package.
- `Packages/` — Unity Package Manager manifest and lockfile; the manifest includes URP, Input System, AI Navigation, Test Framework, and Unity AI packages. Sixteen custom embedded packages live under `Packages/` (up from the initial eight): the `com.madratzz.utilities.*` family (`attributes`, `core`, `coroutines`, `ui`, `addressables`, `buildautomation`, `unity.alwaysstartfromscenezero`), `com.madratzz.platform.device`, and the `com.madratzz.scriptableobject.*` (SOAP) family (`variables`, `variables.database`, `variables.extensions`, `eventsystem.core`, `eventsystem.extensions`, `event.variables`, `statemachine.core`, `time.machine`). Utility packages use `com.madratzz.*` assembly names; scriptableobject packages use `madratzz.scriptableobject.*`; namespaces remain the archived `ProjectCore.*` / `CustomUtilities.*` / `ExtensionMethods` mix (cleanup deferred). Full catalog and dependency graph: `Docs/Home.md` / `Docs/Architecture Overview.md` — GameFlow's vault note moved to `Docs/GameFlow (Template Layer).md` (out of `Docs/Packages/`) and no longer describes itself as a 17th package (2026-09-21).
- `ProjectSettings/` — Unity project configuration.
- `.agents/` — concise, active context for agents.
- `.archive/` — dated historical context, organized by logs, memory, learnings, context snapshots, and agent files.
- `Docs/` — human-facing wiki (Obsidian vault; entry point `Docs/Home.md`) covering getting-started, architecture, contributing, and a per-package catalog. Links out to package READMEs and `AGENTS.md` rather than duplicating them.

## Important Decisions

- `AGENTS.md` is the root entry point for repository-wide AI-agent and automation-harness workflow; `.agents/` supplies current project context. `CLAUDE.md` is a thin pointer to `AGENTS.md`, not a second source of policy.
- Active agent material is maintained in `.agents/`; detailed historical material is retained in `.archive/` rather than deleted.
- The context system uses Markdown, relative links, uppercase active/archive filenames (2026-09-16, superseding the earlier lowercase convention), ISO `YYYY-MM-DD` dates everywhere including archive filenames (superseding `DD-MM-YY`), and reverse-chronological indexes.
- Context files must never include credentials or other sensitive values; use placeholders if redaction is required.
- Version-control policy (feature branches from `development`, PR-only merges, no squash-merge, `main` only via PR from `development`) and version naming (`X.Y.Z` with epoch-minute `Z`) are documented once in `AGENTS.md`; do not duplicate them here.
- (2026-09-20, user decision) This repo is both a package library and a clonable template: reusable SOAP/utility packages stay under `Packages/` as before; the GameFlow application layer lives in `Assets/` as template/integration glue, not as a package. The formerly-hollow `Packages/com.madratzz.scriptableobject.architecture/` (package.json with no code, after the code moved to `Assets/`) was deleted rather than kept as a misleading stub.
- (2026-09-20) `com.madratzz.utilities.attributes` gained `[RequireReference]` + `RequiredReferenceValidator` — a project-wide convention for marking `[SerializeField]` references as required and catching unassigned ones across every scene/prefab (**Tools → Validate Required References**, or `-executeMethod ...ValidateProjectCI` for CI). Applied so far to `GameEventListener`/`GameEventRaiser`/`GameEventRaiserOnEnable`'s `GameEvent` field and `ApplicationBase`/`ApplicationFlowController`'s FSM-chain fields. New required-but-unguarded `[SerializeField]` fields elsewhere should adopt it rather than reinventing null-checking.
- (2026-09-21) `com.madratzz.utilities.attributes` also gained a searchable-dropdown Inspector for **every** `ScriptableObject`-typed `[SerializeField]` field, project-wide — `SearchableAssetDrawer` is registered against `ScriptableObject` itself (`[CustomPropertyDrawer(typeof(ScriptableObject), true)]`), not a marker attribute, deliberately so it's automatic and never needs adding per field/class. Built as the free (no Odin Inspector) alternative evaluated in the prior GameEvent-improvement discussion. First implementation used a `[SearchableAsset]` attribute per field; user asked "can't we make it class level... auto"; replaced with the type-based global registration on the same turn before anything was committed.

## Active Constraints

- The project currently uses Unity `6000.3.24f1` (Unity 6.3) and URP `17.3.0`.
- Keep AI-agent operational files outside Unity runtime folders and avoid generated folders such as `Library/`, `Temp/`, `Logs/`, and `UserSettings/`.
- Maintain context changes idempotently: update existing sections carefully, archive before major restructuring, and never overwrite archive files without explicit user direction.
- Keep active files concise and retain holistic archive summaries and pointers after archival.

## Current Open Questions

- What Verdaccio publishing conventions should the 4 packages still without EditMode tests (`platform.device`, `utilities.coroutines`, `utilities.ui`, `utilities.unity.alwaysstartfromscenezero`) adopt before first publish? (12 of 16 packages already have a `Tests/` folder, up from 11 — `utilities.attributes` gained tests alongside `RequiredReferenceValidator`.)
- What Verdaccio package naming, versioning, publishing, and access conventions should this repository adopt?
- The 2026-09-20 editor bump to `6000.3.24f1` added `com.unity.sdk.linux-x86_64` and `com.unity.toolchain.linux-x86_64-linux` to `Packages/manifest.json` as project-wide dependencies. Assumption: these were pulled in by Unity Hub/Editor package resolution during the version bump rather than a deliberate Linux-build-target decision — not confirmed. Open question: should these be scoped/conditional rather than a default dependency for every contributor?
- Should `Assets/GameEvents/` (currently empty) hold hand-authored `GameEvent` assets checked into the repo, or should the template ship with none and expect consumers to create their own?

- Should the 12 packages that ship EditMode tests declare `com.unity.test-framework` in their `package.json`? None currently does, though their test asmdefs reference `UnityEngine.TestRunner`/`UnityEditor.TestRunner` and Unity's own packages (e.g. `com.unity.addressables`) declare it. Consistent across the family, so it reads as convention rather than oversight — but it should be a deliberate decision before the first Verdaccio publish. (Raised 2026-09-25.)
- Should the 6 genuinely dead asmdef references be swept — `eventsystem.extensions.runtime`/`.tests` → `eventsystem.core`, and `time.machine.tests`/`.playmodetests` → `utilities.core` and `utilities.coroutines`? Harmless at compile time. A related question: should `eventsystem.extensions` keep its `eventsystem.core` `package.json` dependency, the one remaining genuine over-declaration? (Raised 2026-09-25. An earlier count of "11 dead references over-declaring the graph" was wrong — 4 of those are load-bearing via inheritance; see LEARNINGS.)

## Archive Summary

No archived project-context snapshots exist yet. This active file records the initial repository baseline and setup decisions.

## Archive Pointers

- [Archived Context Index](../.archive/context/INDEX.md)
