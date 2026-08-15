# Active Project Context

Last updated: 2026-08-15

## Project Summary

`unity-packages` is a Unity project intended to host a collection of personal Unity game-development packages. The packages are planned to provide reusable tools, foundations, and architecture for game development and are intended to be hosted through a Verdaccio registry.

## Current Goals

- Establish a durable, repository-local agent-context system without placing operational context in Unity's `Assets/` tree.
- Bootstrap reusable Unity package foundations and define package boundaries as implementation work begins.

## Current Architecture / Structure

- `Assets/` — Unity content; no tracked C# scripts or assembly definitions were present at setup time.
- `Packages/` — Unity Package Manager manifest and lockfile; the manifest includes URP, Input System, AI Navigation, Test Framework, and Unity AI packages.
- `ProjectSettings/` — Unity project configuration.
- `.agents/` — concise, active context for agents.
- `.archive/` — dated historical context, organized by logs, memory, learnings, context snapshots, and agent files.

## Important Decisions

- Active agent material is maintained in `.agents/`; detailed historical material is retained in `.archive/` rather than deleted.
- The context system uses Markdown, relative links, ISO dates in content, DD-MM-YY archive filenames, and reverse-chronological indexes.
- Context files must never include credentials or other sensitive values; use placeholders if redaction is required.

## Active Constraints

- The project currently uses Unity `6000.3.21f1` (Unity 6.3) and URP `17.3.0`.
- Keep AI-agent operational files outside Unity runtime folders and avoid generated folders such as `Library/`, `Temp/`, `Logs/`, and `UserSettings/`.
- Maintain context changes idempotently: update existing sections carefully, archive before major restructuring, and never overwrite archive files without explicit user direction.
- Keep active files concise and retain holistic archive summaries and pointers after archival.

## Current Open Questions

- Which reusable Unity packages should be implemented first, and what public package/assembly boundaries should they expose?
- What Verdaccio package naming, versioning, publishing, and access conventions should this repository adopt?
- Should the agent-context folders remain versioned in Git for team sharing, or should a future repository policy exclude some local-only context?

## Archive Summary

No archived project-context snapshots exist yet. This active file records the initial repository baseline and setup decisions.

## Archive Pointers

- [Archived Context Index](../.archive/context/INDEX.md)
