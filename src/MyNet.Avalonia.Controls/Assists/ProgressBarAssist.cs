// -----------------------------------------------------------------------
// <copyright file="ProgressBarAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;

namespace MyNet.Avalonia.Controls.Assists;

public static class ProgressBarAssist
{
    #region BarSize

    /// <summary>
    /// Provides BarSize Property for attached ProgressBarAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> BarSizeProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("BarSize", typeof(ProgressBarAssist), 16.0d);

    /// <summary>
    /// Accessor for Attached  <see cref="BarSizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="BarSizeProperty"/>.</param>
    public static void SetBarSize(StyledElement element, double value) => element.SetValue(BarSizeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="BarSizeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetBarSize(StyledElement element) => element.GetValue(BarSizeProperty);

    #endregion
}
