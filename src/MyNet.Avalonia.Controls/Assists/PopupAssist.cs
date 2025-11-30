// -----------------------------------------------------------------------
// <copyright file="PopupAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Extensions;
using MyNet.Utilities;

namespace MyNet.Avalonia.Controls.Assists;

public static class PopupAssist
{
    static PopupAssist()
    {
        PlacementProperty.Changed.Subscribe(PlacementPropertyChangedCallback);
        OpenOnFocusProperty.Changed.Subscribe(OpenOnFocusChangedCallback);
        EnableShortcutKeysProperty.Changed.Subscribe(OnEnableShortcutKeyChanged);
        AutoFocusOnOpeningProperty.Changed.Subscribe(OnAutoFocusOnOpeningChanged);
    }

    #region Background

    /// <summary>
    /// Provides Background Property for attached PopupAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> BackgroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("Background", typeof(PopupAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="BackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="BackgroundProperty"/>.</param>
    public static void SetBackground(StyledElement element, IBrush value) => element.SetValue(BackgroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="BackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush GetBackground(StyledElement element) => element.GetValue(BackgroundProperty);

    #endregion

    #region Foreground

    /// <summary>
    /// Provides Foreground Property for attached PopupAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> ForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("Foreground", typeof(PopupAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ForegroundProperty"/>.</param>
    public static void SetForeground(StyledElement element, IBrush value) => element.SetValue(ForegroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush GetForeground(StyledElement element) => element.GetValue(ForegroundProperty);

    #endregion

    #region PrimaryColor

    /// <summary>
    /// Provides PrimaryColor Property for attached PopupAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> PrimaryColorProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("PrimaryColor", typeof(PopupAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="PrimaryColorProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PrimaryColorProperty"/>.</param>
    public static void SetPrimaryColor(StyledElement element, IBrush value) => element.SetValue(PrimaryColorProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PrimaryColorProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush GetPrimaryColor(StyledElement element) => element.GetValue(PrimaryColorProperty);

    #endregion

    #region Placement

    /// <summary>
    /// Provides Placement Property for attached PopupAssist element.
    /// </summary>
    public static readonly AttachedProperty<PlacementMode> PlacementProperty = AvaloniaProperty.RegisterAttached<StyledElement, PlacementMode>("Placement", typeof(PopupAssist), PlacementMode.Custom);

    /// <summary>
    /// Accessor for Attached  <see cref="PlacementProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PlacementProperty"/>.</param>
    public static void SetPlacement(StyledElement element, PlacementMode value) => element.SetValue(PlacementProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PlacementProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static PlacementMode GetPlacement(StyledElement element) => element.GetValue(PlacementProperty);

    private static void PlacementPropertyChangedCallback(AvaloniaPropertyChangedEventArgs obj)
    {
        const int largeShadowOffset = 6;
        const int smallShadowOffset = 2;
        const int largeMargin = 8;
        const int smallMargin = 2;

        var flyout = obj.Sender switch
        {
            Button button => button.Flyout,
            ToggleSplitButton toggleSplitButton => toggleSplitButton.Flyout,
            SplitButton splitButton => splitButton.Flyout,
            _ => null
        };
        if (flyout is not PopupFlyoutBase popupFlyout || obj.NewValue is not PlacementMode placement || placement == PlacementMode.Custom)
            return;
        popupFlyout.Placement = placement;

        switch (placement)
        {
            case PlacementMode.Bottom:
            case PlacementMode.Right:
                break;
            case PlacementMode.Left:
                popupFlyout.HorizontalOffset = largeMargin + (largeShadowOffset / 2.0);
                break;
            case PlacementMode.Top:
                popupFlyout.VerticalOffset = largeMargin + (largeShadowOffset / 2.0);
                break;
            case PlacementMode.TopEdgeAlignedLeft:
                popupFlyout.VerticalOffset = largeMargin + (largeShadowOffset / 2.0);
                popupFlyout.HorizontalOffset = -(smallMargin + smallShadowOffset);
                break;
            case PlacementMode.TopEdgeAlignedRight:
                popupFlyout.VerticalOffset = largeMargin + (largeShadowOffset / 2.0);
                popupFlyout.HorizontalOffset = largeMargin + largeShadowOffset;
                break;
            case PlacementMode.BottomEdgeAlignedLeft:
                popupFlyout.HorizontalOffset = -(smallMargin + smallShadowOffset);
                break;
            case PlacementMode.BottomEdgeAlignedRight:
                popupFlyout.HorizontalOffset = largeMargin + largeShadowOffset;
                break;
            case PlacementMode.LeftEdgeAlignedTop:
                popupFlyout.VerticalOffset = -(smallMargin + (smallShadowOffset / 2.0));
                popupFlyout.HorizontalOffset = largeMargin + (largeShadowOffset / 2.0);
                break;
            case PlacementMode.LeftEdgeAlignedBottom:
                popupFlyout.HorizontalOffset = largeMargin + (largeShadowOffset / 2.0);
                popupFlyout.VerticalOffset = largeMargin + largeShadowOffset;
                break;
            case PlacementMode.RightEdgeAlignedTop:
                popupFlyout.VerticalOffset = -(smallMargin + (smallShadowOffset / 2.0));
                break;
            case PlacementMode.RightEdgeAlignedBottom:
                popupFlyout.VerticalOffset = largeMargin + largeShadowOffset;
                break;
            case PlacementMode.Pointer:
            case PlacementMode.Center:
            case PlacementMode.AnchorAndGravity:
            case PlacementMode.Custom:
            default:
                break;
        }
    }
    #endregion

    #region Height

    /// <summary>
    /// Provides Height Property for attached PopupAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> HeightProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("Height", typeof(PopupAssist), double.NaN);

    /// <summary>
    /// Accessor for Attached  <see cref="HeightProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="HeightProperty"/>.</param>
    public static void SetHeight(StyledElement element, double value) => element.SetValue(HeightProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="HeightProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetHeight(StyledElement element) => element.GetValue(HeightProperty);

    #endregion

    #region Width

    /// <summary>
    /// Provides Width Property for attached PopupAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> WidthProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("Width", typeof(PopupAssist), double.NaN);

    /// <summary>
    /// Accessor for Attached  <see cref="WidthProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="WidthProperty"/>.</param>
    public static void SetWidth(StyledElement element, double value) => element.SetValue(WidthProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="WidthProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetWidth(StyledElement element) => element.GetValue(WidthProperty);

    #endregion

    #region OpenOnFocus

    /// <summary>
    /// Provides OpenOnFocus Property for attached ProxyAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> OpenOnFocusProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("OpenOnFocus", typeof(PopupAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="OpenOnFocusProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="OpenOnFocusProperty"/>.</param>
    public static void SetOpenOnFocus(StyledElement element, bool value) => element.SetValue(OpenOnFocusProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="OpenOnFocusProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetOpenOnFocus(StyledElement element) => element.GetValue(OpenOnFocusProperty);

    private static void OpenOnFocusChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not Control control) return;

        if (((bool?)args.NewValue).IsTrue())
        {
            control.GotFocus += onGotFocus;
        }
        else
        {
            control.GotFocus -= onGotFocus;
        }

        static void onGotFocus(object? sender, EventArgs e) => (sender as Control)?.OpenPopup();
    }

    #endregion

    #region EnableShortcutKeys

    /// <summary>
    /// Provides EnableShortcutKeys Property for attached PopupAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> EnableShortcutKeysProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("EnableShortcutKeys", typeof(PopupAssist));

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

    private static void OnEnableShortcutKeyChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (args.Sender is TemplatedControl tc)
        {
            if (args.NewValue.GetValueOrDefault<bool>())
                AttachOnEnableShortcutKey(tc);
            else
                DetachOnEnableShortcutKey(tc);
        }
    }

    private static void AttachOnEnableShortcutKey(TemplatedControl dp) => dp.AddHandler(InputElement.KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);

    private static void DetachOnEnableShortcutKey(TemplatedControl dp) => dp.RemoveHandler(InputElement.KeyDownEvent, OnKeyDown);

    private static void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is not TemplatedControl tc)
            return;

        var popupOpen = tc.IsPopupOpen();

        if (!popupOpen)
        {
            if ((e.Key == Key.Down || e.Key == Key.Up) && e.KeyModifiers == KeyModifiers.Alt)
            {
                tc.OpenPopup();
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                tc.OpenPopup();
                e.Handled = true;
            }
        }
    }

    #endregion

    #region AutoFocusOnOpening

    /// <summary>
    /// Provides AutoFocusOnOpening Property for attached PopupAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> AutoFocusOnOpeningProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("AutoFocusOnOpening", typeof(PopupAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="AutoFocusOnOpeningProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="AutoFocusOnOpeningProperty"/>.</param>
    public static void SetAutoFocusOnOpening(StyledElement element, bool value) => element.SetValue(AutoFocusOnOpeningProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="AutoFocusOnOpeningProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetAutoFocusOnOpening(StyledElement element) => element.GetValue(AutoFocusOnOpeningProperty);

    private static void OnAutoFocusOnOpeningChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (args.Sender is TemplatedControl tc)
        {
            if (args.NewValue.GetValueOrDefault<bool>())
                AttachOnAutoFocusOnOpening(tc);
            else
                DetachOnAutoFocusOnOpening(tc);
        }
    }

    private static void AttachOnAutoFocusOnOpening(TemplatedControl dp) => dp.TemplateApplied += OnTemplateApplied;

    private static void DetachOnAutoFocusOnOpening(TemplatedControl dp) => dp.TemplateApplied -= OnTemplateApplied;

    private static void OnTemplateApplied(object? sender, TemplateAppliedEventArgs e)
    {
        if (sender is not TemplatedControl tc)
            return;

        var popup = e.NameScope.Find<Popup>("PART_Popup");
        if (popup != null)
        {
            popup.Opened += (_, __) =>
            {
                var focusable = popup?.Child?.GetFirstFocusableControl();
                focusable?.Focus();
            };
        }
    }

    #endregion
}
