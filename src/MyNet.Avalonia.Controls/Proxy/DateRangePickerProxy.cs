// -----------------------------------------------------------------------
// <copyright file="DateRangePickerProxy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.Controls.Proxy;

public sealed class DateRangePickerProxy : IControlProxy
{
    private readonly DateRangePicker _control;

    public bool IsEmpty() => _control.SelectedStartDate is null && _control.SelectedEndDate is null;

    public bool IsFocused() => _control.IsKeyboardFocusWithin || _control.IsDropDownOpen;

    public bool IsActive() => !IsEmpty() || IsFocused();

    public event EventHandler? IsEmptyChanged;

    public event EventHandler? IsFocusedChanged;

    public event EventHandler? IsActiveChanged;

    public DateRangePickerProxy(DateRangePicker control)
    {
        _control = control ?? throw new ArgumentNullException(nameof(control));
        _ = DateRangePicker.SelectedStartDateProperty.Changed.Subscribe(e =>
        {
            if (e.Sender is not DateRangePicker dateTimePicker || dateTimePicker != _control)
                return;
            IsEmptyChanged?.Invoke(_control, EventArgs.Empty);
            IsActiveChanged?.Invoke(_control, EventArgs.Empty);
        });
        _ = DateRangePicker.SelectedEndDateProperty.Changed.Subscribe(e =>
        {
            if (e.Sender is not DateRangePicker dateTimePicker || dateTimePicker != _control)
                return;
            IsEmptyChanged?.Invoke(_control, EventArgs.Empty);
            IsActiveChanged?.Invoke(_control, EventArgs.Empty);
        });
        _ = Primitives.DatePickerBase.IsDropDownOpenProperty.Changed.Subscribe(e =>
        {
            if (e.Sender is not DateRangePicker dateTimePicker || dateTimePicker != _control)
                return;
            IsFocusedChanged?.Invoke(_control, EventArgs.Empty);
            IsActiveChanged?.Invoke(_control, EventArgs.Empty);
        });
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

    public void Dispose()
    {
        _control.GotFocus -= OnGotFocus;
        _control.LostFocus -= OnLostFocus;
    }
}
