# Utilities - Always Start From Scene Zero

← [[Home]] · [[Architecture Overview]]

**Package:** `com.madratzz.utilities.unity.alwaysstartfromscenezero` · [Full README & API](../../Packages/com.madratzz.utilities.unity.alwaysstartfromscenezero/README.md)

Editor utility that loads scene index 0 when entering Play Mode, so iterating on a deep scene doesn't skip the boot sequence and leave managers unreferenced.

## Depends on

None. Editor-only — no runtime assembly, no impact on builds.

## Used by

Nothing in this repo yet.

## Usage

Toggle via **EditorUtilities → Always Start From Scene 0** (`Ctrl+Alt+P`). When active, entering Play Mode always loads build index 0 regardless of which scene is open; disable to restore default Unity behaviour.

---
← [[Home]]
