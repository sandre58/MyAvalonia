# NuGet Pre-Publish Review

Package: [MyNet.Avalonia.*]
Version: [current GitVersion]

## Checks

- [ ] Public API has XML documentation
- [ ] No dependency on `demos/` or `tests/`
- [ ] README.md packed
- [ ] Breaking changes listed
- [ ] `dotnet test` + coverage >= assembly threshold
- [ ] `dotnet pack` without warnings

## References

@.cursor/docs/08-nuget-checklist.md
@.cursor/rules/07-nuget-packaging.mdc

## Expected report

GO / NO-GO + blocking items
