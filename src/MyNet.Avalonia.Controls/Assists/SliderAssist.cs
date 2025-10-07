// -----------------------------------------------------------------------
// <copyright file="SliderAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Styling;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.Controls.Assists;

public static class SliderAssist
{
    #region BarSize

    /// <summary>
    /// Provides BarSize Property for attached SliderAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> BarSizeProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("BarSize", typeof(SliderAssist), 6.0d);

    /// <summary>
    /// Accessor for Attached  <see cref="BarSizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="BarSizeProperty"/>.</param>
    public static void SetBarSize(StyledElement element, double value) => element.SetValue(BarSizeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="BarSizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetBarSize(StyledElement element) => element.GetValue(BarSizeProperty);

    #endregion

    #region ThumbSize

    /// <summary>
    /// Provides ThumbSize Property for attached SliderAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> ThumbSizeProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("ThumbSize", typeof(SliderAssist), 16.0);

    /// <summary>
    /// Accessor for Attached  <see cref="ThumbSizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ThumbSizeProperty"/>.</param>
    public static void SetThumbSize(StyledElement element, double value) => element.SetValue(ThumbSizeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ThumbSizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetThumbSize(StyledElement element) => element.GetValue(ThumbSizeProperty);

    #endregion

    #region TickLength

    /// <summary>
    /// Provides TickLength Property for attached SliderAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> TickLengthProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("TickLength", typeof(SliderAssist), 4.0d);

    /// <summary>
    /// Accessor for Attached  <see cref="TickLengthProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="TickLengthProperty"/>.</param>
    public static void SetTickLength(StyledElement element, double value) => element.SetValue(TickLengthProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="TickLengthProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetTickLength(StyledElement element) => element.GetValue(TickLengthProperty);

    #endregion

    #region TickMode

    /// <summary>
    /// Provides TickMode Property for attached SliderAssist element.
    /// </summary>
    public static readonly AttachedProperty<TickMode> TickModeProperty = AvaloniaProperty.RegisterAttached<StyledElement, TickMode>("TickMode", typeof(SliderAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="TickModeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="TickModeProperty"/>.</param>
    public static void SetTickMode(StyledElement element, TickMode value) => element.SetValue(TickModeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="TickModeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static TickMode GetTickMode(StyledElement element) => element.GetValue(TickModeProperty);

    #endregion

    #region ShowValueOnMouseOver

    /// <summary>
    /// Provides ShowValueOnMouseOver Property for attached SliderAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> ShowValueOnMouseOverProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("ShowValueOnMouseOver", typeof(SliderAssist), true);

    /// <summary>
    /// Accessor for Attached  <see cref="ShowValueOnMouseOverProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ShowValueOnMouseOverProperty"/>.</param>
    public static void SetShowValueOnMouseOver(StyledElement element, bool value) => element.SetValue(ShowValueOnMouseOverProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ShowValueOnMouseOverProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetShowValueOnMouseOver(StyledElement element) => element.GetValue(ShowValueOnMouseOverProperty);

    #endregion

    #region ThumbTheme

    /// <summary>
    /// Provides ThumbTheme Property for attached SliderAssist element.
    /// </summary>
    public static readonly AttachedProperty<ControlTheme> ThumbThemeProperty = AvaloniaProperty.RegisterAttached<StyledElement, ControlTheme>("ThumbTheme", typeof(SliderAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ThumbThemeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ThumbThemeProperty"/>.</param>
    public static void SetThumbTheme(StyledElement element, ControlTheme value) => element.SetValue(ThumbThemeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ThumbThemeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static ControlTheme GetThumbTheme(StyledElement element) => element.GetValue(ThumbThemeProperty);

    #endregion
}
