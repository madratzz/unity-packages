# Active Logs

Last updated: 2026-08-15

## Current Session

### 2026-08-17 09:45 PST

Summary of what was done:

- Replaced the mis-copied Unity Companion License in all 8 embedded packages with the MIT License (Copyright (c) 2026 Raza Butt) per user decision.
- Added `"license": "MIT"` to each `package.json` (inserted after `version`, preserving field order).

Files touched:

- `Packages/com.madratzz.*/LICENSE.md` (8 files)
- `Packages/com.madratzz.*/package.json` (8 files)

Decisions made:

- License for the `com.madratzz.*` package family is MIT.

Next steps:

- Commit the full baseline (ported packages, utilities split, platform.device, issue-5 fix, licenses, docs/context updates).

### 2026-08-17 09:35 PST

Summary of what was done:

- **Issue 1:** Split `com.madratzz.utilities.extensions` into 4 packages — `com.madratzz.utilities.attributes` (inspector attributes + editor drawers, zero deps), `com.madratzz.utilities.core` (Singleton*, UnitySerializedDictionary, UnityExtensions, DoNotDestroyOnLoad, DateTime/parsing helpers, zero deps), `com.madratzz.utilities.coroutines` (CoroutineHandler/TimeCounter/sequences; deps: core), `com.madratzz.utilities.ui` (Image/ScrollRect/RectTransform/TMP ext, RectTransformUtilities; deps: com.unity.ugui). Old package deleted.
- **Issue 2:** Fixed `GetOrAddComponent<T>` null-return bug — now `TryGetComponent` + return the added component; constraint widened `MonoBehaviour` → `Component`.
- **Issue 3:** Created `com.madratzz.platform.device` — `DeviceIdentity.GetInstallId()` returns a stable self-generated GUID persisted per platform: iOS Keychain via native plugin `Runtime/Plugins/iOS/KeychainStorage.mm` (SecItem API, `kSecAttrAccessibleAfterFirstUnlockThisDeviceOnly`), PlayerPrefs on Android/editor/other. Replaces broken `GetDeviceId` (deleted with `ParseImageName`/`ParseDialogues`).
- **Issue 4:** No code change needed — `Singleton`/`SingletonPersistent` kept in core with README "legacy interop" guidance directing new code to SOAP/DI wiring.
- **Issue 5:** Deleted dead `DBManager.GetJsonData()` stub (returned null). Static facade shape retained per user direction; per-write `PlayerPrefs.Save()` flush flagged as future work.
- Re-pointed `scriptableobject.variables` + `.database` package.json and asmdefs from `utilities.extensions` → `utilities.attributes`; `variables.extensions` asmdef no longer references utilities.
- Verified via Unity CLI: `package_resolve` completed, recompile `completed` with no errors, 0 console errors/warnings. Type probes 15/15 OK across all new assemblies. Functional probes: `GetOrAddComponent` add-path returns non-null, get-path returns same instance; `GetInstallId()` stable + persisted; `DBManager` int round-trip OK after stub removal.

Files touched:

- `Packages/com.madratzz.utilities.attributes/` (new), `Packages/com.madratzz.utilities.core/` (new), `Packages/com.madratzz.utilities.coroutines/` (new), `Packages/com.madratzz.utilities.ui/` (new), `Packages/com.madratzz.platform.device/` (new)
- `Packages/com.madratzz.utilities.extensions/` (deleted)
- `Packages/com.madratzz.scriptableobject.variables/package.json` + `Runtime/madratzz.scriptableobject.variables.runtime.asmdef`
- `Packages/com.madratzz.scriptableobject.variables.database/package.json` + asmdef + `Runtime/DBVariables/DBManager.cs`
- `Packages/com.madratzz.scriptableobject.variables.extensions/Runtime/...asmdef`
- `Packages/packages-lock.json` (Unity-regenerated)

Decisions made:

