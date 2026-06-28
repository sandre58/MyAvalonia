# Anti-Patterns

Common mistakes when AI assists on this codebase.

## Framework violations

| Anti-pattern | Why wrong | Do instead |
|--------------|-----------|------------|
| `.axaml` in Controls | Breaks logic/visual split | Theme.Controls/Custom/ |
| Business logic in controls | Not reusable NuGet API | Keep in consumer app |
| ViewModel types in `src/` | Framework independence | Showcase or consumer app |
| Service locator in controls | Untestable, hidden deps | Explicit properties / DI at app level |
| Hardcoded UI strings | Breaks localization | `.resx` + `{my:Loc}` |

## Control design

| Anti-pattern | Do instead |
|--------------|------------|
| God control with many flags | Split variants or compose |
| Deep inheritance | Behaviors + attached properties |
| New control for styling only | ControlTheme override |
| Events with business names | UI events only (`SelectionChanged`) |

## Theming

| Anti-pattern | Do instead |
|--------------|------------|
| Wrong startup order | MyTheme before ThemeControlsCatalog |
| Magic resource key strings | `MyNet.*` prefixed keys |
| Subclassing for visuals | ControlTheme + Assists |

## AI-specific

| Anti-pattern | Do instead |
|--------------|------------|
| WPF/WinUI/MAUI syntax | Avalonia 12 APIs only |
| Inventing MyNet.* APIs | Check `10-mynet-companion.md` |
| Copying Showcase DI into Controls | Controls stay DI-free |
| Default migration path over redesign | Propose best long-term design |
| Using `@docs/TODO.md` | Use `@docs/index.md` or `.cursor/docs/` |

## Architecture review trigger

If unsure whether to keep code: read `.cursor/rules/09-architecture-review.mdc`
