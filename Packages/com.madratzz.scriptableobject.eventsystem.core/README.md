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

## Installation

Install via the Unity Package Manager pointing to your Verdaccio registry.

## License

MIT
