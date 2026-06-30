# Picker interaction contract (audit + target)

Internal reference for picker workflow uniformization. Public guide: [`docs/pickers.md`](../../docs/pickers.md).

## Audit matrix (12 controls × 8 axes)

| Control | Open | Close | Commit | Rollback | Focus open | Tab | Keys closed | Keys open |
|---------|------|-------|--------|----------|------------|-----|-------------|-----------|
| **CalendarDatePickerEx** | Btn, zone*, Enter, Alt+↑↓ | Esc, light-dismiss, `CloseOnSingleSelection` | Auto on day | Esc → old value | Selected day | Cycle TextBox↔jours | ↑↓ day | Enter day → select+close; Esc |
| **DateTimePickerEx** | idem | Esc, light-dismiss (stays open on time commit) | Auto live | Esc + close + TextBox | Calendar section | Cycle TextBox↔jours↔heure (saut section) | ↑↓ minute | Enter: day→close OR spinner→next; Esc; F6 |
| **TimePickerEx** | idem | Esc, light-dismiss | Auto live | Esc (base unified) | Heure | Cycle TextBox↔spinners | ↑↓ minute | Enter spinner→next (no close); TextBox Enter→commit; Esc |
| **ColorPickerEx** | idem | Esc, light-dismiss | Auto live | Esc (base unified) | Contenu couleur | Cycle TextBox↔contrôles | ↑↓ hue | Enter child-local; TextBox Enter→commit; Esc |
| **DateRangePickerEx** | idem | Esc; incomplete close → rollback; complete → `CloseOnSingleSelection` | Auto when range complete | Esc; incomplete close | Selected day | Cycle TextBox↔jours | ↑↓ day range | Enter day→select; 2nd day→close; Esc |
| **TimeRangePickerEx** | idem | Esc; incomplete close → rollback | Auto when range complete | Esc; incomplete close | Début, heure | Cycle TextBox↔Début↔Fin | ↑↓ range | Enter spinner→next/boundary; no close; Esc |
| **DateTimeScrollPickerEx** | Flyout btn | Dismiss, light-dismiss | On Accept/Enter only | Dismiss = no commit | Left column | Presenter cyclic | — | Enter confirm, Esc dismiss |
| **DatePicker** (std) | Flyout btn, PopupBehavior keys | Accept/Dismiss | On Accept | Dismiss | AutoFocusOnOpening | Presenter | PopupBehavior | Enter/Esc |
| **TimePicker** (std) | idem | idem | On Accept | Dismiss | idem | idem | idem | idem |
| **CalendarDatePicker** (std) | Toggle btn | IsDropDownOpen | On day select | — | Calendar | Once | — | — |
| **ColorPicker** (std) | DropDownButton Flyout | Flyout native | On pick | — | ColorView | Menu | — | — |
| **MultiComboBox** | Click, Enter, ↑↓, F4 | Esc, Tab, F4 | Immediate per toggle | N/A | Selected item | Once | ↑↓ open | Space/Enter toggle |
| **CulturePicker** | DropDownButton | MenuFlyout | Command per item | N/A | Menu | Menu native | — | — |

\*Zone click toggles popup when text is not editable (`InputBehavior.IsTextEditable`).

## Enter priority cascade (TextPicker)

1. **Child previewer** consumes Enter (Calendar day select, TimeSelector next field, NumericUpDown inline edit).
2. **`TextPicker.ProcessKey`** only if Enter reaches the picker host (typically **TextBox** focus): `CommitFromPreview`.
3. **Close policy** is separate from Enter:
   - `CloseOnSingleSelection` — atomic previewer selection (calendar day, complete date range).
   - `CloseOnCommit` — any `CommitFromPreview` path (opt-in; default `false` on Ex).
4. **Picker closed** — Enter opens popup.

Calendar pickers appear to “close on Enter” because Enter selects a day → `CloseOnSingleSelection`, **not** because `CloseOnCommit=true`.

## Target contract by category

### Category 1 — Simple value (date, time, datetime, color)

- `AutoCommit=true`, `CloseOnCommit=false`, `CloseOnSingleSelection=false` (except calendar day)
- `CalendarDatePickerEx`: `CloseOnSingleSelection=true`
- Escape: rollback + close + refocus TextBox
- Enter (popup, time/color): local navigation — **do not close**

### Category 2 — Range (date, time)

- `DateRangePickerEx`: auto-commit when range complete; `CloseOnSingleSelection=true` when preview complete
- `TimeRangePickerEx`: auto-commit when range complete; popup stays open until dismiss (like `TimePickerEx`); rollback on incomplete close
- `OnDropDownClosing` / `ShouldRollbackOnClose` in base `TextPicker`

### Category 3 — Scroll + confirm

- Live preview in presenter; commit on Accept / Enter
- Theme: `PopupBehavior.EnableShortcutKeys`, `AutoFocusOnOpening`

### Category 4 — Multi / menu (documented exceptions)

- `MultiComboBox`, `CulturePicker` — not aligned to `TextPicker`; see public guide

## Property semantics

| Property | Meaning |
|----------|---------|
| `AutoCommit` | Commit previewer changes immediately |
| `CloseOnCommit` | Close popup after any commit (`CommitFromPreview`: Enter on TextBox, text parse, auto-commit) |
| `CloseOnSingleSelection` | Close after atomic previewer selection (day click/Enter, complete date range) — **independent of `CloseOnCommit`** |

Legacy: `CloseOnCommit=true` without `CloseOnSingleSelection` preserves pre-unification behavior.

Future (not implemented): `CloseOnInputComplete` could close on `InputCompleted` + `EnterKey` on last time spinner — opt-in only.
