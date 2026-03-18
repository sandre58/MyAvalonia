// -----------------------------------------------------------------------
// <copyright file="VariantAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Media;

namespace MyNet.Avalonia.Theme.Assists;

public static class VariantAssist
{
    #region DefaultBackground

    /// <summary>
    /// Provides DefaultBackground Property for attached VariantAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> DefaultBackgroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>(
        "DefaultBackground",
        typeof(VariantAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="DefaultBackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="DefaultBackgroundProperty"/>.</param>
    public static void SetDefaultBackground(StyledElement element, IBrush value) => element.SetValue(DefaultBackgroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="DefaultBackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush GetDefaultBackground(StyledElement element) => element.GetValue(DefaultBackgroundProperty);

    #endregion

    #region DefaultBorderBrush

    /// <summary>
    /// Provides DefaultBorderBrush Property for attached VariantAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> DefaultBorderBrushProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>(
        "DefaultBorderBrush",
        typeof(VariantAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="DefaultBorderBrushProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="DefaultBorderBrushProperty"/>.</param>
    public static void SetDefaultBorderBrush(StyledElement element, IBrush value) => element.SetValue(DefaultBorderBrushProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="DefaultBorderBrushProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush GetDefaultBorderBrush(StyledElement element) => element.GetValue(DefaultBorderBrushProperty);

    #endregion

    #region DefaultForeground

    /// <summary>
    /// Provides DefaultForeground Property for attached VariantAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> DefaultForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>(
        "DefaultForeground",
        typeof(VariantAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="DefaultForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="DefaultForegroundProperty"/>.</param>
    public static void SetDefaultForeground(StyledElement element, IBrush value) => element.SetValue(DefaultForegroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="DefaultForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush GetDefaultForeground(StyledElement element) => element.GetValue(DefaultForegroundProperty);

    #endregion

    #region DefaultPrimary

    /// <summary>
    /// Provides DefaultPrimary Property for attached VariantAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> DefaultPrimaryProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>(
        "DefaultPrimary",
        typeof(VariantAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="DefaultPrimaryProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="DefaultPrimaryProperty"/>.</param>
    public static void SetDefaultPrimary(StyledElement element, IBrush value) => element.SetValue(DefaultPrimaryProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="DefaultPrimaryProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush GetDefaultPrimary(StyledElement element) => element.GetValue(DefaultPrimaryProperty);

    #endregion

    #region Background

    /// <summary>
    /// Provides Background Property for attached VariantAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> BackgroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("Background", typeof(VariantAssist));

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
    /// Provides Foreground Property for attached VariantAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> ForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("Foreground", typeof(VariantAssist));

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

    #region BorderBrush

    /// <summary>
    /// Provides BorderBrush Property for attached VariantAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> BorderBrushProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("BorderBrush", typeof(VariantAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="BorderBrushProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="BorderBrushProperty"/>.</param>
    public static void SetBorderBrush(StyledElement element, IBrush value) => element.SetValue(BorderBrushProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="BorderBrushProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush GetBorderBrush(StyledElement element) => element.GetValue(BorderBrushProperty);

    #endregion

    #region BorderThickness

    /// <summary>
    /// Provides BorderThickness Property for attached VariantAssist element.
    /// </summary>
    public static readonly AttachedProperty<Thickness> BorderThicknessProperty = AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("BorderThickness", typeof(VariantAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="BorderThicknessProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="BorderThicknessProperty"/>.</param>
    public static void SetBorderThickness(StyledElement element, Thickness value) => element.SetValue(BorderThicknessProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="BorderThicknessProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Thickness GetBorderThickness(StyledElement element) => element.GetValue(BorderThicknessProperty);

    #endregion

    #region Primary

    /// <summary>
    /// Provides Primary Property for attached VariantAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> PrimaryProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, IBrush>("Primary", typeof(VariantAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="PrimaryProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PrimaryProperty"/>.</param>
    public static void SetPrimary(AvaloniaObject element, IBrush value) => element.SetValue(PrimaryProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PrimaryProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush GetPrimary(AvaloniaObject element) => element.GetValue(PrimaryProperty);

    #endregion
}
