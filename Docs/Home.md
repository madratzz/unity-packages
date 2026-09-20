# unity-packages Wiki

This is an [Obsidian](https://obsidian.md) vault documenting `unity-packages` — a collection of reusable Unity game-development packages built on a ScriptableObject Architecture (SOAP) pattern, and a clonable template project (the `Assets/` layer) that wires them into a working game scaffold. See `IDEA.md` for the `Packages/` vs. `Assets/` split.

**Open the repository root (not just this `Docs/` folder) as the Obsidian vault.** Notes here link out to the canonical source — package `README.md`/`CHANGELOG.md` files, `AGENTS.md`, `.agents/CONTEXT.md` — instead of duplicating it, so those links only resolve if the whole repo is the vault.

## Start here

- [[Getting Started]] — clone, open the project, install a package, run tests
- [[Architecture Overview]] — the SOAP pattern, package layers, dependency graph
- [[Contributing]] — branch/PR workflow, version naming, documentation policy (summarizes `AGENTS.md`, which stays canonical)
- [[Glossary]] — SOAP, FSM, GameFlow, and other recurring terms

## Package catalog

Sixteen reusable packages live under `Packages/com.madratzz.*`. The [[GameFlow (Template Layer)]] application code isn't one of them — see the Template layer section below.

### Utilities — zero/low-dependency leaves

| Note | Package | What it does |
|---|---|---|
| [[Utilities - Attributes]] | `com.madratzz.utilities.attributes` | `[InlineEditor]` / `[Button]` / `[RequireReference]` inspector attributes + a required-reference validator. Zero-dependency. |
| [[Utilities - Core]] | `com.madratzz.utilities.core` | Legacy `Singleton<T>`, serializable dictionary, engine/DateTime extensions. Zero-dependency. |
| [[Utilities - Coroutines]] | `com.madratzz.utilities.coroutines` | Static coroutine runner for non-MonoBehaviour code (`CoroutineHandler`, `TimeCounter`). |
| [[Utilities - UI]] | `com.madratzz.utilities.ui` | uGUI/TextMeshPro extensions (alpha, RectTransform, ScrollRect snapping). |
| [[Utilities - Addressables]] | `com.madratzz.utilities.addressables` | Coroutine-based safe Addressables loading with leak-free failure handling. |
| [[Utilities - Build Automation]] | `com.madratzz.utilities.buildautomation` | Editor menu iOS/Android build triggers with keystore-safe config. |
| [[Utilities - Always Start From Scene Zero]] | `com.madratzz.utilities.unity.alwaysstartfromscenezero` | Editor-only: Play Mode always boots scene 0. |
| [[Platform - Device]] | `com.madratzz.platform.device` | Privacy-compliant per-install identity (iOS Keychain / PlayerPrefs). |

### ScriptableObject Architecture (SOAP) family

| Note | Package | What it does |
|---|---|---|
| [[SOAP - Variables]] | `com.madratzz.scriptableobject.variables` | Int/Float/Bool/String SO variables. |
| [[SOAP - Variables Database]] | `com.madratzz.scriptableobject.variables.database` | PlayerPrefs-persisted variants of the above, plus `DBManager`. |
| [[SOAP - Variables Extensions]] | `com.madratzz.scriptableobject.variables.extensions` | SO arrays, shared vectors, Odin-backed dictionaries. |
| [[SOAP - Event System]] | `com.madratzz.scriptableobject.eventsystem.core` | `GameEvent` SO event bus, listener/raiser components. |
| [[SOAP - Event System Extensions]] | `com.madratzz.scriptableobject.eventsystem.extensions` | Typed/generic/return-value `GameEvent` variants. |
| [[SOAP - Event Variables]] | `com.madratzz.scriptableobject.event.variables` | Variables that auto-raise a `GameEvent` on change. |
| [[SOAP - State Machine]] | `com.madratzz.scriptableobject.statemachine.core` | Coroutine-driven FSM with declarative `Transition` assets. |
| [[SOAP - Time Machine]] | `com.madratzz.scriptableobject.time.machine` | SO interval timer that fires a `GameEvent` every tick. |

See [[Architecture Overview]] for how these packages depend on each other.

## Template layer (`Assets/`)

Not a package — the `Assets/` integration layer that wires the packages above into a working, clonable game scaffold.

| Note | Location | What it does |
|---|---|---|
| [[GameFlow (Template Layer)]] | `Assets/Runtime`, `Assets/Tests`, `Assets/Prefabs`, `Assets/Scenes` | Top-level GameFlow orchestrator: wires FSM transitions to a decision table via `ApplicationBase`/`ApplicationFlowController`. |
