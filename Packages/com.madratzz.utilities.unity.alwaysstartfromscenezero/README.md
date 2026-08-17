# Always Start From Scene Zero

Editor utility that loads scene index 0 when entering Play Mode, ensuring consistent game startup regardless of the currently active scene.

## Overview

When iterating on a scene deep in the game (e.g. a level scene), hitting Play normally runs that scene rather than the boot sequence, which can cause missing manager references or incorrect game state. This package overrides that behaviour so Unity always starts from scene 0, mirroring what players experience at launch.

## Usage

Enable via the Unity menu: **EditorUtilities → Always Start From Scene 0** (shortcut: `%+P` / Ctrl+Alt+P).

When the toggle is active, entering Play Mode always loads the scene at build index 0. Disable the toggle to restore default Unity behaviour (plays the currently open scene).

This is an Editor-only package — there is no runtime assembly and no impact on builds.

## Installation

Install via the Unity Package Manager pointing to your Verdaccio registry.

## License

MIT
