// -----------------------------------------------------------------------
// <copyright file="InputBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using MyNet.Avalonia.Controls.Extensions;

namespace MyNet.Avalonia.Controls.Behaviors;

public static class InputBehavior
{
    static InputBehavior()
    {
        UpdateValueOnMouseWheelProperty.Changed.Subscribe(OnUpdateValueOnMouseWheelChanged);
        UpdateValueOnKeyboardProperty.Changed.Subscribe(OnUpdateValueOnKeyboardChanged);
    }

    #region IsTextEditable

    /// <summary>
    /// Provides IsTextEditable Property for attached InputBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> IsTextEditableProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("IsTextEditable", typeof(InputBehavior), true);

    /// <summary>
    /// Accessor for Attached  <see cref="IsTextEditableProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IsTextEditableProperty"/>.</param>
    public static void SetIsTextEditable(StyledElement element, bool value) => element.SetValue(IsTextEditableProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IsTextEditableProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetIsTextEditable(StyledElement element) => element.GetValue(IsTextEditableProperty);

    #endregion

    #region UpdateValueOnMouseWheel

    /// <summary>
    /// Provides UpdateValueOnMouseWheel Property for attached InputBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> UpdateValueOnMouseWheelProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("UpdateValueOnMouseWheel", typeof(InputBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="UpdateValueOnMouseWheelProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="UpdateValueOnMouseWheelProperty"/>.</param>
    public static void SetUpdateValueOnMouseWheel(StyledElement element, bool value) => element.SetValue(UpdateValueOnMouseWheelProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="EnableShortcutKeysProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetUpdateValueOnMouseWheel(StyledElement element) => element.GetValue(UpdateValueOnMouseWheelProperty);

    private static void OnUpdateValueOnMouseWheelChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (args.Sender is TemplatedControl tc)
        {
            if (args.NewValue.GetValueOrDefault<bool>())
                AttachOnUpdateValueOnMouseWheel(tc);
            else
                DetachOnUpdateValueOnMouseWheel(tc);
        }
    }

    private static void AttachOnUpdateValueOnMouseWheel(TemplatedControl dp) => dp.AddHandler(InputElement.PointerWheelChangedEvent, OnPointerWheelChanged, RoutingStrategies.Tunnel);

    private static void DetachOnUpdateValueOnMouseWheel(TemplatedControl dp) => dp.RemoveHandler(InputElement.PointerWheelChangedEvent, OnPointerWheelChanged);

    private static void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (sender is not TemplatedControl tc)
            return;

        if (!e.Handled && tc.IsKeyboardFocusWithin)
        {
            e.Handled = tc.Increment(e.Delta.Y > 0 ? -1 : 1);
        }
    }

    #endregion

    #region UpdateValueOnKeyboard

    /// <summary>
    /// Provides UpdateValueOnMouseWheel Property for attached InputBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> UpdateValueOnKeyboardProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("UpdateValueOnKeyboard", typeof(InputBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="UpdateValueOnKeyboardProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="UpdateValueOnKeyboardProperty"/>.</param>
    public static void SetUpdateValueOnKeyboard(StyledElement element, bool value) => element.SetValue(UpdateValueOnKeyboardProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="UpdateValueOnKeyboardProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetUpdateValueOnKeyboard(StyledElement element) => element.GetValue(UpdateValueOnKeyboardProperty);

    private static void OnUpdateValueOnKeyboardChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (args.Sender is TemplatedControl tc)
        {
            if (args.NewValue.GetValueOrDefault<bool>())
                AttachOnUpdateValueOnKeyboard(tc);
            else
                DetachOnUpdateValueOnKeyboard(tc);
        }
    }

    private static void AttachOnUpdateValueOnKeyboard(TemplatedControl dp) => dp.AddHandler(InputElement.KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);

    private static void DetachOnUpdateValueOnKeyboard(TemplatedControl dp) => dp.RemoveHandler(InputElement.KeyDownEvent, OnKeyDown);

    private static void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is not TemplatedControl tc)
            return;

        var popupOpen = tc.IsPopupOpen();

        if (!popupOpen)
        {
            switch (e)
            {
                case { Key: Key.Down, KeyModifiers: KeyModifiers.None }:
                    tc.Increment(1);
                    e.Handled = true;
                    break;
                case { Key: Key.Up, KeyModifiers: KeyModifiers.None }:
                    tc.Increment(-1);
                    e.Handled = true;
                    break;
                case { Key: Key.PageDown, KeyModifiers: KeyModifiers.None }:
                    tc.IncrementLarge(1);
                    e.Handled = true;
                    break;
                case { Key: Key.PageUp, KeyModifiers: KeyModifiers.None }:
                    tc.IncrementLarge(-1);
                    e.Handled = true;
                    break;

                default:
                    {
                        if ((e.Key is Key.Down or Key.Up && e.KeyModifiers == KeyModifiers.Alt) || e.Key is Key.Enter or Key.Space)
                        {
                            tc.OpenPopup();
                            e.Handled = true;
                        }

                        break;
                    }
            }
        }
    }

    #endregion
}
