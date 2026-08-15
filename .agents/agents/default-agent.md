# Default Agent

Last updated: 2026-08-15

## Agent Role

This agent is responsible for maintaining project context, memory, logs, learnings, and archives.

## Operating Rules

- Read the root [`AGENTS.md`](../../AGENTS.md) first.
- Read `.agents/context.md` second.
- Read `.agents/memory.md` third.
- Read `.agents/learnings.md` fourth.
- Read `.agents/logs.md` fifth.
- Update active files after meaningful work.
- Archive older files when the date changes or active files become too large.
- Never delete historical context without archiving it first.
- Keep summaries concise but useful.
- Use relative links.
- Do not store secrets, API keys, passwords, tokens, private keys, or credentials.

## Project Context

This repository is an early-stage Unity 6.3 project for reusable personal Unity game-development packages intended for Verdaccio distribution. The root [`AGENTS.md`](../../AGENTS.md) establishes repository-wide AI-agent and automation-harness workflow. Active project context lives in `.agents/`; dated historical detail lives in `.archive/`. The project currently has no tracked C# scripts or assembly definitions, so package boundaries and publishing conventions remain open design work.

## Responsibilities

- Maintain context.
- Maintain logs.
- Maintain memory.
- Maintain learnings.
- Maintain archives.
- Keep archive indexes updated.

## Workflow

1. Start by reading the root `AGENTS.md` and active context files.
2. Perform the requested task.
3. Update `.agents/logs.md`.
4. Update `.agents/memory.md` if stable facts were discovered.
5. Update `.agents/learnings.md` if new lessons were learned.
6. Update `.agents/context.md` if project direction, structure, or goals changed.
7. Archive old material when needed.
8. Update all relevant indexes.

## Archive Thresholds

- Archive meaningful prior-day log detail when a new day starts.
- Archive older sections after summarizing them when `logs.md` exceeds 300–500 lines, `learnings.md` or `memory.md` exceeds 200–300 lines, or `context.md` exceeds 150–250 lines.
- Use `category-DD-MM-YY-short-jist.md` filenames, preserve active summaries and archive pointers, and add the newest archive-index entry first.
