# Pickers — interaction guide

Reference for MyNet picker controls: families, properties, keyboard/mouse workflows, and defaults.

See also the internal audit: [`.cursor/docs/picker-interaction-contract.md`](../.cursor/docs/picker-interaction-contract.md).

## Families

| Family | Controls | Input model |
|--------|----------|-------------|
| **TextPicker** | `CalendarDatePickerEx`, `DateTimePickerEx`, `TimePickerEx`, `ColorPickerEx`, `DateRangePickerEx`, `TimeRangePickerEx` | Editable text + rich previewer popup |
| **Scroll + confirm** | `DatePicker`, `TimePicker`, `DateTimeScrollPickerEx` | Segment display + scroll columns + Accept/Dismiss |
| **Multi** | `MultiComboBox` | Immediate commit per item; popup stays open |
| **Menu** | `CulturePicker` | `MenuFlyout` via `DropDownButton` |

## Which picker?

- **Single date (calendar)** → `CalendarDatePickerEx` or `CalendarDatePicker` (standard)
- **Date + time** → `DateTimePickerEx` or `DateTimeScrollPickerEx`
- **Time only** → `TimePickerEx` or `TimePicker` (standard)
- **Date range** → `DateRangePickerEx`
- **Time range** → `TimeRangePickerEx`
- **Color** → `ColorPickerEx` or `ColorPicker` (standard Flyout)
- **Multi-select list** → `MultiComboBox`

## Common properties (`TextPicker` / `DropDownControl`)

| Property | Description |
|----------|-------------|
| `SelectedValue` | Committed value (TwoWay) |
| `IsDropDownOpen` | Popup state (TwoWay) |
| `Text` | Display text in `PART_TextBox` |
| `DisplayFormat` | Format for text conversion |
| `AutoCommit` | Commit previewer changes immediately (default `true`) |
| `CloseOnCommit` | Close popup after **any** commit (Enter, text parse, auto-commit). Default `false` on Ex pickers. |
| `CloseOnSingleSelection` | Close after atomic previewer selection (e.g. calendar day). Default `true` on `CalendarDatePickerEx` and `DateRangePickerEx` (when range complete). |
| `AllowSpin` | Mouse wheel / arrow keys when closed (default `true`) |
| `PlaceholderText` | Shown when empty |

### Commit properties — when to use which

- **`CloseOnSingleSelection`** — calendar day click/Enter, complete date range selection. Popup closes; value already committed via `AutoCommit`. **Not** the same as Enter at the `TextPicker` level.
- **`CloseOnCommit`** — close popup after **any** commit path (Enter on TextBox, text parse, auto-commit). Default `false` on Ex pickers. Prefer `CloseOnSingleSelection` for calendar-like UX. Opt-in for consumers who want Enter-on-TextBox to dismiss.
- **`TimeRangePickerEx`** — `AutoCommit=true` when the range is complete (like `TimePickerEx`); popup stays open until dismiss. Incomplete range on close → rollback.

### `CloseOnCommit` vs `CloseOnSingleSelection` vs Enter (local)

| Mechanism | Who triggers close? | Typical trigger |
|-----------|---------------------|-----------------|
| `CloseOnSingleSelection` | Previewer atomic selection → `OnPreviewValueChanged` | Calendar day click/Enter, complete date range |
| `CloseOnCommit` | `CommitFromPreview` after any commit | Enter on TextBox (if `true`), legacy apps |
| Enter (local) | **Does not close** by itself | Time spinners advance fields; consumed before `TextPicker` |

All Ex pickers default to `CloseOnCommit=false`. Calendar pickers close because `CloseOnSingleSelection=true`, not because Enter reaches `TextPicker.ProcessKey`.

## Enter in the popup (`TextPicker` family)

Enter follows a **priority cascade**. Child controls consume Enter first; the picker acts only when Enter is not handled locally.

### Priority cascade

1. **Inline editor** (e.g. `NumericUpDown` text edit) → Enter validates the local field.
2. **Rich previewer** (focus inside popup):
   - **Calendar** → Enter/Space selects the focused day (`CommitPreview`). If `CloseOnSingleSelection`, popup closes.
   - **TimeSelector** (`TimePickerEx`, `DateTimePickerEx`, `TimeRangePickerEx`) → Enter advances to the next spinner; on the last spinner → `InputCompleted` (internal signal, **does not close** the popup).
   - **ColorView** → Enter handled by focused child control if any.
3. **Picker TextBox** (popup open, Enter not consumed above) → `CommitFromPreview` (sync). Popup stays open unless `CloseOnCommit=true`.
4. **Picker closed** → Enter **opens** the popup.

