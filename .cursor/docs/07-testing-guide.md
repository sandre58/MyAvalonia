# Testing Guide

## Test projects

| Source package | Unit tests | Headless tests |
|----------------|------------|----------------|
| MyNet.Avalonia | MyNet.Avalonia.Tests | — |
| Controls | Controls.Tests | Controls.Headless.Tests |
| Extended | Extended.Tests | Extended.Headless.Tests |
| Theme | Theme.Tests | — |
| Geography | Geography.Tests | — |
| Showcase | Showcase.Tests | — |

## Coverage thresholds

From `build/coverage/assembly-thresholds.json`:

- Default: **50%** line rate
- Controls: **60%**
- Extended, Geography, Core: **70%**
- Theme: **50%**

## What to test

### Controls (unit)

- Property default values, coercion, validation callbacks
- Pseudo-class state changes
- Command CanExecute / Execute

### Controls (headless)

- Template applies without exception
- Template parts resolve (`PART_*`)
- Visual state after property change

### Extended

- Service registration resolves
- Dialog/navigation flow with mocked TopLevel

## Commands

```bash
dotnet test MyAvalonia.slnx
dotnet test /p:CollectCoverage=true
powershell build/coverage/Run-SolutionCoverage.ps1   # local full run
```

New public controls require tests — they are framework contracts.
