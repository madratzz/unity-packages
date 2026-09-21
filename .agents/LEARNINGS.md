# Active Learnings

Last updated: 2026-09-22

## Recent Learnings

- **Unity's `t:<TypeName>` asset search does not index types declared in test assemblies** — verified for both nested and top-level declarations. A test fixture type used with `AssetDatabase.FindAssets("t:...")` must therefore live in a **non-test** assembly; declaring it in the test assembly makes every lookup return 0 regardless of which assets exist. The working pattern: a dedicated Editor-only asmdef under `Tests/Fixtures/` with **no** TestRunner references and **no** `UNITY_INCLUDE_TESTS` constraint — that absence is exactly what keeps it out of the test-assembly category that `t:` skips, while it still never ships in a build. Editor-assembly types *are* indexed (confirmed: `t:SceneTemplateAsset` found 3). Also note `t:` treats a **base-class** name as match-all (`t:ScriptableObject` returned every one of 974 derived assets), `UnityEditor.LightmapParameters` is not a `ScriptableObject` at all, and `UnityEngine.U2D.SpriteAtlas` inherits from `Object`, not `ScriptableObject` — so none of those work as a drop-in fixture.
- **EditMode tests never receive Unity lifecycle callbacks.** `OnEnable` does not fire for a component added to an already-active GameObject, nor when an inactive one is activated — both confirmed by probe. Tests asserting lifecycle behaviour must invoke the private handler by reflection (matching the reflection these tests already use for private fields) or move to PlayMode. `[ExecuteAlways]` is not an option for components whose `OnEnable` has side effects, since it would run them inside the Editor.
- **Per-test failure messages live in `~/Library/Application Support/DefaultCompany/<project>/TestResults.xml`.** The Unity Test Framework writes an NUnit XML report there on every `run_tests`; it is the *only* place with the actual assertion text. `unity command console` retains run summaries only (not per-test messages), and `unity command test_status` reports `no_tests`. Read this file instead of guessing at a failure's cause.
- **`unity pipeline upgrade` can report `alreadyLatest: true` while a newer version is published.** Confirmed on a project at `0.6.0-exp.1` while `0.7.0-exp.1` was `Latest` per both `unity pipeline list-versions` and the registry's `dist-tags`. Confirm with `list-versions`, then install explicitly: `unity pipeline install --package-version <v>`. A running Editor re-resolves on its own — watch `Packages/packages-lock.json` flip, then the Tundra build; no Editor restart is needed.
- **Unity's asset watcher stalls while the Editor is unfocused**, so external file writes (agent edits, scripted changes) can go unnoticed for many minutes — and `unity command recompile` then answers `up_to_date`, which is misleading. Force it by activating the app (`osascript -e 'tell application "Unity" to activate'`) **and** running `AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate)` via `eval_file`. The eval call may return `Main thread operation timed out after 5000ms` while the refresh still completes.
- **`unity command eval_file` harness constraints**: class declarations are rejected, and local functions or `try`/`catch` produce a spurious `Unreachable code detected` at the trailing `return`. Write probes as straight-line statements ending in a single `return ...;`.
- **`[SerializeField]` naming is inconsistent across the repo, not a settled convention.** The ported `scriptableobject.*` family (time.machine, statemachine.core, variables.extensions, variables.database) uses PascalCase for serialized private/protected fields (`TickInterval`, `BootState`, `Key`, `Value`); the newer `scriptableobject.architecture` package (ported from asteroids-demo on `feature/architecture`) uses camelCase (`androidTargetFrameRate`, `applicationTimeMachine`, `appPaused`). Do not assume or assert a single project-wide rule without checking the specific package/file first.
- **PlayMode tests hang when `alwaysstartfromscenezero` is enabled.** The package's `RuntimeInitializeOnLoadMethod(BeforeSceneLoad)` calls `EditorSceneManager.LoadScene(0)` before UTF's play-mode setup completes, destroying the test scene. Disable via `EditorPrefs.SetBool("EditorUtilities/Always Start From Scene 0 &p", false)` before running `unity command run_tests --mode PlayMode`. The pref is per-editor-process and re-arms whenever anyone toggles the menu item.
- Version bookkeeping convention: until a package is first published to Verdaccio, its CHANGELOG keeps a single `## [0.0.1] - Unreleased` entry collecting all pre-release changes; `package.json` stays at `0.0.1`. Do not introduce 0.0.2/0.0.3 headings for unpublished work — they describe releases that don't exist.
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
