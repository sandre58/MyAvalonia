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

        var handled = ProcessKey(e);

        base.OnKeyDown(e);

        e.Handled = handled;
    }

    protected virtual bool ProcessKey(KeyEventArgs e)
    {
        var result = TextPickerKeyboardHelper.Resolve(
            e.Key,
            IsDropDownOpen,
            SelectedValue is not null,
            e.KeyModifiers);

        switch (result.Action)
        {
            case TextPickerKeyAction.CommitPreview:
                CommitFromPreview();
                return true;

            case TextPickerKeyAction.Rollback:
                Rollback();
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
