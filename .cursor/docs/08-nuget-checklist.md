# NuGet Pre-Publish Checklist

Run before `dotnet pack` or creating a release tag.

## API surface

- [ ] All public types/members have XML documentation
- [ ] No accidental `public` on internal helpers
- [ ] Breaking changes documented in CHANGELOG
- [ ] README.md updated for affected package

## Dependencies

- [ ] No reference to `demos/` or `tests/` from `src/`
- [ ] New packages added to `Directory.Packages.props` (not inline in csproj)
- [ ] Avalonia version matches repo pin (12.x)

## Build & test

- [ ] `dotnet build MyAvalonia.slnx` — zero warnings
- [ ] `dotnet test MyAvalonia.slnx` — all pass
- [ ] Coverage ≥ assembly threshold (see `07-testing-guide.md`)

## Pack

- [ ] `dotnet pack src/{Package}/{Package}.csproj -c Release`
- [ ] Output in `packages/` with `.snupkg` symbols
- [ ] Package README renders correctly (transformed from project README)

## Versioning

- GitVersion from Conventional Commits
- `main` → alpha; feature branches → beta

## Report format

```
Package: MyNet.Avalonia.*
Version: {GitVersion}
Result: GO | NO-GO
Blocking items: [...]
Warnings: [...]
```

Use command: `/nuget-review`
