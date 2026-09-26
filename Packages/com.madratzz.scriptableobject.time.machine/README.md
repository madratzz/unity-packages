# Time Machine

ScriptableObject timer that fires a `GameEvent` on a fixed interval, enabling
time-driven logic without direct MonoBehaviour coupling.

## Overview

`TimeMachine` is a ScriptableObject asset whose tick loop runs through
`CoroutineHandler` (from `com.madratzz.utilities.coroutines`) — the persistent
coroutine runner. That means the timer survives scene loads, needs no
MonoBehaviour ownership, and can be started or stopped from anywhere.

## Usage

1. Assign a `GameEvent` asset to the TimeMachine's **Tick Event** field.
2. Call `timeMachine.StartTicking()` / `StopTicking()` from any code or hook it to a `GameEventRaiserOnEnable`.
3. Add a `GameEventListener` wherever you need per-tick callbacks.

**Timing options (Inspector):**
- **Tick Interval** — seconds between ticks (default `1`).
- **Use Real Time** — when off, ticking uses scaled time and pauses while
  `Time.timeScale == 0` (e.g. game-pause). When on, ticks on unscaled wall-clock
  time (survives pause). Default is scaled time.

The `TimeMachine Sample Assets` sample (Package Manager → Samples) includes a
pre-configured `TimeMachine` and `e_TimeMachineTick` event asset.

## Installation

Install via the Unity Package Manager pointing to your Verdaccio registry.

## License

MIT
