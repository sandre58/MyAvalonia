// -----------------------------------------------------------------------
// <copyright file="NumericUpDownProxy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Input;

namespace MyNet.Avalonia.Controls.Proxy;

public sealed class NumericUpDownProxy : IControlProxy
{
    private readonly NumericUpDown _control;

    public bool IsEmpty() => string.IsNullOrEmpty(_control.Text);

    public bool IsFocused() => _control.IsKeyboardFocusWithin || _control.IsFocused;

    public bool IsActive() => !IsEmpty() || IsFocused();

    public event EventHandler? IsEmptyChanged;

    public event EventHandler? IsFocusedChanged;

    public event EventHandler? IsActiveChanged;

    public NumericUpDownProxy(NumericUpDown control)
    {
        _control = control ?? throw new ArgumentNullException(nameof(control));
        _ = NumericUpDown.TextProperty.Changed.Subscribe(e =>
        {
            if (e.Sender is not NumericUpDown upDown || upDown != _control)
                return;
            IsEmptyChanged?.Invoke(_control, EventArgs.Empty);
            IsActiveChanged?.Invoke(_control, EventArgs.Empty);
        });
        _control.GotFocus += OnGotFocus;
        _control.LostFocus += OnLostFocus;
    }

    private void OnGotFocus(object? sender, GotFocusEventArgs e)
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
        _control.GotFocus -= OnGotFocus;
        _control.LostFocus -= OnLostFocus;
    }
}
