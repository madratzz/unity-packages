# Repository Guide for AI Agents and Harnesses

This is the repository entry point for any AI coding agent, automation harness, or human-maintained workflow. It defines how to discover context, make safe changes, validate work, preserve knowledge, and manage version control in `unity-packages`. `CLAUDE.md` and any other tool-specific entry point point here rather than duplicating this guide.

## Instruction Precedence

Within this repository, follow instructions in this order:

1. Direct user or task instructions.
2. This `AGENTS.md` file.
3. More-specific instructions located nearer to the files being changed.
4. Active project context in `.agents/`.
5. Archived context in `.archive/` when deeper history is required.

Higher-priority platform, security, and system instructions always take precedence over repository guidance.

Only create or update the context system (`.agents/`, `.archive/`) when the task authorizes writing. For read-only, review, research, or diagnostic work, do not modify context files unless explicitly asked. Keep context maintenance proportionate — record durable, useful information; do not log routine or trivial activity, and never let context maintenance expand scope or bypass a blocker on the primary task.

## Start-of-Work Procedure

Before changing files:

1. Read this file.
2. Read `README.md` and `IDEA.md` for the public project purpose and high-level intent.
3. Read `.agents/README.md` and `.agents/INDEX.md`.
4. Read `.agents/CONTEXT.md` always; read `.agents/MEMORY.md`, `.agents/LEARNINGS.md`, and recent `.agents/LOGS.md` when relevant to the task.
5. Read `.agents/agents/DEFAULT-AGENT.md` unless a more-specific agent profile applies.
6. Inspect the relevant source files, package manifest, project settings, Git status, and existing tests before choosing an implementation.
7. Consult `.archive/` only through its category indexes when the active summary does not answer the question.
8. Reuse compatible existing structure rather than replacing it. Label uncertain observations as assumptions or open questions in `.agents/CONTEXT.md` rather than inventing project facts. Report any incompatible pre-existing context structure before restructuring it.

Do not infer undocumented product requirements, package APIs, registry policies, or architectural boundaries.

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
| `.agents/` | Active, concise project context | Read at startup and update after meaningful, authorized work. |
| `.archive/` | Dated historical context | Archive rather than delete useful history. |
| `AGENTS.md` | Repository-wide operating guide | Keep this guide concise, accurate, harness-neutral, and the single canonical source of policy. |
| `CLAUDE.md` | Tool-specific entry pointer | Keep as a short pointer to this file; never duplicate policy into it. |
| `IDEA.md` | High-level project intent | Update only when the product/package direction changes. |

Never store durable information in Unity-generated directories such as `Library/`, `Temp/`, `Logs/`, or `UserSettings/`.

## Change Discipline

