// -----------------------------------------------------------------------
// <copyright file="LoaderAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;

namespace MyNet.Avalonia.Theme.Controls.Assists;

/// <summary>
/// Attached properties for <see cref="Controls.Loader"/> theming.
/// </summary>
public static class LoaderAssist
{
    #region DotSize

    /// <summary>
    /// Defines the <see cref="DotSizeProperty"/> attached property.
    /// </summary>
    public static readonly AttachedProperty<double> DotSizeProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, double>("DotSize", typeof(LoaderAssist), 6.0d);

    /// <summary>Sets <see cref="DotSizeProperty"/>.</summary>
    public static void SetDotSize(StyledElement element, double value) => element.SetValue(DotSizeProperty, value);

    /// <summary>Gets <see cref="DotSizeProperty"/>.</summary>
    public static double GetDotSize(StyledElement element) => element.GetValue(DotSizeProperty);

    #endregion

    #region BarWidth

    /// <summary>
    /// Defines the <see cref="BarWidthProperty"/> attached property.
    /// </summary>
    public static readonly AttachedProperty<double> BarWidthProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, double>("BarWidth", typeof(LoaderAssist), 3.0d);

    /// <summary>Sets <see cref="BarWidthProperty"/>.</summary>
    public static void SetBarWidth(StyledElement element, double value) => element.SetValue(BarWidthProperty, value);

    /// <summary>Gets <see cref="BarWidthProperty"/>.</summary>
    public static double GetBarWidth(StyledElement element) => element.GetValue(BarWidthProperty);

    #endregion
}
