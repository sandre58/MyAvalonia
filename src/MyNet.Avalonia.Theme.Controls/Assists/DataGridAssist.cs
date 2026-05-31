// -----------------------------------------------------------------------
// <copyright file="DataGridAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Theme.Controls.Assists;

public static class DataGridAssist
{
    #region UseAlternateRowBackground

    /// <summary>
    /// Provides UseAlternateRowBackground Property for attached DataGridBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> UseAlternateRowBackgroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("UseAlternateRowBackground", typeof(DataGridAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="UseAlternateRowBackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="UseAlternateRowBackgroundProperty"/>.</param>
    public static void SetUseAlternateRowBackground(StyledElement element, bool value) => element.SetValue(UseAlternateRowBackgroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="UseAlternateRowBackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetUseAlternateRowBackground(StyledElement element) => element.GetValue(UseAlternateRowBackgroundProperty);

    #endregion

    #region ShowCellSelection

    /// <summary>
    /// Provides ShowCellSelection Property for attached DataGridBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> ShowCellSelectionProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("ShowCellSelection", typeof(DataGridAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ShowCellSelectionProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ShowCellSelectionProperty"/>.</param>
    public static void SetShowCellSelection(StyledElement element, bool value) => element.SetValue(ShowCellSelectionProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ShowCellSelectionProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetShowCellSelection(StyledElement element) => element.GetValue(ShowCellSelectionProperty);

    #endregion

    #region ShowSelection

    /// <summary>
    /// Provides ShowSelection Property for attached DataGridBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> ShowSelectionProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("ShowSelection", typeof(DataGridAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ShowSelectionProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ShowSelectionProperty"/>.</param>
    public static void SetShowSelection(StyledElement element, bool value) => element.SetValue(ShowSelectionProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ShowSelectionProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetShowSelection(StyledElement element) => element.GetValue(ShowSelectionProperty);

    #endregion

    #region AlternateRowBackground

    /// <summary>
    /// Provides AlternateRowBackground Property for attached DataGridBehavior element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> AlternateRowBackgroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("AlternateRowBackground", typeof(DataGridAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="AlternateRowBackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="AlternateRowBackgroundProperty"/>.</param>
    public static void SetAlternateRowBackground(StyledElement element, IBrush value) => element.SetValue(AlternateRowBackgroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="AlternateRowBackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush GetAlternateRowBackground(StyledElement element) => element.GetValue(AlternateRowBackgroundProperty);

    #endregion

    #region RowBorderBrush

    /// <summary>
    /// Provides RowBorderBrush Property for attached DataGridBehavior element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> RowBorderBrushProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("RowBorderBrush", typeof(DataGridAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="RowBorderBrushProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="RowBorderBrushProperty"/>.</param>
    public static void SetRowBorderBrush(StyledElement element, IBrush value) => element.SetValue(RowBorderBrushProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="RowBorderBrushProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush GetRowBorderBrush(StyledElement element) => element.GetValue(RowBorderBrushProperty);

    #endregion

    #region RowForeground

    /// <summary>
    /// Provides RowForeground Property for attached DataGridBehavior element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> RowForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("RowForeground", typeof(DataGridAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="RowForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="RowForegroundProperty"/>.</param>
    public static void SetRowForeground(StyledElement element, IBrush value) => element.SetValue(RowForegroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="RowForegroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush GetRowForeground(StyledElement element) => element.GetValue(RowForegroundProperty);

    #endregion

    #region RowBorderThickness

    /// <summary>
    /// Provides RowBorderThickness Property for attached DataGridBehavior element.
    /// </summary>
    public static readonly AttachedProperty<Thickness> RowBorderThicknessProperty = AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("RowBorderThickness", typeof(DataGridAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="RowBorderThicknessProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="RowBorderThicknessProperty"/>.</param>
    public static void SetRowBorderThickness(StyledElement element, Thickness value) => element.SetValue(RowBorderThicknessProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="RowBorderThicknessProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Thickness GetRowBorderThickness(StyledElement element) => element.GetValue(RowBorderThicknessProperty);

    #endregion

    #region RowShadowDepth

    /// <summary>
    /// Provides RowShadowDepth Property for attached DataGridBehavior element.
    /// </summary>
    public static readonly AttachedProperty<ShadowDepth> RowShadowDepthProperty = AvaloniaProperty.RegisterAttached<StyledElement, ShadowDepth>("RowShadowDepth", typeof(DataGridAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="RowShadowDepthProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="RowShadowDepthProperty"/>.</param>
    public static void SetRowShadowDepth(StyledElement element, ShadowDepth value) => element.SetValue(RowShadowDepthProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="RowShadowDepthProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static ShadowDepth GetRowShadowDepth(StyledElement element) => element.GetValue(RowShadowDepthProperty);

    #endregion

    #region RowCornerRadius

    /// <summary>
    /// Provides RowCornerRadius Property for attached DataGridBehavior element.
    /// </summary>
    public static readonly AttachedProperty<CornerRadius> RowCornerRadiusProperty = AvaloniaProperty.RegisterAttached<StyledElement, CornerRadius>("RowCornerRadius", typeof(DataGridAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="RowCornerRadiusProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="RowCornerRadiusProperty"/>.</param>
    public static void SetRowCornerRadius(StyledElement element, CornerRadius value) => element.SetValue(RowCornerRadiusProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="RowCornerRadiusProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static CornerRadius GetRowCornerRadius(StyledElement element) => element.GetValue(RowCornerRadiusProperty);

    #endregion

    #region RowMargin

    /// <summary>
    /// Provides RowMargin Property for attached DataGridBehavior element.
    /// </summary>
    public static readonly AttachedProperty<Thickness> RowMarginProperty = AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("RowMargin", typeof(DataGridAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="RowMarginProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="RowMarginProperty"/>.</param>
    public static void SetRowMargin(StyledElement element, Thickness value) => element.SetValue(RowMarginProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="RowMarginProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Thickness GetRowMargin(StyledElement element) => element.GetValue(RowMarginProperty);

    #endregion
}
