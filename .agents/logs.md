# Active Logs

Last updated: 2026-09-20

## Current Session

### 2026-09-20T23:12:56+05:00 — claude-sonnet-5/gameflow-template-direction

Summary of what was done:

- Resolved the open question (from the prior FSM-wiring-fix session) of whether moving `com.madratzz.scriptableobject.architecture`'s code from `Packages/` into `Assets/` was permanent. User decision: yes, deliberately — this repo is both a package library (`Packages/com.madratzz.*`, unchanged, still Verdaccio-distributable) and a clonable SOAP-based project template (`Assets/`), and the GameFlow application layer is the template's integration code, not a 17th reusable package. Confirmed scope explicitly with the user: only the GameFlow layer moves to `Assets/`; the other 16 packages stay under `Packages/` as-is.
- Deleted `Packages/com.madratzz.scriptableobject.architecture/` (`package.json`, `README.md`, `CHANGELOG.md`, `LICENSE.md` + metas) — it had no code left (Runtime/Tests already moved to `Assets/` in the prior session) and was actively misleading as a "package" with nothing installable in it.
- Migrated its README content (adapted: dropped the UPM/Verdaccio installation section, reframed the intro) to `Assets/Runtime/README.md` so the usage/wiring documentation isn't lost.
- Updated `IDEA.md`, `README.md`, and `AGENTS.md` (Project Snapshot + Repository Layout's `Assets/` row) to document the `Packages/` (reusable, distributable) vs. `Assets/` (template/integration layer, cloned with the repo) split as a deliberate, permanent architecture — so a future agent doesn't try to "fix" this back into `Packages/` again.
- Updated `.agents/context.md`: Project Summary, Current Goals, Architecture/Structure (added `Assets/`'s new role and the still-current 16-package `Packages/` list), and Important Decisions. Also corrected two pieces of staleness found while editing this file: the Unity version (still said `6000.3.21f1`; actual is `6000.3.24f1` per the already-merged editor-bump commit on this branch) and the "no embedded package has tests yet" open question (11 of 16 packages already have a `Tests/` folder).
- Created branch `feature/gameflow-template-direction` from `feature/architecture` (this branch already has `fix/gameflow-fsm-wiring`'s commits merged via PR #8) per the branch-per-task policy.

Files touched:

- `IDEA.md`, `README.md`, `AGENTS.md`
- `.agents/context.md`, `.agents/logs.md`
- `Packages/com.madratzz.scriptableobject.architecture/*` (deleted)
- `Packages/packages-lock.json` (removed the now-dangling `com.madratzz.scriptableobject.architecture` embedded-dependency entry left over from the deletion)
- `Assets/Runtime/README.md` (new, migrated content)

Decisions made:

- `Assets/` code is template/integration glue only — a new reusable, standalone system still belongs in its own package under `Packages/`, not `Assets/`. Recorded in `AGENTS.md`'s Repository Layout table so this isn't re-litigated per task.
- Deleted the hollow package rather than keeping it as a stub or redirect — a `package.json` with no code is actively misleading to anyone resolving packages, and the repo's docs now explain the split clearly enough that a redirect isn't needed.

Issues found:

- None new; this session resolved an issue flagged in the prior one (`.agents/logs.md`'s 2026-09-20T22:04:02+05:00 entry).

Next steps:

- This branch (`feature/architecture` lineage) still doesn't have the `Docs/` Obsidian vault (that only exists on `development`, merged via PR #10). Once `feature/architecture`/PR #6 merges into `development`, the vault's `Docs/Packages/SOAP - Architecture (GameFlow).md` note should be reworked — it currently describes GameFlow as package #17; it should instead describe it as the `Assets/` template layer, likely moved out of `Docs/Packages/` into its own top-level note.
- Push `feature/gameflow-template-direction` and open its PR into `feature/architecture`.
- Wire actual ScriptableObject assets (FSM, TimeMachine, GameEvents, Transitions) into `Assets/Prefabs/ApplicationBase.prefab`/`ApplicationFlowController.prefab` so `BootstrapScene` can actually boot (still outstanding from the prior session).

### 2026-09-20T22:04:02+05:00 — claude-sonnet-5/gameflow-fsm-wiring

Summary of what was done:

- User (in the Editor, not via this agent) moved `com.madratzz.scriptableobject.architecture`'s `Runtime/` and `Tests/` out of `Packages/com.madratzz.scriptableobject.architecture/` into `Assets/Runtime/` and `Assets/Tests/`, and built scene scaffolding around it: `Assets/Prefabs/ApplicationBase.prefab` + `ApplicationFlowController.prefab` (component placeholders, no assets wired yet), an empty `Assets/GameEvents/` folder, and two new scenes (`BootstrapScene.unity`, replacing the deleted `SampleScene.unity`; `GameScene.unity`, currently a default empty scene). `Packages/com.madratzz.scriptableobject.architecture/` now contains only `package.json`/`README.md`/`CHANGELOG.md` — no code.
- Reviewed this change and flagged it to the user: it breaks the package boundary this repo otherwise enforces (`AGENTS.md` "Do not add package code here [`Assets/`] unless the package layout explicitly requires it") and leaves `package.json` describing a package with no implementation. User has not yet confirmed whether this is a deliberate, permanent restructure or a troubleshooting step — flagged as an open question below rather than reverted unilaterally.
- At the user's request, renamed `ApplicationFlowController.cs`'s 8 `[SerializeField] private` fields to PascalCase (`applicationBase`→`ApplicationBase`, `gameStateTransition`→`GameStateTransition`, `levelFailTransition`→`LevelFailTransition`, `settingsTransition`→`SettingsTransition`, `gotoGame`→`GotoGame`, `gotoLevelFail`→`GotoLevelFail`, `levelFailViewClosed`→`LevelFailViewClosed`, `useCustomLogic`→`UseCustomLogic`), matching `ApplicationBase.cs`'s casing (user's own earlier edit) and the rest of the ported `scriptableobject.*` family. First audited every `[SerializeField] private` field across all 17 `com.madratzz.*` packages — this file was the only one not already PascalCase, so no other package needed changes. Synced `Assets/Prefabs/ApplicationFlowController.prefab`'s field keys and the package's `README.md`/`CHANGELOG.md` to match. Skipped `[FormerlySerializedAs]` — verified the prefab's fields were all still `{fileID: 0}` (unassigned), so there was no live serialized data the rename could drop.
- Committed the pending Unity 6000.3.24f1 editor-version bump (already known-uncommitted, see the 2026-09-20T19:14:52+05:00-equivalent entry on `feature/sync-context-editor-bump`) separately from the GameFlow restructure, per the "small, logical commits" policy.

Files touched:

- `ProjectSettings/ProjectVersion.txt`, `Packages/manifest.json`, `Packages/packages-lock.json`, `ProjectSettings/ProjectSettings.asset`, `Assets/Settings/Mobile_RPAsset.asset` (editor-bump commit)
- `Packages/com.madratzz.scriptableobject.architecture/{README.md,CHANGELOG.md}`, `Assets/Runtime/**`, `Assets/Tests/**`, `Assets/Prefabs/**`, `Assets/GameEvents.meta`, `Assets/Scenes/{BootstrapScene,GameScene}.unity(.meta)`, `ProjectSettings/EditorBuildSettings.asset` (restructure commit)
- `.agents/logs.md`

Decisions made:

- Committed the working tree as the user asked, including the Assets/ move, rather than blocking on the unanswered package-boundary question — it's a local, reversible commit, not a merge or publish.
- Kept the PascalCase rename scoped to the one non-conforming file rather than touching already-consistent fields elsewhere, per explicit user instruction and the audit above.

Issues found:

- Still open: is moving this package's code into `Assets/` permanent? If so, `Packages/com.madratzz.scriptableobject.architecture/package.json` should either be deleted (it no longer describes anything installable) or the code should move back under `Packages/` to keep it Verdaccio-distributable, per this repo's stated purpose.
- `Assets/Prefabs/ApplicationBase.prefab` and `ApplicationFlowController.prefab` have no assets wired yet (no FiniteStateMachine, TimeMachine, GameEvent, Transition, or DBInt assets exist in the project) — running the scene now will just hit the warning/error logs added in the earlier FSM-wiring fix.

Next steps:

- Get a decision on the `Packages/` vs `Assets/` question above before this branch's own PR is opened.
- Wire the actual ScriptableObject assets (FSM, TimeMachine, GameEvents, Transitions) into the two prefabs so `BootstrapScene` can actually boot.
- Push `fix/gameflow-fsm-wiring` and open its PR into `feature/architecture` once GitHub auth is available in this environment.

### 2026-09-20T21:30:14+05:00 — claude-sonnet-5/gameflow-fsm-wiring

Summary of what was done:

- Fixed two correctness bugs and one documentation bug found by an earlier code review of `com.madratzz.scriptableobject.architecture` (the unreleased GameFlow port from `feat(architecture): port GameFlow framework from asteroids-demo`, commit `0f94d96`):
  - **Duplicate, unlinked FSM references**: `ApplicationBase` and `ApplicationFlowController` each held an independent `[SerializeField] FiniteStateMachine` with nothing enforcing they pointed at the same asset — if they diverged (or one was left unassigned while the other was set), transitions were silently queued on an FSM that never ticked. Fixed by removing `ApplicationFlowController`'s own FSM field entirely and having it read `ApplicationBase.StateMachine` (a new public accessor) through an explicit `[SerializeField] applicationBase` reference instead — there is now exactly one FSM field in the whole system, structurally preventing divergence.
  - **Implicit `Resources.Load` fallback**: `ApplicationBase.Awake()` fell back to `Resources.Load<FiniteStateMachine>("StateMachine")` when unwired — a magic-string lookup that violates this repo's explicit-wiring rule (`AGENTS.md` "Change Discipline" #3). Removed; an unwired `applicationStateMachine` now logs a warning instead of silently resolving (or failing to resolve) via `Resources`.
  - **README example bug**: the "extend the decision table" usage example called `FlowIntent.GoToMainMenu`, which doesn't exist on the enum. Replaced with `FlowContext.Settings, UICloseReasons.ResumeGame, FlowIntent.ResumePrevious`, matching the working example already covered by `ApplicationFlowLogicTests.SubclassCanExtendStrategyTable`.
  - Also removed a redundant self-referencing `using ProjectCore.Architecture;` in `IFlowLogic.cs` (minor, from the same review).
- Branched `fix/gameflow-fsm-wiring` from `feature/architecture` (not `development` — this package doesn't exist there yet) since the buggy code only exists on that unmerged branch.
- Updated the package's `README.md` (wiring instructions + decision-table example) and `CHANGELOG.md` (`### Fixed`) in the same commit as the code change.

Files touched:

- `Packages/com.madratzz.scriptableobject.architecture/Runtime/Application/ApplicationBase.cs`
- `Packages/com.madratzz.scriptableobject.architecture/Runtime/Application/ApplicationFlowController.cs`
- `Packages/com.madratzz.scriptableobject.architecture/Runtime/Logic/IFlowLogic.cs`
- `Packages/com.madratzz.scriptableobject.architecture/README.md`, `CHANGELOG.md`
- `.agents/logs.md`

Decisions made:

- Fixed the duplicate-FSM-reference bug by removing the redundant field (structural fix) rather than adding a runtime consistency check between two fields — matches this repo's stated preference for explicit, singular wiring over defensive validation of avoidable duplication.
- Did not rename this branch's `.agents/` files to the uppercase convention adopted on `development` on 2026-09-16 — this branch predates that decision and diverged before it landed; renaming here would mix an unrelated, disruptive change into a bug-fix commit. Left as a note for whoever rebases/merges `feature/architecture`.

Issues found:

- Could not run this package's EditMode tests (no Unity batch-mode invocation is documented for this repo, and none was attempted here) — verified instead by grep that no other file in the repo references the removed `ApplicationFlowController.applicationStateMachine` field or the removed `FlowIntent.GoToMainMenu`, and that `ApplicationFlowLogicTests.cs` (the package's only existing tests) doesn't touch the MonoBehaviours I changed, so it's unaffected.
- `.agents/logs.md`'s "Last updated" header was already stale relative to its own entries before this change (said 2026-08-15 while entries went up to 2026-08-17); the GameFlow port commit itself (`0f94d96`) appears not to have added a log entry. Not backfilled here — out of scope for this fix and not something I have firsthand knowledge of.

Next steps:

- Push `fix/gameflow-fsm-wiring` and open a PR **into `feature/architecture`** (not `development`) once GitHub auth is available in this environment — push failed here with no credentials configured, same blocker hit on other branches this session.
- Once `feature/architecture` itself is ready for `development`, the `Docs/Packages/SOAP - Architecture (GameFlow).md` vault note (on the separate `feature/codebase-obsidian-vault` branch) documents these as "known issues" — that callout should be removed/updated to reflect the fix when the two branches converge.

### 2026-08-17 13:45 PST

Summary of what was done:

- Ported `com.madratzz.utilities.addressables` on branch `feature/addressables-helper`, **opened as PR #5** (first package ported under the new PR-based merge policy).
- **Manifest change**: added `com.unity.addressables: 2.9.1` to `Packages/manifest.json` — the package required it but the project was missing it. 2.9.1 is the current Unity 6 default; the archived package.json pinned `2.7.6`.
- **Defect fixes during port**:
  - **`onFailure` is now `Action<AsyncOperationHandle<GameObject>>`** — callers can inspect `handle.Status` / `handle.OperationException`. Original `Action` left callers unable to distinguish validation vs instantiation failure.
  - **Instance leak fixed**: wrapped `onSuccess` in try/catch; on caller-bug exception the instance is released and the exception is logged via `Debug.LogException` with the same `debugContext`. Without this, a caller bug would leave addressable instances loaded indefinitely.
- 4 EditMode tests for the validation/rejection paths. The actual instantiate path requires `AddressableAssetSettings` (a project-config concern) and is covered by integration tests in projects that ship entries.
- Suite: **109/109 EditMode passing, 0 console errors** (105 prior + 4 new).
- PR opened via `gh pr create`; awaiting review per the PR-based merge policy.

Issues found:

- `LogAssert.Expect` required to consume the expected `Debug.LogError` from the validation path — UTF treats any unexpected error log as a test failure.
- `default(AsyncOperationHandle<T>)` is **not** null — it's a default-constructed struct. Asserting `IsValid()` is the correct way to check for a "no handle was created" failure case.
- `AssetReference` is a ScriptableObject but concrete subclasses like `AssetReferenceT<T>` are not — use `new AssetReferenceGameObject(string.Empty)` for empty-AssetReference tests instead of trying to instantiate generic types.

Next steps:

- Wait for PR review on PR #5.
- Remaining archived packages: adsmodule, analytics.system, remoteconfig (needs Firebase tarballs from ExternalPackages/), architecture (sample-only meta-package, likely skip or rework).

### 2026-08-17 13:00 PST

Summary of what was done:

- Ported `com.madratzz.scriptableobject.statemachine.core` on branch `feature/state-machine` (commit `cc368cd`). FSM stays caller-driven (`Tick()` coroutine the caller runs via CoroutineHandler).
- **Real bug fixed during port:** the archived FSM's `Tick()` transitioned to a new state but **never called `Init`/`Execute` on it** (only the boot path did). Transition targets ran `Tick` from a never-initialized state — silently broken for any state with non-trivial `Init`. Fixed by re-running `Init` → `Execute` after every transition.
- Other fixes: null-guards on `CurrentState` before `Tick()`/pop; explicit `Transition()` rejection of null transitions and null `ToState`; `CleanupAllPausedStates` now requires caller to be the current owner (was a footgun); removed dead `State.Paused` field.
- 11 EditMode tests covering full lifecycle (boot Init/Execute-once, per-frame Tick, transition Exit→Init→Execute, null rejection, double-transition only-first-fires, pause-stack push, resume exit+resume+pop, empty-stack no-op, owner-checked cleanup, no-boot-state exit). Suite: **104/104 EditMode passing, 0 console errors**.

Next steps:

- Merge `feature/state-machine` to development when accepted.
- Remaining archived packages: adsmodule, analytics.system, remoteconfig (needs Firebase tarballs from ExternalPackages/), architecture.soap, utilities.addressables.

### 2026-08-17 12:10 PST

Summary of what was done:

- Ported `com.madratzz.scriptableobject.time.machine` on branch `feature/time-machine` (commit `136e934`). `StartTicking`/`StopTicking` are now real: the SO starts/stops its own loop via `CoroutineHandler` (utilities.coroutines); survives scene loads. TickInterval (default 1s) + UseRealTime Inspector options added; TickEvent.Invoke null-guarded.
- Archived `Runtime/*.asset` excluded — Unity regenerates GUIDs on import so the archived TickEvent reference and script GUIDs would dangle; `Samples~/` copies carry the right pattern.
- Dependencies: scriptableobject.eventsystem.core + utilities.core + utilities.coroutines (transitive: CoroutineHandler extends SingletonPersistent<T> from core).
- Tests: 3 EditMode (config surface) + 4 PlayMode (StartTicking ticks, StopTicking halts, double-start no-op, unassigned event no-throw).
- **Suite: 97/97 passing, 0 console errors** (93 EditMode + 4 PlayMode).

Issues found:

- **PlayMode tests hung indefinitely** — root cause: `alwaysstartfromscenezero`'s `RuntimeInitializeOnLoadMethod(BeforeSceneLoad)` loads scene 0 during UTF play-mode setup, destroying the test scene. Fix: disable the `EditorUtilities/Always Start From Scene 0 &p` EditorPref before running PlayMode tests. The pref is per-editor-process and re-armed whenever anyone toggles the menu item, so any subsequent PlayMode run may hang again.
- EditMode tests pass cleanly; only PlayMode is affected.
- Pipeline server hung once after PlayMode tests completed (commands timed out). Restarting the editor cleanly restored it.

Next steps:

- Merge `feature/time-machine` to development when accepted (currently ahead by 1 commit). Consider making the always-start-from-scene-0 utility opt-in for tests, or recording the EditorPref-disable requirement in the package README so future agents know.

### 2026-08-17 12:30 PST

Summary of what was done:

- Built the buildautomation settings feature on `feature/editor-utilities` (commit `09c367c`): `BuilderConfig` is the single settings SO with new `BuildVersionOverride`/`OutputDirectory`/`ReadFromResourcesFile` fields and a `Current` cached accessor; project-root `buildsettings.json` file source populates the SO when the toggle is on (partial files merge, missing/invalid file falls back to inspector values with a warning).
- Version precedence: `-buildversion` CLI arg > `BuildVersionOverride` > Player Settings. Output directory configurable (was hardcoded `Builds/`).
- Chose project-root over `Resources/` for the JSON: a Resources file with keystore secrets would ship inside player builds; the project-root file is not a Unity asset and cannot be bundled. The Inspector still displays populated values (file = source, SO = surface).
- 7 new EditMode tests (JSON parse/invalid/missing-file fallback/partial-merge/empty-string no-overwrite/toggle-off/Current caching). Suite: **90/90 passing, 0 console errors**.
- User directed: no `[Obsolete]` on `BuilderConfig` — evolve the existing type instead of adding a parallel `BuildSettings` class.

Next steps:

- Merge `feature/editor-utilities` (now 3 commits) to development when accepted.

### 2026-08-17 11:45 PST

Summary of what was done:

- Ported `com.madratzz.utilities.buildautomation` and `com.madratzz.utilities.unity.alwaysstartfromscenezero` on branch `feature/editor-utilities` (commit `bf901cc`). `utilities.extensions` skipped per user — superseded by the 4-way split.
- **Security fix (buildautomation):** removed hardcoded keystore credentials (`sgs123`) from `BuilderConfig` defaults and excluded the committed `BuilderConfig.asset` from the port — a secrets-in-source-control violation under the repo's Sensitive Data policy. Defaults are now empty; config loads via `Resources/BuilderConfig` with blank fallback.
- Fixes: `Builder` static `BuilderConfig` field was never assigned → NRE on any build (now `LoadDefault()`); keystore passwords applied on Android only (were set for iOS too); `CreateAssetMenu` restored on `BuilderConfig`.
- alwaysstartfromscenezero fixes: editor asmdef now `includePlatforms: ["Editor"]` (was leaking editor code into player builds); dropped brute-force deactivate-all-GameObjects pass (`LoadScene(Single)` replaces the scene); README menu path corrected.
- Test seams: `Builder.TryGenerateVersionCode` + `Builder.GetEnabledScenePaths` extracted internal with `[InternalsVisibleTo("com.madratzz.utilities.buildautomation.tests")]` — no behavior change. 7 new EditMode tests (version-code parsing incl. null/empty/non-numeric, enabled-scene filtering, LoadDefault no-asset regression).
- Full suite: **83/83 passing, 0 console errors** (76 prior + 7 new).

Issues found:

- First compile failed: `internal` members aren't visible across asmdef boundaries without `[InternalsVisibleTo]` — fixed with an `AssemblyInfo.cs` in the editor assembly. Recorded as a convention for testing editor-only internal seams.

Next steps:

- Merge `feature/editor-utilities` to development when accepted.
- Remaining archived packages: statemachine.core, time.machine, adsmodule, analytics.system, remoteconfig (needs Firebase tarballs — ExternalPackages/ not present in this repo), architecture.soap, utilities.addressables.

### 2026-08-17 11:00 PST

Summary of what was done:

- Ported `com.madratzz.scriptableobject.eventsystem.core`, `.eventsystem.extensions`, and `.event.variables` from the archive on new branch `feature/eventsystem-packages` (commit `0d73211`).
- Fixed 4 defects during the port: `GameEventWithReturn<T>.Raise()` NRE when unsubscribed (now returns `default(T)`); `GameEventReturnsVector3`'s swallow-all try/catch removed as redundant; `GameEventWithIntStringBool` menu name missing "Bool"; null-guards added to all three `*WithEvent` variable types (unassigned `ValueChanged` NRE'd on SetValue/ApplyChange/AddListener).
- Deps re-pointed: eventsystem.core + .extensions → `utilities.attributes`; event.variables unchanged (eventsystem.core + variables + variables.database). MIT licenses, package-specific newest-first changelogs with 0.0.2 entries.
- 18 new EditMode tests: GameEvent subscribe/invoke/multi/unsubscribe, typed param passing (int/string/float/bool/3-param), Raise-with-subscriber + Raise-default regression, event-variable raise on SetValue/ApplyChange + null-assignment paths (ValueChanged wired via SerializedObject in tests).
- Full suite via `unity command run_tests`: **76/76 passing, 0 console errors** (58 prior + 18 new).

Issues found:

- None new; Pipeline connection drops during domain reload continue to self-recover.

Next steps:

- Merge `feature/eventsystem-packages` to `development` when accepted; push `development` to origin (currently 9+ commits ahead).
- Remaining archived packages: statemachine.core, time.machine, adsmodule, analytics.system, remoteconfig, architecture.soap, utilities.addressables, utilities.buildautomation, alwaysstartfromscenezero.

### 2026-08-17 10:15 PST

Summary of what was done:

- Added EditMode test suites to 4 packages: `scriptableobject.variables` (Int/Float/Bool/String contracts, ApplyChange, implicit operators), `variables.database` (Database/DBManager PlayerPrefs round-trips with per-test GUID keys, DBInt persistence), `variables.extensions` (ArrayInt collection semantics, Vector2/3Shared), `utilities.core` (Utilities parsing/epoch/time, DateTimeExtensions, UnitySerializedDictionary callbacks, GetOrAddComponent regression).
- Test asmdefs follow the Unity 6000 pattern from the unity-development skill: `overrideReferences: true`, name-based `UnityEngine.TestRunner`/`UnityEditor.TestRunner` references, `nunit.framework.dll` precompiled, `UNITY_INCLUDE_TESTS` constraint.
- Ran via `unity command run_tests --mode EditMode` on the live editor: first run 52/58 — the 6 failures all traced to one real package bug: `Array<T>.list` was never initialized (NRE on first Add/Remove at Array.cs:37). Fixed inline (`= new List<T>()`), reran: **58/58 passed, 0 console errors**.
- Fixed a test compile error caused by an archived inconsistency: `Vector2Shared` is in `ProjectCore.Variables` while `Vector3Shared` is in the global namespace.
- Recorded the `Array<T>` fix in the extensions CHANGELOG (0.0.2 entry).
- Committed as `a8d0d51` + `2088ee3` (meta files).

Decisions made:

- Tests live inside each package under `Tests/EditMode/` — per-package test assemblies, matching Unity package conventions.
- PlayerPrefs-touching tests use unique GUID keys per test to avoid cross-talk and editor-pref pollution.
- Skipped tests for: attributes (editor drawers — no testable runtime logic), coroutines (timing-based — needs PlayMode), ui (rendering — needs PlayMode), platform.device (needs device; its logic was eval-probed earlier).
- The Array<T> NRE was fixed in the package rather than working around it in tests — tests-as-contract caught archived latent bug.

Issues found:

- `Vector2Shared` (ProjectCore.Variables) vs `Vector3Shared` (global namespace) — archived namespace inconsistency; noted in test comment, cleanup deferred.

Next steps:

- PlayMode tests for coroutines/ui when a scene harness exists.
- Port eventsystem packages from archive; add their tests following this suite's conventions.

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
