# Active Learnings

Last updated: 2026-08-15

## Recent Learnings

- The repository is at an early Unity-project baseline: no tracked C# source files or assembly-definition files were present when this context system was created.
- The existing project README and IDEA document identify the repository as a collection of Unity game-development packages intended for Verdaccio hosting.
- Generated Unity directories (`Library/`, `Temp/`, `Logs/`, and `UserSettings/`) are present locally and are ignored by Git; they are not appropriate locations for durable agent context.
- A concise root `AGENTS.md` can provide harness-neutral operating rules while `.agents/` retains current, project-specific context and `.archive/` retains detailed history.

## Patterns

- Keep durable repository knowledge in versionable Markdown under `.agents/` and `.archive/`, not in Unity-generated state.
- Prefer compact active summaries with deeper dated records discoverable through category indexes.
- Use `AGENTS.md` as the stable entry point and avoid duplicating all active-context detail into the root guide.

## Mistakes to Avoid

- Do not put package source, runtime assets, or agent operational files into generated Unity folders.
- Do not duplicate all historical material in active files; summarize it and link to archive indexes.
- Do not infer package architecture or registry conventions that have not yet been documented.

## Archive Summary

No archived learning records exist yet.

## Archive Pointers

- [Archived Learnings Index](../.archive/learnings/INDEX.md)
