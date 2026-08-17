# Active Learnings

Last updated: 2026-08-17

## Recent Learnings

- `unity command eval_file` can return HTTP 500 "main thread timed out" while the evaluated code still runs to completion — treat `console`/`get_console_logs` as the source of truth for eval results, not the eval call's HTTP status.
- Splitting one embedded package into several is safe when done as: move files (excluding `.meta`), author fresh asmdefs/package.json per concern, re-point consumer package.json + asmdef references in the same pass, delete the old package dir, then `package_resolve` + recompile. Unity regenerates the lockfile and `.meta` GUIDs.
- `TryGetComponent` + `AddComponent` is the correct `GetOrAddComponent` pattern; the archived version discarded `AddComponent`'s return and yielded null on the add path.
- Unity package asmdef references must match the *assembly name* in the target asmdef (`com.madratzz.utilities.attributes.runtime`), not the package name — the archived repo mixed `madratzz.*` and `com.madratzz.*` assembly-name conventions intentionally.
- The Unity CLI Pipeline server (port 7800) drops connections during domain reloads; retry `editor_status` after a few seconds instead of treating the editor as lost.
- `unity command eval_file --file <path.cs>` is the reliable way to run C# probes in the live editor — quoting multiline code inline through `--code` breaks in bash.
- Embedded packages dropped into `Packages/` need only `unity command package_resolve` — no `manifest.json` entry. Unity then records them in `packages-lock.json` with `source: embedded` and regenerates `.meta` GUIDs (excluding `.meta` on copy is safe).
- `RuntimeDictionary` in `scriptableobject.variables.extensions` compiles without Odin Inspector because usage is guarded by `#if ODIN_INSPECTOR`; dictionary types are unavailable until Odin is imported.
- The repository baseline now includes the 4 ported `com.madratzz.*` embedded packages; earlier baseline had no tracked C# source files or assembly definitions.
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
