// -----------------------------------------------------------------------
// <copyright file="ColorPickerAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Styling;

namespace MyNet.Avalonia.Controls.Assists;

public static class ColorPickerAssist
{
    #region ButtonTheme

    /// <summary>
    /// Provides ButtonTheme Property for attached ColorPickerAssist element.
    /// </summary>
    public static readonly AttachedProperty<ControlTheme> ButtonThemeProperty = AvaloniaProperty.RegisterAttached<StyledElement, ControlTheme>("ButtonTheme", typeof(ColorPickerAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ButtonThemeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ButtonThemeProperty"/>.</param>
    public static void SetButtonTheme(StyledElement element, ControlTheme value) => element.SetValue(ButtonThemeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ButtonThemeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static ControlTheme GetButtonTheme(StyledElement element) => element.GetValue(ButtonThemeProperty);

    #endregion

    #region ColorViewTheme

    /// <summary>
    /// Provides ColorViewTheme Property for attached ColorPickerAssist element.
    /// </summary>
    public static readonly AttachedProperty<ControlTheme> ColorViewThemeProperty = AvaloniaProperty.RegisterAttached<StyledElement, ControlTheme>("ColorViewTheme", typeof(ColorPickerAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ColorViewThemeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ColorViewThemeProperty"/>.</param>
    public static void SetColorViewTheme(StyledElement element, ControlTheme value) => element.SetValue(ColorViewThemeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ColorViewThemeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static ControlTheme GetColorViewTheme(StyledElement element) => element.GetValue(ColorViewThemeProperty);

    #endregion

}
