// -----------------------------------------------------------------------
// <copyright file="ColorInterpolation.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Primitives;

namespace MyNet.Avalonia.Colors;

/// <summary>
/// Color interpolation parameters for opacity, contrast, darkening, and lightening adjustments.
/// </summary>
/// <param name="Opacity">The opacity value (0.0 to 1.0).</param>
/// <param name="Contrast">Whether to apply contrast transformation.</param>
/// <param name="Darken">The darken factor (0.0 to 1.0).</param>
/// <param name="Lighten">The lighten factor (0.0 to 1.0).</param>
public record ColorInterpolation(double? Opacity = null, bool Contrast = false, double? Darken = null, double? Lighten = null)
{
    /// <summary>
    /// Gets a value indicating whether no interpolation parameters are set.
    /// </summary>
    public bool IsEmpty => (!Opacity.HasValue || Opacity.Value.IsCloseTo(1.0)) && !Darken.HasValue && !Lighten.HasValue;

    /// <summary>
    /// Gets a value indicating whether the opacity has been explicitly set.
    /// </summary>
    public bool HasOpacity => Opacity.HasValue;

    /// <summary>
    /// Gets a value indicating whether the darken factor has been explicitly set.
    /// </summary>
    public bool HasDarken => Darken.HasValue;

    /// <summary>
    /// Gets a value indicating whether the lighten factor has been explicitly set.
    /// </summary>
    public bool HasLighten => Lighten.HasValue;

    /// <inheritdoc />
    public override string ToString() => $"Opacity: {Opacity}, Contrast: {Contrast}, Darken: {Darken}, Lighten: {Lighten}";
}
