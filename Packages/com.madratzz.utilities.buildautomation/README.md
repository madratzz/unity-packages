# Build Automation

Editor utility for triggering iOS and Android (APK/AAB) builds, driven by a single
`BuilderConfig` ScriptableObject with an optional project-root JSON source.

## How it works

Every **Build → …** menu item resolves its settings in this order:

1. **`-buildversion` CLI argument** — always wins (CI override).
2. **`buildsettings.json`** at the project root — when the config's
   **Read From Resources File** toggle is enabled. Missing keys merge over
   inspector values; a missing/invalid file falls back to them with a warning.
3. **Inspector values** on the `BuilderConfig` asset.

## Setup

1. **Create the config asset:** **Assets → Create → Build Automation → Builder Config**,
   place it under a `Resources/` folder, name it `BuilderConfig`.
2. **Set values in the Inspector:** keystore passwords (empty = keep Player Settings'
   values), optional bundle-version override, and output directory (default `Builds`).
3. **Build:** **Build → Android APK**, **Android AAB**, **Android Development APK**, or **iOS**.

⚠️ **Never commit keystore passwords** — not in the asset, not in the JSON file.
Keep `BuilderConfig.asset` and `buildsettings.json` in `.gitignore`, or commit the
JSON without the password fields (missing keys merge, they don't blank).

## JSON file source (toggle)

Enable **Read From Resources File** on the config to populate it from
`buildsettings.json` at the project root (next to `Packages/`, not inside `Assets/`):

```json
{
  "buildVersion": "1.2.3",
  "outputDirectory": "CIBuilds",
  "keystorePassword": "<SECRET>",
  "keyAliasPassword": "<SECRET>"
}
```

The project-root location is deliberate: the file is **not** a Unity asset, so it can
never be bundled into a player build (a `Resources/` JSON with secrets would ship
inside your APK/AAB). The Inspector still shows the populated values — the file is
the source, the SO is the surface.

## CI usage

```bash
"$UNITY" -batchmode -projectPath "$PROJECT" \
  -executeMethod CustomEditorUtilities.Builder.BuildAndroidAAB \
  -buildversion 1.4.2
```

Version codes derive from the bundle version by stripping dots (`1.4.2` → `142`).
Keystore passwords apply to Android only.

## License

MIT
