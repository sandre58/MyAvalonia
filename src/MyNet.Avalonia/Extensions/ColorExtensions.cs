// -----------------------------------------------------------------------
// <copyright file="ColorExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia.Media;
using MyNet.Avalonia.Resources;
using MyNet.Primitives;

namespace MyNet.Avalonia.Extensions;

/// <summary>
/// Provides extension methods for color manipulation, conversion, and analysis.
/// Supports conversions between RGB, Hex, color names, and color space transformations (XYZ, LAB).
/// </summary>
[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Extensions methods must be in a static class, and extension methods cannot be in a nested class.")]
public static class ColorExtensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="Color"/> struct, including conversions to hex, name, and color manipulation.
    /// </summary>
    extension(Color color)
    {
        /// <summary>
        /// Converts a <see cref="Color"/> to its hexadecimal string representation.
        /// Includes alpha channel if not fully opaque (255).
        /// </summary>
        /// <returns>A hex string in the format "#RRGGBB" or "#AARRGGBB" if alpha is not 255.</returns>
        public string ToHex()
            => color.A != 255
                ? string.Format(CultureInfo.InvariantCulture, "#{0:X2}{1:X2}{2:X2}{3:X2}", color.A, color.R, color.G, color.B)
                : string.Format(CultureInfo.InvariantCulture, "#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);

        /// <summary>
        /// Gets the localized or resource name of a color, or its hex representation if no name is found.
        /// </summary>
        /// <returns>The localized color name or hex string if no name exists.</returns>
        public string ToName() => ColorResourcesLocator.GetName(color) is { } name ? $"{name}" : color.ToHex();

        /// <summary>
        /// Determines the contrasting foreground color (black or white) for optimal readability against the given background color.
        /// </summary>
        /// <returns><see cref="Colors.Black"/> if the color is light; otherwise, <see cref="Colors.White"/>.</returns>
        public Color ContrastingForegroundColor() => color.IsLightColor() ? Colors.Black : Colors.White;

        /// <summary>
        /// Determines if a color is light based on its relative luminance using the sRGB color space.
        /// Uses the WCAG luminance formula.
        /// </summary>
        /// <returns>True if the color is light (luminance > 0.179); otherwise, false.</returns>
        public bool IsLightColor()
        {
            var r = rgbSrgb(color.R);
            var g = rgbSrgb(color.G);
            var b = rgbSrgb(color.B);

            var luminance = (0.2126 * r) + (0.7152 * g) + (0.0722 * b);
            return luminance > 0.179;

            static double rgbSrgb(double d)
            {
                d /= 255.0;
                return d > 0.03928
                    ? Math.Pow((d + 0.055) / 1.055, 2.4)
                    : d / 12.92;
            }
        }

        /// <summary>
        /// Determines if a color is dark based on its relative luminance.
        /// </summary>
        /// <returns>True if the color is dark; otherwise, false.</returns>
        public bool IsDarkColor() => !color.IsLightColor();

        /// <summary>
        /// Darkens a color by reducing its lightness in the LAB color space.
        /// </summary>
        /// <param name="amount">The amount to darken (default is 1).</param>
        /// <returns>The darkened color.</returns>
        public Color Darken(int amount = 1) => color.ShiftLightness(amount);

        /// <summary>
        /// Lightens a color by increasing its lightness in the LAB color space.
        /// </summary>
        /// <param name="amount">The amount to lighten (default is 1).</param>
        /// <returns>The lightened color.</returns>
        public Color Lighten(int amount = 1) => color.ShiftLightness(-amount);

        /// <summary>
        /// Shifts the lightness of a color in the LAB color space.
        /// </summary>
        /// <param name="amount">The amount to shift (positive darkens, negative lightens).</param>
        /// <returns>The color with shifted lightness.</returns>
        public Color ShiftLightness(int amount = 1)
        {
            var lab = color.ToLab();
            var shifted = new Lab(lab.L - (LabConstants.Kn * amount), lab.A, lab.B);
            return shifted.ToColor();
        }

        /// <summary>
        /// Converts an RGB color to the LAB color space via XYZ.
        /// </summary>
        private Lab ToLab()
        {
            var xyz = color.ToXyz();
            return xyz.ToLab();
        }

        public Color Apply(ColorInterpolation colorInterpolation)
        {
            if (colorInterpolation.Darken.HasValue)
            {
                var amount = Math.Max(1, (int)(colorInterpolation.Darken.Value * 5));
                color = color.Darken(amount);
            }

            if (colorInterpolation.Lighten.HasValue)
            {
                var amount = Math.Max(1, (int)(colorInterpolation.Lighten.Value * 5));
                color = color.Lighten(amount);
            }

            if (colorInterpolation.Contrast)
                color = color.ContrastingForegroundColor();

            if (colorInterpolation.Opacity.HasValue)
                color = Color.FromArgb(Convert.ToByte(255 * colorInterpolation.Opacity.Value), color.R, color.G, color.B);

            return color;
        }

        /// <summary>
        /// Converts an RGB color to XYZ color space.
        /// </summary>
        private Xyz ToXyz()
        {
            var r = rgbXyz(color.R);
            var g = rgbXyz(color.G);
            var b = rgbXyz(color.B);

            var x = (0.4124564 * r) + (0.3575761 * g) + (0.1804375 * b);
            var y = (0.2126729 * r) + (0.7151522 * g) + (0.0721750 * b);
            var z = (0.0193339 * r) + (0.1191920 * g) + (0.9503041 * b);
            return new(x, y, z);

            static double rgbXyz(double v)
            {
                v /= 255;
                return v > 0.04045 ? Math.Pow((v + 0.055) / 1.055, 2.4) : v / 12.92;
            }
        }
    }

    /// <summary>
    /// Converts XYZ color space to LAB color space.
    /// </summary>
    private static Lab ToLab(this Xyz xyz)
    {
        var fx = xyzLab(xyz.X / LabConstants.WhitePointX);
        var fy = xyzLab(xyz.Y / LabConstants.WhitePointY);
        var fz = xyzLab(xyz.Z / LabConstants.WhitePointZ);

        var l = (116 * fy) - 16;
        var a = 500 * (fx - fy);
        var b = 200 * (fy - fz);
        return new(l, a, b);

        static double xyzLab(double v) => v > LabConstants.E ? Math.Pow(v, 1 / 3.0) : ((v * LabConstants.K) + 16) / 116;
    }

    /// <summary>
    /// Converts a LAB color to RGB via XYZ.
    /// </summary>
    private static Color ToColor(this Lab lab)
    {
        var xyz = lab.ToXyz();
        return xyz.ToColor();
    }

    /// <summary>
    /// Converts XYZ color space to RGB.
    /// </summary>
    private static Color ToColor(this Xyz xyz)
    {
        var r = xyzRgb((3.2404542 * xyz.X) - (1.5371385 * xyz.Y) - (0.4985314 * xyz.Z));
        var g = xyzRgb((-0.9692660 * xyz.X) + (1.8760108 * xyz.Y) + (0.0415560 * xyz.Z));
        var b = xyzRgb((0.0556434 * xyz.X) - (0.2040259 * xyz.Y) + (1.0572252 * xyz.Z));

        return Color.FromRgb(clip(r), clip(g), clip(b));

        double xyzRgb(double d) => d > 0.0031308 ? 255.0 * ((1.055 * Math.Pow(d, 1.0 / 2.4)) - 0.055) : 255.0 * (12.92 * d);
        byte clip(double d) => d < 0 ? (byte)0 : d > 255 ? (byte)255 : (byte)Math.Round(d);
    }

    /// <summary>
    /// Converts LAB color space to XYZ color space.
    /// </summary>
    private static Xyz ToXyz(this Lab lab)
    {
        var y = (lab.L + 16.0) / 116.0;
        var x = double.IsNaN(lab.A) ? y : y + (lab.A / 500.0);
        var z = double.IsNaN(lab.B) ? y : y - (lab.B / 200.0);

        y = LabConstants.WhitePointY * labXyz(y);
        x = LabConstants.WhitePointX * labXyz(x);
        z = LabConstants.WhitePointZ * labXyz(z);

        return new(x, y, z);

        static double labXyz(double d) => d > LabConstants.ECubedRoot ? d * d * d : ((116 * d) - 16) / LabConstants.K;
    }

    /// <summary>
    /// Represents a color in the LAB color space.
    /// </summary>
    private readonly struct Lab(double l, double a, double b)
    {
        /// <summary>
        /// Gets the lightness component (0-100).
        /// </summary>
        public double L { get; } = l;

        /// <summary>
        /// Gets the green-red component.
        /// </summary>
        public double A { get; } = a;

        /// <summary>
        /// Gets the blue-yellow component.
        /// </summary>
        public double B { get; } = b;
    }

    /// <summary>
    /// Represents a color in the XYZ color space.
    /// </summary>
    private readonly struct Xyz(double x, double y, double z)
    {
        /// <summary>
        /// Gets the X component.
        /// </summary>
        public double X { get; } = x;

        /// <summary>
        /// Gets the Y component (luminance).
        /// </summary>
        public double Y { get; } = y;

        /// <summary>
        /// Gets the Z component.
        /// </summary>
        public double Z { get; } = z;
    }

    /// <summary>
    /// Constants for LAB color space conversions.
    /// </summary>
    private static class LabConstants
    {
        /// <summary>
        /// Lightness shift constant for darken/lighten operations.
        /// </summary>
        public const double Kn = 18;

        /// <summary>
        /// D65 white point X component.
        /// </summary>
        public const double WhitePointX = 0.95047;

        /// <summary>
        /// D65 white point Y component.
        /// </summary>
        public const double WhitePointY = 1;

        /// <summary>
        /// D65 white point Z component.
        /// </summary>
        public const double WhitePointZ = 1.08883;

        /// <summary>
        /// CIE standard constant for XYZ to LAB conversion.
        /// </summary>
        public const double K = 24389 / 27.0;

        /// <summary>
        /// CIE standard constant for XYZ to LAB conversion.
        /// </summary>
        public const double E = 216 / 24389.0;

        /// <summary>
        /// Cube root of E, precomputed for performance.
        /// </summary>
        public static readonly double ECubedRoot = Math.Pow(E, 1.0 / 3);
    }
}

/// <summary>
/// Gets or sets color interpolation parameters for opacity, contrast, darkening, and lightening adjustments.
/// </summary>
/// <param name="Opacity">The opacity value (0.0 to 1.0).</param>
/// <param name="Contrast">Whether to apply contrast transformation.</param>
/// <param name="Darken">The darken factor (0.0 to 1.0).</param>
/// <param name="Lighten">The lighten factor (0.0 to 1.0).</param>
public record ColorInterpolation(double? Opacity = null, bool Contrast = false, double? Darken = null, double? Lighten = null)
{
    /// <summary>
    /// Gets a value indicating whether no interpolation parameters are set (i.e., all are null or false).
    /// </summary>
    public bool IsEmpty => (!Opacity.HasValue || Opacity.Value.IsCloseTo(1.0)) && !Darken.HasValue && !Lighten.HasValue;

    /// <summary>
    /// Gets a value indicating whether the opacity has been explicitly set for the element.
    /// </summary>
    /// <remarks>This property returns <see langword="true"/> if the Opacity property has a value assigned;
    /// otherwise, it returns <see langword="false"/>. Use this property to determine if the element's opacity is
    /// controlled by a specific value or is using the default behavior.</remarks>
    public bool HasOpacity => Opacity.HasValue;

    /// <summary>
    /// Gets a value indicating whether the darken factor has been explicitly set for the element.
    /// </summary>
    public bool HasDarken => Darken.HasValue;

    /// <summary>
    /// Gets a value indicating whether the Lighten property has been set.
    /// </summary>
    public bool HasLighten => Lighten.HasValue;

    /// <summary>
    /// Provides a string representation of the ColorInterpolation instance, showing the values of Opacity, Contrast, Darken, and Lighten.
    /// </summary>
    /// <returns>A string representation of the ColorInterpolation instance.</returns>
    public override string ToString() => $"Opacity: {Opacity}, Contrast: {Contrast}, Darken: {Darken}, Lighten: {Lighten}";
}
