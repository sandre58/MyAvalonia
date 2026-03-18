// -----------------------------------------------------------------------
// <copyright file="WindowAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Media;

namespace MyNet.Avalonia.Theme.Assists;

public static class WindowAssist
{
    // ------------------------------------------------------------------
    // Title bar
    // ------------------------------------------------------------------
    public static readonly AttachedProperty<double> TitleBarHeightProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, double>(
            "TitleBarHeight",
            typeof(WindowAssist),
            40,
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

    public static readonly AttachedProperty<object?> RightTitleBarContentProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, object?>(
            "RightTitleBarContent",
            typeof(WindowAssist),
            inherits: true);

    public static object? GetRightTitleBarContent(StyledElement element) => element.GetValue(RightTitleBarContentProperty);

    public static void SetRightTitleBarContent(StyledElement element, object? value) => element.SetValue(RightTitleBarContentProperty, value);

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
            false,
            inherits: true);

    public static bool GetShowFullScreenButton(StyledElement element) => element.GetValue(ShowFullScreenButtonProperty);

    public static void SetShowFullScreenButton(StyledElement element, bool value) => element.SetValue(ShowFullScreenButtonProperty, value);

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
            false,
            inherits: true);

    public static bool GetExtendContentIntoTitleBar(StyledElement element) => element.GetValue(ExtendContentIntoTitleBarProperty);

    public static void SetExtendContentIntoTitleBar(StyledElement element, bool value) => element.SetValue(ExtendContentIntoTitleBarProperty, value);
}
