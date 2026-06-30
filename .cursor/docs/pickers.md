# Pickers — AI quick reference

Full guide: [`docs/pickers.md`](../../docs/pickers.md) · Audit: [`picker-interaction-contract.md`](picker-interaction-contract.md)

## Families

1. **TextPicker** — text + previewer (`*PickerEx`)
2. **Scroll+confirm** — `DatePicker`, `TimePicker`, `DateTimeScrollPickerEx`
3. **Multi** — `MultiComboBox`
4. **Menu** — `CulturePicker`

## Key properties

- `AutoCommit` — live commit from previewer
- `CloseOnCommit` — close after **any** `CommitFromPreview` (Enter on TextBox, text parse, auto-commit). Default `false` on Ex.
- `CloseOnSingleSelection` — close after atomic previewer selection (calendar day Enter/click, complete date range). Default `true` on calendar/range pickers.

**Do not confuse:** calendar closes on Enter because `CloseOnSingleSelection`, not `CloseOnCommit`. Time pickers consume Enter locally (next spinner) and **do not close**.

## Enter in popup (cascade)

1. Child previewer handles Enter first (Calendar → select day; TimeSelector → next field).
2. TextBox + popup open → `CommitFromPreview` (no close unless `CloseOnCommit`).
3. Picker closed → Enter opens popup.

Dismiss multi-step previewers: **Esc** or click outside, not Enter.

## Base behavior (`TextPicker`)

- `Rollback()` → restore open value, close, refocus TextBox
- `OnDropDownClosing()` → calls `ShouldRollbackOnClose()` for incomplete ranges
- Tab cycle via `TextPickerPopupFocusHelper`
- New picker: inherit `TextPicker`, call `base.AddPreviewerHandlers()`, override `TryFocusPopupContent`
