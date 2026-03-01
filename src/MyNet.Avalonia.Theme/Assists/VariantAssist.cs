// -----------------------------------------------------------------------
// <copyright file="VariantAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Media;
using MyNet.Avalonia.Helpers;

namespace MyNet.Avalonia.Theme.Assists;

public static class VariantAssist
{
    static VariantAssist()
    {
        DefaultBackgroundProperty.Changed.AddClassHandler<StyledElement>((element, args) => update(BackgroundProperty, element, args));
        DefaultBorderBrushProperty.Changed.AddClassHandler<StyledElement>((element, args) => update(BorderBrushProperty, element, args));
        DefaultForegroundProperty.Changed.AddClassHandler<StyledElement>((element, args) => update(ForegroundProperty, element, args));
        DefaultPrimaryProperty.Changed.AddClassHandler<StyledElement>((element, args) => update(PrimaryProperty, element, args));

        static void update(AttachedProperty<IBrush> property, StyledElement element, AvaloniaPropertyChangedEventArgs args)
        {
            // if (element.GetValue(property) is null && args.NewValue is IBrush brush)
            //     element.SetValue(property, brush);
        }
    }

    #region Variant

    /// <summary>
    /// Provides Variant Property for attached VariantAssist element.
    /// </summary>
    public static readonly AttachedProperty<ControlVariant> VariantProperty = AvaloniaPropertyHelper.RegisterEnumProperty("Variant", ControlVariant.None, ClassName.Prefix.Variant);

    /// <summary>
    /// Accessor for Attached  <see cref="VariantProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="VariantProperty"/>.</param>
    public static void SetVariant(StyledElement element, ControlVariant value) => element.SetValue(VariantProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="VariantProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static ControlVariant GetVariant(StyledElement element) => element.GetValue(VariantProperty);

    #endregion

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

/// <summary>
/// Specifies the visual style variants that can be applied to a control. This enumeration supports combining multiple
/// styles using bitwise operations.
/// </summary>
/// <remarks>Each value represents a distinct appearance option, such as solid, light, outlined, or text. The
/// enumeration is marked with the <see cref="System.FlagsAttribute"/>, allowing multiple styles to be combined to
/// achieve composite visual effects.</remarks>
[Flags]
public enum ControlVariant
{
    /// <summary>
    /// Represents a value indicating that no specific option is selected.
    /// </summary>
    None = 0,

    /// <summary>
    /// Represents a solid shape type in the bitwise shape enumeration.
    /// </summary>
    /// <remarks>This value can be combined with other shape types using bitwise operations to specify
    /// multiple shape characteristics.</remarks>
    Solid = 1 << 0,

    /// <summary>
    /// Represents the light setting, which can be combined with other values to configure the appearance or behavior of
    /// a control.
    /// </summary>
    /// <remarks>This value is typically used as a flag in bitwise operations to enable or check for the light
    /// variant in a set of control variants.</remarks>
    Light = 1 << 1,

    /// <summary>
    /// Represents a visual element that is displayed in an outlined style.
    /// </summary>
    Outlined = 1 << 2,

    /// <summary>
    /// Specifies the text option in the bitwise enumeration.
    /// </summary>
    /// <remarks>This value can be combined with other enumeration values using bitwise operations to enable
    /// multiple options simultaneously.</remarks>
    Text = 1 << 3
}
