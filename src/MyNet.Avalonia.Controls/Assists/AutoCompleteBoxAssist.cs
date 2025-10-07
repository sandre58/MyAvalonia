// -----------------------------------------------------------------------
// <copyright file="AutoCompleteBoxAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MyNet.Utilities;

namespace MyNet.Avalonia.Controls.Assists;

public static class AutoCompleteBoxAssist
{
    static AutoCompleteBoxAssist() => OpenDropDownOnFocusProperty.Changed.Subscribe(OpenDropDownOnFocusChangedCallback);

    #region OpenDropDownOnFocus

    /// <summary>
    /// Provides OpenDropDownOnFocus Property for attached ProxyAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> OpenDropDownOnFocusProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("OpenDropDownOnFocus", typeof(AutoCompleteBoxAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="OpenDropDownOnFocusProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="OpenDropDownOnFocusProperty"/>.</param>
    public static void SetOpenDropDownOnFocus(StyledElement element, bool value) => element.SetValue(OpenDropDownOnFocusProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="OpenDropDownOnFocusProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetOpenDropDownOnFocus(StyledElement element) => element.GetValue(OpenDropDownOnFocusProperty);

    private static void OpenDropDownOnFocusChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not AutoCompleteBox control) return;

        if (((bool?)args.NewValue).IsTrue())
        {
            control.GotFocus += onGotFocus;
            control.AddHandler(InputElement.PointerPressedEvent, onBoxPointerPressed, RoutingStrategies.Tunnel);
        }
        else
        {
            control.GotFocus -= onGotFocus;
            control.RemoveHandler(InputElement.PointerPressedEvent, onBoxPointerPressed);
        }

        void onGotFocus(object? sender, EventArgs e) => (sender as AutoCompleteBox)?.SetCurrentValue(AutoCompleteBox.IsDropDownOpenProperty, true);

        void onBoxPointerPressed(object? sender, PointerPressedEventArgs e) => (sender as AutoCompleteBox).IfNotNull(x => e.GetCurrentPoint(x).Properties.IsLeftButtonPressed.IfTrue(() => x.SetCurrentValue(AutoCompleteBox.IsDropDownOpenProperty, true)));
    }

    #endregion
}
