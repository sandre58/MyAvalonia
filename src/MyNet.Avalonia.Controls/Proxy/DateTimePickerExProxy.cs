// -----------------------------------------------------------------------
// <copyright file="DateTimePickerExProxy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace MyNet.Avalonia.Controls.Proxy;

public sealed class DateTimePickerExProxy : IControlProxy
{
    private readonly DateTimePickerEx _control;

    public bool IsEmpty() => _control.IsEmpty();

    public bool IsFocused() => _control.IsKeyboardFocusWithin || _control.IsDropDownOpen;

    public bool IsActive() => !IsEmpty() || IsFocused();

    public event EventHandler? IsEmptyChanged;

    public event EventHandler? IsFocusedChanged;

    public event EventHandler? IsActiveChanged;

    public DateTimePickerExProxy(DateTimePickerEx control)
    {
        _control = control ?? throw new ArgumentNullException(nameof(control));
        _ = DropDownControl.IsDropDownOpenProperty.Changed.Subscribe(e =>
        {
            if (e.Sender is not DateTimePickerEx picker || picker != _control)
                return;

            IsFocusedChanged?.Invoke(_control, EventArgs.Empty);
            IsActiveChanged?.Invoke(_control, EventArgs.Empty);
        });
        _control.GotFocus += OnGotFocus;
        _control.LostFocus += OnLostFocus;
        _control.SelectedValueChanged += OnSelectedValueChanged;
        _control.TextChanged += OnTextChanged;
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        IsEmptyChanged?.Invoke(_control, EventArgs.Empty);
        IsActiveChanged?.Invoke(_control, EventArgs.Empty);
    }

    private void OnSelectedValueChanged(object? sender, SelectionChangedEventArgs e)
    {
        IsEmptyChanged?.Invoke(_control, EventArgs.Empty);
        IsActiveChanged?.Invoke(_control, EventArgs.Empty);
    }

    private void OnGotFocus(object? sender, FocusChangedEventArgs e)
    {
        IsFocusedChanged?.Invoke(sender, EventArgs.Empty);
        IsActiveChanged?.Invoke(sender, EventArgs.Empty);
    }

    private void OnLostFocus(object? sender, RoutedEventArgs e)
    {
        IsFocusedChanged?.Invoke(sender, EventArgs.Empty);
        IsActiveChanged?.Invoke(sender, EventArgs.Empty);
    }

    public void Dispose()
    {
        _control.GotFocus -= OnGotFocus;
        _control.LostFocus -= OnLostFocus;
        _control.SelectedValueChanged -= OnSelectedValueChanged;
        _control.TextChanged -= OnTextChanged;
    }
}
