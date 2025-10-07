// -----------------------------------------------------------------------
// <copyright file="ColorPickerProxy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.Controls.Proxy;

public sealed class ColorPickerProxy : IControlProxy
{
    private readonly ColorPicker _control;

    public bool IsEmpty() => string.IsNullOrEmpty(_control.Text);

    public bool IsFocused() => _control.IsKeyboardFocusWithin || _control.IsDropDownOpen;

    public bool IsActive() => !IsEmpty() || IsFocused();

    public event EventHandler? IsEmptyChanged;

    public event EventHandler? IsFocusedChanged;

    public event EventHandler? IsActiveChanged;

    public ColorPickerProxy(ColorPicker control)
    {
        _control = control ?? throw new ArgumentNullException(nameof(control));

        _control.TextChanged += OnTextChanged;
        _control.ColorChanged += OnColorChanged;
        _control.ColorViewOpened += OnCalendarOpenedOrClosed;
        _control.ColorViewClosed += OnCalendarOpenedOrClosed;
        _control.GotFocus += OnGotFocus;
        _control.LostFocus += OnLostFocus;
    }

    private void OnCalendarOpenedOrClosed(object? sender, EventArgs e)
    {
        IsFocusedChanged?.Invoke(sender, EventArgs.Empty);
        IsActiveChanged?.Invoke(sender, EventArgs.Empty);
    }

    private void OnTextChanged(object? sender, ColorTextChangedEventArgs e)
    {
        IsEmptyChanged?.Invoke(_control, EventArgs.Empty);
        IsActiveChanged?.Invoke(_control, EventArgs.Empty);
    }

    private void OnColorChanged(object? sender, global::Avalonia.Controls.ColorChangedEventArgs e)
    {
        IsEmptyChanged?.Invoke(_control, EventArgs.Empty);
        IsActiveChanged?.Invoke(_control, EventArgs.Empty);
    }

    private void OnGotFocus(object? sender, global::Avalonia.Input.GotFocusEventArgs e)
    {
        IsFocusedChanged?.Invoke(sender, EventArgs.Empty);
        IsActiveChanged?.Invoke(sender, EventArgs.Empty);
    }

    private void OnLostFocus(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
    {
        IsFocusedChanged?.Invoke(sender, EventArgs.Empty);
        IsActiveChanged?.Invoke(sender, EventArgs.Empty);
    }

    public void Dispose()
    {
        _control.TextChanged -= OnTextChanged;
        _control.ColorChanged -= OnColorChanged;
        _control.ColorViewOpened -= OnCalendarOpenedOrClosed;
        _control.ColorViewClosed -= OnCalendarOpenedOrClosed;
        _control.GotFocus -= OnGotFocus;
        _control.LostFocus -= OnLostFocus;
    }
}
