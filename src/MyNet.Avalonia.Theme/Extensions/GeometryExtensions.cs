// -----------------------------------------------------------------------
// <copyright file="GeometryExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Enums;

namespace MyNet.Avalonia.Theme.Extensions;

public static class GeometryExtensions
{
    /// <summary>
    /// Converts an <see cref="IconData"/> value to a <see cref="StreamGeometry"/> by looking up the corresponding resource.
    /// </summary>
    /// <param name="icon">The icon data to convert.</param>
    /// <returns>The <see cref="StreamGeometry"/> associated with the icon.</returns>
    public static StreamGeometry ToGeometry(this IconData icon) => ResourceLocator.GetResource<StreamGeometry>(ThemeResourceKeyFactory.Geometry(icon.ToString()));

    /// <summary>
    /// Converts an <see cref="IconData"/> value to a <see cref="PathIcon"/> control, optionally specifying the icon size.
    /// </summary>
    /// <param name="icon">The icon data to convert.</param>
    /// <param name="size">Optional size for the icon (width and height). If not specified, the default size is used.</param>
    /// <returns>A <see cref="PathIcon"/> representing the icon.</returns>
    public static PathIcon ToIcon(this IconData icon, double? size = null)
    {
        var item = new PathIcon
        {
            Data = icon.ToGeometry(),
            Focusable = false,
            Opacity = 1
        };

        if (!size.HasValue)
        {
            return item;
        }

        item.Width = size.Value;
        item.Height = size.Value;

        return item;
    }
}
