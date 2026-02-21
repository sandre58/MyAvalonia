// -----------------------------------------------------------------------
// <copyright file="ExpanderBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace MyNet.Avalonia.Controls.Behaviors;

public static class ExpanderBehavior
{
    static ExpanderBehavior() => EnableShortcutKeysProperty.Changed.Subscribe(OnEnableShortcutKeysChanged);

    #region EnableShortcutKeys

    /// <summary>
    /// Provides EnableShortcutKeys Property for attached KeyboardAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> EnableShortcutKeysProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("EnableShortcutKeys", typeof(ExpanderBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="EnableShortcutKeysProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="EnableShortcutKeysProperty"/>.</param>
    public static void SetEnableShortcutKeys(StyledElement element, bool value) => element.SetValue(EnableShortcutKeysProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="EnableShortcutKeysProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetEnableShortcutKeys(StyledElement element) => element.GetValue(EnableShortcutKeysProperty);

    private static void OnEnableShortcutKeysChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not Control ctrl)
            return;

        var enable = args.NewValue as bool? ?? false;

        if (enable)
        {
            ctrl.AddHandler(InputElement.KeyDownEvent, OnExpanderKeyDown, RoutingStrategies.Tunnel);
        }
        else
        {
            ctrl.RemoveHandler(InputElement.KeyDownEvent, OnExpanderKeyDown);
        }
    }

    private static void OnExpanderKeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is not Expander { IsFocused: true, IsKeyboardFocusWithin: true } ctrl)
            return;

        ctrl.IsExpanded = e.Key switch
        {
            Key.Down => ctrl switch
            {
                { ExpandDirection: ExpandDirection.Down, IsExpanded: false } => true,
                { ExpandDirection: ExpandDirection.Up, IsExpanded: true } => false,
                _ => ctrl.IsExpanded
            },
            Key.Up => ctrl switch
            {
                { ExpandDirection: ExpandDirection.Up, IsExpanded: false } => true,
                { ExpandDirection: ExpandDirection.Down, IsExpanded: true } => false,
                _ => ctrl.IsExpanded
            },
            Key.Left => ctrl switch
            {
                { ExpandDirection: ExpandDirection.Left, IsExpanded: false } => true,
                { ExpandDirection: ExpandDirection.Right, IsExpanded: true } => false,
                _ => ctrl.IsExpanded
            },
            Key.Right => ctrl switch
            {
                { ExpandDirection: ExpandDirection.Right, IsExpanded: false } => true,
                { ExpandDirection: ExpandDirection.Left, IsExpanded: true } => false,
                _ => ctrl.IsExpanded
            },
            _ => ctrl.IsExpanded
        };
    }

    #endregion
}
