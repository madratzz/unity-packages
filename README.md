# unity-so-starter-template

A Unity **6000.3.21f1** starter template: Universal Render Pipeline with mobile and
PC render-pipeline assets, the Input System, and a sample scene — a clean project
skeleton to start a new Unity project from.

## Quick start

This repository is a GitHub [template](https://docs.github.com/en/repositories/creating-and-managing-repositories/creating-a-repository-from-a-template),
so a new project starts as its own repository with its own history rather than a fork:

1. On GitHub, click **Use this template → Create a new repository**.
2. Clone your new repository and open it in Unity.

**Prerequisites**

- Unity **6000.3.21f1** (this exact version; see `ProjectSettings/ProjectVersion.txt`)
- **`git` on your `PATH`** if you add git-URL package dependencies — Unity's package
  manager shells out to `git`, and without it UPM resolve fails on first open.

## What's in the box

| Path | Contents |
|---|---|
| `Assets/Scenes/SampleScene.unity` | Empty sample scene |
| `Assets/Settings/` | URP assets — `Mobile_RPAsset` / `PC_RPAsset` (with their renderers), `DefaultVolumeProfile`, `SampleSceneProfile`, global URP settings |
| `Assets/InputSystem_Actions.inputactions` | Input System action map |
| `Packages/manifest.json` | URP 17.3.0, Input System 1.20.0, Test Framework 1.6.0, Timeline, uGUI, Visual Scripting, AI Navigation |
| `ProjectSettings/` | Unity 6000.3.21f1 project settings |

The project ships no game code — add your own under `Assets/`.