1. **Discover first.** Inspect existing conventions, dependency versions, and relevant tests before editing.
2. **Keep scope explicit.** Change only files needed for the task. Do not stage unrelated modifications or untracked files.
3. **Preserve boundaries.** Prefer clear package, assembly, and interface boundaries over convenience coupling. Do not introduce global state, service locators, or scene searches as hidden dependencies. Cross-system wiring should be explicit — ScriptableObject references (the project's SOAP architecture) or constructor injection — never implicit lookups.
4. **Keep changes reviewable.** Use small, logically independent commits, each a cohesive reviewable unit including its relevant documentation and agent-context updates. Do not mix documentation, generated files, dependency upgrades, and feature code without a concrete reason.
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
- **C# field-naming convention is not yet settled.** The ported `scriptableobject.*` packages use PascalCase for `[SerializeField]` fields (e.g. `[SerializeField] private float TickInterval;`); the newer `scriptableobject.architecture` package uses camelCase (e.g. `[SerializeField] private int androidTargetFrameRate;`). Do not assume either is "the" convention — match the surrounding file/package until the project explicitly picks one (tracked as an open question in `.agents/CONTEXT.md`).

This repository does not currently document a canonical Unity executable path or CI command. Discover available tooling before running a build; do not invent build commands.

## Version Control, Branching, and Pull Requests

- `.agents/` and `.archive/` are version-controlled like any other content. Do not add either to `.gitignore`, and include their meaningful changes in the relevant commits.
- Start each task from the current `development` branch. Create a new feature branch per task, named `feature/<short-kebab-case-description>` unless a different prefix is required elsewhere in this file.
- Make small, logical commits — one cohesive, reviewable unit of related changes per commit, including relevant documentation and agent-context updates.
- Open a pull request for every merge. Never merge a branch directly; never squash-merge — preserve the logical commits.
- Merge feature branches into `development` through their pull requests. Use `development` for day-to-day integration. Update `main` only via a pull request from `development`, for an actual release.
- Do not commit unrelated changes, rewrite shared history, force-push, or change Git configuration unless explicitly requested.
- If `development` does not exist, or this branch policy conflicts with repository-enforced rules (branch protection, required status checks, etc.), stop and report the conflict before creating a branch or pull request.

## Version Naming

Use versions in the form `X.Y.Z`:

- `X` (major): incremented for intentional breaking changes or a new major release.
- `Y` (minor): incremented for backward-compatible feature releases.
- `Z` (patch): whole minutes since the Unix epoch (UTC) at assignment time — `floor(unix_timestamp_seconds / 60)`. Never reset, decrement, or reuse `Z`.

Record the full assigned version, its UTC assignment time, and the reason for a major/minor increment in the relevant release notes, changelog, or version-history documentation.

## Documentation and Context Maintenance

Every completed task must include a documentation update: the project README, an architecture/usage guide, a changelog/release note, and `.agents/` files, as applicable to the change. Keep the documentation update in the same logical commit as the change it describes. If no project documentation needs a substantive content change, add a precise task summary plus a documentation-review note to `.agents/LOGS.md` instead of making a cosmetic edit.

After meaningful, authorized work:

1. Add a newest-first entry to `.agents/LOGS.md` with an ISO 8601 date/time (with UTC offset), an agent/session identifier when more than one agent may be active, work completed, files touched, decisions, issues, and next steps. Skip trivial reads, failed tool calls, or repetitive status checks.
2. Update `.agents/CONTEXT.md` when project goals, architecture, structure, or constraints change.
3. Update `.agents/MEMORY.md` only for stable, durable facts and explicit user preferences.
4. Update `.agents/LEARNINGS.md` for useful discoveries, failures, and repeatable gotchas.
5. Keep active files concise (soft budget: `LOGS.md` 300–500 lines, `LEARNINGS.md`/`MEMORY.md` 200–300 lines, `CONTEXT.md` 150–250 lines, or roughly 20 KB per file when line length varies). When a file crosses its threshold, or a new day begins with meaningful prior-day content, summarize the older material into the active file, copy the detail into a new immutable archive file, verify it, link it from the category index, and only then trim the active file.
6. Update `.agents/INDEX.md` and every relevant archive index after an archive operation — update only the indexes whose contents or metadata actually changed.

Archive filenames use `CATEGORY-YYYY-MM-DD-SHORT-SLUG.md` (uppercase, hyphenated, 3–8 words, no secrets). If the intended filename already exists with different content, append `-02`, `-03`, etc. Archive files are never edited after creation. Maintain `.agents/LOGS.md`, `.agents/LEARNINGS.md`, and every `.archive/*/INDEX.md` in reverse-chronological order (newest first), sorted by the archive's own `YYYY-MM-DD` (then full timestamp, then filename, for same-date ties).

Context files and indexes are shared state when more than one agent may be active: re-read a target file immediately before editing it, merge the intended change into its latest content rather than replacing the whole file, and leave a file unchanged and report the conflict if a safe merge is not possible. Create and verify an archive file before trimming or re-pointing the active file and indexes that depend on it.

## Sensitive Data and External Actions

- Never add secrets, credentials, passwords, tokens, API keys, private keys, personally identifiable information, customer content, internal-only URLs/hostnames, sensitive business information, or full account numbers to source, logs, context, commits, or pull requests.
- Replace sensitive values with placeholders such as `<SECRET>`, `<TOKEN>`, `<API_KEY>`, `<PASSWORD>`, `<PRIVATE_KEY>`, `<ACCOUNT_NUMBER>`, `<EMAIL>`, or `<INTERNAL_URL>` if they must be discussed.
- Do not publish packages, change registry configuration, alter GitHub settings, or perform other external side effects unless the task explicitly asks for them.
- Before any irreversible operation, verify the target branch, files, and scope.

## Verification and Completion

Before declaring work complete:

- Run the most relevant available checks and report actual results.
- For Markdown-only changes, validate changed links, formatting, and Git whitespace errors. Confirm every archive-index link resolves and every archive file has exactly one index entry.
- For code changes, run targeted tests first and expand validation when the change crosses package, Unity, or platform boundaries.
- Confirm `git status` and identify any unrelated existing changes rather than claiming a clean tree incorrectly.
- Summarize the change, validation performed, unresolved risks, and any follow-up work.
