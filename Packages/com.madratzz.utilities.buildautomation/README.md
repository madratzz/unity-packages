# Build Automation

Editor utility for triggering iOS and Android (APK/AAB) builds with keystore credential management via a ScriptableObject configuration asset.

## Overview

`BuilderConfig` stores Android keystore credentials as a ScriptableObject asset, keeping sensitive values out of code and source control. `Builder` adds a `Build/` menu to the Unity Editor with one-click targets for iOS, Android APK, and Android AAB. This is an Editor-only package with no runtime overhead.

## Types

| Type | Description |
|------|-------------|
| `BuilderConfig` | ScriptableObject storing keystore path, keystore password, and key alias password |
| `Builder` | Editor class providing `Build/` menu items for iOS, Android APK, and Android AAB |

## Usage

1. Create a `BuilderConfig` asset via **Assets → Create → Build Automation → Builder Config**, place it under a `Resources/` folder, and name it `BuilderConfig`.
2. Fill in keystore credentials in the Inspector. **Never commit this asset to source control** — add it to `.gitignore`. Defaults are empty; with no asset present, builds fall back to the keystore passwords set in Player Settings.
3. Use **Build → Android APK** or **Build → Android AAB** from the Unity menu bar to trigger a build.

Keystore credentials are only applied to Android builds. Version codes are derived from the bundle version by stripping dots (`1.2.3` → `123`); override the bundle version from CI with the `-buildversion <version>` command-line argument.

## Installation

Install via the Unity Package Manager pointing to your Verdaccio registry.

## License

MIT
