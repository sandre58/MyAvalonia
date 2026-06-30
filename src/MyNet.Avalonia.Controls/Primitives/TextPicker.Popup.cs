// -----------------------------------------------------------------------
// <copyright file="TextPicker.Popup.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Input;
using MyNet.Avalonia.Controls.Primitives.Internal;

namespace MyNet.Avalonia.Controls.Primitives;

public abstract partial class TextPicker<T, TPreviewer>
{
    /// <summary>
    /// Called when the drop-down is closing. Override to perform rollback when the preview is incomplete.
    /// </summary>
    protected virtual void OnDropDownClosing()
    {
        if (ShouldRollbackOnClose())
            Rollback();
    }

    /// <summary>
    /// When <see langword="true"/>, <see cref="OnDropDownClosing"/> rolls back to the value captured at open.
    /// </summary>
    protected virtual bool ShouldRollbackOnClose() => false;

    /// <summary>
    /// When <see langword="true"/>, the popup closes after an atomic previewer selection (see <see cref="CloseOnSingleSelection"/>).
    /// </summary>
    protected virtual bool ShouldCloseAfterSingleSelection() =>
        CloseOnSingleSelection && IsDropDownOpen;

    protected void CloseAfterSingleSelection()
    {
        ClosePopup();
        Focus();
    }

    protected virtual void FocusPreviewerOnTabFromTextBox(Control previewer) =>
        TextPickerPopupFocusHelper.FocusFirst(previewer);

    protected virtual void OnPreviewerKeyDown(object? sender, KeyEventArgs e)
    {
        if (!IsDropDownOpen || Previewer is not { } previewer)
            return;

        if (TextPickerPopupFocusHelper.TryHandlePreviewerTab(previewer, TextBox, e))
            e.Handled = true;
    }
}
