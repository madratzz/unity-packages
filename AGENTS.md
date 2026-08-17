# Repository Guide for AI Agents and Harnesses

This is the repository entry point for any AI coding agent, automation harness, or human-maintained workflow. It defines how to discover context, make safe changes, validate work, and preserve knowledge in `unity-packages`.

## Instruction Precedence

Within this repository, follow instructions in this order:

1. Direct user or task instructions.
2. This `AGENTS.md` file.
3. More-specific instructions located nearer to the files being changed.
4. Active project context in `.agents/`.
5. Archived context in `.archive/` when deeper history is required.

Higher-priority platform, security, and system instructions always take precedence over repository guidance.

## Start-of-Work Procedure

Before changing files:

1. Read this file.
2. Read `README.md` and `IDEA.md` for the public project purpose and high-level intent.
3. Read `.agents/README.md` and `.agents/INDEX.md`.
4. Read `.agents/context.md`, `.agents/memory.md`, `.agents/learnings.md`, and `.agents/logs.md`.
5. Read `.agents/agents/default-agent.md` unless a more-specific agent file applies.
6. Inspect the relevant source files, package manifest, project settings, Git status, and existing tests before choosing an implementation.
7. Consult `.archive/` only through its category indexes when the active summary does not answer the question.

Do not infer undocumented product requirements, package APIs, registry policies, or architectural boundaries. Record unresolved questions instead.

## Project Snapshot

- `unity-packages` is a Unity project for reusable personal Unity game-development packages.
- The intended distribution mechanism is a Verdaccio package registry.
- The current Unity editor version is recorded in `ProjectSettings/ProjectVersion.txt`; at the time this guide was added it was Unity `6000.3.21f1`.
- Unity package dependencies are defined by `Packages/manifest.json`.
- The project currently has an early-stage source baseline; establish package and assembly boundaries deliberately before adding reusable code.

## Repository Layout

| Path | Role | Agent Guidance |
|---|---|---|
| `Assets/` | Unity assets and project content | Do not add package code here unless the package layout explicitly requires it. |
| `Packages/` | Unity Package Manager configuration | Treat `manifest.json` and `packages-lock.json` as dependency records; make dependency changes deliberately. |
| `ProjectSettings/` | Unity configuration | Preserve Unity serialization and only change settings required by the task. |
| `.agents/` | Active, concise project context | Read at startup and update after meaningful work. |
| `.archive/` | Dated historical context | Archive rather than delete useful history. |
| `AGENTS.md` | Repository-wide operating guide | Keep this guide concise, accurate, and harness-neutral. |
| `IDEA.md` | High-level project intent | Update only when the product/package direction changes. |

Never store durable information in Unity-generated directories such as `Library/`, `Temp/`, `Logs/`, or `UserSettings/`.

## Change Discipline

1. **Discover first.** Inspect existing conventions, dependency versions, and relevant tests before editing.
2. **Keep scope explicit.** Change only files needed for the task. Do not stage unrelated modifications or untracked files.
3. **Preserve boundaries.** Prefer clear package, assembly, and interface boundaries over convenience coupling. Do not introduce global state, service locators, or scene searches as hidden dependencies. Cross-system wiring should be explicit — ScriptableObject references (the project's SOAP architecture) or constructor injection — never implicit lookups.
4. **Keep changes reviewable.** Use focused, logically independent commits when commits are requested. Do not mix documentation, generated files, dependency upgrades, and feature code without a concrete reason.
5. **Treat generated state as disposable.** Do not commit `Library/`, `Temp/`, `Logs/`, `UserSettings/`, build output, or IDE caches.
6. **Do not fabricate results.** Report blocked validation, unavailable tools, and failed builds honestly.

## Unity and Package Work

When implementing a package or runtime feature:

- Confirm the intended package location, package name, public API, and assembly boundary before creating source files.
- Keep runtime code, editor code, tests, and samples separated according to Unity package conventions once a package structure is selected.
- Prefer ScriptableObject Architecture (SOAP) for shared state and cross-system communication: ScriptableObject variables, events, and small single-purpose systems, following the conventions of the `com.madratzz.scriptableobject.*` packages. Dependency injection (VContainer) is an acceptable option for genuine service dependencies, but is not a default requirement — do not mandate a DI container where ScriptableObject wiring is sufficient. In all cases, prefer narrow interfaces for cross-system behavior.
- Add or update Unity Test Framework Edit Mode tests for deterministic logic; use Play Mode tests only for engine/scene integration that requires them.
- Avoid per-frame allocations, implicit scene lookups, and implementation details that prevent isolated testing.
- Validate on the lowest-cost relevant path first, then run Unity or device-specific validation when the requested change requires it.

This repository does not currently document a canonical Unity executable path or CI command. Discover available tooling before running a build; do not invent build commands.

## Documentation and Context Maintenance

After meaningful work:

1. Add a newest-first entry to `.agents/logs.md` with the date/time, work completed, files touched, decisions, issues, and next steps.
2. Update `.agents/context.md` when project goals, architecture, structure, or constraints change.
3. Update `.agents/memory.md` only for stable, durable facts and explicit user preferences.
4. Update `.agents/learnings.md` for useful discoveries, failures, and repeatable gotchas.
5. Keep active files concise. When an active file crosses its documented threshold or a new day begins, summarize older material, create a dated archive file, and insert the newest archive-index entry first.
6. Update `.agents/INDEX.md` and every relevant archive index after archive operations.

Use ISO dates (`YYYY-MM-DD`) in content. Use `DD-MM-YY` plus an optional short lowercase hyphenated slug in archive filenames.

## Sensitive Data and External Actions

- Never add secrets, credentials, passwords, tokens, API keys, private keys, or personal account data to source, logs, context, commits, or pull requests.
- Replace sensitive values with placeholders such as `<SECRET>`, `<TOKEN>`, or `<API_KEY>` if they must be discussed.
- Do not publish packages, change registry configuration, alter GitHub settings, or perform other external side effects unless the task explicitly asks for them.
- Before any irreversible operation, verify the target branch, files, and scope.

## Verification and Completion

Before declaring work complete:

- Run the most relevant available checks and report actual results.
- For Markdown-only changes, validate changed links, formatting, and Git whitespace errors.
- For code changes, run targeted tests first and expand validation when the change crosses package, Unity, or platform boundaries.
- Confirm `git status` and identify any unrelated existing changes rather than claiming a clean tree incorrectly.
- Summarize the change, validation performed, unresolved risks, and any follow-up work.