### Enter behavior by control (popup open)

| Control | Focus typical | Enter action | Popup closes? |
|---------|---------------|--------------|---------------|
| `CalendarDatePickerEx` | Focused day | Select day | **Yes** (`CloseOnSingleSelection`) |
| `DateRangePickerEx` | Focused day | Select day / complete range | **Yes** when range complete |
| `TimePickerEx` | Hour/minute spinner | Next spinner / `InputCompleted` | **No** |
| `DateTimePickerEx` | Calendar day or time spinner | Day select **or** next spinner | Calendar: **yes**; time section: **no** |
| `TimeRangePickerEx` | Start/end spinner | Next spinner / boundary switch | **No** |
| `ColorPickerEx` | Spectrum, sliders, etc. | Child-local | **No** |
| Any Ex picker | `PART_TextBox` | `CommitFromPreview` | **No** (default `CloseOnCommit=false`) |

### Dismiss without Enter

For multi-step previewers (time, datetime, color, time range): **Esc** (rollback + close), **click outside** (light dismiss), or Tab cycle back to TextBox. Enter is **not** the dismiss gesture.

Industry alignment: atomic date selection closes on commit; multi-field time flyouts keep Enter for field navigation (WinUI, Ant Design, etc.).

## Keyboard (`TextPicker` family)