- Package split by concern with dependency hygiene as the driver: only `utilities.ui` carries uGUI/TextMeshPro; `variables` family now depends only on `utilities.attributes`.
- `SwitchToRectTransform` moved from the deleted `Utilities` grab-bag into `utilities.ui` as `RectTransformUtilities` (it is RectTransform-specific).
- `GetDeviceId`/`ParseImageName`/`ParseDialogues` deleted rather than ported (broken / game-specific).
- Kept existing namespaces unchanged to avoid breaking consumers; the mixed-namespace cleanup is deferred.
- Issue 5 scope was minimal per user choice — no DBManager refactor, no ISaveService yet.

Issues found:

- The ported `LICENSE.md` (attributes package) is a mis-copied Unity Companion License referencing `com.unity.collections` — needs a real license decision; copied to new packages for now.
- `unity command eval_file` returned an HTTP 500 "main thread timed out" but the code executed fully — Pipeline's synchronous request wrapper times out while the eval runs; console logs are the source of truth.
- Pipeline connection drops briefly during domain reload (known, self-recovers).

Next steps:

- Replace the mis-copied LICENSE.md files with the intended license (MIT?).
- Commit the split + new packages.
- Add EditMode tests establishing repo test conventions (variables contracts, Utilities parsing, DeviceIdentity persistence).
- Future: batch `Database` PlayerPrefs.Save() on pause/quit instead of per-write; consider ISaveService when a real save system is built.

### 2026-08-15 20:20 PST

Summary of what was done:

- Aligned project and profile guidance with the user's current architecture priority: SOAP (ScriptableObject Architecture) first, DI optional.
- Updated `AGENTS.md`: "Preserve boundaries" now names ScriptableObject references or constructor injection as the two explicit wiring styles; the "Unity and Package Work" DI bullet now states SOAP is preferred for shared state/cross-system communication (following the `com.madratzz.scriptableobject.*` conventions) and DI (VContainer) is acceptable but never mandatory.
- Updated the Prometheus profile `SOUL.md` (`~/.hermes/profiles/prometheus/SOUL.md`): architecture section is SOAP-first with DI as a suggestion, anti-patterns softened for legacy singletons and serialized SO wiring, SOAP play-session state-reset gotcha added, Unity version default corrected to `6000.3.21f1`.
- Saved the SOAP-first preference to Hermes durable memory.

Files touched:

- `AGENTS.md`
- `~/.hermes/profiles/prometheus/SOUL.md` (profile file, outside repo)
- `.agents/logs.md`

Decisions made:

- The project prioritizes SOAP (ScriptableObject variables/events/systems) as its architecture; DI is an option for genuine service dependencies, never a requirement.
- The archived package family (`scriptableobject.*`) is the convention reference for new SOAP code.

Issues found:

- `AGENTS.md` previously mandated DI for cross-system behavior, contradicting the user's SOAP direction — resolved.
- `SOUL.md` Unity version default was stale (`6000.3.16f1` vs actual `6000.3.21f1`) — corrected.

Next steps:

- Decide on the `utilities.extensions` 4-way split proposal (attributes / core / coroutines / ui).
- Port remaining archived packages (eventsystem.core etc.) as SOAP work continues.

### 2026-08-15 20:00 PST

Summary of what was done:

- Ported 4 packages from `unity-packages-archived-2` into `Packages/` as embedded packages (`.meta` files excluded; Unity regenerated GUIDs): `com.madratzz.utilities.extensions`, `com.madratzz.scriptableobject.variables`, `com.madratzz.scriptableobject.variables.database`, `com.madratzz.scriptableobject.variables.extensions`.
- User chose faithful port (including `utilities.extensions` dependency for `CustomUtilities.Attributes.InlineEditor`).
- Verified via Unity CLI Pipeline: `package_resolve` completed, recompile completed with no errors, zero console errors/warnings, all 4 packages listed as `Embedded` at `0.0.1`, and 5 type probes (`Int`, `Float`, `DBManager`, `ArrayInt`, `InlineEditorAttribute`) resolved `OK` in the live editor.

Files touched:

- `Packages/com.madratzz.utilities.extensions/` (new)
- `Packages/com.madratzz.scriptableobject.variables/` (new)
- `Packages/com.madratzz.scriptableobject.variables.database/` (new)
- `Packages/com.madratzz.scriptableobject.variables.extensions/` (new)
- `Packages/packages-lock.json` (Unity-added embedded entries)
- `ProjectSettings/ProjectSettings.asset` (editor-side changes: `runInBackground: 1`, `SENTIS_ANALYTICS_ENABLED` define — pre-existing, not from this task)

