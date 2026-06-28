# Add Feature Module

Module: [Dialogs / Navigation / Toasting / Theming / Geography]
Package: MyNet.Avalonia.Extended (or other)

## Checklist

1. Interface in MyNet.UI if Avalonia adapter
2. Implementation + `Extensions/ServiceCollectionExtensions.cs`
3. Theme `Generic.axaml` if visual
4. Registration in `AddMyNetAvaloniaExtended`
5. Showcase demo page
6. Extended.Tests or Headless tests

## Constraints

Do not create an abstraction if Avalonia or MyNet.UI already covers the need.

## References

@.cursor/docs/04-extending-library.md
@docs/guides/extended-host.md
