// -----------------------------------------------------------------------
// <copyright file="WindowAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace MyNet.Avalonia.Theme.Controls.Assists;

public static class WindowAssist
{
    static WindowAssist()
    {
        Control.LoadedEvent.AddClassHandler<Window>((window, _) =>
        {
            UpdateCaptionButtonsWidth(window);
            UpdateTitleBarContentInset(window);
        });

        ShowMinimizeButtonProperty.Changed.AddClassHandler<Window>(OnCaptionButtonVisibilityChanged);
        ShowMaximizeButtonProperty.Changed.AddClassHandler<Window>(OnCaptionButtonVisibilityChanged);
        ShowCloseButtonProperty.Changed.AddClassHandler<Window>(OnCaptionButtonVisibilityChanged);
        ShowFullScreenButtonProperty.Changed.AddClassHandler<Window>(OnCaptionButtonVisibilityChanged);

        TitleBarHeightProperty.Changed.AddClassHandler<Window>((w, _) => UpdateTitleBarContentInset(w));
        ExtendContentIntoTitleBarProperty.Changed.AddClassHandler<Window>((w, _) => UpdateTitleBarContentInset(w));
        ReserveTitleBarSafeAreaProperty.Changed.AddClassHandler<Window>((w, _) => UpdateTitleBarContentInset(w));
    }

    private static void OnCaptionButtonVisibilityChanged(Window window, AvaloniaPropertyChangedEventArgs e) => UpdateCaptionButtonsWidth(window);

    private static void UpdateCaptionButtonsWidth(Window window)
        => window.SetCurrentValue(
            CaptionButtonsWidthProperty,
            WindowCaptionLayout.CalculateCaptionButtonsWidth(
                GetShowMinimizeButton(window),
                GetShowMaximizeButton(window),
                GetShowCloseButton(window),
                GetShowFullScreenButton(window)));

    private static void UpdateTitleBarContentInset(Window window)
        => window.SetCurrentValue(
            TitleBarContentInsetProperty,
            WindowCaptionLayout.CalculateContentInset(
                GetExtendContentIntoTitleBar(window),
                GetReserveTitleBarSafeArea(window),
                GetTitleBarHeight(window)));

    // ------------------------------------------------------------------
    // Title bar
    // ------------------------------------------------------------------
    public static readonly AttachedProperty<double> TitleBarHeightProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, double>(
            "TitleBarHeight",
            typeof(WindowAssist),
            WindowLayoutMetrics.TitleBarHeight,
            inherits: true);

    public static double GetTitleBarHeight(StyledElement element) => element.GetValue(TitleBarHeightProperty);

    public static void SetTitleBarHeight(StyledElement element, double value) => element.SetValue(TitleBarHeightProperty, value);

    public static readonly AttachedProperty<bool> IsTitleVisibleProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, bool>(
            "IsTitleVisible",
            typeof(WindowAssist),
            true,
            inherits: true);

    public static bool GetIsTitleVisible(StyledElement element) => element.GetValue(IsTitleVisibleProperty);

    public static void SetIsTitleVisible(StyledElement element, bool value) => element.SetValue(IsTitleVisibleProperty, value);

    public static readonly AttachedProperty<IBrush?> TitleBarBackgroundProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>(
            "TitleBarBackground",
            typeof(WindowAssist),
            inherits: true);

    public static IBrush? GetTitleBarBackground(StyledElement element) => element.GetValue(TitleBarBackgroundProperty);

    public static void SetTitleBarBackground(StyledElement element, IBrush? value) => element.SetValue(TitleBarBackgroundProperty, value);

    public static readonly AttachedProperty<IBrush?> TitleBarForegroundProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>(
            "TitleBarForeground",
            typeof(WindowAssist),
            inherits: true);

    public static IBrush? GetTitleBarForeground(StyledElement element) => element.GetValue(TitleBarForegroundProperty);

    public static void SetTitleBarForeground(StyledElement element, IBrush? value) => element.SetValue(TitleBarForegroundProperty, value);

    /// <summary>
    /// When true and <see cref="ExtendContentIntoTitleBar"/> is true, adds top padding to window content so it does not sit under the title bar chrome.
    /// </summary>
    public static readonly AttachedProperty<bool> ReserveTitleBarSafeAreaProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, bool>(
            "ReserveTitleBarSafeArea",
            typeof(WindowAssist),
            inherits: true);

