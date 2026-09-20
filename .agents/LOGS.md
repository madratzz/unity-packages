# Active Logs

Last updated: 2026-09-21

## Current Session

### 2026-09-21T00:22:08+05:00 — claude-sonnet-5/searchable-asset-dropdown

Summary of what was done:

- Follow-up on the earlier GameEvent-wiring discussion: user asked whether adding Odin Inspector would let `GameEvent` fields render as a searchable dropdown. Answered with both options (Odin `[ValueDropdown]` vs. a free `AdvancedDropdown`-based drawer built into `com.madratzz.utilities.attributes`) and recommended the free one given this repo's consistent zero/optional-dependency posture; user asked to build the no-Odin version first, "to test."
- **First pass**: added `SearchableAssetAttribute` (marker `PropertyAttribute`) + `SearchableAssetDrawer`/`SearchableAssetDropdown`/`SearchableAssetFinder` to `com.madratzz.utilities.attributes`, applied `[SearchableAsset]` to the three `GameEvent` fields in `eventsystem.core` and (per a follow-up "also do this to all scriptable object Variables") to `ApplicationBase.AppPausedTime` (a `DBInt`) — the only Variable-typed reference field that currently exists anywhere in the codebase (checked by grep before claiming "all").
- **User then asked**: "Can't we make it class level? so it gets inherited and apply auto? instead of adding attribute to all fields in all classes?" — correct instinct. Reworked before committing anything:
  - Deleted `SearchableAssetAttribute` entirely.
  - Re-registered `SearchableAssetDrawer` against the **type** `ScriptableObject` itself — `[CustomPropertyDrawer(typeof(ScriptableObject), true)]` — instead of against a marker attribute. This is a different, well-documented Unity mechanism (type-based drawer dispatch, matching how Unity resolves drawers for e.g. `Vector2Int` natively) that applies automatically to every field of every `ScriptableObject` subclass in the entire project, with zero attributes needed anywhere, ever, including on future new types.
  - Removed the now-unnecessary `[SearchableAsset]` lines from all 4 fields — they get the treatment automatically now.
  - Deliberately scoped the type registration to `ScriptableObject`, not the broader `UnityEngine.Object` — `GameObject`/`Component` fields are often scene references, not project assets, so an `AssetDatabase`-backed search would be actively wrong for those; every current and likely-future use case here (`GameEvent`, the `Variables` family, `FiniteStateMachine`, `Transition`, `TimeMachine`) is a `ScriptableObject`, so this scope is both correct and sufficient.
  - `SearchableAssetDrawer` also resolves array/`List<T>` element types before searching (`fieldInfo.FieldType` on a collection field is the collection type, not the element type — would have searched for `t:GameEvent[]` and found nothing).
  - `[RequireReference]` stays a manual per-field attribute — unlike searchability, "is this field mandatory" isn't inferable from type alone (`ApplicationStateMachine` and `ApplicationTimeMachine` are both `ScriptableObject` references but only one is required), so there's no equivalent type-level shortcut for it.
- Added `Tests/EditMode/SearchableAssetFinderTests.cs` for the pure asset-lookup logic (creates and cleans up real temp `.asset` files under a scratch `Assets/` folder via `AssetDatabase.CreateAsset`/`DeleteAsset`, since `AssetDatabase.FindAssets` only sees real assets on disk, not runtime-created ScriptableObject instances). The dropdown UI itself isn't unit-testable, same reasoning as `RequiredReferenceValidator`'s UI half.
- Updated `com.madratzz.utilities.attributes` and `eventsystem.core`'s README/CHANGELOG, and the `Docs/` vault notes touched earlier this session (`Utilities - Attributes`, `SOAP - Event System`, `GameFlow (Template Layer)`) to describe the final (type-based, automatic) design — not the discarded per-field-attribute one.

Files touched:

- `Packages/com.madratzz.utilities.attributes/Editor/{SearchableAssetDrawer,SearchableAssetDropdown,SearchableAssetFinder}.cs` (new), `Tests/EditMode/SearchableAssetFinderTests.cs` (new), `README.md`, `CHANGELOG.md`
- `Packages/com.madratzz.scriptableobject.eventsystem.core/README.md`, `CHANGELOG.md`
- `Assets/Runtime/README.md`
- `Docs/Packages/{Utilities - Attributes,SOAP - Event System}.md`, `Docs/GameFlow (Template Layer).md`
- `.agents/CONTEXT.md`, `.agents/LOGS.md`

Decisions made:

