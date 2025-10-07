// -----------------------------------------------------------------------
// <copyright file="AutoCompleteBoxProxy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;

namespace MyNet.Avalonia.Controls.Proxy;

public sealed class AutoCompleteBoxProxy : IControlProxy
{
    private readonly AutoCompleteBox _control;

    public bool IsEmpty() => string.IsNullOrEmpty(_control.Text);

    public bool IsFocused() => _control.IsKeyboardFocusWithin;

    public bool IsActive() => !IsEmpty() || IsFocused();

    public event EventHandler? IsEmptyChanged;

    public event EventHandler? IsFocusedChanged;

    public event EventHandler? IsActiveChanged;

    public AutoCompleteBoxProxy(AutoCompleteBox control)
    {
        _control = control ?? throw new ArgumentNullException(nameof(control));
        _control.TextChanged += TextChanged;
        _control.GotFocus += OnGotFocus;
        _control.LostFocus += OnLostFocus;
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

    private void TextChanged(object? sender, TextChangedEventArgs e)
    {
        IsEmptyChanged?.Invoke(sender, EventArgs.Empty);
        IsActiveChanged?.Invoke(sender, EventArgs.Empty);
    }

    public void Dispose()
    {
        _control.TextChanged -= TextChanged;
        _control.GotFocus -= OnGotFocus;
        _control.LostFocus -= OnLostFocus;
    }
}
