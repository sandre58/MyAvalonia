// -----------------------------------------------------------------------
// <copyright file="ExpanderAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;

namespace MyNet.Avalonia.Controls.Assists;

public static class ExpanderAssist
{
    static ExpanderAssist() => EnableShortcutKeysProperty.Changed.Subscribe(OnEnableShortcutKeysChanged);

    #region ButtonTheme

    /// <summary>
    /// Provides ButtonTheme Property for attached ExpanderAssist element.
    /// </summary>
    public static readonly AttachedProperty<ControlTheme> ButtonThemeProperty = AvaloniaProperty.RegisterAttached<StyledElement, ControlTheme>("ButtonTheme", typeof(ExpanderAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ButtonThemeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ButtonThemeProperty"/>.</param>
    public static void SetButtonTheme(StyledElement element, ControlTheme value) => element.SetValue(ButtonThemeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ButtonThemeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static ControlTheme GetButtonTheme(StyledElement element) => element.GetValue(ButtonThemeProperty);

    #endregion

    #region EnableShortcutKeys

    /// <summary>
    /// Provides EnableShortcutKeys Property for attached KeyboardAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> EnableShortcutKeysProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("EnableShortcutKeys", typeof(ExpanderAssist));

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
        if (sender is not Expander ctrl)
            return;

        switch (e.Key)
        {
            case Key.Down:
                if (ctrl.ExpandDirection == ExpandDirection.Down && !ctrl.IsExpanded)
                {
                    ctrl.IsExpanded = true;
                }
                else if (ctrl.ExpandDirection == ExpandDirection.Up && ctrl.IsExpanded)
                {
                    ctrl.IsExpanded = false;
                }

                break;

            case Key.Up:
                if (ctrl.ExpandDirection == ExpandDirection.Up && !ctrl.IsExpanded)
                {
                    ctrl.IsExpanded = true;
                }
                else if (ctrl.ExpandDirection == ExpandDirection.Down && ctrl.IsExpanded)
                {
                    ctrl.IsExpanded = false;
                }

                break;

            case Key.Left:
                if (ctrl.ExpandDirection == ExpandDirection.Left && !ctrl.IsExpanded)
                {
                    ctrl.IsExpanded = true;
                }
                else if (ctrl.ExpandDirection == ExpandDirection.Right && ctrl.IsExpanded)
                {
                    ctrl.IsExpanded = false;
                }

                break;

            case Key.Right:
                if (ctrl.ExpandDirection == ExpandDirection.Right && !ctrl.IsExpanded)
                {
                    ctrl.IsExpanded = true;
                }
                else if (ctrl.ExpandDirection == ExpandDirection.Left && ctrl.IsExpanded)
                {
                    ctrl.IsExpanded = false;
                }

                break;
        }
    }

    #endregion
}
