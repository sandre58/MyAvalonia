// -----------------------------------------------------------------------
// <copyright file="ToggleSwitchAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;

namespace MyNet.Avalonia.Theme.Controls.Assists;

public static class ToggleSwitchAssist
{
    #region Width

    /// <summary>
    /// Provides Width Property for attached ToggleSwitchAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> WidthProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("Width", typeof(ToggleSwitchAssist), 44.0d);

    /// <summary>
    /// Accessor for Attached  <see cref="WidthProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="WidthProperty"/>.</param>
    public static void SetWidth(StyledElement element, double value) => element.SetValue(WidthProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="WidthProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetWidth(StyledElement element) => element.GetValue(WidthProperty);

    #endregion

    #region Height

    /// <summary>
    /// Provides Height Property for attached ToggleSwitchAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> HeightProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("Height", typeof(ToggleSwitchAssist), 24.0d);

    /// <summary>
    /// Accessor for Attached  <see cref="HeightProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="HeightProperty"/>.</param>
    public static void SetHeight(StyledElement element, double value) => element.SetValue(HeightProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="HeightProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetHeight(StyledElement element) => element.GetValue(HeightProperty);

    #endregion
}
