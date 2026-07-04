// -----------------------------------------------------------------------
// <copyright file="PlaceholderAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using MyNet.Avalonia.Controls;

namespace MyNet.Avalonia.Theme.Controls.Assists;

/// <summary>
/// Layout attached properties for <see cref="PlaceholderContentControl"/> placeholder presentation.
/// </summary>
public static class PlaceholderAssist
{
    static PlaceholderAssist()
    {
        MinHeightProperty.Changed.AddClassHandler<PlaceholderContentControl>((c, e) =>
            c.PlaceholderMinHeight = e.GetNewValue<double>());

        PaddingProperty.Changed.AddClassHandler<PlaceholderContentControl>((c, e) =>
            c.PlaceholderPadding = e.GetNewValue<Thickness>());
    }

    #region MinHeight

    /// <summary>
    /// Provides MinHeight Property for attached PlaceholderAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> MinHeightProperty =
        AvaloniaProperty.RegisterAttached<PlaceholderContentControl, double>("MinHeight", typeof(PlaceholderAssist), double.NaN);

    /// <summary>
    /// Accessor for Attached <see cref="MinHeightProperty"/>.
    /// </summary>
    public static void SetMinHeight(PlaceholderContentControl element, double value) =>
        element.PlaceholderMinHeight = value;

    /// <summary>
    /// Accessor for Attached <see cref="MinHeightProperty"/>.
    /// </summary>
    public static double GetMinHeight(PlaceholderContentControl element) =>
        element.PlaceholderMinHeight;

    #endregion

    #region Padding

    /// <summary>
    /// Provides Padding Property for attached PlaceholderAssist element.
    /// </summary>
    public static readonly AttachedProperty<Thickness> PaddingProperty =
        AvaloniaProperty.RegisterAttached<PlaceholderContentControl, Thickness>("Padding", typeof(PlaceholderAssist));

    /// <summary>
    /// Accessor for Attached <see cref="PaddingProperty"/>.
    /// </summary>
    public static void SetPadding(PlaceholderContentControl element, Thickness value) =>
        element.PlaceholderPadding = value;

    /// <summary>
    /// Accessor for Attached <see cref="PaddingProperty"/>.
    /// </summary>
    public static Thickness GetPadding(PlaceholderContentControl element) =>
        element.PlaceholderPadding;

    #endregion
}
