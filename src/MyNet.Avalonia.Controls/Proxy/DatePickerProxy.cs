// -----------------------------------------------------------------------
// <copyright file="DatePickerProxy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using MyNet.Avalonia.Controls.Assists;

namespace MyNet.Avalonia.Controls.Proxy;

public sealed class DatePickerProxy : IControlProxy
{
    private readonly DatePicker _control;

    public bool IsEmpty() => _control.SelectedDate is null && !DateTimePickerAssist.GetOverrideWatermark(_control);

    public bool IsFocused() => _control.IsKeyboardFocusWithin;

    public bool IsActive() => !IsEmpty() || DateTimePickerAssist.GetOverrideWatermark(_control);

    public event EventHandler? IsEmptyChanged;

    public event EventHandler? IsFocusedChanged;

    public event EventHandler? IsActiveChanged;

    public DatePickerProxy(DatePicker control)
    {
        _control = control ?? throw new ArgumentNullException(nameof(control));
        _ = DateTimePickerAssist.OverrideWatermarkProperty.Changed.Subscribe(e =>
        {
            if (e.Sender is DatePicker datePicker && datePicker == _control)
            {
                IsActiveChanged?.Invoke(_control, EventArgs.Empty);
            }
        });
        _control.SelectedDateChanged += OnSelectedDateChanged;
        _control.GotFocus += OnGotFocus;
        _control.LostFocus += OnLostFocus;
    }

    private void OnGotFocus(object? sender, global::Avalonia.Input.GotFocusEventArgs e) => IsFocusedChanged?.Invoke(sender, EventArgs.Empty);

    private void OnLostFocus(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e) => IsFocusedChanged?.Invoke(sender, EventArgs.Empty);

    private void OnSelectedDateChanged(object? sender, DatePickerSelectedValueChangedEventArgs e)
    {
        IsEmptyChanged?.Invoke(sender, EventArgs.Empty);
        IsActiveChanged?.Invoke(sender, EventArgs.Empty);
    }

    public void Dispose()
    {
        _control.SelectedDateChanged -= OnSelectedDateChanged;
        _control.GotFocus -= OnGotFocus;
        _control.LostFocus -= OnLostFocus;
    }
}
