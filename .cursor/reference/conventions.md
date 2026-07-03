# MyNet.Avalonia Coding Conventions

# General Principles

All components should prioritize:

- clarity
- consistency
- reusability
- MVVM compatibility
- discoverability
- maintainability

Controls are public framework components and must be designed as reusable building blocks.

Avoid application-specific assumptions.

---

# Avalonia First

Use native Avalonia concepts whenever possible.

Prefer:

- StyledProperty
- DirectProperty
- AttachedProperty
- ControlTheme
- DataTemplate
- CompiledBinding

Avoid introducing abstractions that hide Avalonia behavior.

Consumers should feel they are using Avalonia, not a proprietary framework.

---

# MVVM Compatibility

Controls must remain MVVM-friendly.

Controls should:

- expose bindable properties
- expose commands when appropriate
- avoid business logic
- avoid ViewModel assumptions

Never require code-behind to use a control.

---

# Control Design

Controls should solve a single UI problem.

Prefer:

- focused controls
- composable controls
- reusable controls

Avoid:

- god controls
- multi-purpose controls
- excessive configuration

---

# Property Design

## StyledProperty

Use StyledProperty by default.

Preferred for:

- bindings
- styling
- templates
- customization

Example:

```csharp
public static readonly StyledProperty<string?> TitleProperty = AvaloniaProperty.Register<MyControl, string?>(nameof(Title));
```

---

## DirectProperty

Use DirectProperty only when:

- a backing field is required
- performance is critical
- styling is not required

Avoid DirectProperty by default.

---

## Attached Properties

Attached properties should:

- solve cross-cutting concerns
- remain discoverable
- avoid hidden side effects

Do not use attached properties as service locators.

---

# Property Naming

Property names should be:

- explicit
- descriptive
- UI-oriented

Avoid abbreviations.

Preferred:

```csharp
SelectedItem
Watermark
Header
DisplayMemberPath
```

Avoid:

```csharp
SelItem
Txt
Hdr
```

---

# Control Templates

Controls should support templating.

Avoid hardcoding visual structures.

Use:

- TemplateParts
- ControlTheme
- TemplateBindings

Whenever customization is expected.

---

# Template Parts

Required template parts should:

- be documented
- use TemplatePart attributes
- have predictable names

Preferred:

```csharp
PART_ContentPresenter
PART_SearchBox
PART_ItemsHost
```

---

# Styling

Every control should be stylable.

Controls should:

- expose theme resources
- expose styling hooks
- support application-level customization

Avoid requiring inheritance for visual customization.

---

# Commands

Prefer ICommand for user actions.

Examples:

```csharp
SaveCommand
OpenCommand
SelectCommand
```

Avoid event-only APIs when command support is appropriate.

---

# Events

Events should represent UI interactions.

Events should:

- use EventArgs types
- remain predictable
- avoid business semantics

Preferred:

```csharp
SelectionChanged
ItemClicked
DialogOpened
```

Avoid:

```csharp
CustomerSaved
InvoiceValidated
```

---

# Behaviors

Behaviors should:

- solve a single concern
- remain reusable
- avoid hidden dependencies

Prefer multiple small behaviors over large behaviors.

---

# Visual Tree Usage

Avoid unnecessary visual tree traversal.

Avoid:

```csharp
GetVisualDescendants()
```

inside frequently executed code.

Cache lookups when possible.

---

# Binding Conventions

Prefer compiled bindings whenever possible.

Avoid reflection-based bindings when a compiled alternative exists.

Bindings should be:

- explicit
- maintainable
- easy to diagnose

---

# Control Lifecycle

Use Avalonia lifecycle methods appropriately.

Examples:

```csharp
OnApplyTemplate
OnAttachedToVisualTree
OnDetachedFromVisualTree
```

Avoid initialization in constructors when template access is required.

---

# Memory Management

Controls must clean up subscriptions.

Always unsubscribe from:

- events
- observables
- reactive subscriptions

Prefer disposable patterns when appropriate.

Avoid memory leaks.

---

# Localization

Controls should support localization.

Avoid:

- hardcoded strings
- culture-dependent assumptions

Text displayed by controls should be localizable.

---

# Validation

Validation should integrate naturally with Avalonia.

Support:

- IDataErrorInfo
- INotifyDataErrorInfo

Avoid custom validation systems unless necessary.

---

# Dependency Injection

Controls should not directly resolve services.

Avoid:

```csharp
ServiceLocator.Current
```

Prefer:

- explicit services
- dialog services
- injected infrastructure

when appropriate.

---

# Performance

Avoid:

- repeated allocations
- repeated visual tree scans
- unnecessary boxing
- unnecessary reflection

