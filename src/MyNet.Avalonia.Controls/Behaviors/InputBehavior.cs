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

/// <summary>
/// Provides attached behaviors that let any <see cref="TemplatedControl"/> react to mouse-wheel
/// and keyboard input by calling <c>Increment</c> / <c>IncrementLarge</c> on
/// <see cref="ControlExtensions"/>.  For <see cref="Avalonia.Controls.Spinner"/>-based controls
/// those calls in turn raise the <see cref="Avalonia.Controls.Spinner.SpinEvent"/>, which
/// <see cref="SpinnerBehavior"/> can then handle to execute commands or ViewModel methods.
/// </summary>
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
    /// Accessor for Attached <see cref="IsTextEditableProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set <see cref="IsTextEditableProperty"/>.</param>
    public static void SetIsTextEditable(StyledElement element, bool value) => element.SetValue(IsTextEditableProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="IsTextEditableProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetIsTextEditable(StyledElement element) => element.GetValue(IsTextEditableProperty);

    #endregion

    #region UpdateValueOnMouseWheel

    /// <summary>
    /// When <see langword="true"/>, scrolling the mouse wheel over the control increments or
    /// decrements its value.  The wheel is active whenever the pointer is over the control
    /// (<see cref="InputElement.IsPointerOver"/>), so no prior click/focus is required.
    /// </summary>
    public static readonly AttachedProperty<bool> UpdateValueOnMouseWheelProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("UpdateValueOnMouseWheel", typeof(InputBehavior));

    /// <summary>
    /// Accessor for Attached <see cref="UpdateValueOnMouseWheelProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set <see cref="UpdateValueOnMouseWheelProperty"/>.</param>
    public static void SetUpdateValueOnMouseWheel(StyledElement element, bool value) => element.SetValue(UpdateValueOnMouseWheelProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="UpdateValueOnMouseWheelProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetUpdateValueOnMouseWheel(StyledElement element) => element.GetValue(UpdateValueOnMouseWheelProperty);

    private static void OnUpdateValueOnMouseWheelChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (args.Sender is TemplatedControl tc)
        {
            if (args.NewValue.GetValueOrDefault())
                tc.AddHandler(InputElement.PointerWheelChangedEvent, OnPointerWheelChanged, RoutingStrategies.Tunnel);
            else
                tc.RemoveHandler(InputElement.PointerWheelChangedEvent, OnPointerWheelChanged);
        }
    }

    private static void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (sender is not TemplatedControl tc || e.Handled)
            return;

        // Activate on pointer-over: no keyboard focus required (natural scroll UX for spinners).
        if (!tc.IsPointerOver)
            return;

        e.Handled = tc.Increment(e.Delta.Y > 0 ? 1 : -1);
    }

    #endregion

    #region UpdateValueOnKeyboard

    /// <summary>
    /// When <see langword="true"/>, arrow keys and page keys increment or decrement the control's
    /// value, and Alt+Up/Down, Enter, or Space open its popup (when present).
    /// Only active when no popup is already open.
    /// </summary>
    public static readonly AttachedProperty<bool> UpdateValueOnKeyboardProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("UpdateValueOnKeyboard", typeof(InputBehavior));

    /// <summary>
    /// Accessor for Attached <see cref="UpdateValueOnKeyboardProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set <see cref="UpdateValueOnKeyboardProperty"/>.</param>
    public static void SetUpdateValueOnKeyboard(StyledElement element, bool value) => element.SetValue(UpdateValueOnKeyboardProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="UpdateValueOnKeyboardProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetUpdateValueOnKeyboard(StyledElement element) => element.GetValue(UpdateValueOnKeyboardProperty);

    private static void OnUpdateValueOnKeyboardChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (args.Sender is TemplatedControl tc)
        {
            if (args.NewValue.GetValueOrDefault())
                tc.AddHandler(InputElement.KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);
            else
                tc.RemoveHandler(InputElement.KeyDownEvent, OnKeyDown);
        }
    }

    private static void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is not TemplatedControl tc || e.Handled)
            return;

        if (tc.IsPopupOpen())
            return;

        switch (e)
        {
            case { Key: Key.Down, KeyModifiers: KeyModifiers.None }:
                e.Handled = tc.Increment(-1);
                break;
            case { Key: Key.Up, KeyModifiers: KeyModifiers.None }:
                e.Handled = tc.Increment(1);
                break;
            case { Key: Key.PageDown, KeyModifiers: KeyModifiers.None }:
                e.Handled = tc.IncrementLarge(-1);
                break;
            case { Key: Key.PageUp, KeyModifiers: KeyModifiers.None }:
                e.Handled = tc.IncrementLarge(1);
                break;
            default:
                if ((e.Key is Key.Down or Key.Up && e.KeyModifiers == KeyModifiers.Alt) || e.Key is Key.Enter or Key.Space)
                {
                    tc.OpenPopup();
                    e.Handled = true;
                }

                break;
        }
    }

    #endregion
}
