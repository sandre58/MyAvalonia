// -----------------------------------------------------------------------
// <copyright file="DateTimeScrollPickerExProxy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Input;
using Avalonia.Interactivity;
using MyNet.Avalonia.Controls.Behaviors;

namespace MyNet.Avalonia.Controls.Proxy;

public sealed class DateTimeScrollPickerExProxy : IControlProxy
{
    private readonly DateTimeScrollPickerEx _control;

    public bool IsEmpty() => _control.IsEmpty();

    public bool IsFocused() => _control.IsKeyboardFocusWithin || _control.IsDropDownOpen;

    public bool IsActive() => !IsEmpty() || DateTimePickerBehavior.GetOverridePlaceholderText(_control);

    public event EventHandler? IsEmptyChanged;

    public event EventHandler? IsFocusedChanged;

    public event EventHandler? IsActiveChanged;

    public DateTimeScrollPickerExProxy(DateTimeScrollPickerEx control)
    {
        _control = control ?? throw new ArgumentNullException(nameof(control));
        _ = DateTimePickerBehavior.OverridePlaceholderTextProperty.Changed.Subscribe(e =>
        {
            if (e.Sender is DateTimeScrollPickerEx picker && picker == _control)
                IsActiveChanged?.Invoke(_control, EventArgs.Empty);
        });
        _ = DateTimeScrollPickerEx.IsDropDownOpenProperty.Changed.Subscribe(e =>
        {
            if (e.Sender is not DateTimeScrollPickerEx picker || picker != _control)
                return;

            IsFocusedChanged?.Invoke(_control, EventArgs.Empty);
            IsActiveChanged?.Invoke(_control, EventArgs.Empty);
        });
        _control.GotFocus += OnGotFocus;
        _control.LostFocus += OnLostFocus;
        _control.SelectedDateTimeChanged += OnSelectedDateTimeChanged;
    }

    private void OnSelectedDateTimeChanged(object? sender, DateTimeScrollPickerSelectedValueChangedEventArgs e)
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
        _control.SelectedDateTimeChanged -= OnSelectedDateTimeChanged;
    }
}
