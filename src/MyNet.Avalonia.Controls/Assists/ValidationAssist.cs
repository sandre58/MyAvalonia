// -----------------------------------------------------------------------
// <copyright file="ValidationAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Styling;

namespace MyNet.Avalonia.Controls.Assists;

public static class ValidationAssist
{
    #region Theme

    /// <summary>
    /// Provides Theme Property for attached ValidationAssist element.
    /// </summary>
    public static readonly AttachedProperty<ControlTheme> ThemeProperty = AvaloniaProperty.RegisterAttached<StyledElement, ControlTheme>("Theme", typeof(ValidationAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ThemeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ThemeProperty"/>.</param>
    public static void SetTheme(StyledElement element, ControlTheme value) => element.SetValue(ThemeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ThemeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static ControlTheme GetTheme(StyledElement element) => element.GetValue(ThemeProperty);

    #endregion
}
