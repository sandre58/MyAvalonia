// -----------------------------------------------------------------------
// <copyright file="GeometryExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Theme.Extensions;

[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Extensions methods must be in a static class, and extension methods cannot be in a nested class.")]
public static class GeometryExtensions
{
    extension(IconData icon)
    {
        /// <summary>
        /// Converts an <see cref="IconData"/> value to a <see cref="StreamGeometry"/> by looking up the corresponding resource.
        /// </summary>
        /// <returns>The <see cref="StreamGeometry"/> associated with the icon.</returns>
        public StreamGeometry ToGeometry() => ThemeResources.Icons.Get(icon.ToString()).Value;

        /// <summary>
        /// Converts an <see cref="IconData"/> value to a <see cref="PathIcon"/> control, optionally specifying the icon size.
        /// </summary>
        /// <param name="size">Optional size for the icon (width and height). If not specified, the default size is used.</param>
        /// <returns>A <see cref="PathIcon"/> representing the icon.</returns>
        public PathIcon ToIcon(double? size = null)
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
}
