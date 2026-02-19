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
    #region Background

    /// <summary>
    /// Provides Background Property for attached PaletteAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> BackgroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("Background", typeof(PaletteAssist));

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

    #region Border

    /// <summary>
    /// Provides Border Property for attached PaletteAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> BorderProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("Border", typeof(PaletteAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="BorderProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="BorderProperty"/>.</param>
    public static void SetBorder(StyledElement element, IBrush value) => element.SetValue(BorderProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="BorderProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush GetBorder(StyledElement element) => element.GetValue(BorderProperty);

    #endregion

    #region Foreground

    /// <summary>
    /// Provides Foreground Property for attached PaletteAssist element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> ForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("Foreground", typeof(PaletteAssist));

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
}