    public static bool GetReserveTitleBarSafeArea(StyledElement element) => element.GetValue(ReserveTitleBarSafeAreaProperty);

    public static void SetReserveTitleBarSafeArea(StyledElement element, bool value) => element.SetValue(ReserveTitleBarSafeAreaProperty, value);

    /// <summary>
    /// Computed top padding applied to the window content presenter (see <see cref="ReserveTitleBarSafeAreaProperty"/> and <see cref="ExtendContentIntoTitleBarProperty"/>).
    /// </summary>
    public static readonly AttachedProperty<Thickness> TitleBarContentInsetProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, Thickness>(
            "TitleBarContentInset",
            typeof(WindowAssist),
            inherits: true);

    public static Thickness GetTitleBarContentInset(StyledElement element) => element.GetValue(TitleBarContentInsetProperty);

    public static void SetTitleBarContentInset(StyledElement element, Thickness value) => element.SetValue(TitleBarContentInsetProperty, value);

    // ------------------------------------------------------------------
    // Title bar content
    // ------------------------------------------------------------------
    public static readonly AttachedProperty<object?> LeftTitleBarContentProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, object?>(
            "LeftTitleBarContent",
            typeof(WindowAssist),
            inherits: true);

    public static object? GetLeftTitleBarContent(StyledElement element) => element.GetValue(LeftTitleBarContentProperty);

    public static void SetLeftTitleBarContent(StyledElement element, object? value) => element.SetValue(LeftTitleBarContentProperty, value);

    public static readonly AttachedProperty<object?> CenterTitleBarContentProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, object?>(
            "CenterTitleBarContent",
            typeof(WindowAssist),
            inherits: true);

    public static object? GetCenterTitleBarContent(StyledElement element) => element.GetValue(CenterTitleBarContentProperty);

    public static void SetCenterTitleBarContent(StyledElement element, object? value) => element.SetValue(CenterTitleBarContentProperty, value);

    public static readonly AttachedProperty<object?> RightTitleBarContentProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, object?>(
            "RightTitleBarContent",
            typeof(WindowAssist),
            inherits: true);

    public static object? GetRightTitleBarContent(StyledElement element) => element.GetValue(RightTitleBarContentProperty);

    public static void SetRightTitleBarContent(StyledElement element, object? value) => element.SetValue(RightTitleBarContentProperty, value);

    /// <summary>
    /// Reserved width on the left for system caption controls (e.g. macOS traffic lights). Defaults to <see cref="WindowLayoutMetrics.MacTitleBarInset"/> on macOS.
    /// </summary>
    public static readonly AttachedProperty<double> LeftCaptionButtonsWidthProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, double>(
            "LeftCaptionButtonsWidth",
            typeof(WindowAssist),
            GetDefaultLeftCaptionButtonsWidth(),
            inherits: true);

    public static double GetLeftCaptionButtonsWidth(StyledElement element) => element.GetValue(LeftCaptionButtonsWidthProperty);

    public static void SetLeftCaptionButtonsWidth(StyledElement element, double value) => element.SetValue(LeftCaptionButtonsWidthProperty, value);

    // ------------------------------------------------------------------
    // Buttons
    // ------------------------------------------------------------------
    public static readonly AttachedProperty<bool> ShowMinimizeButtonProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, bool>(
            "ShowMinimizeButton",
            typeof(WindowAssist),
            true,
            inherits: true);

    public static bool GetShowMinimizeButton(StyledElement element) => element.GetValue(ShowMinimizeButtonProperty);

    public static void SetShowMinimizeButton(StyledElement element, bool value) => element.SetValue(ShowMinimizeButtonProperty, value);

    public static readonly AttachedProperty<bool> ShowMaximizeButtonProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, bool>(
            "ShowMaximizeButton",
            typeof(WindowAssist),
            true,
            inherits: true);

    public static bool GetShowMaximizeButton(StyledElement element) => element.GetValue(ShowMaximizeButtonProperty);

    public static void SetShowMaximizeButton(StyledElement element, bool value) => element.SetValue(ShowMaximizeButtonProperty, value);

