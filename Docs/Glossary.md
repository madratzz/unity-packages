# Glossary

← [[Home]]

**SOAP (ScriptableObject Architecture)**
Pattern used across this repo where shared state (variables) and cross-system signals (events) live in ScriptableObject assets rather than MonoBehaviours or singletons, so systems can communicate without holding references to each other. See [[Architecture Overview]].

**FSM (Finite State Machine)**
Provided by [[SOAP - State Machine]]. States and transitions are ScriptableObject assets; the machine doesn't self-drive — a caller runs its `Tick()` coroutine, typically via [[Utilities - Coroutines]]'s `CoroutineHandler`.

**GameEvent**
The core SO event type from [[SOAP - Event System]] — a parameterless, asset-based signal any number of listeners can subscribe to and any number of raisers can invoke.

**GameFlow**
The application-level orchestration layer implemented by [[SOAP - Architecture (GameFlow)]] — translates UI close events into FSM transitions via a `(FlowContext, UICloseReasons) → FlowIntent` decision table.

**DB variable**
A ScriptableObject variable from [[SOAP - Variables Database]] that auto-persists to `PlayerPrefs` on every `SetValue`/`ApplyChange` call — no manual save step.

**Verdaccio**
The private npm-compatible registry this repo's packages are intended to be distributed through (see `IDEA.md`). Not yet configured — see [[Getting Started]].

**Leaf package**
A package with zero dependencies on other packages in this repo (e.g. [[Utilities - Attributes]], [[Utilities - Core]], [[Platform - Device]]) — safe to depend on from anywhere without pulling in unrelated functionality.

← [[Home]]
