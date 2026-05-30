// -----------------------------------------------------------------------
// <copyright file="ColorManipulations.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Media;
using MyNet.Avalonia.Colors;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Color manipulation, conversion, and analysis for <see cref="Color"/>.
/// </summary>
public static class ColorManipulations
{
    extension(Color color)
    {
        /// <summary>
        /// Converts a <see cref="Color"/> to its hexadecimal string representation.
        /// </summary>
        public string ToHex()
            => color.A != 255
                ? string.Format(CultureInfo.InvariantCulture, "#{0:X2}{1:X2}{2:X2}{3:X2}", color.A, color.R, color.G, color.B)
                : string.Format(CultureInfo.InvariantCulture, "#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);

        /// <summary>
        /// Gets the localized color name, or its hex representation if no name is found.
        /// </summary>
        public string ToName() => ColorRegistry.Instance.GetDisplayName(color) ?? color.ToHex();

        /// <summary>
        /// Determines the contrasting foreground color for optimal readability.
        /// </summary>
        public Color ContrastingForegroundColor() => color.IsLightColor() ? global::Avalonia.Media.Colors.Black : global::Avalonia.Media.Colors.White;

        /// <summary>
        /// Determines if a color is light based on WCAG relative luminance.
        /// </summary>
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
        public bool IsDarkColor() => !color.IsLightColor();

        /// <summary>
        /// Darkens a color by reducing its lightness in the LAB color space.
        /// </summary>
        public Color Darken(int amount = 1) => color.ShiftLightness(amount);

        /// <summary>
        /// Lightens a color by increasing its lightness in the LAB color space.
        /// </summary>
        public Color Lighten(int amount = 1) => color.ShiftLightness(-amount);

        /// <summary>
        /// Shifts the lightness of a color in the LAB color space.
        /// </summary>
        public Color ShiftLightness(int amount = 1)
        {
            var lab = color.ToLab();
            var shifted = new Lab(lab.L - (LabConstants.Kn * amount), lab.A, lab.B);
            return shifted.ToColor();
        }

        private Lab ToLab()
        {
            var xyz = color.ToXyz();
            return xyz.ToLab();
        }

        /// <summary>
        /// Applies interpolation parameters to the color.
        /// </summary>
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

    private static Color ToColor(this Lab lab)
    {
        var xyz = lab.ToXyz();
        return xyz.ToColor();
    }

    private static Color ToColor(this Xyz xyz)
    {
        var r = xyzRgb((3.2404542 * xyz.X) - (1.5371385 * xyz.Y) - (0.4985314 * xyz.Z));
        var g = xyzRgb((-0.9692660 * xyz.X) + (1.8760108 * xyz.Y) + (0.0415560 * xyz.Z));
        var b = xyzRgb((0.0556434 * xyz.X) - (0.2040259 * xyz.Y) + (1.0572252 * xyz.Z));

        return Color.FromRgb(clip(r), clip(g), clip(b));

        double xyzRgb(double d) => d > 0.0031308 ? 255.0 * ((1.055 * Math.Pow(d, 1.0 / 2.4)) - 0.055) : 255.0 * (12.92 * d);
        byte clip(double d) => d < 0 ? (byte)0 : d > 255 ? (byte)255 : (byte)Math.Round(d);
    }

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

    private readonly struct Lab(double l, double a, double b)
    {
        public double L { get; } = l;

        public double A { get; } = a;

        public double B { get; } = b;
    }

    private readonly struct Xyz(double x, double y, double z)
    {
        public double X { get; } = x;

        public double Y { get; } = y;

        public double Z { get; } = z;
    }

    private static class LabConstants
    {
        public const double Kn = 18;
        public const double WhitePointX = 0.95047;
        public const double WhitePointY = 1;
        public const double WhitePointZ = 1.08883;
        public const double K = 24389 / 27.0;
        public const double E = 216 / 24389.0;
        public static readonly double ECubedRoot = Math.Pow(E, 1.0 / 3);
    }
}
