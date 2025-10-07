// -----------------------------------------------------------------------
// <copyright file="TextFieldAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Media;

namespace MyNet.Avalonia.Controls.Assists;

public static class TextFieldAssist
{
    #region Watermark

    /// <summary>
    /// Provides Watermark Property for attached TextFieldAssist element.
    /// </summary>
    public static readonly AttachedProperty<string?> WatermarkProperty = AvaloniaProperty.RegisterAttached<StyledElement, string?>("Watermark", typeof(TextFieldAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="WatermarkProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="WatermarkProperty"/>.</param>
    public static void SetWatermark(StyledElement element, string? value) => element.SetValue(WatermarkProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="WatermarkProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string? GetWatermark(StyledElement element) => element.GetValue(WatermarkProperty);

    #endregion

    #region UseFloatingWatermark

    /// <summary>
    /// Provides UseFloatingWatermark Property for attached TextFieldAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> UseFloatingWatermarkProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("UseFloatingWatermark", typeof(TextFieldAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="UseFloatingWatermarkProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="UseFloatingWatermarkProperty"/>.</param>
    public static void SetUseFloatingWatermark(StyledElement element, bool value) => element.SetValue(UseFloatingWatermarkProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="UseFloatingWatermarkProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetUseFloatingWatermark(StyledElement element) => element.GetValue(UseFloatingWatermarkProperty);

    #endregion

    #region FloatingScale

    /// <summary>
    /// Provides FloatingScale Property for attached TextFieldAssist element.
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
    /// Provides FloatingOffset Property for attached TextFieldAssist element.
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

    #region WatermarkForeground

    /// <summary>
    /// Provides WatermarkForeground Property for attached TextFieldAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> WatermarkForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("WatermarkForeground", typeof(TextFieldAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="WatermarkForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="WatermarkForegroundProperty"/>.</param>
    public static void SetWatermarkForeground(StyledElement element, IBrush value) => element.SetValue(WatermarkForegroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="WatermarkForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush GetWatermarkForeground(StyledElement element) => element.GetValue(WatermarkForegroundProperty);

    #endregion

    #region WatermarkFontSize

    /// <summary>
    /// Provides WatermarkFontSize Property for attached TextFieldAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> WatermarkFontSizeProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("WatermarkFontSize", typeof(TextFieldAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="WatermarkFontSizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="WatermarkFontSizeProperty"/>.</param>
    public static void SetWatermarkFontSize(StyledElement element, double value) => element.SetValue(WatermarkFontSizeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="WatermarkFontSizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetWatermarkFontSize(StyledElement element) => element.GetValue(WatermarkFontSizeProperty);

    #endregion

    #region InnerLeftContent

    /// <summary>
    /// Provides InnerLeftContent Property for attached TextFieldAssist element.
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
    /// Provides InnerRightContent Property for attached TextFieldAssist element.
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
    /// Provides InnerForeground Property for attached TextFieldAssist element.
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
    /// Provides InnerFontSize Property for attached TextFieldAssist element.
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
    /// Provides InnerPadding Property for attached TextFieldAssist element.
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
    /// Provides UnderText Property for attached TextFieldAssist element.
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
    /// Provides UnderForeground Property for attached TextFieldAssist element.
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
    /// Provides UnderFontSize Property for attached TextFieldAssist element.
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
    /// Provides UnderFontWeight Property for attached TextFieldAssist element.
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
    /// Provides UnderFontStyle Property for attached TextFieldAssist element.
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
    /// Provides ShowClearButton Property for attached TextFieldAssist element.
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
    /// Provides ShowRevealButton Property for attached TextFieldAssist element.
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
    /// Provides ShowClipboardButton Property for attached TextFieldAssist element.
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

    #region IsEditable

    /// <summary>
    /// Provides IsEditable Property for attached TextFieldAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> IsEditableProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("IsEditable", typeof(TextFieldAssist), true);

    /// <summary>
    /// Accessor for Attached  <see cref="IsEditableProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IsEditableProperty"/>.</param>
    public static void SetIsEditable(StyledElement element, bool value) => element.SetValue(IsEditableProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IsEditableProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetIsEditable(StyledElement element) => element.GetValue(IsEditableProperty);

    #endregion
}
