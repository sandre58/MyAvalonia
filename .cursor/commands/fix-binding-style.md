# Fix Binding / Style Issue

Symptom: [visual description or binding error]
Files: @[Page.axaml] @[ControlTheme.axaml]

## Diagnostic order

1. Is `x:DataType` present and correct?
2. Does `StyledProperty` exist on the control?
3. Is ControlTheme `x:Key="{x:Type ...}"` applied?
4. Is startup order `MyTheme` then `ThemeControlsCatalog`?
5. Is pseudo-class / CssClass selector correct?
6. Do template part `PART_*` names match?

## Constraints

Minimal fix — no out-of-scope refactoring.

## References

@.cursor/docs/03-theming-guide.md
