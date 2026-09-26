# Event System

ScriptableObject-based event bus providing parameterless GameEvent assets with MonoBehaviour listener and raiser components for decoupled scene communication.

## Overview

A `GameEvent` asset acts as a shared signal that any number of `GameEventListener` components subscribe to and any number of `GameEventRaiser` components (or code) can invoke. Because the event lives in a ScriptableObject asset, scenes and systems remain fully decoupled — listeners and raisers don't need references to each other.

## Types

| Type | Description |
|------|-------------|
| `GameEvent` | ScriptableObject event asset with `Action`-based handler and `Invoke()` method |
| `GameEventListener` | MonoBehaviour that subscribes to a `GameEvent` and forwards it to a `UnityEvent` |
| `GameEventRaiser` | MonoBehaviour that invokes a stored `GameEvent` via `InvokeEvent()` |
| `GameEventRaiserOnEnable` | MonoBehaviour that automatically invokes its `GameEvent` when enabled |

## Usage

1. Create a `GameEvent` asset (right-click → Create → Game Event).
2. Add a `GameEventListener` to the receiver GameObject and assign the event asset and a `UnityEvent` response.
3. Call `myEvent.Invoke()` in code, or add a `GameEventRaiser` to the sender GameObject.

## Wiring mistakes

The most common bug with SO-based events isn't a crash, it's silence: a raiser and listener pointing at *different* `GameEvent` assets fire and listen correctly, just to nothing. An unassigned `GameEvent` field now at least logs a clear error instead of throwing or doing nothing — and every `GameEvent` field on these three components is marked `[RequireReference]` (from `com.madratzz.utilities.attributes`), so running that package's `RequiredReferenceValidator` (**Tools → Validate Required References**) catches an unassigned one across every scene and prefab in the project. It can't catch the "wired to the wrong asset" case — there's no way to know two different assets weren't meant to be different — so double-check raiser/listener pairs point at the same asset when debugging a "nothing happened" bug.

If `com.madratzz.utilities.attributes` is in the project, all three fields also get a searchable dropdown automatically — no attribute needed, it applies to every `ScriptableObject`-typed field project-wide (see that package's README). Clicking the field opens a dropdown listing every `GameEvent` asset in the project by name, instead of the default object picker, making it easier to find and pick the right one.

## Installation

Install via the Unity Package Manager pointing to your Verdaccio registry.

## License

MIT
