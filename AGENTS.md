# MyAvalonia — Agent Entry Point

This repository is a **reusable Avalonia UI framework** published as NuGet packages — not an application.

## Read first

| Purpose | Path |
|---------|------|
| Deep architecture principles | `.cursor/reference/architecture.md` |
| Coding conventions + markup extensions | `.cursor/reference/conventions.md` |
| Source generator guidelines | `.cursor/reference/source-generators.md` |
| Quick AI guides (start here) | `.cursor/docs/README.md` |
| Human documentation | `docs/index.md` |

## Stack

- .NET 10, Avalonia 12, C# preview
- MyNet companion packages (pinned in `Directory.Packages.props`)
- Central Package Management — never pin versions in individual `.csproj`

## Package boundaries (non-negotiable)

| Package | Role |
|---------|------|
| `MyNet.Avalonia` | Core: commands, converters, markup, localization hooks |
| `MyNet.Avalonia.Controls` | **C# logic only — zero `.axaml`** |
| `MyNet.Avalonia.Theme` | Design tokens, `MyTheme`, CSS utility classes |
| `MyNet.Avalonia.Theme.Controls` | **ControlTheme AXAML only** |
| `MyNet.Avalonia.Extended` | Avalonia adapters for MyNet.UI (dialogs, nav, toast) |
| `MyNet.Avalonia.Geography` | Geography localization for Avalonia |
| `demos/MyNet.Avalonia.Showcase` | Reference MVVM demo — patterns here do **not** belong in `src/` |

## Build commands

```bash
dotnet build MyAvalonia.slnx
dotnet test MyAvalonia.slnx
dotnet pack src/MyNet.Avalonia/MyNet.Avalonia.csproj -c Release
```

Coverage gate (CI): `build/coverage/Verify-CriticalCoverage.ps1`

## Before any change

1. Identify the target package.
2. Ask: *"Would hundreds of NuGet consumers accept this API?"*
3. Anchor prompts to an existing similar control (`Card`, `Banner`, etc.).

## Cursor environment

| Folder | Purpose |
|--------|---------|
| `.cursor/rules/` | Scoped agent rules (auto-applied by file glob) |
| `.cursor/docs/` | Short actionable guides for AI |
| `.cursor/reference/` | Deep reference docs |
| `.cursor/commands/` | Slash commands — type `/new-control` in chat |

**IDE:** files are linked in the solution via `solution-files/MyAvalonia.Cursor.files/` under **Solution Files → Cursor**.

## Never in `src/` libraries

- Domain/business logic, persistence, ViewModel types
- Hardcoded user-facing strings
- Service locator in controls
- `.axaml` files in `MyNet.Avalonia.Controls`
