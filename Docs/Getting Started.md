# Getting Started

← [[Home]]

## 1. Prerequisites

- Unity **6000.3.24f1** (Unity 6.3) — the version recorded in `ProjectSettings/ProjectVersion.txt` on `development`. Use Unity Hub to install this exact version; a newer patch will trigger an upgrade prompt.
- Git.
- [Odin Inspector](https://odininspector.com/) only if you need `RuntimeDictionary` types from [[SOAP - Variables Extensions]] — everything else builds without it.

## 2. Clone and open

```bash
git clone https://github.com/madratzz/unity-packages.git
cd unity-packages
git checkout development
```

Open the folder in Unity Hub. All 16 packages under `Packages/` are **embedded** (not registry-installed) — they resolve automatically from `Packages/manifest.json`/`packages-lock.json` when the project opens. No registry configuration is required to work in this repo itself.

## 3. Using a package in another project

The intended distribution path is a private **Verdaccio** registry (see `IDEA.md`), not yet configured. Until that's live, the practical option is to reference a package folder directly (e.g. via a local `file:` path or git submodule) — check `Packages/<name>/package.json` for that package's `dependencies` before copying it standalone, since most SOAP packages depend on others (see [[Architecture Overview]]).

## 4. Running tests

Most packages ship EditMode tests (12 of 16 have a `Tests/` folder; the exceptions are `platform.device`, `utilities.coroutines`, `utilities.ui`, and `utilities.unity.alwaysstartfromscenezero`). Run them from Unity: **Window → General → Test Runner → EditMode → Run All**.

The `Assets/` GameFlow layer has its own tests at `Assets/Tests/EditMode/ApplicationFlowLogicTests.cs`.

`com.madratzz.utilities.attributes` also ships `RequiredReferenceValidator`, an Editor tool (not a Test Runner test) that scans every scene and prefab under `Assets/` for unassigned `[RequireReference]` fields — **Tools → Validate Required References**, or `-batchmode -executeMethod CustomEditorUtilities.RequiredReferenceValidator.ValidateProjectCI` for a CI gate. See [[Utilities - Attributes]].

## 5. Trying a sample

Two packages ship Unity Package Manager samples (Package Manager window → select the package → **Samples** tab → **Import**):

| Package | Sample | What it gives you |
|---|---|---|
| [[SOAP - Time Machine]] | `TimeMachine Sample Assets` | A pre-configured `TimeMachine` + `e_TimeMachineTick` event asset. |

[[SOAP - Architecture (GameFlow)]] has no `Samples~/` folder — its README's usage snippet is the closest thing to a worked example, and it's unmerged.

## 6. Making a change

Follow the workflow in [[Contributing]] (full policy: `AGENTS.md`): branch from `development`, small logical commits, PR to merge, documentation update in the same commit as the change it describes.

← [[Home]]
