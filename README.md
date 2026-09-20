# unity-packages
A collection of reusable Unity ScriptableObject Architecture (SOAP) packages, and a clonable template project that wires them into a working game scaffold. See `IDEA.md` for the intended role of `Packages/` vs. `Assets/`.

## Quick start

**Prerequisites:** Unity **6000.3.24f1** (see `ProjectSettings/ProjectVersion.txt`) and Git.

```bash
git clone https://github.com/madratzz/unity-packages.git
cd unity-packages
git checkout development
```

Open the folder in Unity Hub — all packages under `Packages/` are embedded and resolve automatically, no registry setup required. See [`Docs/Getting Started.md`](Docs/Getting%20Started.md) for the full walkthrough, including using a package in another project and running tests.

## Running tests

**Window → General → Test Runner → EditMode → Run All.** Most packages under `Packages/` ship EditMode tests (see [`Docs/Getting Started.md`](Docs/Getting%20Started.md) for which don't yet); the `Assets/` GameFlow layer has its own at `Assets/Tests/EditMode/`.

## Documentation

See [`Docs/Home.md`](Docs/Home.md) for the full wiki — architecture overview, a getting-started guide, and a per-package catalog. It's written as an [Obsidian](https://obsidian.md) vault; open this repository's root folder in Obsidian to browse it with working links.

For AI-agent-specific guidance, see [`AGENTS.md`](AGENTS.md).

## License

[MIT](LICENSE) — Copyright (c) 2026 Raza Butt. Each package under `Packages/` also carries its own `LICENSE.md` (same terms) since packages are meant to be distributed independently.
