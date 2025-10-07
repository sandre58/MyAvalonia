// -----------------------------------------------------------------------
// <copyright file="HeaderAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Layout;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.Controls.Assists;

public static class HeaderAssist
{
    #region Background

    /// <summary>
    /// Provides Background Property for attached HeaderAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> BackgroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("Background", typeof(HeaderAssist));

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
    /// Provides Foreground Property for attached HeaderAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> ForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("Foreground", typeof(HeaderAssist));

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
    /// Provides BorderBrush Property for attached HeaderAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> BorderBrushProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("BorderBrush", typeof(HeaderAssist));

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
    /// Provides BorderThickness Property for attached HeaderAssist element.
    /// </summary>
    public static readonly AttachedProperty<Thickness> BorderThicknessProperty = AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("BorderThickness", typeof(HeaderAssist));

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

    #region CornerRadius

    /// <summary>
    /// Provides CornerRadius Property for attached HeaderAssist element.
    /// </summary>
    public static readonly AttachedProperty<CornerRadius> CornerRadiusProperty = AvaloniaProperty.RegisterAttached<StyledElement, CornerRadius>("CornerRadius", typeof(HeaderAssist));

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

    #region ShadowDepth

    public static readonly AvaloniaProperty<ShadowDepth> ShadowDepthProperty = AvaloniaProperty.RegisterAttached<StyledElement, ShadowDepth>("ShadowDepth", typeof(HeaderAssist));

    public static void SetShadowDepth(StyledElement element, ShadowDepth value) => element.SetValue(ShadowDepthProperty, value);

    public static ShadowDepth GetShadowDepth(StyledElement element) => element.GetValue<ShadowDepth>(ShadowDepthProperty);

    #endregion ShadowDepth

    #region Padding

    /// <summary>
    /// Provides Padding Property for attached HeaderAssist element.
    /// </summary>
    public static readonly AttachedProperty<Thickness> PaddingProperty = AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("Padding", typeof(HeaderAssist));

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

    #region Size

    /// <summary>
    /// Provides Size Property for attached HeaderAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> SizeProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("Size", typeof(HeaderAssist), double.NaN);

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

    #region FontSize

    /// <summary>
    /// Provides FontSize Property for attached HeaderAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> FontSizeProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("FontSize", typeof(HeaderAssist), 12.0);

    /// <summary>
    /// Accessor for Attached  <see cref="FontSizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="FontSizeProperty"/>.</param>
    public static void SetFontSize(StyledElement element, double value) => element.SetValue(FontSizeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="FontSizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetFontSize(StyledElement element) => element.GetValue(FontSizeProperty);

    #endregion

    #region FontFamily

    /// <summary>
    /// Provides FontFamily Property for attached HeaderAssist element.
    /// </summary>
    public static readonly AttachedProperty<FontFamily> FontFamilyProperty = AvaloniaProperty.RegisterAttached<StyledElement, FontFamily>("FontFamily", typeof(HeaderAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="FontFamilyProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="FontFamilyProperty"/>.</param>
    public static void SetFontFamily(StyledElement element, FontFamily value) => element.SetValue(FontFamilyProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="FontFamilyProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static FontFamily GetFontFamily(StyledElement element) => element.GetValue(FontFamilyProperty);

    #endregion

    #region FontWeight

    /// <summary>
    /// Provides FontWeight Property for attached HeaderAssist element.
    /// </summary>
    public static readonly AttachedProperty<FontWeight> FontWeightProperty = AvaloniaProperty.RegisterAttached<StyledElement, FontWeight>("FontWeight", typeof(HeaderAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="FontWeightProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="FontWeightProperty"/>.</param>
    public static void SetFontWeight(StyledElement element, FontWeight value) => element.SetValue(FontWeightProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="FontWeightProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static FontWeight GetFontWeight(StyledElement element) => element.GetValue(FontWeightProperty);

    #endregion

    #region FontStyle

    /// <summary>
    /// Provides FontStyle Property for attached HeaderAssist element.
    /// </summary>
    public static readonly AttachedProperty<FontStyle> FontStyleProperty = AvaloniaProperty.RegisterAttached<StyledElement, FontStyle>("FontStyle", typeof(HeaderAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="FontStyleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="FontStyleProperty"/>.</param>
    public static void SetFontStyle(StyledElement element, FontStyle value) => element.SetValue(FontStyleProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="FontStyleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static FontStyle GetFontStyle(StyledElement element) => element.GetValue(FontStyleProperty);

    #endregion

    #region Opacity

    /// <summary>
    /// Provides Opacity Property for attached HeaderAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> OpacityProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("Opacity", typeof(HeaderAssist), 1.0);

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

    #region HorizontalAlignment

    /// <summary>
    /// Provides HorizontalAlignment Property for attached HeaderAssist element.
    /// </summary>
    public static readonly AttachedProperty<HorizontalAlignment> HorizontalAlignmentProperty = AvaloniaProperty.RegisterAttached<StyledElement, HorizontalAlignment>("HorizontalAlignment", typeof(HeaderAssist));

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
    /// Provides VerticalAlignment Property for attached HeaderAssist element.
    /// </summary>
    public static readonly AttachedProperty<VerticalAlignment> VerticalAlignmentProperty = AvaloniaProperty.RegisterAttached<StyledElement, VerticalAlignment>("VerticalAlignment", typeof(HeaderAssist));

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

    #region IsVisible

    /// <summary>
    /// Provides IsVisible Property for attached HeaderAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> IsVisibleProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("IsVisible", typeof(HeaderAssist), true);

    /// <summary>
    /// Accessor for Attached  <see cref="IsVisibleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IsVisibleProperty"/>.</param>
    public static void SetIsVisible(StyledElement element, bool value) => element.SetValue(IsVisibleProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IsVisibleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetIsVisible(StyledElement element) => element.GetValue(IsVisibleProperty);

    #endregion
}
