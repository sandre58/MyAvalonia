// -----------------------------------------------------------------------
// <copyright file="IconExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Classes.Enums;
using static MyNet.Avalonia.Theme.ThemeResources;

namespace MyNet.Avalonia.Theme.Extensions;

/// <summary>
/// Provides extension methods for converting <see cref="IconData"/> and <see cref="Geometry"/> instances into visual representations such as <see cref="StreamGeometry"/> and <see cref="PathIcon"/> controls. These extensions facilitate the use of icons in Avalonia applications by allowing developers to easily create icon controls from their data representations. The methods include options for specifying icon sizes, ensuring that the resulting controls can be customized to fit various design requirements.
/// </summary>
[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Extensions methods must be in a static class, and extension methods cannot be in a nested class.")]
[SuppressMessage("Naming", "CA1708:Identifiers should differ by more than case", Justification = "Extension methods use the same name as the type they extend.")]
public static class IconExtensions
{
    extension(IconData icon)
    {
        /// <summary>
        /// Converts an <see cref="IconData"/> value to a <see cref="StreamGeometry"/> by looking up the corresponding resource.
        /// </summary>
        /// <returns>The <see cref="StreamGeometry"/> associated with the icon.</returns>
        public StreamGeometry ToGeometry() => Icons.Get(icon.ToString()).Value;

        /// <summary>
        /// Converts an <see cref="IconData"/> value to a <see cref="PathIcon"/> control, optionally specifying the icon size.
        /// </summary>
        /// <param name="size">Optional size for the icon (width and height). If not specified, the default size is used.</param>
        /// <returns>A <see cref="PathIcon"/> representing the icon.</returns>
        public PathIcon ToIcon(double? size = null) => CreateIcon(icon.ToGeometry(), size);
    }

    extension(Geometry geometry)
    {
        /// <summary>
        /// Creates a PathIcon that represents the current geometry, optionally specifying the icon's size in pixels.
        /// </summary>
        /// <remarks>If the size parameter is specified, it must be greater than zero; otherwise, the
        /// default icon size is applied.</remarks>
        /// <param name="size">The optional size, in pixels, for the icon. If not specified, a default size is used. Must be a positive
        /// value if provided.</param>
        /// <returns>A PathIcon that displays the geometry with the specified size, or the default size if none is provided.</returns>
        public PathIcon ToIcon(double? size = null) => CreateIcon(geometry, size);
    }

    /// <summary>
    /// Converts a <see cref="Geometry"/> to a <see cref="PathIcon"/> control, optionally specifying the icon size. This method allows you to create a visual representation of the geometry as an icon that can be used in user interfaces. The resulting <see cref="PathIcon"/> will have its data set to the provided geometry, and its focusability and opacity configured for typical icon usage.
    /// </summary>
    /// <param name="geometry">The geometry to be converted into a <see cref="PathIcon"/>.</param>
    /// <param name="size">Optional size for the icon (width and height). If not specified, the default size is used.</param>
    /// <returns>A <see cref="PathIcon"/> representing the geometry.</returns>
    private static PathIcon CreateIcon(Geometry geometry, double? size = null)
    {
        var item = new PathIcon
        {
            Data = geometry,
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
