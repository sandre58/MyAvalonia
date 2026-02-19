// -----------------------------------------------------------------------
// <copyright file="TextFieldAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Media;

namespace MyNet.Avalonia.Theme.Assists;

public static class TextFieldAssist
{
    #region PlaceholderText

    /// <summary>
    /// Provides PlaceholderText Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<string?> PlaceholderTextProperty = AvaloniaProperty.RegisterAttached<StyledElement, string?>("PlaceholderText", typeof(TextFieldAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderTextProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PlaceholderTextProperty"/>.</param>
    public static void SetPlaceholderText(StyledElement element, string? value) => element.SetValue(PlaceholderTextProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderTextProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string? GetPlaceholderText(StyledElement element) => element.GetValue(PlaceholderTextProperty);

    #endregion

    #region UseFloatingPlaceholder

    /// <summary>
    /// Provides UseFloatingPlaceholder Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> UseFloatingPlaceholderProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("UseFloatingPlaceholder", typeof(TextFieldAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="UseFloatingPlaceholderProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="UseFloatingPlaceholderProperty"/>.</param>
    public static void SetUseFloatingPlaceholder(StyledElement element, bool value) => element.SetValue(UseFloatingPlaceholderProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="UseFloatingPlaceholderProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetUseFloatingPlaceholder(StyledElement element) => element.GetValue(UseFloatingPlaceholderProperty);

    #endregion

    #region FloatingScale

    /// <summary>
    /// Provides FloatingScale Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<double> FloatingScaleProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("FloatingScale", typeof(TextFieldAssist), 0.75d);

    /// <summary>
    /// Accessor for Attached  <see cref="FloatingScaleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="FloatingScaleProperty"/>.</param>
    public static void SetFloatingScale(StyledElement element, double value) => element.SetValue(FloatingScaleProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="FloatingScaleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetFloatingScale(StyledElement element) => element.GetValue(FloatingScaleProperty);

    #endregion

    #region FloatingOffset

    /// <summary>
    /// Provides FloatingOffset Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<double> FloatingOffsetProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("FloatingOffset", typeof(TextFieldAssist), 12.0d);

    /// <summary>
    /// Accessor for Attached  <see cref="FloatingOffsetProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="FloatingOffsetProperty"/>.</param>
    public static void SetFloatingOffset(StyledElement element, double value) => element.SetValue(FloatingOffsetProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="FloatingOffsetProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetFloatingOffset(StyledElement element) => element.GetValue(FloatingOffsetProperty);

    #endregion

    #region PlaceholderForeground

    /// <summary>
    /// Provides PlaceholderForeground Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> PlaceholderForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("PlaceholderForeground", typeof(TextFieldAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PlaceholderForegroundProperty"/>.</param>
    public static void SetPlaceholderForeground(StyledElement element, IBrush value) => element.SetValue(PlaceholderForegroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush GetPlaceholderForeground(StyledElement element) => element.GetValue(PlaceholderForegroundProperty);

    #endregion

    #region PlaceholderFontSize

    /// <summary>
    /// Provides PlaceholderFontSize Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<double> PlaceholderFontSizeProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("PlaceholderFontSize", typeof(TextFieldAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderFontSizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PlaceholderFontSizeProperty"/>.</param>
    public static void SetPlaceholderFontSize(StyledElement element, double value) => element.SetValue(PlaceholderFontSizeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderFontSizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetPlaceholderFontSize(StyledElement element) => element.GetValue(PlaceholderFontSizeProperty);

    #endregion

    #region InnerLeftContent

    /// <summary>
    /// Provides InnerLeftContent Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<object?> InnerLeftContentProperty = AvaloniaProperty.RegisterAttached<StyledElement, object?>("InnerLeftContent", typeof(TextFieldAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="InnerLeftContentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="InnerLeftContentProperty"/>.</param>
    public static void SetInnerLeftContent(StyledElement element, object? value) => element.SetValue(InnerLeftContentProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="InnerLeftContentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static object? GetInnerLeftContent(StyledElement element) => element.GetValue(InnerLeftContentProperty);

    #endregion

    #region InnerRightContent

    /// <summary>
    /// Provides InnerRightContent Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<object?> InnerRightContentProperty = AvaloniaProperty.RegisterAttached<StyledElement, object?>("InnerRightContent", typeof(TextFieldAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="InnerRightContentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="InnerRightContentProperty"/>.</param>
    public static void SetInnerRightContent(StyledElement element, object? value) => element.SetValue(InnerRightContentProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="InnerRightContentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static object? GetInnerRightContent(StyledElement element) => element.GetValue(InnerRightContentProperty);

    #endregion

    #region InnerForeground

    /// <summary>
    /// Provides InnerForeground Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> InnerForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("InnerForeground", typeof(TextFieldAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="InnerForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="InnerForegroundProperty"/>.</param>
    public static void SetInnerForeground(StyledElement element, IBrush? value) => element.SetValue(InnerForegroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="InnerForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetInnerForeground(StyledElement element) => element.GetValue(InnerForegroundProperty);

    #endregion

    #region InnerFontSize

    /// <summary>
    /// Provides InnerFontSize Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<double> InnerFontSizeProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("InnerFontSize", typeof(TextFieldAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="InnerFontSizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="InnerFontSizeProperty"/>.</param>
    public static void SetInnerFontSize(StyledElement element, double value) => element.SetValue(InnerFontSizeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="InnerFontSizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetInnerFontSize(StyledElement element) => element.GetValue(InnerFontSizeProperty);

    #endregion

    #region InnerPadding

    /// <summary>
    /// Provides InnerPadding Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<Thickness> InnerPaddingProperty = AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("InnerPadding", typeof(TextFieldAssist), new Thickness(0));

    /// <summary>
    /// Accessor for Attached  <see cref="InnerPaddingProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="InnerPaddingProperty"/>.</param>
    public static void SetInnerPadding(StyledElement element, Thickness value) => element.SetValue(InnerPaddingProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="InnerPaddingProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Thickness GetInnerPadding(StyledElement element) => element.GetValue(InnerPaddingProperty);

    #endregion

    #region UnderText

    /// <summary>
    /// Provides UnderText Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<string> UnderTextProperty = AvaloniaProperty.RegisterAttached<StyledElement, string>("UnderText", typeof(TextFieldAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="UnderTextProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="UnderTextProperty"/>.</param>
    public static void SetUnderText(StyledElement element, string value) => element.SetValue(UnderTextProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="UnderTextProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string GetUnderText(StyledElement element) => element.GetValue(UnderTextProperty);

    #endregion

    #region UnderForeground

    /// <summary>
    /// Provides UnderForeground Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> UnderForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("UnderForeground", typeof(TextFieldAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="UnderForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="UnderForegroundProperty"/>.</param>
    public static void SetUnderForeground(StyledElement element, IBrush? value) => element.SetValue(UnderForegroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="UnderForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetUnderForeground(StyledElement element) => element.GetValue(UnderForegroundProperty);

    #endregion

    #region UnderFontSize

    /// <summary>
    /// Provides UnderFontSize Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<double> UnderFontSizeProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("UnderFontSize", typeof(TextFieldAssist), 10.0d);

    /// <summary>
    /// Accessor for Attached  <see cref="UnderFontSizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="UnderFontSizeProperty"/>.</param>
    public static void SetUnderFontSize(StyledElement element, double value) => element.SetValue(UnderFontSizeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="UnderFontSizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetUnderFontSize(StyledElement element) => element.GetValue(UnderFontSizeProperty);

    #endregion

    #region UnderFontWeight

    /// <summary>
    /// Provides UnderFontWeight Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<FontWeight> UnderFontWeightProperty = AvaloniaProperty.RegisterAttached<StyledElement, FontWeight>("UnderFontWeight", typeof(TextFieldAssist), FontWeight.Normal);

    /// <summary>
    /// Accessor for Attached  <see cref="UnderFontWeightProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="UnderFontWeightProperty"/>.</param>
    public static void SetUnderFontWeight(StyledElement element, FontWeight value) => element.SetValue(UnderFontWeightProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="UnderFontWeightProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static FontWeight GetUnderFontWeight(StyledElement element) => element.GetValue(UnderFontWeightProperty);

    #endregion

    #region UnderFontStyle

    /// <summary>
    /// Provides UnderFontStyle Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<FontStyle> UnderFontStyleProperty = AvaloniaProperty.RegisterAttached<StyledElement, FontStyle>("UnderFontStyle", typeof(TextFieldAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="UnderFontStyleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="UnderFontStyleProperty"/>.</param>
    public static void SetUnderFontStyle(StyledElement element, FontStyle value) => element.SetValue(UnderFontStyleProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="UnderFontStyleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static FontStyle GetUnderFontStyle(StyledElement element) => element.GetValue(UnderFontStyleProperty);

    #endregion

    #region ShowClearButton

    /// <summary>
    /// Provides ShowClearButton Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> ShowClearButtonProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("ShowClearButton", typeof(TextFieldAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ShowClearButtonProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ShowClearButtonProperty"/>.</param>
    public static void SetShowClearButton(StyledElement element, bool value) => element.SetValue(ShowClearButtonProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ShowClearButtonProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetShowClearButton(StyledElement element) => element.GetValue(ShowClearButtonProperty);

    #endregion

    #region ShowRevealButton

    /// <summary>
    /// Provides ShowRevealButton Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> ShowRevealButtonProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("ShowRevealButton", typeof(TextFieldAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ShowRevealButtonProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ShowRevealButtonProperty"/>.</param>
    public static void SetShowRevealButton(StyledElement element, bool value) => element.SetValue(ShowRevealButtonProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ShowRevealButtonProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetShowRevealButton(StyledElement element) => element.GetValue(ShowRevealButtonProperty);

    #endregion

    #region ShowClipboardButton

    /// <summary>
    /// Provides ShowClipboardButton Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> ShowClipboardButtonProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("ShowClipboardButton", typeof(TextFieldAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ShowClipboardButtonProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ShowClipboardButtonProperty"/>.</param>
    public static void SetShowClipboardButton(StyledElement element, bool value) => element.SetValue(ShowClipboardButtonProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ShowClipboardButtonProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetShowClipboardButton(StyledElement element) => element.GetValue(ShowClipboardButtonProperty);

    #endregion
}
