// -----------------------------------------------------------------------
// <copyright file="IconExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Avalonia.Media;
using Material.Icons;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130

/// <summary>
/// Provides extension methods for converting <see cref="MaterialIconKind"/> and <see cref="Geometry"/> instances into visual representations such as <see cref="StreamGeometry"/> and <see cref="ExtendedIcon"/> controls. These extensions facilitate the use of icons in Avalonia applications by allowing developers to easily create icon controls from their data representations. The methods include options for specifying icon sizes, ensuring that the resulting controls can be customized to fit various design requirements.
/// </summary>
[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Extensions methods must be in a static class, and extension methods cannot be in a nested class.")]
[SuppressMessage("Naming", "CA1708:Identifiers should differ by more than case", Justification = "Extension methods use the same name as the type they extend.")]
public static class IconExtensions
{
    static IconExtensions() => MaterialIconDataProvider.InitializeGeometryParser(Geometry.Parse);

    extension(MaterialIconKind icon)
    {
        /// <summary>
        /// Converts an <see cref="MaterialIconKind"/> value to a <see cref="StreamGeometry"/> by looking up the corresponding resource.
        /// </summary>
        /// <returns>The <see cref="StreamGeometry"/> associated with the icon.</returns>
        public Geometry ToGeometry() => MaterialIconDataProvider.Get<Geometry>(icon);

        /// <summary>
        /// Converts an <see cref="MaterialIconKind"/> value to a <see cref="MaterialIcon"/> control, optionally specifying the icon size.
        /// </summary>
        /// <param name="size">Optional size for the icon (width and height). If not specified, the default size is used.</param>
        /// <returns>A <see cref="MaterialIcon"/> representing the icon.</returns>
        public MaterialIcon ToIcon(double? size = null) => CreateIcon(icon, size);
    }

    /// <summary>
    /// Converts a <see cref="MaterialIconKind"/> to a <see cref="MaterialIcon"/> control, optionally specifying the icon size. This method allows you to create a visual representation of the geometry as an icon that can be used in user interfaces. The resulting <see cref="MaterialIcon"/> will have its data set to the provided geometry, and its focusability and opacity configured for typical icon usage.
    /// </summary>
    /// <param name="kind">The kind to be converted into a <see cref="MaterialIcon"/>.</param>
    /// <param name="size">Optional size for the icon (width and height). If not specified, the default size is used.</param>
    /// <returns>A <see cref="MaterialIcon"/> representing the geometry.</returns>
    private static MaterialIcon CreateIcon(MaterialIconKind kind, double? size = null)
    {
        var item = new MaterialIcon { Kind = kind };

        if (size.HasValue)
        {
            item.IconSize = size.Value;
        }

        return item;
    }
}
