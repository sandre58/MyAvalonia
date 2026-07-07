// -----------------------------------------------------------------------
// <copyright file="TextPicker.Input.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Input;
using MyNet.Avalonia.Controls.Behaviors;

namespace MyNet.Avalonia.Controls.Primitives;

public abstract partial class TextPicker<T, TPreviewer>
{
    #region Mouse Handlers

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            e.Handled = true;
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (e.InitialPressMouseButton == MouseButton.Left)
        {
            e.Handled = true;

            if (!InputBehavior.GetIsTextEditable(this))
                TogglePopup();
        }
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);
        if (!e.Handled && SelectedValue is not null && AllowSpin && IsKeyboardFocusWithin)
        {
            var newValue = IncrementValue(e.Delta.Y > 0 ? 1 : -1);

            if (newValue is null) return;

            SetCurrentValue(SelectedValueProperty, newValue);

            e.Handled = true;
        }
    }

    #endregion

    #region Focus

    protected override void OnGotFocus(FocusChangedEventArgs e)
    {
        base.OnGotFocus(e);

        if (IsDropDownOpen)
            return;

        if (IsEnabled && InputBehavior.GetIsTextEditable(this) && TextBox is not null && e.NavigationMethod == NavigationMethod.Tab)
        {
            TextBox.Focus();
            var text = TextBox.Text;
            if (!string.IsNullOrEmpty(text))
            {
                TextBox.SelectionStart = 0;
                TextBox.SelectionEnd = text.Length;
            }
        }
    }

    protected override void OnLostFocus(FocusChangedEventArgs e)
    {
        var focused = TopLevel.GetTopLevel(this)?.FocusManager?.GetFocusedElement();

        if (IsKeyboardFocusWithinDropDown(focused))
            return;

        CommitFromTextBox();

        base.OnLostFocus(e);
    }

    #endregion

    #region TextBox

    private void OnTextBoxKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Handled) return;

        OnKeyDown(e);
    }

    private void OnTextBoxTextChanged()
    {
        if (_textBoxTextChangedSuspender.IsSuspended) return;

        using (_textBoxTextChangedSuspender.Suspend())
            SetCurrentValue(TextProperty, TextBox?.Text);
    }

    #endregion
}