Decisions made:

- Packages live as embedded packages under `Packages/` (Unity auto-discovers; no `manifest.json` entries), matching the archived repo's layout.
- Kept namespaces (`ProjectCore.Variables`, `CustomUtilities.*`) and assembly names (`madratzz.scriptableobject.*`, `com.madratzz.utilities.*`) unchanged.

Issues found:

- `scriptableobject.variables` and `.database` have a hard dependency on `com.madratzz.utilities.extensions` (InlineEditor attribute + asmdef reference) — resolved by porting it too.
- `RuntimeDictionary` types in `.extensions` require Odin Inspector (`ODIN_INSPECTOR` define); Odin is not installed, and the code guards with `#if ODIN_INSPECTOR` so compilation succeeds without it.
- `DBManager.GetJsonData()` is dead code (`return null` with commented-out `ToJson()` call) — known archived behavior, left as-is.
- Pipeline connection dropped once during the domain reload after `package_resolve`; editor recovered to `ready` on its own.

Next steps:

- Commit the new packages if the port is accepted.
- Add EditMode tests for variable get/set/reset contracts and DB round-trips to establish test conventions.
- Decide Verdaccio naming/versioning/publishing conventions (open question from context).

### 2026-08-15 19:54 PST

Summary of what was done:

- Added root `AGENTS.md` as the repository entry point for AI agents and automation harnesses.
- Formalized the previously untracked `IDEA.md` as the high-level project-intent document.
- Integrated the root guide into active context, memory, learnings, and default-agent startup rules.

Files touched:

- `AGENTS.md`
- `IDEA.md`
- `.agents/README.md`
- `.agents/INDEX.md`
- `.agents/context.md`
- `.agents/memory.md`
- `.agents/learnings.md`
- `.agents/logs.md`
- `.agents/agents/default-agent.md`

Decisions made:

- `AGENTS.md` is the concise, repository-wide guide; `.agents/` remains the source for active project-specific context, and `.archive/` remains the source for deeper history.
- The guide avoids undocumented Unity build or package-layout assumptions and requires agents to discover available tooling before using it.

Issues found:

- No root `AGENTS.md` existed before this work.

Next steps:

- Use the root startup sequence before future package implementation work.
- Define the first package's public API, assembly, testing, versioning, and Verdaccio publishing conventions.

## Recent Previous Sessions

### 2026-08-15 19:44 PST

Summary of what was done:

- Inspected the repository baseline, Unity version, package manifest, Git status, and existing project documentation.
- Created the initial active `.agents/` context system and `.archive/` category/index structure.
- Recorded only non-sensitive repository facts and the context-system operating rules.
- Verified that all 14 required Markdown files exist, their relative links resolve, active-file sizes are within limits, no trailing whitespace is present, and no credential-like values were written.

Files touched:

- `.agents/README.md`
- `.agents/INDEX.md`
- `.agents/context.md`
- `.agents/memory.md`
- `.agents/learnings.md`
- `.agents/logs.md`
- `.agents/agents/default-agent.md`
- `.archive/README.md`
- `.archive/INDEX.md`
- `.archive/logs/INDEX.md`
- `.archive/memory/INDEX.md`
- `.archive/learnings/INDEX.md`
- `.archive/context/INDEX.md`
- `.archive/agents/INDEX.md`

Decisions made:

- The initial setup creates no dated archive snapshot because there was no earlier active material to preserve.
- Future archive indexes will be maintained in recent-to-oldest order and archive filenames will use the requested DD-MM-YY convention.

Issues found:

- No prior `.agents/` or `.archive/` structure existed.
- `IDEA.md` was already untracked before this setup and was not modified.

Next steps:

- Decide the first package scope and package registry conventions.
- Commit the agent-context system if it should be shared with other repository users.

## Archive Summary

No archived logs exist yet; this is the initial setup session.

## Archive Pointers

- [Archived Logs Index](../.archive/logs/INDEX.md)