    public static readonly AttachedProperty<bool> ShowCloseButtonProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, bool>(
            "ShowCloseButton",
            typeof(WindowAssist),
            true,
            inherits: true);

    public static bool GetShowCloseButton(StyledElement element) => element.GetValue(ShowCloseButtonProperty);

    public static void SetShowCloseButton(StyledElement element, bool value) => element.SetValue(ShowCloseButtonProperty, value);

    public static readonly AttachedProperty<bool> ShowFullScreenButtonProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, bool>(
            "ShowFullScreenButton",
            typeof(WindowAssist),
            inherits: true);

    public static bool GetShowFullScreenButton(StyledElement element) => element.GetValue(ShowFullScreenButtonProperty);

    public static void SetShowFullScreenButton(StyledElement element, bool value) => element.SetValue(ShowFullScreenButtonProperty, value);

    public static readonly AttachedProperty<double> CaptionButtonsWidthProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, double>(
            "CaptionButtonsWidth",
            typeof(WindowAssist),
            3 * WindowLayoutMetrics.CaptionButtonWidth,
            inherits: true);

    public static double GetCaptionButtonsWidth(StyledElement element) => element.GetValue(CaptionButtonsWidthProperty);

    public static void SetCaptionButtonsWidth(StyledElement element, double value) => element.SetValue(CaptionButtonsWidthProperty, value);

    // ------------------------------------------------------------------
    // Window appearance
    // ------------------------------------------------------------------
    public static readonly AttachedProperty<bool> HasShadowProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, bool>(
            "HasShadow",
            typeof(WindowAssist),
            true,
            inherits: true);

    public static bool GetHasShadow(StyledElement element) => element.GetValue(HasShadowProperty);

    public static void SetHasShadow(StyledElement element, bool value) => element.SetValue(HasShadowProperty, value);

    public static readonly AttachedProperty<bool> HasBorderProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, bool>(
            "HasBorder",
            typeof(WindowAssist),
            true,
            inherits: true);

    public static bool GetHasBorder(StyledElement element) => element.GetValue(HasBorderProperty);

    public static void SetHasBorder(StyledElement element, bool value) => element.SetValue(HasBorderProperty, value);

    // ------------------------------------------------------------------
    // Content positioning
    // ------------------------------------------------------------------
    public static readonly AttachedProperty<bool> ExtendContentIntoTitleBarProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, bool>(
            "ExtendContentIntoTitleBar",
            typeof(WindowAssist),
            inherits: true);

    public static bool GetExtendContentIntoTitleBar(StyledElement element) => element.GetValue(ExtendContentIntoTitleBarProperty);

    public static void SetExtendContentIntoTitleBar(StyledElement element, bool value) => element.SetValue(ExtendContentIntoTitleBarProperty, value);

    private static double GetDefaultLeftCaptionButtonsWidth() => OperatingSystem.IsMacOS() ? WindowLayoutMetrics.MacTitleBarInset : 0;
}

internal static class WindowCaptionLayout
{
    public static double CalculateCaptionButtonsWidth(
        bool showMinimize,
        bool showMaximize,
        bool showClose,
        bool showFullScreen)
    {
        var width = 0.0;
        if (showMinimize) width += WindowLayoutMetrics.CaptionButtonWidth;
        if (showMaximize) width += WindowLayoutMetrics.CaptionButtonWidth;
        if (showClose) width += WindowLayoutMetrics.CaptionButtonWidth;
        if (showFullScreen) width += WindowLayoutMetrics.CaptionButtonWidth;
        return width;
    }

    public static Thickness CalculateContentInset(bool extendContentIntoTitleBar, bool reserveTitleBarSafeArea, double titleBarHeight)
    {
        var top = extendContentIntoTitleBar && reserveTitleBarSafeArea ? titleBarHeight : 0;
        return new(0, top, 0, 0);
    }
}

/// <summary>
/// Default window chrome measurements. Values must stay aligned with <c>Tokens/Layout.axaml</c>.
/// </summary>
internal static class WindowLayoutMetrics
{
    public const double TitleBarHeight = 30;
    public const double CaptionButtonWidth = 45;
    public const double MacTitleBarInset = 78;
}