| State | Key | Action |
|-------|-----|--------|
| Closed | `Enter` | Open popup |
| Closed | `Alt+↑` / `Alt+↓` | Open popup |
| Closed | `↑` / `↓` | Increment value (if `SelectedValue` set, `AllowSpin`) |
| Closed | `PgUp` / `PgDn` | Large increment |
| Open | `Enter` | See [Enter in the popup](#enter-in-the-popup-textpicker-family) — local previewer semantics first; TextBox Enter commits without closing (default) |
| Open | `Escape` | Rollback to open value, close, refocus TextBox |
| Open | `Tab` / `Shift+Tab` | Cycle fermé **TextBox ↔ previewer** (dernier focusable du previewer → TextBox). La popup reste ouverte. |

### Tab stops (previewers)

| Contrôle | Entrée depuis TextBox | Tab stops dans le previewer | Fin de cycle → TextBox |
|----------|----------------------|-----------------------------|-------------------------|
| `CalendarDatePickerEx` / `DateRangePickerEx` | Jour sélectionné | `CalendarDayButton` uniquement (pas le `Calendar` racine, pas le header) | Dernier jour visible |
| `TimePickerEx` | Spinner Heure | Heure → Minute → [Secondes] → [AM/PM] | Dernier spinner actif |
| `DateTimePickerEx` | Section calendrier (jour sélectionné) | Jours → **saut** → spinners heure | Dernier spinner heure |
| `TimeRangePickerEx` | Début, heure | Spinners Début → Fin | Dernier spinner Fin |
| `ColorPickerEx` | Contenu couleur actif | Spectrum / palette / contrôles de l'onglet actif | Dernier contrôle |

**Hors Tab** : `ClockSelector` (`Focusable`, `IsTabStop=false`) — commit au clic uniquement.

**DateTimePickerEx** : `F6` ou `Ctrl+←/→` bascule explicitement calendrier ↔ heure.

## Mouse (`TextPicker` family)

- **Trigger button** (`PART_Button` / `ToggleButton`): toggle popup
- **Non-editable zone click**: toggle popup
- **Light dismiss**: click outside closes; incomplete range → rollback (`DateRangePickerEx`, `TimeRangePickerEx`)
- **Wheel** (focus within, `AllowSpin`): increment value when closed

## Focus

- **Tab into control**: focuses `PART_TextBox`, selects all text (if editable)
- **Popup open**: focus initial via `TryFocusPopupContent()` (jour sélectionné, heure, section calendrier, etc.)
- **Popup Tab trap**: cycle fermé TextBox ↔ previewer — pas de sortie vers la page tant que la popup est ouverte
- **Calendar**: le conteneur `Calendar` n'est **pas** un tab stop ; seuls les `CalendarDayButton` le sont
- **Popup close**: refocus host; `Rollback` refocuses TextBox

## Defaults matrix (Ex pickers)

| Control | AutoCommit | CloseOnCommit | CloseOnSingleSelection |
|---------|------------|---------------|------------------------|
| `CalendarDatePickerEx` | true | false | **true** |
| `DateTimePickerEx` | true | false | false |
| `TimePickerEx` | true | false | false |
| `ColorPickerEx` | true | false | false |
| `DateRangePickerEx` | true | false | **true** (complete range) |
| `TimeRangePickerEx` | true | false | false |

## Scroll + confirm family

| Key | Action |
|-----|--------|
| `Enter` | Confirm selection |
| `Escape` | Dismiss (no commit) |

`DateTimeScrollPickerEx` implements `IPopupControl` (`OpenPopup`, `ClosePopup`, `DropDownOpened` / `DropDownClosed`).

## Exceptions

- **`MultiComboBox`**: immediate commit; `F4` / `Enter` opens; popup stays open for multi-toggle. Optional popup search via `IsSearchEnabled` / `ItemsSearchBehavior.IsEnabled` on `ComboBox`.
- **`CulturePicker`**: menu flyout; selection via command — not a `TextPicker`.
- **Avalonia standard pickers** (`DatePicker`, `TimePicker`, `CalendarDatePicker`, `ColorPicker`): logic in Avalonia; MyNet provides themes only (`PopupBehavior`, Accept/Dismiss).

## Popup item search (`ItemsSearchBehavior`)

Optional in-popup filtering for `ComboBox` (MyNet theme) and `MultiComboBox`:

| API | Package | Role |
|-----|---------|------|
| `ItemsSearchBehavior.*` | Controls | Enable search, `Text`, `FilterMode`, `MinimumLength`, `FilterDelay`, `ClearOnClose`, `SearchMemberPath` |
| `ItemsSearchAssist.*` | Theme.Controls | `PlaceholderText`, `TextBoxTheme` (ComboBox default theme) |
| `MultiComboBox.IsSearchEnabled` / `SearchText` / `SearchMemberPath` / … | Controls | Styled aliases for discoverability |

**TextBox popup themes** (compare via `ItemsSearchAssist.TextBoxTheme` on `ComboBox`, or `MultiComboBox.SearchTextBoxTheme`):

| Theme key                                            | Style |
|------------------------------------------------------|--------|
| `MyNet.Theme.TextBox.Embedded.Popup.Search`          | Borderless Clean variant for embedded popup |
| `MyNet.Theme.TextBox.Embedded.Popup.Search.Outlined` | Standard bordered TextBox + search icon (default) |

**Search with custom item templates**: when items use `{my:Display}` or localized data templates (e.g. `Country`), omit `SearchMemberPath` — filtering uses the same display text as the UI (`DisplayTextResolver`). Set `SearchMemberPath` only for non-display properties (e.g. `Alpha2` on `Country`, POCO fields). Falls back to `DisplayMemberBinding`, then registered display types, then `ToString()`.

**Large local lists**: set `FilterDelay` (default `150` ms) to debounce in-popup filtering. Use `MinimumLength` (≥ 2 recommended) to skip filtering until enough characters are typed. Set `FilterDelay="0"` for immediate filtering. Filtering keeps the host `ItemsPanel` (typically `VirtualizingStackPanel`) — only visible containers are materialized.

**Large static catalogs** (e.g. `MaterialIconCatalog.Groups`, 7000+ items): set `SearchMemberPath="DisplayName"` on `MaterialIconKindGroup`. For browse-all UX with pagination, prefer a ViewModel-filtered list (see showcase `IconsPage`) over in-popup search alone.

Keyboard when search is enabled:

| Key | Action |
|-----|--------|
| Popup open | Initial focus on `PART_SearchBox` |
| `Enter` (search, 1 match) | Select item; `ComboBox` closes popup |
| `Enter` (search, N matches) | Focus first visible item |
| `Enter` (search, 0 matches) | No-op |
| `↓` from search | Focus first visible item |
| `↑` from first visible item | Return to search |
| `Ctrl+F` from item | Focus search and select all search text |
| `Esc` (filter non-empty) | Clear filter, keep popup open |
| `Esc` (filter empty) | Close popup |
| No matches | Empty message shown; item list hidden |
| `MultiComboBox` Select All | Selects **visible/filtered** items only |

Remote/async search: bind `Text` with `Delay` and refresh `ItemsSource` from the ViewModel — no built-in loader in v1.

## Creating a new Ex picker

1. Inherit `TextPicker<T, TPreviewer>` in `MyNet.Avalonia.Controls`
2. Override `SetPreviewValue` / `GetPreviewValue` / convert & increment methods
3. Override `AddPreviewerHandlers` — call `base` first
4. Override `TryFocusPopupContent` for meaningful initial focus
5. Set `CloseOnSingleSelection` / `AutoCommit` defaults in static constructor if needed
6. Override `ShouldRollbackOnClose` for incomplete preview on close
7. Add `Theme.Controls/Custom/{Name}.axaml` with `ToggleButton` `PART_Button`, `PART_Popup` TwoWay `IsDropDownOpen`
8. Headless tests for open/close/commit/rollback
