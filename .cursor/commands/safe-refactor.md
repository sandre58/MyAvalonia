# Safe Refactor

Target: @[file or folder]
Goal: [simplify / extract / rename internal / hot-path performance]

## Constraints

- No breaking changes to public NuGet API without explicit approval
- One concern per refactor — minimal diff
- Preserve behavior except when fixing a bug

## Workflow

1. Identify public vs internal surface
2. Targeted refactor
3. `dotnet test MyAvalonia.slnx`
4. Report any API impact (should be "none")

## References

@.cursor/rules/01-csharp-dotnet.mdc
@.cursor/rules/06-testing.mdc
@.cursor/rules/07-nuget-packaging.mdc

## Deliverables

- Summary of changes
- Files touched
- Test result
- API impact
