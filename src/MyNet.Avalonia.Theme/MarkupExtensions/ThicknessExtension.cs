// -----------------------------------------------------------------------
// <copyright file="ThicknessExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Theme.Enums;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

/// <summary>
/// Markup extension for creating a <see cref="Thickness"/> value in XAML with support for theme-based sizes and directions.
/// Allows specifying a standard thickness size, direction, or explicit side values for consistent spacing in the UI.
/// </summary>
/// <example>
/// <code>
/// <!-- Use with default size and direction -->
/// <Border Padding="{my:Thickness Size=Medium}" />
///
/// <!-- Use with specific direction -->
/// <Border Padding="{my:Thickness Size=Large, Direction=Horizontal}" />
///
/// <!-- Use with explicit side values -->
/// <Border Padding="{my:Thickness Left=8, Top=4, Right=8, Bottom=4}" />
/// </code>
/// </example>
public class ThicknessExtension : MarkupExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThicknessExtension"/> class.
    /// </summary>
    public ThicknessExtension() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThicknessExtension"/> class with the specified size.
    /// </summary>
    /// <param name="size">The standard thickness size to use.</param>
    public ThicknessExtension(ThicknessSize size) => Size = size;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThicknessExtension"/> class with the specified size and direction.
    /// </summary>
    /// <param name="size">The standard thickness size to use.</param>
    /// <param name="direction">The direction to apply the thickness.</param>
    public ThicknessExtension(ThicknessSize size, ThicknessDirection direction)
    {
        Size = size;
        Direction = direction;
    }

    /// <summary>
    /// Gets or sets the standard thickness size to use.
    /// </summary>
    [ConstructorArgument("size")]
    public ThicknessSize Size { get; set; } = ThicknessSize.None;

    /// <summary>
    /// Gets or sets the direction to apply the thickness (All, Horizontal, Vertical, Left, Top, Right, Bottom).
    /// </summary>
    [ConstructorArgument("direction")]
    public ThicknessDirection Direction { get; set; } = ThicknessDirection.All;

    /// <summary>
    /// Gets or sets the explicit left thickness value. If set, overrides the calculated value.
    /// </summary>
    public double? Left { get; set; }

    /// <summary>
    /// Gets or sets the explicit top thickness value. If set, overrides the calculated value.
    /// </summary>
    public double? Top { get; set; }

    /// <summary>
    /// Gets or sets the explicit right thickness value. If set, overrides the calculated value.
    /// </summary>
    public double? Right { get; set; }

    /// <summary>
    /// Gets or sets the explicit bottom thickness value. If set, overrides the calculated value.
    /// </summary>
    public double? Bottom { get; set; }

    /// <summary>
    /// Provides the value for the markup extension, returning a <see cref="Thickness"/> with the specified size, direction, and explicit side values.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A <see cref="Thickness"/> instance configured with the specified values.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var top = getSize(Top, Direction is ThicknessDirection.All or ThicknessDirection.Vertical or ThicknessDirection.Top);
        var bottom = getSize(Bottom, Direction is ThicknessDirection.All or ThicknessDirection.Vertical or ThicknessDirection.Bottom);
        var left = getSize(Left, Direction is ThicknessDirection.All or ThicknessDirection.Horizontal or ThicknessDirection.Left);
        var right = getSize(Right, Direction is ThicknessDirection.All or ThicknessDirection.Horizontal or ThicknessDirection.Right);

        return new Thickness(left, top, right, bottom);

        double getSize(double? prioritySize, bool condition) => prioritySize ?? (condition ? (int)Size : 0);
    }
}
