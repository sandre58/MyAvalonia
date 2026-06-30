// -----------------------------------------------------------------------
// <copyright file="TextPicker.Keyboard.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Input;
using MyNet.Avalonia.Controls.Primitives.Internal;

namespace MyNet.Avalonia.Controls.Primitives;

public abstract partial class TextPicker<T, TPreviewer>
{
    #region Keyboard handlers

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Handled) return;

        if (ProcessKey(e))
        {
            e.Handled = true;
            return;
        }

        base.OnKeyDown(e);
    }

    protected virtual bool ProcessKey(KeyEventArgs e)
    {
        if (IsDropDownOpen && Previewer is { } previewer && TextBox is { } textBox)
        {
            if (ReferenceEquals(e.Source, textBox)
                && TextPickerPopupFocusHelper.TryHandleTextBoxTab(previewer, textBox, e, FocusPreviewerOnTabFromTextBox))
            {
                return true;
            }

            if (TextPickerPopupFocusHelper.TryHandlePreviewerTab(previewer, TextBox, e))
                return true;
        }

        var result = TextPickerKeyboardHelper.Resolve(
            e.Key,
            IsDropDownOpen,
            SelectedValue is not null,
            e.KeyModifiers);

        switch (result.Action)
        {
            case TextPickerKeyAction.CommitPreview:
                CommitFromPreview();
                if (ShouldCloseAfterSingleSelection())
                    CloseAfterSingleSelection();
                return true;

            case TextPickerKeyAction.Rollback:
                Rollback();
                ClosePopup();
                return true;

            case TextPickerKeyAction.IncrementByOffset:
                SetCurrentValue(SelectedValueProperty, IncrementValue(result.Offset));
                return true;

            case TextPickerKeyAction.IncrementLargeByOffset:
                SetCurrentValue(SelectedValueProperty, IncrementLargeValue(result.Offset));
                return true;

            default:
                return false;
        }
    }

    #endregion
}
