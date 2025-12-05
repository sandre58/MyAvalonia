// -----------------------------------------------------------------------
// <copyright file="PaletteAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Media;

namespace MyNet.Avalonia.Theme.Assists;

public static class PaletteAssist
{
    #region Primary

    /// <summary>
    /// Provides Primary Property for attached PaletteAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> PrimaryProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, IBrush?>("Primary", typeof(PaletteAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="PrimaryProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PrimaryProperty"/>.</param>
    public static void SetPrimary(AvaloniaObject element, IBrush? value) => element.SetValue(PrimaryProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PrimaryProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetPrimary(AvaloniaObject element) => element.GetValue(PrimaryProperty);

    #endregion

    #region Secondary

    /// <summary>
    /// Provides Secondary Property for attached PaletteAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> SecondaryProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, IBrush?>("Secondary", typeof(PaletteAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="SecondaryProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="SecondaryProperty"/>.</param>
    public static void SetSecondary(AvaloniaObject element, IBrush? value) => element.SetValue(SecondaryProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="SecondaryProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetSecondary(AvaloniaObject element) => element.GetValue(SecondaryProperty);

    #endregion

    #region Tertiary

    /// <summary>
    /// Provides Tertiary Property for attached PaletteAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush?> TertiaryProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, IBrush?>("Tertiary", typeof(PaletteAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="TertiaryProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="TertiaryProperty"/>.</param>
    public static void SetTertiary(AvaloniaObject element, IBrush? value) => element.SetValue(TertiaryProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="TertiaryProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush? GetTertiary(AvaloniaObject element) => element.GetValue(TertiaryProperty);

    #endregion
}
