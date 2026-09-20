# Contributing

← [[Home]]

`AGENTS.md` at the repository root is the **canonical** policy source for all of this — this note is a summary for quick reference, not a replacement. If anything here and `AGENTS.md` disagree, `AGENTS.md` wins.

## Branch and PR workflow

1. Start from `development`.
2. Create a feature branch: `feature/<short-kebab-case-description>`.
3. Make small, logical commits — each one a cohesive reviewable unit, including its documentation/agent-context updates.
4. Open a pull request for every merge. Never merge directly, never squash-merge.
5. `development` is for day-to-day integration; `main` is updated only via a PR from `development`, for an actual release.

## Version naming

`X.Y.Z`:
- `X` — major, for intentional breaking changes.
- `Y` — minor, for backward-compatible feature releases.
- `Z` — whole minutes since the Unix epoch (UTC) at assignment time. Never reset or reused.

## Documentation policy

Every completed task needs a documentation update — README, changelog, or `.agents/` context — in the **same commit** as the change it describes. If nothing substantive needs updating, a documentation-review note in `.agents/LOGS.md` satisfies the rule instead of a cosmetic edit.

## Agent context system

This repo also maintains `.agents/` (active project context: `CONTEXT.md`, `MEMORY.md`, `LEARNINGS.md`, `LOGS.md`) and `.archive/` (dated history) for AI coding agents — separate from this human-facing wiki. Start-of-work and archival rules for that system live in `AGENTS.md`; this `Docs/` vault does not duplicate them.

## Unity/package conventions

- Prefer SOAP wiring (ScriptableObject references) for shared state; VContainer is acceptable for genuine services, not a default requirement. See [[Architecture Overview]].
- Add EditMode tests for deterministic logic; Play Mode tests only where engine/scene integration requires them.
- `[SerializeField]` casing is not yet settled project-wide — match the surrounding file/package rather than assuming a convention (see [[Architecture Overview]]#Open architectural questions).

## Sensitive data

Never commit secrets, API keys, passwords, tokens, or credentials — including in `BuilderConfig` assets or `buildsettings.json` (see [[Utilities - Build Automation]]). Use placeholders like `<SECRET>` if a value must be discussed.

← [[Home]]
