# Active Project Context

Last updated: 2026-09-20

## Project Summary

`unity-packages` is a Unity project intended to host a collection of personal Unity game-development packages. The packages are planned to provide reusable tools, foundations, and architecture for game development and are intended to be hosted through a Verdaccio registry.

## Current Goals

- Maintain a durable, repository-local agent-context system without placing operational context in Unity's `Assets/` tree.
- Provide a root `AGENTS.md` guide so AI agents and automation harnesses start from one consistent workflow.
- Bootstrap reusable Unity package foundations and define package boundaries as implementation work begins.

## Current Architecture / Structure

- `AGENTS.md` — repository-wide operating guide for AI agents and automation harnesses (canonical policy source, including version-control/branch/PR policy and version naming).
- `CLAUDE.md` — short pointer to `AGENTS.md` for Claude Code; never duplicates policy.
- `IDEA.md` — high-level project intent and intended Verdaccio distribution.
- `Assets/` — Unity content; URP settings, sample scene, and Input System actions only (no tracked C# scripts).
- `Packages/` — Unity Package Manager manifest and lockfile; the manifest includes URP, Input System, AI Navigation, Test Framework, and Unity AI packages. Sixteen custom embedded packages now live under `Packages/` (up from the initial eight): the `com.madratzz.utilities.*` family (`attributes`, `core`, `coroutines`, `ui`, `addressables`, `buildautomation`, `unity.alwaysstartfromscenezero`), `com.madratzz.platform.device`, and the `com.madratzz.scriptableobject.*` (SOAP) family (`variables`, `variables.database`, `variables.extensions`, `eventsystem.core`, `eventsystem.extensions`, `event.variables`, `statemachine.core`, `time.machine`). A seventeenth, `scriptableobject.architecture` (the GameFlow orchestrator), exists only on the unmerged `feature/architecture` branch. Utility packages use `com.madratzz.*` assembly names; scriptableobject packages use `madratzz.scriptableobject.*`; namespaces remain the archived `ProjectCore.*` / `CustomUtilities.*` / `ExtensionMethods` mix (cleanup deferred). Full catalog and dependency graph: `Docs/Home.md` / `Docs/Architecture Overview.md`.
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

## Active Constraints

- The project currently uses Unity `6000.3.24f1` (Unity 6.3) and URP `17.3.0`.
- Keep AI-agent operational files outside Unity runtime folders and avoid generated folders such as `Library/`, `Temp/`, `Logs/`, and `UserSettings/`.
- Maintain context changes idempotently: update existing sections carefully, archive before major restructuring, and never overwrite archive files without explicit user direction.
- Keep active files concise and retain holistic archive summaries and pointers after archival.

## Current Open Questions

- What Verdaccio publishing conventions should the 5 packages still without EditMode tests (`utilities.attributes`, `utilities.core`, `utilities.coroutines`, `utilities.ui`, `platform.device`) adopt before first publish? (11 of 16 packages already have a `Tests/` folder — the "no package has tests yet" framing of this question is superseded.)
- What Verdaccio package naming, versioning, publishing, and access conventions should this repository adopt?
- `[SerializeField]` field-naming convention is inconsistent across packages: PascalCase in the ported `scriptableobject.*` family vs. camelCase in the newer `scriptableobject.architecture` package. Not yet resolved — see `AGENTS.md` "Unity and Package Work".
- The 2026-09-20 editor bump to `6000.3.24f1` added `com.unity.sdk.linux-x86_64` and `com.unity.toolchain.linux-x86_64-linux` to `Packages/manifest.json` as project-wide dependencies. Assumption: these were pulled in by Unity Hub/Editor package resolution during the version bump rather than a deliberate Linux-build-target decision — not confirmed. Open question: should these be scoped/conditional rather than a default dependency for every contributor?

## Archive Summary

No archived project-context snapshots exist yet. This active file records the initial repository baseline and setup decisions.

## Archive Pointers

- [Archived Context Index](../.archive/context/INDEX.md)
