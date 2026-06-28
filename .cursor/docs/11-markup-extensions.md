# Markup Extensions Quick Reference

Extracted from `.cursor/reference/conventions.md`. Use namespace `xmlns:my="http://mynet.com/avalonia"`.

## When to use what

| Need | Use | Avoid |
|------|-----|-------|
| Static `.resx` key | `{my:Loc Key, Style=Abbreviation, Filename=…, Format=…}` | Hard-coded strings |
| Bound formatted value | `{my:Display Path, Style=…, Format=…, Quantity=True}` | `{Binding}` + converter without culture refresh |
| Label on non-bindable property | `{my:LocObject Key}` | Duplicate localizable types |
| Pre-bound TextBlock content | `{my:DisplayTextBlock Path}` | Manual TextBlock + binding |
| Combo/list enum items | `my:ItemsBehavior.EnumSourceType` | Inline enum lists in markup |
| Country pick lists | `{geo:Countries}` (Geography package) | Custom country enum |

## Requirements

- Call `services.UseGlobalization()` before markup resolves culture
- Enum items: `LocalizedEnum` / `LocalizedSmartEnum` from MyNet.Observable
- Resx keys via `TranslationOptions` (`Style`, `Quantity`, …) from MyNet.Globalization
- `Quantity=True` on `{my:Display}` passes bound numeric value for pluralization

## Theme markup (Theme package)

| Extension | Example |
|-----------|---------|
| ThemeContext | `{my:ThemeContext Surface.Level1}` |
| ThemeRole | `{my:ThemeRole Background}` |
| Foreground | `{my:Foreground Opacity=High}` |

Full details: `.cursor/reference/conventions.md` § XAML Markup Extensions
