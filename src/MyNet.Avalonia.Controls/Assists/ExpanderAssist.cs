// -----------------------------------------------------------------------
// <copyright file="ExpanderAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Styling;

namespace MyNet.Avalonia.Controls.Assists;

public static class ExpanderAssist
{
    #region ButtonTheme

    /// <summary>
    /// Provides ButtonTheme Property for attached ExpanderAssist element.
    /// </summary>
    public static readonly AttachedProperty<ControlTheme> ButtonThemeProperty = AvaloniaProperty.RegisterAttached<StyledElement, ControlTheme>("ButtonTheme", typeof(ExpanderAssist));

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
}
