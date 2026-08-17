# Active Memory

Last updated: 2026-08-17

## Stable Project Facts

- Repository name: `unity-packages`.
- License for all `com.madratzz.*` packages is MIT (Copyright (c) 2026 Raza Butt); each package carries `LICENSE.md` and `"license": "MIT"` in `package.json`.
- The repository is a Unity project for personal reusable game-development packages.
- Package distribution is intended to use a Verdaccio server.
- The project uses Unity `6000.3.21f1` (Unity 6.3) and includes the Unity Test Framework and URP.
- `AGENTS.md` is the root repository guide for AI agents and automation harnesses; `.agents/` provides the active project context it directs them to read.
- `IDEA.md` records the high-level reusable-package intent and Verdaccio distribution direction.

## User Preferences

- Prioritize SOAP (ScriptableObject Architecture) for Unity design: ScriptableObject variables for shared state, ScriptableObject events for decoupled communication, small single-purpose systems. DI (VContainer) may be suggested for genuine service dependencies but must never be mandated or used to reject a SOAP design.
- Maintain practical, inspectable, idempotent agent context with active files kept in `.agents/` and historical material kept in `.archive/`.
- Keep archive indexes and dated lists in reverse chronological order.
- Preserve concise active summaries after archival so agents do not need to read every historical file.
- Never record secrets, API keys, passwords, tokens, private keys, credentials, or other sensitive values.
- Provide one concise root guide that works for AI agents and automation harnesses, then place evolving project-specific knowledge in `.agents/`.

## Naming Conventions

- Use ISO format (`YYYY-MM-DD`) for dates in Markdown content and tables.
- Use `DD-MM-YY` in archive filenames, optionally followed by a short lowercase hyphenated descriptive slug.
- Use relative Markdown links between context and archive files.

## Important Entities

- `AGENTS.md` — root AI-agent and automation-harness guide.
- `IDEA.md` — high-level project-intent document.
- `.agents/` — active AI-agent context.
- `.archive/` — dated historical AI-agent material.
- `Packages/manifest.json` — Unity package dependencies.
- `ProjectSettings/ProjectVersion.txt` — authoritative Unity editor-version record.
- Verdaccio — intended package registry host.

## Do Not Forget

- Archive rather than delete useful history.
- Do not overwrite existing archive files without explicit user direction.
- Redact sensitive data with placeholders such as `<API_KEY>`, `<TOKEN>`, or `<SECRET>` if encountered.

## Archive Summary

No archived memory snapshots exist yet.

## Archive Pointers

- [Archived Memory Index](../.archive/memory/INDEX.md)
