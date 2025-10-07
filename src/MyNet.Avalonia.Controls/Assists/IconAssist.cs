// -----------------------------------------------------------------------
// <copyright file="IconAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls.Templates;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.Controls.Assists;

public static class IconAssist
{
    #region Icon

    /// <summary>
    /// Provides Icon Property for attached IconAssist element.
    /// </summary>
    public static readonly AttachedProperty<object?> IconProperty = AvaloniaProperty.RegisterAttached<StyledElement, object?>("Icon", typeof(IconAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="IconProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IconProperty"/>.</param>
    public static void SetIcon(StyledElement element, object? value) => element.SetValue(IconProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IconProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static object? GetIcon(StyledElement element) => element.GetValue(IconProperty);

    #endregion

    #region IconTemplate

    /// <summary>
    /// Provides IconTemplate Property for attached IconAssist element.
    /// </summary>
    public static readonly AttachedProperty<IDataTemplate> IconTemplateProperty = AvaloniaProperty.RegisterAttached<StyledElement, IDataTemplate>("IconTemplate", typeof(IconAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="IconTemplateProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IconTemplateProperty"/>.</param>
    public static void SetIconTemplate(StyledElement element, IDataTemplate value) => element.SetValue(IconTemplateProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IconTemplateProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IDataTemplate GetIconTemplate(StyledElement element) => element.GetValue(IconTemplateProperty);

    #endregion

    #region Opacity

    /// <summary>
    /// Provides Opacity Property for attached IconAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> OpacityProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("Opacity", typeof(IconAssist), 0.7);

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

    #region Margin

    /// <summary>
    /// Provides Margin Property for attached IconAssist element.
    /// </summary>
    public static readonly AttachedProperty<Thickness> MarginProperty = AvaloniaProperty.RegisterAttached<StyledElement, Thickness>("Margin", typeof(IconAssist), new Thickness(0, 0, 5, 0));

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

    #region Alignment

    /// <summary>
    /// Provides Alignment Property for attached IconAssist element.
    /// </summary>
    public static readonly AttachedProperty<Position> AlignmentProperty = AvaloniaProperty.RegisterAttached<StyledElement, Position>("Alignment", typeof(IconAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="AlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="AlignmentProperty"/>.</param>
    public static void SetAlignment(StyledElement element, Position value) => element.SetValue(AlignmentProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="AlignmentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Position GetAlignment(StyledElement element) => element.GetValue(AlignmentProperty);

    #endregion
}
