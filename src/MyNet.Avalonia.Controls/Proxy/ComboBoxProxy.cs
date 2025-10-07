// -----------------------------------------------------------------------
// <copyright file="ComboBoxProxy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;

namespace MyNet.Avalonia.Controls.Proxy;

public sealed class ComboBoxProxy : IControlProxy
{
    private readonly ComboBox _control;

    public bool IsEmpty() => _control.SelectedItem is null;

    public bool IsFocused() => _control.IsKeyboardFocusWithin || _control.IsDropDownOpen;

    public bool IsActive() => !IsEmpty();

    public event EventHandler? IsEmptyChanged;

    public event EventHandler? IsFocusedChanged;

    public event EventHandler? IsActiveChanged;

    public ComboBoxProxy(ComboBox control)
    {
        _control = control ?? throw new ArgumentNullException(nameof(control));
        _control.SelectionChanged += OnSelectionChanged;
        _control.DropDownOpened += OnDropDownOpened;
        _control.GotFocus += OnGotFocus;
        _control.LostFocus += OnLostFocus;
    }

    private void OnDropDownOpened(object? sender, EventArgs e) => IsFocusedChanged?.Invoke(sender, EventArgs.Empty);

    private void OnGotFocus(object? sender, global::Avalonia.Input.GotFocusEventArgs e) => IsFocusedChanged?.Invoke(sender, EventArgs.Empty);

    private void OnLostFocus(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e) => IsFocusedChanged?.Invoke(sender, EventArgs.Empty);

    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        IsEmptyChanged?.Invoke(sender, EventArgs.Empty);
        IsActiveChanged?.Invoke(sender, EventArgs.Empty);
    }

    public void Dispose()
    {
        _control.SelectionChanged -= OnSelectionChanged;
        _control.DropDownOpened -= OnDropDownOpened;
        _control.GotFocus -= OnGotFocus;
        _control.LostFocus -= OnLostFocus;
    }
}
