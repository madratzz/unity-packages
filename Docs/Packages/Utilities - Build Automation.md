# Utilities - Build Automation

← [[Home]] · [[Architecture Overview]]

**Package:** `com.madratzz.utilities.buildautomation` · [Full README & API](../../Packages/com.madratzz.utilities.buildautomation/README.md)

Editor-only utility for triggering iOS and Android (APK/AAB) builds from a single `BuilderConfig` ScriptableObject, with an optional project-root JSON source for CI.

## Depends on

None declared. Editor-only — no runtime assembly.

## Used by

Nothing in this repo yet.

## Setup

1. **Assets → Create → Build Automation → Builder Config**, place it under a `Resources/` folder, name it `BuilderConfig`.
2. Set keystore passwords, optional version override, and output directory in the Inspector.
3. **Build → Android APK / Android AAB / Android Development APK / iOS**.

## Settings precedence

1. `-buildversion` CLI argument (CI override) — always wins.
2. `buildsettings.json` at the **project root** (not `Assets/`) — only when `Read From Resources File` is enabled on the config. Missing keys merge over inspector values.
3. Inspector values on `BuilderConfig`.

## ⚠️ Secrets

**Never commit keystore passwords** — not in the `BuilderConfig` asset, not in `buildsettings.json`. Keep both in `.gitignore`, or commit the JSON without password fields (missing keys merge, they don't blank). See [[Contributing]]#Sensitive data.

The project-root JSON location is deliberate: it's not a Unity asset, so it can never be bundled into a player build.

## CI usage

```bash
"$UNITY" -batchmode -projectPath "$PROJECT" \
  -executeMethod CustomEditorUtilities.Builder.BuildAndroidAAB \
  -buildversion 1.4.2
```

Version codes derive from the bundle version by stripping dots (`1.4.2` → `142`). Keystore passwords apply to Android only.

---
← [[Home]]
