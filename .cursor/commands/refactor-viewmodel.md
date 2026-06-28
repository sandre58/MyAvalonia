# Refactor ViewModel (Showcase)

ViewModel: @[path/ViewModel.cs]
Goal: [simplify / extract / fix binding]

## Showcase constraints

- Inherit `PageViewModel` if navigable page
- Compiled bindings (`x:DataType`)
- `[UpdateOnCultureChanged]` on localized properties
- Commands via injected `ICommandFactory`
- Do not move business logic into `src/`

## Deliverables

- Refactored ViewModel
- Updated AXAML
- Test if logic is non-trivial

## References

@.cursor/docs/05-showcase-structure.md
@.cursor/docs/06-mvvm-patterns.md