- Chose type-based `CustomPropertyDrawer` registration over the initial attribute-based design once asked — it's strictly better for this use case (same UI result, no per-field maintenance burden, no risk of someone adding a new `GameEvent`/Variable field and forgetting the attribute) and there's no real downside for `ScriptableObject` specifically (unlike `UnityEngine.Object` broadly, every `ScriptableObject` field here is genuinely an asset reference).

Issues found:

- None new. Noticed while investigating: `InlineEditorDrawer.cs` (pre-existing, this package) has code that looks like it's trying to do a similar "check the class type, not just the field" fallback for `InlineEditorAttribute`, registered via the attribute-based `[CustomPropertyDrawer(typeof(InlineEditorAttribute))]` mechanism — which (unlike the type-based registration used here) only ever invokes the drawer for fields that already carry the attribute directly, making that fallback branch likely dead code. Did not investigate further or touch it — out of scope for this task, flagging for whoever next touches `InlineEditorDrawer`.

Next steps:

- Push `feature/searchable-asset-dropdown` and open its PR into `development`.
- Try it in the Editor (per the user's "to test") — open `Assets/Prefabs/ApplicationBase.prefab` or `ApplicationFlowController.prefab` and confirm the `ScriptableObject` fields render as searchable-dropdown buttons.

### 2026-09-21T00:08:32+05:00 — claude-sonnet-5/gameflow-template-vault-rework

Summary of what was done:

- User asked to finish the `Docs/` vault rework that had been flagged as outstanding across three separate prior sessions' logs: GameFlow merged into `development` (PR #6/#12) and moved from a package into the `Assets/` template layer, but the vault still described it as an unmerged 17th package in several places.
- **Moved** `Docs/Packages/SOAP - Architecture (GameFlow).md` → `Docs/GameFlow (Template Layer).md` (out of `Docs/Packages/`, since it isn't a package) and rewrote it from scratch: dropped the "unmerged" status callout and the whole "Known issues" section (duplicate FSM refs, `Resources.Load` fallback, README bug — all fixed in earlier sessions), pointed its README link at `Assets/Runtime/README.md` instead of a `feature/architecture`-branch GitHub URL, added `[[Utilities - Attributes]]` to its dependencies and a "Catching missing wiring" section documenting `[RequireReference]`, and updated the usage snippet's field names/wiring steps to match current reality.
- Renamed every `[[SOAP - Architecture (GameFlow)]]` wikilink across the vault to `[[GameFlow (Template Layer)]]` (bulk `sed`, since Obsidian wikilinks resolve by filename and both old and new titles pointed at what's conceptually the same note) — hit 9 other files.
- Removed the "*(unmerged)*"/"*(unmerged — ...)*" annotations from every "Used by" list entry referencing it (`SOAP - Time Machine`, `SOAP - State Machine`, `SOAP - Variables`, `SOAP - Variables Database`, `SOAP - Event System Extensions`), and rewrote `SOAP - State Machine.md`'s "known issue" paragraph to describe the bug as fixed (single source of truth + `[RequireReference]`) rather than present.
- `Docs/Home.md`: removed GameFlow from the 16-package SOAP-family table (it was never actually a 17th package by the time this session started, just still labeled like one) and added a new "Template layer (`Assets/`)" section with its own one-row table, matching the `Packages/` vs. `Assets/` split `IDEA.md` already documents. Updated the intro paragraph and the `Utilities - Attributes` row to mention `[RequireReference]`.
- `Docs/Architecture Overview.md`: removed the stale "Known exception" callout about the `Resources.Load` violation (replaced with a past-tense note on how it was fixed), relabeled the Mermaid diagram's `Layer 4` subgraph from `"GameFlow (unmerged)"` to `"Assets/ template layer, not a package"` and its node from `scriptableobject.architecture` to `GameFlow` (adding the `gameflow --> attrs` edge it gained from depending on `utilities.attributes` now), reworded the "GameFlow is the composition root" bullet to stop calling it a package, and removed the now-resolved `[SerializeField]`-casing open question (PascalCase was adopted project-wide two sessions ago).
- `Docs/Getting Started.md`: reworded the "no Samples~ folder" line to explain *why* (not a package) instead of calling it unmerged.
- Fixed the one remaining stale cross-reference in `.agents/CONTEXT.md` itself (a note saying the vault "still describes it as a 17th package — needs a follow-up pass"), since that follow-up is what this session did.
- Verified with scripted checks (not by inspection alone, given how much moved): every `[[wikilink]]` in the vault resolves to an existing note title, and every relative markdown link (`](path)`) resolves to an existing file — including the new note's link to `Assets/Runtime/README.md`.

Files touched:

- `Docs/GameFlow (Template Layer).md` (new, replaces `Docs/Packages/SOAP - Architecture (GameFlow).md`, deleted)
- `Docs/Home.md`, `Docs/Architecture Overview.md`, `Docs/Getting Started.md`
- `Docs/Packages/SOAP - {Time Machine,State Machine,Variables,Variables Database,Event System Extensions}.md`
- `.agents/CONTEXT.md`, `.agents/LOGS.md`

Decisions made:

- Kept the note's content aligned with what's actually true today rather than doing a minimal find-replace of "unmerged" → "" — several sentences needed rewriting (past tense for fixed bugs, updated field names, new dependency) or the note would read as accurate on a skim but contain leftover false claims.
- Did not touch `.agents/LOGS.md`'s own historical entries even though several say "unmerged" — those are an accurate record of what was true when they were written, not live facts to keep current.

Issues found:

- None new. This entire session was closing a gap flagged (but deliberately not fixed) in three prior sessions' "Next steps."

Next steps:

- None outstanding for the vault specifically. `Assets/Prefabs/ApplicationBase.prefab`/`ApplicationFlowController.prefab` still have no assets wired (from earlier sessions) — `RequiredReferenceValidator` will report both `[RequireReference]` fields as missing until that's done, which is accurate, not a bug.

### 2026-09-20T23:57:53+05:00 — claude-sonnet-5/required-reference-validation

Summary of what was done:

- User asked "what about tests? and license and readme?" as a follow-up on the `[RequireReference]`/`RequiredReferenceValidator` work, and pointed at `github.com/madratzz/unity-starter-template` (a separate, more elaborate personal template repo — VContainer/MessagePipe/Ports-and-Adapters) for reference conventions. Fetched its file tree, root `README.md`, and `Assets/Tests/EditMode/Application/WiringGuardTests.cs` (a wiring-completeness test in the same spirit as `RequiredReferenceValidator`) via `gh api` to see what conventions to borrow — root `LICENSE`, a `README.md` quick-start section, and its `internal` + `InternalsVisibleTo` seam pattern for testing Editor-only code, which this repo's own `utilities.buildautomation` package already uses (confirmed by grep before copying it, rather than assuming the reference repo's pattern applied here unmodified).
- **Tests added** (repo had none for this PR's changes):
  - `com.madratzz.utilities.attributes` had **no test infrastructure at all** — created `Tests/EditMode/` + asmdef + `RequiredReferenceValidatorTests.cs` (4 tests: unassigned-is-reported, assigned-is-not-reported, unmarked-field-is-not-reported, nested-GameObject-hierarchy-path-is-correct). Made `RequiredReferenceValidator.ValidateGameObject` `internal` (was `private`) and added `Editor/AssemblyInfo.cs` with `[InternalsVisibleTo("com.madratzz.utilities.attributes.tests")]` so the tests can call it directly against constructed GameObjects instead of needing a real scene/prefab on disk — this exact pattern (internal seam + AssemblyInfo) already exists in `com.madratzz.utilities.buildautomation`, so it's consistent with this repo, not borrowed wholesale from the reference.
  - `com.madratzz.scriptableobject.eventsystem.core` already had `Tests/EditMode/GameEventTests.cs` (for `GameEvent` itself) but nothing for the three MonoBehaviours I null-guarded last session — added `GameEventMonoTests.cs`: regression tests for the null-guard fix (`LogAssert.Expect` + `Assert.DoesNotThrow`) on all three, plus one sanity test that a correctly-assigned `GameEventRaiser` still raises its event.
- **License**: added a root `LICENSE` (MIT, same text as every per-package `LICENSE.md`) — the repo had per-package licenses but nothing at the root, which matters now that `IDEA.md` describes this repo as also being a clonable template people use directly, not just a bag of independently-licensed packages.
- **README**: added a "Quick start" section (prerequisites, clone command, pointer to `Docs/Getting Started.md`), a "Running tests" section, and a "License" section linking the new root `LICENSE`. Did not copy the reference repo's "Verified status" section (test pass counts, 0 warnings) — that would mean either fabricating numbers or actually running the Unity test suite, and no Unity batch invocation is documented for this repo (flagged again as a gap, same as in the earlier FSM-wiring-fix session).
- **Found and fixed adjacent staleness while touching these files**: `Docs/Getting Started.md` still said Unity `6000.3.21f1` (actual: `6000.3.24f1`) and listed a stale 5-package "no tests" set that included `utilities.attributes` (now has tests) and `utilities.core` (turned out to already have tests — the "5 packages" figure was already wrong before this session, not just made wrong by it) while omitting `utilities.unity.alwaysstartfromscenezero` (which has none). Recomputed the actual count (12 of 16 packages have `Tests/`) and corrected both `Docs/Getting Started.md` and `.agents/CONTEXT.md`'s matching open question. Also removed a stray empty `Packages/com.madratzz.scriptableobject.architecture/` directory left over from an earlier `git rm` (untracked, harmless, but confusing `ls`).
- **Explicitly did not** do a full sweep of the `Docs/` vault's other stale "unmerged"/GameFlow-as-17th-package references (`Home.md`, `Architecture Overview.md`, `SOAP - Architecture (GameFlow).md`, several `SOAP - *.md` notes) — that's the larger vault-rework follow-up flagged in two earlier sessions' logs, out of scope for a "tests/license/readme" ask. Only touched `Docs/Getting Started.md` since it's literally the tests/prerequisites guide.

Files touched:

- `LICENSE` (new)
- `README.md`
- `Packages/com.madratzz.utilities.attributes/Tests/EditMode/{com.madratzz.utilities.attributes.tests.asmdef,RequiredReferenceValidatorTests.cs}` (new), `Editor/AssemblyInfo.cs` (new), `Editor/RequiredReferenceValidator.cs` (private → internal seam), `CHANGELOG.md`
- `Packages/com.madratzz.scriptableobject.eventsystem.core/Tests/EditMode/GameEventMonoTests.cs` (new), `CHANGELOG.md`
- `Docs/Getting Started.md`
- `AGENTS.md` (added a `LICENSE` row to Repository Layout)
- `.agents/CONTEXT.md`, `.agents/LOGS.md`

Decisions made:

- Referenced the `unity-starter-template` repo for *conventions* (root LICENSE, README quick-start shape, internal-seam testing) and not for its architecture (VContainer/MessagePipe/Ports-and-Adapters) — this repo's SOAP/no-DI-container direction is a settled, separate decision from earlier sessions, not something this ask reopened.
- Did not fabricate a "tests passing" status line in the README — reported only what's structurally true (which packages have a `Tests/` folder) rather than invent pass counts without having run them.

Issues found:

- None new — the stale test-count figure and empty `architecture/` directory were pre-existing, found while working in adjacent files, not introduced here.

Next steps:

- This is still on `feature/required-reference-validation` (PR #13, open, not yet merged) — these are additional commits on that same branch/PR rather than a new one, since the tests are regression coverage for that PR's own changes.
- The `Docs/` vault "unmerged"/17th-package sweep remains outstanding (see above) — worth its own task.
- No documented way to actually run the Unity test suite or get a real pass/fail count in this environment — if that's ever set up, the README's "Running tests" section could gain the kind of verified-status line the reference repo has.

### 2026-09-20T23:43:46+05:00 — claude-sonnet-5/required-reference-validation

Summary of what was done:

- User asked how to improve `GameEvent`s given "reference can be missed" issues, and whether an EventBus would help. Answered with a design comparison (EventBus solves it structurally but trades away this repo's explicit-wiring principle; recommended two lower-risk fixes instead) and, on confirmation, implemented both:
  1. **Null-guarded the actual crash sites.** `GameEventListener.OnEnable`/`OnDisable`, `GameEventRaiser.InvokeEvent`, and `GameEventRaiserOnEnable.OnEnable` previously threw `NullReferenceException` on an unassigned `GameEvent` field with no guard at all — now they log a `Debug.LogError` and no-op.
  2. **Added a project-wide required-reference convention**: `RequireReferenceAttribute` (new, in `com.madratzz.utilities.attributes` — zero-dependency, so this adds no new package coupling) marks a `[SerializeField]` as required; `RequiredReferenceValidator` (Editor) reflects over every scene and prefab under `Assets/` for `[RequireReference]` fields left unassigned, via a menu item (**Tools → Validate Required References**) or a CI entry point (`-batchmode -executeMethod CustomEditorUtilities.RequiredReferenceValidator.ValidateProjectCI`, non-zero exit on any miss).
- Applied `[RequireReference]` to the fields identified as genuinely required (crash/hard-fail without a value): the three `GameEvent` fields above, `ApplicationBase.ApplicationStateMachine`, and `ApplicationFlowController.ApplicationBase`. Deliberately did *not* mark `ApplicationBase`'s other fields (`ApplicationTimeMachine`, `AppPaused`, `AppResumed`, `AppPausedTime`) or `ApplicationFlowController`'s transition/event fields — the code already null-guards those gracefully, so marking them would just be validator noise for legitimately-optional wiring.
- Added a reference from the GameFlow assembly (`Assets/Runtime/madratzz.scriptableobject.architecture.runtime.asmdef`) to `com.madratzz.utilities.attributes.runtime` so `ApplicationBase`/`ApplicationFlowController` can use the attribute; `eventsystem.core` already depended on `utilities.attributes`, no asmdef change needed there.
- Explicitly did **not** implement an EventBus or attempt the "wired to a different-but-valid asset" case — flagged in `Docs/Packages/SOAP - Event System.md` as a known, currently-unsolved failure mode (nothing can distinguish "two assets deliberately different" from "two assets that should've been the same one" without a stronger identity/registry mechanism).

Files touched:

- `Packages/com.madratzz.utilities.attributes/Runtime/Attributes/RequireReferenceAttribute.cs` (new), `Editor/RequiredReferenceValidator.cs` (new), `README.md`, `CHANGELOG.md`
- `Packages/com.madratzz.scriptableobject.eventsystem.core/Runtime/Mono/{GameEventListener,GameEventRaiser,GameEventRaiserOnEnable}.cs`, `README.md`, `CHANGELOG.md`
- `Assets/Runtime/Application/{ApplicationBase,ApplicationFlowController}.cs`, `madratzz.scriptableobject.architecture.runtime.asmdef`, `README.md`
- `Docs/Packages/{Utilities - Attributes,SOAP - Event System}.md` (also fixed a stale "unmerged" note on the GameFlow cross-link while in that file — GameFlow merged into `development` earlier this session)
- `.agents/CONTEXT.md`, `.agents/LOGS.md`

Decisions made:

- Implemented the validator as an attribute (`[RequireReference]`) scanned via reflection, not a central type→field-name registry — the registry approach would force the validator package to reference every package with wiring components (`eventsystem.core`, the GameFlow assembly, …), which is exactly the kind of coupling this fix is supposed to reduce. The attribute approach keeps `utilities.attributes` at zero package dependencies.
- Only marked fields that currently hard-fail (crash or, for `ApplicationFlowController`, disable the whole component) when unassigned — not every `[SerializeField]` reference in these files. A validator that flags legitimately-optional fields trains people to ignore its output.

Issues found:

- None new.

Next steps:

- Branch is `feature/required-reference-validation`, started fresh from `development` (both `feature/architecture`'s PRs — #6 and #12 — are merged, so `development` now has the full GameFlow package and the `Docs/` vault). Push and open a PR into `development`.
- Consider running `RequiredReferenceValidator` on `Assets/Prefabs/ApplicationBase.prefab`/`ApplicationFlowController.prefab` once real assets exist to wire in — right now every field on both is still unassigned (`{fileID: 0}`), so the validator would report both `[RequireReference]` fields as missing, which is accurate but not yet actionable until those assets exist.

### 2026-09-20T23:24:14+05:00 — claude-sonnet-5/merge-development-into-architecture

Summary of what was done:

- Prepared PR #6 (`feature/architecture` → `development`) to be mergeable. It had gone `CONFLICTING`/`DIRTY` because `feature/architecture` forked before `development`'s 2026-09-16 `.agents/` uppercase rename (PR #7) and had kept using the lowercase files (`.agents/context.md`, `.agents/logs.md`) all session.
- **Found a real data-loss risk, not just a text conflict**: a first dry-run merge silently dropped `.agents/context.md`'s content entirely — git did not detect it as a rename of `.agents/CONTEXT.md` (too much content drift by now) and instead just kept `development`'s `CONTEXT.md` as-is with no conflict marker to catch it. A second attempt (after PR #11 merged, adding more history) did flag it as an explicit `modify/delete` conflict instead — resolved by hand-merging every unique fact from `.agents/context.md` (the `Assets/`-as-template-layer decision, the updated 16-package list, the `[SerializeField]` casing resolution, the `Assets/GameEvents` open question, the Unity-version and test-coverage corrections) into `.agents/CONTEXT.md`, then deleting the lowercase file.
- Resolved the `.agents/LOGS.md` conflict the same way as the earlier PR #10 conflict: both sides' newest-first entries kept, in their already-correct chronological order (this branch's newer entries first, `development`'s next, converging into the shared older history both branches already agree on).
- Resolved a `README.md` conflict: kept this branch's updated intro line plus `development`'s "Documentation" pointer section — both additive, no actual disagreement.
- `AGENTS.md` auto-merged cleanly (git correctly wove this branch's `Assets/`-vs-`Packages/` edits into `development`'s fuller v2 policy structure) — but its "C# field-naming convention is not yet settled" note was stale (referenced `scriptableobject.architecture`'s old camelCase example field, already renamed to PascalCase in PR #11). Updated it to state the convention is resolved.
- `Packages/packages-lock.json` auto-merged cleanly with no conflict.
- Did this on a dedicated branch (`chore/merge-development-into-architecture`, off `feature/architecture`) rather than merging directly on `feature/architecture`, so the resolution itself goes through review like any other change.

Files touched:

- `.agents/CONTEXT.md` (merged), `.agents/LOGS.md` (merged), `.agents/context.md` (deleted, content preserved in `CONTEXT.md`)
- `README.md`, `AGENTS.md`

Decisions made:

- Treat a "successful" auto-merge or silently-resolved rename as something to verify, not trust by default, when two branches used different filenames for the same conceptual file — this session's first dry-run would have silently destroyed real content if committed without checking.

Issues found:

- None new beyond the data-loss risk above, which was caught before anything was committed or pushed.

Next steps:

- Push `chore/merge-development-into-architecture` and open a PR into `feature/architecture`.
- Once merged, PR #6 (`feature/architecture` → `development`) should be clean and mergeable — verify before merging.
- The `Docs/Packages/SOAP - Architecture (GameFlow).md` vault note still needs its rework (GameFlow described as the `Assets/` template layer, not package #17) once PR #6 lands on `development` and the vault becomes visible from this lineage.

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

### 2026-09-20T19:32:11+05:00 — claude-sonnet-5/codebase-obsidian-vault

Summary of what was done:

- User asked for "how to" documentation for the whole codebase, in the form of an Obsidian vault. Created `Docs/` at the repository root (outside `Assets/`, alongside `.agents/`/`.archive/`) as that vault: `Home.md` (entry point + package catalog), `Architecture Overview.md` (SOAP explanation + Mermaid dependency graph across all packages), `Getting Started.md`, `Contributing.md` (summary — `AGENTS.md` stays canonical), `Glossary.md`, and one note per package under `Docs/Packages/` (17 notes: 16 merged packages + the unmerged `scriptableobject.architecture` GameFlow package, marked as unmerged/WIP with its known issues from the 2026-09-20 code review carried over).
- Design choice: notes link out to the real source (package `README.md`/`CHANGELOG.md` files, `AGENTS.md`) via relative links/wikilinks rather than duplicating their content, to avoid drift — same anti-duplication principle `AGENTS.md`/`CLAUDE.md` already use. `Home.md` instructs opening the *repository root* as the vault, not just `Docs/`, so those cross-links resolve.
- Verified every `[[wikilink]]` in the vault resolves to an existing note title and every `../../Packages/...` README link resolves to an existing file (scripted check, no broken links).
- While writing `Docs/Getting Started.md`'s "Running tests" section, found `.agents/CONTEXT.md`'s open question "no embedded package has tests yet" is stale — 11 of 16 packages actually have a `Tests/` folder. Corrected both the vault note and the `.agents/CONTEXT.md` open question.
- Updated `.agents/CONTEXT.md`'s package list, which still said "eight custom embedded packages" — 16 are now merged (plus the unmerged 17th on `feature/architecture`); added a `Docs/` row to the architecture/structure list.
- Added a `Docs/` row to `AGENTS.md`'s Repository Layout table.
- Added a "Documentation" pointer section to the root `README.md` (was previously just a one-line title) linking to `Docs/Home.md`.
- Created branch `feature/codebase-obsidian-vault` from `development` (not stacked on the still-unmerged `feature/sync-context-editor-bump`, since this is an unrelated task) per the branch-per-task policy.

Files touched:

- `Docs/Home.md`, `Docs/Architecture Overview.md`, `Docs/Getting Started.md`, `Docs/Contributing.md`, `Docs/Glossary.md` (all new)
- `Docs/Packages/*.md` — 17 new notes (one per package, including the unmerged `scriptableobject.architecture`)
- `README.md`, `AGENTS.md`, `.agents/CONTEXT.md`, `.agents/LOGS.md`

Decisions made:

- The vault lives at `Docs/` and is scoped to the whole repository root, not a subfolder vault — this lets it link into `Packages/*/README.md` and `AGENTS.md` without copying them.
- Documented the unmerged `scriptableobject.architecture` package in the vault (clearly marked unmerged) rather than omitting it, since "the whole codebase" reasonably includes in-progress branches a reader would otherwise not know about.

Issues found:

- `.agents/CONTEXT.md`'s package list and "no tests yet" open question were stale relative to the actual repository state (16 packages merged, 11 with tests) — likely because `CONTEXT.md` wasn't updated as packages were ported across multiple past sessions. Corrected in this pass; worth checking for further drift the next time `.agents/CONTEXT.md` is touched.

Next steps:

- Push `feature/codebase-obsidian-vault` and open its PR once GitHub auth is available in this environment (`git push` failed here with no GitHub credentials configured — the same blocker hit on `feature/sync-context-editor-bump` earlier today).
- Consider whether `scriptableobject.architecture`'s known issues (duplicate FSM references, implicit `Resources.Load` fallback) should be fixed before that branch's PR, since the vault now documents them as known, unfixed issues.

### 2026-09-20T19:14:52+05:00 — claude-sonnet-5/sync-context-editor-bump

Summary of what was done:

- A `/code-review` on the working tree flagged pending, uncommitted changes to `Packages/manifest.json`, `Packages/packages-lock.json`, and `ProjectSettings/ProjectVersion.txt` (Unity editor `6000.3.21f1` → `6000.3.24f1`, `com.unity.collab-proxy` `2.12.4`→`2.13.6`, `com.unity.timeline` `1.8.12`→`1.8.13`, plus new `com.unity.sdk.linux-x86_64` and `com.unity.toolchain.linux-x86_64-linux` dependencies at `1.1.0`) as undocumented: `.agents/CONTEXT.md` still stated the old editor version and no `.agents/LOGS.md` entry existed for the bump.
- Received a separate user request to bootstrap a generic "AI Agent Context System Setup" spec. Discovery found this repository's existing `.agents/`/`.archive/` system already matches that spec (same `AGENTS.md` policy content, same archive structure) except for filename casing (`CONTEXT.md` vs. spec's `context.md`, etc.) — a decision already made and recorded on 2026-09-16 (see prior entry). Per the spec's own "report before major restructure" rule and `AGENTS.md`'s Start-of-Work Procedure, reported the conflict; user chose to keep the existing uppercase system as-is rather than create a colliding/duplicate lowercase set, and asked to fix the known staleness instead.
- Created branch `feature/sync-context-editor-bump` from `development` (the pending manifest/version-file changes were already sitting uncommitted on `development`; branched with them carried over per the repository's feature-branch-per-task policy).
- Updated `.agents/CONTEXT.md`: corrected the Unity editor version in "Active Constraints" to `6000.3.24f1`, and added an open question about whether the new Linux SDK/toolchain packages were a deliberate project-wide dependency decision or an artifact of Editor package resolution during the version bump (not confirmed — recorded as an assumption, not a fact).

Files touched:

- `Packages/manifest.json`, `Packages/packages-lock.json`, `ProjectSettings/ProjectVersion.txt` (pre-existing uncommitted changes, not authored in this session)
- `.agents/CONTEXT.md`, `.agents/LOGS.md`

Decisions made:

- Keep the existing uppercase `.agents`/`.archive` filenames and structure; do not create a parallel lowercase set from the generic setup spec.
- Bundle the editor/package-bump documentation update into the same commit as the dependency-record change it describes, per `AGENTS.md`'s documentation policy.

Issues found:

- Rationale for the new `com.unity.sdk.linux-x86_64` / `com.unity.toolchain.linux-x86_64-linux` project-wide dependencies is unknown — flagged as an open question in `.agents/CONTEXT.md` rather than asserted as a deliberate decision.

Next steps:

- Confirm with whoever performed the editor upgrade whether the Linux SDK/toolchain packages are an intentional project-wide dependency; if not, consider scoping or removing them.
- Open a PR from `feature/sync-context-editor-bump` into `development` per branch policy.

### 2026-09-16T22:34:50+05:00 — claude-sonnet-5/agent-context-system-v2

Summary of what was done:

- Executed a user-supplied "AI Agent Context System Setup" spec against the already-established `.agents/`/`.archive/` system (in use since 2026-08-15 with substantial real history). Discovery found the existing system incompatible with the new spec on two points: lowercase active filenames (`context.md`, etc.) vs. required uppercase, and `DD-MM-YY` archive-date convention vs. required `YYYY-MM-DD` — both previously recorded as deliberate decisions in `.agents/MEMORY.md`. Per the spec's own "report before major restructure" rule, asked the user; they chose the uppercase/`YYYY-MM-DD` spec convention.
- Created branch `feature/agent-context-system-v2` from `development` (not from `feature/architecture`, which carries unrelated, unmerged GameFlow-port work) per the spec's branch-per-task policy.
- Renamed active files to uppercase via `git mv`: `context.md`→`CONTEXT.md`, `memory.md`→`MEMORY.md`, `learnings.md`→`LEARNINGS.md`, `logs.md`→`LOGS.md`, `agents/default-agent.md`→`agents/DEFAULT-AGENT.md`. Updated all cross-links in `AGENTS.md`, `.agents/README.md`, `.agents/INDEX.md`, and `.agents/agents/DEFAULT-AGENT.md`. Left historical "Files touched" references to the old lowercase names inside existing `LOGS.md` entries as-is (accurate record of what was literally touched at the time).
- Added `Schema version: 1` to every README/INDEX file under `.agents/` and `.archive/`.
- Rewrote root `AGENTS.md` to fold in the full policy set from the pasted spec: a "Version Control, Branching, and Pull Requests" section (feature branches from `development`, PR-only merges, no squash-merge, `main` only via PR from `development`), a "Version Naming" section (`X.Y.Z` with `Z` = epoch-minutes), an expanded documentation-policy paragraph (doc update or LOGS.md doc-review note required per task, same commit), multi-agent shared-state editing rules, and expanded sensitive-data placeholders — while preserving all existing project-specific content (SOAP/DI guidance, Unity conventions, repository layout).
- Added root `CLAUDE.md` as a short pointer to `AGENTS.md` (did not exist before).
- Updated `.agents/MEMORY.md` naming-conventions and stable-facts sections to record the new uppercase/`YYYY-MM-DD` convention, the branch/PR/version-naming facts, and a note that the prior convention is superseded (no archive files existed yet, so no historical archive filenames needed migration).
- Updated `.agents/CONTEXT.md` (decisions, architecture list, open questions) and `.agents/LEARNINGS.md` to record an inconsistency found while drafting the Unity-naming guidance: `[SerializeField]` fields are PascalCase in the ported `scriptableobject.*` family but camelCase in the newer `scriptableobject.architecture` package — did not adopt the spec's example naming rule as fact since it doesn't match this repo's actual code.

Files touched:

- `AGENTS.md`, `CLAUDE.md` (new)
- `.agents/README.md`, `.agents/INDEX.md`, `.agents/CONTEXT.md`, `.agents/MEMORY.md`, `.agents/LEARNINGS.md`, `.agents/LOGS.md`
- `.agents/agents/DEFAULT-AGENT.md` (renamed from `default-agent.md`)
- `.agents/context.md`, `.agents/memory.md`, `.agents/learnings.md`, `.agents/logs.md` (renamed to uppercase)
- `.archive/README.md`, `.archive/INDEX.md`, `.archive/logs/INDEX.md`, `.archive/memory/INDEX.md`, `.archive/learnings/INDEX.md`, `.archive/context/INDEX.md`, `.archive/agents/INDEX.md` (Schema version added)

Decisions made:

- Adopt uppercase active/archive filenames and `YYYY-MM-DD` archive-date convention going forward (user-approved, superseding the 2026-08-15 lowercase/`DD-MM-YY` convention).
- `AGENTS.md` remains the single canonical policy source; `CLAUDE.md` stays a pointer only, per the spec's own anti-duplication instruction.
- Did not archive anything in this session — no active file exceeded its size/age threshold, and the only content change to existing entries was additive.

Issues found:

- Existing system was materially more mature (dozens of real log entries, settled conventions) than the spec's "brand-new setup" framing assumed; a blind literal execution would have silently overwritten a documented, deliberate decision — flagged instead of auto-restructuring.
- `[SerializeField]` naming is not actually consistent project-wide (see Learnings); recorded as an open question rather than asserting the spec's example convention as project fact.

Next steps:

- Commit this work as small logical commits (rename+links, then policy/CLAUDE.md, then this log/context/memory/learnings update — bundled here per the doc-policy "same commit as the change" rule) and open a PR from `feature/agent-context-system-v2` into `development`.
- Decide the `[SerializeField]` casing convention project-wide and record it in `AGENTS.md`/`MEMORY.md` once decided.

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
