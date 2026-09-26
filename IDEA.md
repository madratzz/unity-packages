# Project Idea

`unity-packages` is a collection of personal Unity game-development packages, and — via its `Assets/` integration layer — a clonable Unity project template built on top of them.

## Intended Role

- **Packages** (`Packages/com.madratzz.*`): reusable tools, foundational systems, and ScriptableObject Architecture (SOAP) building blocks — variables, events, state machine, utilities. Each is a standalone, independently distributable Unity package with its own dependency boundary.
- **Template** (`Assets/`): a GameFlow-driven project scaffold — boot/game scenes, prefabs, and wired ScriptableObject assets — that demonstrates the package family working together end-to-end. Cloning this repo gives a working SOAP-based starting point, not just headless installable code.

## Distribution

- Packages are intended to be hosted through a Verdaccio registry, installable independently in other projects.
- The repository itself also works directly as a Unity project template: clone it, open it, and the `Assets/` layer is a working starting point already wired to the packages.

## Direction

Build package boundaries, public APIs, versioning, and publishing conventions deliberately as each package is introduced. Keep the `Assets/` template layer thin — it wires packages together (ScriptableObject references, scenes, prefabs) and demonstrates them; it does not reimplement functionality that belongs in a package. Keep high-level intent here; record current implementation decisions and active work in `.agents/`.
