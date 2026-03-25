// -----------------------------------------------------------------------
// <copyright file="ItemsAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Theming.Core;

namespace MyNet.Avalonia.Theme.Assists;

public static class ItemsAssist
{
    static ItemsAssist() => RoleProperty.Changed.AddClassHandler<AvaloniaObject>(OnRoleChangedCallback);

    #region Background

    /// <summary>
    /// Provides Background Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> BackgroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("Background", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="BackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="BackgroundProperty"/>.</param>
    public static void SetBackground(StyledElement element, IBrush? value) => element.SetValue(BackgroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="BackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetBackground(StyledElement element) => element.GetValue(BackgroundProperty);

    #endregion

    #region Foreground

    /// <summary>
    /// Provides Foreground Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> ForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("Foreground", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ForegroundProperty"/>.</param>
    public static void SetForeground(StyledElement element, IBrush? value) => element.SetValue(ForegroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="BackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetForeground(StyledElement element) => element.GetValue(ForegroundProperty);

    #endregion

    #region BorderBrush

    /// <summary>
    /// Provides BorderBrush Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> BorderBrushProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("BorderBrush", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="BorderBrushProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="BorderBrushProperty"/>.</param>
    public static void SetBorderBrush(StyledElement element, IBrush? value) => element.SetValue(BorderBrushProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="BorderBrushProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetBorderBrush(StyledElement element) => element.GetValue(BorderBrushProperty);

    #endregion

    #region BorderThickness

    /// <summary>
    /// Provides BorderThickness Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<Thickness> BorderThicknessProperty = AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("BorderThickness", typeof(ItemsAssist), new Thickness(0));

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

    #region Opacity

    /// <summary>
    /// Provides Opacity Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> OpacityProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("Opacity", typeof(ItemsAssist), 1.0D);

    /// <summary>
    /// Accessor for Attached  <see cref="OpacityProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="OpacityProperty"/>.</param>
    public static void SetOpacity(StyledElement element, double value) => element.SetValue(OpacityProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="OpacityProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetOpacity(StyledElement element) => element.GetValue(OpacityProperty);

    #endregion

    #region HoverBackground

    /// <summary>
    /// Provides HoverBackground Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> HoverBackgroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("HoverBackground", typeof(ItemsAssist));

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
    /// Provides HoverBorderBrush Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> HoverBorderBrushProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("HoverBorderBrush", typeof(ItemsAssist));

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
    /// Provides HoverForeground Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> HoverForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("HoverForeground", typeof(ItemsAssist));

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

    #region HoverOpacity

    /// <summary>
    /// Provides HoverOpacity Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> HoverOpacityProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("HoverOpacity", typeof(ItemsAssist), 1.0D);

    /// <summary>
    /// Accessor for Attached  <see cref="HoverOpacityProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="HoverOpacityProperty"/>.</param>
    public static void SetHoverOpacity(StyledElement element, double value) => element.SetValue(HoverOpacityProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="HoverOpacityProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetHoverOpacity(StyledElement element) => element.GetValue(HoverOpacityProperty);

    #endregion

    #region ActiveBackground

    /// <summary>
    /// Provides ActiveBackground Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> ActiveBackgroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("ActiveBackground", typeof(ItemsAssist));

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
    /// Provides ActiveBorderBrush Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> ActiveBorderBrushProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("ActiveBorderBrush", typeof(ItemsAssist));

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
    /// Provides ActiveForeground Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> ActiveForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("ActiveForeground", typeof(ItemsAssist));

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

    #region ActiveOpacity

    /// <summary>
    /// Provides ActiveOpacity Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> ActiveOpacityProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("ActiveOpacity", typeof(ItemsAssist), 1.0D);

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveOpacityProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ActiveOpacityProperty"/>.</param>
    public static void SetActiveOpacity(StyledElement element, double value) => element.SetValue(ActiveOpacityProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveOpacityProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetActiveOpacity(StyledElement element) => element.GetValue(ActiveOpacityProperty);

    #endregion

    #region RippleColor

    /// <summary>
    /// Provides RippleColor Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> RippleColorProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("RippleColor", typeof(ItemsAssist));

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

    #region CornerRadius

    /// <summary>
    /// Provides CornerRadius Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<CornerRadius> CornerRadiusProperty = AvaloniaProperty.RegisterAttached<StyledElement, CornerRadius>("CornerRadius", typeof(ItemsAssist), new CornerRadius(0));

    /// <summary>
    /// Accessor for Attached  <see cref="CornerRadiusProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="CornerRadiusProperty"/>.</param>
    public static void SetCornerRadius(StyledElement element, CornerRadius value) => element.SetValue(CornerRadiusProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="CornerRadiusProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static CornerRadius GetCornerRadius(StyledElement element) => element.GetValue(CornerRadiusProperty);

    #endregion

    #region Padding

    /// <summary>
    /// Provides Padding Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<Thickness> PaddingProperty = AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("Padding", typeof(ItemsAssist), new Thickness(0));

    /// <summary>
    /// Accessor for Attached  <see cref="PaddingProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PaddingProperty"/>.</param>
    public static void SetPadding(StyledElement element, Thickness value) => element.SetValue(PaddingProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PaddingProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Thickness GetPadding(StyledElement element) => element.GetValue(PaddingProperty);

    #endregion

    #region Margin

    /// <summary>
    /// Provides Margin Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<Thickness> MarginProperty = AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("Margin", typeof(ItemsAssist), new Thickness(0));

    /// <summary>
    /// Accessor for Attached  <see cref="MarginProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="MarginProperty"/>.</param>
    public static void SetMargin(StyledElement element, Thickness value) => element.SetValue(MarginProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="MarginProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Thickness GetMargin(StyledElement element) => element.GetValue(MarginProperty);

    #endregion

    #region Spacing

    /// <summary>
    /// Provides Spacing Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<double> SpacingProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("Spacing", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="SpacingProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="SpacingProperty"/>.</param>
    public static void SetSpacing(StyledElement element, double value) => element.SetValue(SpacingProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="SpacingProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetSpacing(StyledElement element) => element.GetValue(SpacingProperty);

    #endregion

    #region HorizontalAlignment

    /// <summary>
    /// Provides HorizontalAlignment Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<HorizontalAlignment> HorizontalAlignmentProperty = AvaloniaProperty.RegisterAttached<StyledElement, HorizontalAlignment>("HorizontalAlignment", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="HorizontalAlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="HorizontalAlignmentProperty"/>.</param>
    public static void SetHorizontalAlignment(StyledElement element, HorizontalAlignment value) => element.SetValue(HorizontalAlignmentProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="HorizontalAlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static HorizontalAlignment GetHorizontalAlignment(StyledElement element) => element.GetValue(HorizontalAlignmentProperty);

    #endregion

    #region VerticalAlignment

    /// <summary>
    /// Provides VerticalContentAlignment Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<VerticalAlignment> VerticalAlignmentProperty = AvaloniaProperty.RegisterAttached<StyledElement, VerticalAlignment>("VerticalAlignment", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="VerticalAlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="VerticalAlignmentProperty"/>.</param>
    public static void SetVerticalAlignment(StyledElement element, VerticalAlignment value) => element.SetValue(VerticalAlignmentProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="VerticalAlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static VerticalAlignment GetVerticalAlignment(StyledElement element) => element.GetValue(VerticalAlignmentProperty);

    #endregion

    #region HorizontalAlignment

    /// <summary>
    /// Provides HorizontalContentAlignment Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<HorizontalAlignment> HorizontalContentAlignmentProperty = AvaloniaProperty.RegisterAttached<StyledElement, HorizontalAlignment>("HorizontalAlignment", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="HorizontalAlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="HorizontalAlignmentProperty"/>.</param>
    public static void SetHorizontalContentAlignment(StyledElement element, HorizontalAlignment value) => element.SetValue(HorizontalContentAlignmentProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="HorizontalContentAlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static HorizontalAlignment GetHorizontalContentAlignment(StyledElement element) => element.GetValue(HorizontalContentAlignmentProperty);

    #endregion

    #region VerticalContentAlignment

    /// <summary>
    /// Provides VerticalAlignment Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<VerticalAlignment> VerticalContentAlignmentProperty = AvaloniaProperty.RegisterAttached<StyledElement, VerticalAlignment>("VerticalContentAlignment", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="VerticalContentAlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="VerticalAlignmentProperty"/>.</param>
    public static void SetVerticalContentAlignment(StyledElement element, VerticalAlignment value) => element.SetValue(VerticalContentAlignmentProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="VerticalContentAlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static VerticalAlignment GetVerticalContentAlignment(StyledElement element) => element.GetValue(VerticalContentAlignmentProperty);

    #endregion

    #region ShadowDepth

    public static readonly AvaloniaProperty<ShadowDepth> ShadowDepthProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, ShadowDepth>("ShadowDepth", typeof(ItemsAssist));

    public static void SetShadowDepth(AvaloniaObject element, ShadowDepth value) => element.SetValue(ShadowDepthProperty, value);

    public static ShadowDepth GetShadowDepth(AvaloniaObject element) => element.GetValue<ShadowDepth>(ShadowDepthProperty);

    #endregion ShadowDepth

    #region Size

    /// <summary>
    /// Provides Size Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<double> SizeProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("Size", typeof(ItemsAssist), 55.0d);

    /// <summary>
    /// Accessor for Attached  <see cref="SizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="SizeProperty"/>.</param>
    public static void SetSize(StyledElement element, double value) => element.SetValue(SizeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="SizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetSize(StyledElement element) => element.GetValue(SizeProperty);

    #endregion

    #region IndicatorPlacement

    /// <summary>
    /// Provides IndicatorPlacement Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<Dock> IndicatorPlacementProperty = AvaloniaProperty.RegisterAttached<StyledElement, Dock>("IndicatorPlacement", typeof(ItemsAssist), Dock.Bottom);

    /// <summary>
    /// Accessor for Attached  <see cref="IndicatorPlacementProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IndicatorPlacementProperty"/>.</param>
    public static void SetIndicatorPlacement(StyledElement element, Dock value) => element.SetValue(IndicatorPlacementProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IndicatorPlacementProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Dock GetIndicatorPlacement(StyledElement element) => element.GetValue(IndicatorPlacementProperty);

    #endregion

    #region IndicatorLength

    /// <summary>
    /// Provides IndicatorLength Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> IndicatorLengthProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("IndicatorLength", typeof(ItemsAssist), 20.0d);

    /// <summary>
    /// Accessor for Attached  <see cref="IndicatorLengthProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IndicatorLengthProperty"/>.</param>
    public static void SetIndicatorLength(StyledElement element, double value) => element.SetValue(IndicatorLengthProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IndicatorLengthProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetIndicatorLength(StyledElement element) => element.GetValue(IndicatorLengthProperty);

    #endregion

    #region IndicatorThickness

    /// <summary>
    /// Provides IndicatorThickness Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> IndicatorThicknessProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("IndicatorThickness", typeof(ItemsAssist), 3.0d);

    /// <summary>
    /// Accessor for Attached  <see cref="IndicatorThicknessProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IndicatorThicknessProperty"/>.</param>
    public static void SetIndicatorThickness(StyledElement element, double value) => element.SetValue(IndicatorThicknessProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IndicatorThicknessProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetIndicatorThickness(StyledElement element) => element.GetValue(IndicatorThicknessProperty);

    #endregion

    #region IndicatorMargin

    /// <summary>
    /// Provides IndicatorMargin Property for attached ItemsAssist element.
    /// </summary>
    public static readonly AttachedProperty<Thickness> IndicatorMarginProperty = AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("IndicatorMargin", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="IndicatorMarginProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IndicatorMarginProperty"/>.</param>
    public static void SetIndicatorMargin(StyledElement element, Thickness value) => element.SetValue(IndicatorMarginProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IndicatorMarginProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Thickness GetIndicatorMargin(StyledElement element) => element.GetValue(IndicatorMarginProperty);

    #endregion

    #region Role

    /// <summary>
    /// Defines the Role attached property for assigning a semantic color role to a control.
    /// </summary>
    public static readonly AttachedProperty<ThemeRole> RoleProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, ThemeRole>("Role", typeof(ItemsAssist));

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

    private static void OnRoleChangedCallback(AvaloniaObject avaloniaObject, AvaloniaPropertyChangedEventArgs args)
    {
        if (avaloniaObject is StyledElement c)
        {
            SetHasRole(c, args.NewValue is ThemeRole role && role != ThemeRole.Default);
        }
    }

    #endregion

    #region HasRole

    /// <summary>
    /// Provides HasRole Property for attached ThemeAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> HasRoleProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("HasRole", typeof(ItemsAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="HasRoleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="HasRoleProperty"/>.</param>
    private static void SetHasRole(StyledElement element, bool value) => element.SetValue(HasRoleProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="HasRoleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetHasRole(StyledElement element) => element.GetValue(HasRoleProperty);

    #endregion
}