Optimize:

- item controls
- virtualization scenarios
- frequently executed paths

---

# Public APIs

Public APIs should be:

- strongly typed
- discoverable
- self-documenting

Avoid:

- magic strings
- ambiguous flags
- hidden behaviors

Preferred:

```csharp
SelectionMode.Single
```

Avoid:

```csharp
SelectionMode = 1
```

---

# Documentation

All public controls must provide:

- XML documentation
- usage examples when appropriate

Document:

- required template parts
- expected behaviors
- important limitations

---

# Testing

Controls should have tests for:

- property behavior
- commands
- events
- templates
- interactions

Public controls are considered framework contracts.

---

# Code Style

Prefer:

- file-scoped namespaces
- explicit access modifiers
- sealed controls when inheritance is not intended
- composition over inheritance

Avoid unnecessary abstraction layers.

---

# XAML Markup Extensions (MyNet.Avalonia)

Use the `my` XML namespace for globalization-aware UI text and enum lists.

| Need | Use | Avoid |
|------|-----|-------|
| Static `.resx` key | `{my:Loc Key, Style=Abbreviation, Filename=…, Format=…}` | Hard-coded strings |
| Bound value (date, enum, number, object) | `{my:Display Path, Style=…, Format=…, Quantity=True}` | `{Binding}` + converter without culture refresh |
| Label on non-bindable property | `{my:LocObject Key}` | Duplicate `LocalizableString` types |
| Content = pre-bound `TextBlock` | `{my:DisplayTextBlock Path}` | Manual `TextBlock` + binding |
| Combo/list enum items | `my:ItemsBehavior.EnumSourceType` | Inline enum markup lists |
| Country pick lists | `{geo:Countries}` via `MyNet.Avalonia.Geography` | Custom country enumeration |

Requirements:

- Call `services.UseGlobalization()` in the host before markup extensions resolve culture.
- Enum items use `LocalizedEnum` / `LocalizedSmartEnum` from MyNet.Observable (culture updates via item wrappers, not snapshot lists).
- Resx and format keys resolve through `TranslationOptions` (`Style`, `Quantity`, …) from MyNet.Globalization.
- `LetterCasing` remains an Avalonia post-processing step on the final string.
- `Quantity=True` on `{my:Display}` passes the bound numeric value as pluralization quantity for the `Format` key.

---

# Helper placement

Internal helpers and static utilities follow a predictable layout.

**Namespace rule:** colocate under `MyNet.Avalonia.Controls.Internals.{Domain}` when a public control type shares the domain name (e.g. `Calendar`, `Rating`, `Pagination` — C# forbids `MyNet.Avalonia.Controls.Calendar.Internal` alongside `class Calendar`). Use `MyNet.Avalonia.Controls.{Domain}.Internal` only when no name collision exists (e.g. `DateTimePickers`, `Dialogs.Overlay`).

| Location | When to use | Visibility |
|---|---|---|
| `{Domain}/Internal/` | Logic specific to one control family (Calendar, Rating, Pagination…) | `internal` |
| `Primitives/Internal/` | Shared TextPicker infrastructure (date/time/color pickers) | `internal` |
| `Extensions/` | Public extensions on Avalonia or MyNet types | `public` |
| `Icons/` | Public catalog APIs (e.g. `MaterialIconCatalog`) | `public` |
| **`MyNet.*` (external repo)** | Pure .NET logic with no Avalonia dependency | `public` in the relevant MyNet package |

**Keep in Controls:** Avalonia-specific logic (focus, popup, calendar cell state, eyedropper bitmap).

**Prefer MyNet over local duplication:** Before adding a helper, check `MyNet.Primitives`, `MyNet.Globalization`, and `MyNet.Collections` for existing APIs (`SafeClamp`, `DiscardTime`, `DateTime.Range`, `.Translate()`, etc.).

**Do not push UI state into MyNet:** Keep partial picker state (`TryBuild` with `IsValid`), edited-boundary enums, and Avalonia-specific result types in Controls. MyNet returns domain values (`Period`, tuples, nullable dates); Controls compose incomplete UI state (e.g. `TimeRangeBuildResult`, `TimeRangeBoundary`).

**Do not merge blindly:** Keep separate helpers when a file hides substantial private logic or improves parent control readability (`CalendarKeyboardNavigationHelper`, `TextPickerValidationHelper`).

**Obsolete public APIs:** When relocating public helpers (e.g. `IconsHelper` → `MaterialIconCatalog`), keep a one-version `[Obsolete]` shim in the old namespace.

---

# Architectural Goal

Every control should feel:

- native to Avalonia
- easy to understand
- easy to style
- easy to bind
- easy to test
- production ready