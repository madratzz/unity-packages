# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).
The initial release collects all pre-release changes into a single entry; versions will increment once the package is first published to the Verdaccio registry.

## [0.0.1] - Unreleased

### Added
- Initial release
- `PlayFromFirstScene` — editor utility that overrides Play Mode entry to always load scene index 0

### Changed
- Removed the brute-force deactivate-all-GameObjects pass before loading scene 0 — `LoadScene(0, LoadSceneMode.Single)` already replaces the active scene

### Fixed
- Editor asmdef now declares `includePlatforms: ["Editor"]` — the assembly previously targeted all platforms, leaking editor-only code into player builds
- Corrected the editor asmdef typo and aligned the assembly name/filename with the `com.madratzz.utilities.*` utilities-family convention
- README menu path corrected (`EditorUtilities/Always Start From Scene 0`, not `Tools/Play From First Scene`)
