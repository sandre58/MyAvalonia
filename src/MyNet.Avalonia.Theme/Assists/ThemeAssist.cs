// -----------------------------------------------------------------------
// <copyright file="ThemeAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls.Documents;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Theme.Assists;

public static class ThemeAssist
{
    static ThemeAssist() => TextElement.ForegroundProperty.Changed.AddClassHandler<AvaloniaObject>((ctrl, args) => SetInheritedForeground(ctrl, args.NewValue as IBrush));

    #region Role

    /// <summary>
    /// Defines the Role attached property for assigning a semantic color role to a control.
    /// </summary>
    public static readonly AttachedProperty<ThemeRole> RoleProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, ThemeRole>("Role", typeof(ThemeAssist), ThemeRole.Default);

    /// <summary>
    /// Gets the theme role for the specified control.
    /// </summary>
    /// <param name="element">The control to query.</param>
    /// <returns>The assigned theme role.</returns>
    public static ThemeRole GetRole(AvaloniaObject element) => element.GetValue(RoleProperty);

    /// <summary>
    /// Sets the theme role for the specified control.
    /// </summary>
    /// <param name="element">The control to update.</param>
    /// <param name="value">The theme role to assign.</param>
    public static void SetRole(AvaloniaObject element, ThemeRole value) => element.SetValue(RoleProperty, value);

    #endregion

    #region InheritedForeground

    /// <summary>
    /// Provides InheritedForeground Property for attached ThemeAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> InheritedForegroundProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, AvaloniaObject, IBrush?>("InheritedForeground", inherits: true);

    /// <summary>
    /// Accessor for Attached  <see cref="InheritedForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="InheritedForegroundProperty"/>.</param>
    public static void SetInheritedForeground(AvaloniaObject element, IBrush? value) => element.SetValue(InheritedForegroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="InheritedForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetInheritedForeground(AvaloniaObject element) => element.GetValue(InheritedForegroundProperty);

    #endregion

    #region HoverBackground

    /// <summary>
    /// Provides HoverBackground Property for attached ThemeAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> HoverBackgroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("HoverBackground", typeof(ThemeAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="HoverBackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="HoverBackgroundProperty"/>.</param>
    public static void SetHoverBackground(StyledElement element, IBrush? value) => element.SetValue(HoverBackgroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="HoverBackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetHoverBackground(StyledElement element) => element.GetValue(HoverBackgroundProperty);

    #endregion

    #region HoverBorderBrush

    /// <summary>
    /// Provides HoverBorderBrush Property for attached ThemeAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> HoverBorderBrushProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("HoverBorderBrush", typeof(ThemeAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="HoverBorderBrushProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="HoverBorderBrushProperty"/>.</param>
    public static void SetHoverBorderBrush(StyledElement element, IBrush? value) => element.SetValue(HoverBorderBrushProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="HoverBorderBrushProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetHoverBorderBrush(StyledElement element) => element.GetValue(HoverBorderBrushProperty);

    #endregion

    #region HoverForeground

    /// <summary>
    /// Provides HoverForeground Property for attached ThemeAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> HoverForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("HoverForeground", typeof(ThemeAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="HoverForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="HoverForegroundProperty"/>.</param>
    public static void SetHoverForeground(StyledElement element, IBrush? value) => element.SetValue(HoverForegroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="HoverForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetHoverForeground(StyledElement element) => element.GetValue(HoverForegroundProperty);

    #endregion

    #region ActiveBackground

    /// <summary>
    /// Provides ActiveBackground Property for attached ThemeAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> ActiveBackgroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("ActiveBackground", typeof(ThemeAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveBackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ActiveBackgroundProperty"/>.</param>
    public static void SetActiveBackground(StyledElement element, IBrush? value) => element.SetValue(ActiveBackgroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveBackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetActiveBackground(StyledElement element) => element.GetValue(ActiveBackgroundProperty);

    #endregion

    #region ActiveBorderBrush

    /// <summary>
    /// Provides ActiveBorderBrush Property for attached ThemeAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> ActiveBorderBrushProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("ActiveBorderBrush", typeof(ThemeAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveBorderBrushProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ActiveBorderBrushProperty"/>.</param>
    public static void SetActiveBorderBrush(StyledElement element, IBrush? value) => element.SetValue(ActiveBorderBrushProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveBorderBrushProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetActiveBorderBrush(StyledElement element) => element.GetValue(ActiveBorderBrushProperty);

    #endregion

    #region ActiveForeground

    /// <summary>
    /// Provides ActiveForeground Property for attached ThemeAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> ActiveForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("ActiveForeground", typeof(ThemeAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ActiveForegroundProperty"/>.</param>
    public static void SetActiveForeground(StyledElement element, IBrush? value) => element.SetValue(ActiveForegroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetActiveForeground(StyledElement element) => element.GetValue(ActiveForegroundProperty);

    #endregion

    #region RippleColor

    /// <summary>
    /// Provides RippleColor Property for attached ThemeAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> RippleColorProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("RippleColor", typeof(ThemeAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="RippleColorProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="RippleColorProperty"/>.</param>
    public static void SetRippleColor(StyledElement element, IBrush? value) => element.SetValue(RippleColorProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="RippleColorProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetRippleColor(StyledElement element) => element.GetValue(RippleColorProperty);

    #endregion

    #region IndicatorColor

    /// <summary>
    /// Provides IndicatorColor Property for attached ThemeAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> IndicatorColorProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("IndicatorColor", typeof(ThemeAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="IndicatorColorProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IndicatorColorProperty"/>.</param>
    public static void SetIndicatorColor(StyledElement element, IBrush value) => element.SetValue(IndicatorColorProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IndicatorColorProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush GetIndicatorColor(StyledElement element) => element.GetValue(IndicatorColorProperty);

    #endregion

    #region ActiveIndicatorColor

    /// <summary>
    /// Provides ActiveIndicatorColor Property for attached ThemeAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> ActiveIndicatorColorProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("ActiveIndicatorColor", typeof(ThemeAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveIndicatorColorProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ActiveIndicatorColorProperty"/>.</param>
    public static void SetActiveIndicatorColor(StyledElement element, IBrush value) => element.SetValue(ActiveIndicatorColorProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveIndicatorColorProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush GetActiveIndicatorColor(StyledElement element) => element.GetValue(ActiveIndicatorColorProperty);

    #endregion
}
