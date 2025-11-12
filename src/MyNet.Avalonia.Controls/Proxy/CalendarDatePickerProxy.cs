// -----------------------------------------------------------------------
// <copyright file="CalendarDatePickerProxy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using MyNet.Avalonia.Controls.Primitives;

namespace MyNet.Avalonia.Controls.Proxy;

public sealed class CalendarDatePickerProxy : IControlProxy
{
    private readonly CalendarDatePicker _control;

    public bool IsEmpty() => _control.IsEmpty();

    public bool IsFocused() => _control.IsKeyboardFocusWithin || _control.IsDropDownOpen;

    public bool IsActive() => !IsEmpty() || IsFocused();

    public event EventHandler? IsEmptyChanged;

    public event EventHandler? IsFocusedChanged;

    public event EventHandler? IsActiveChanged;

    public CalendarDatePickerProxy(CalendarDatePicker control)
    {
        _control = control ?? throw new ArgumentNullException(nameof(control));
        _ = DatePickerBase.IsDropDownOpenProperty.Changed.Subscribe(e =>
        {
            if (e.Sender is not CalendarDatePicker calendarDatePicker || calendarDatePicker != _control)
                return;
            IsFocusedChanged?.Invoke(_control, EventArgs.Empty);
            IsActiveChanged?.Invoke(_control, EventArgs.Empty);
        });
        _control.GotFocus += OnGotFocus;
        _control.LostFocus += OnLostFocus;
        _control.SelectedDateChanged += OnSeletedDateChanged;
        _control.TextChanged += OnTextChanged;
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        IsEmptyChanged?.Invoke(_control, EventArgs.Empty);
        IsActiveChanged?.Invoke(_control, EventArgs.Empty);
    }

    private void OnSeletedDateChanged(object? sender, SelectionChangedEventArgs e)
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
        _control.GotFocus -= OnGotFocus;
        _control.LostFocus -= OnLostFocus;
        _control.SelectedDateChanged -= OnSeletedDateChanged;
        _control.TextChanged -= OnTextChanged;
    }
}
