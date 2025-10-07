// -----------------------------------------------------------------------
// <copyright file="ColorPair.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Media;
using MyNet.Avalonia.Extensions;

namespace MyNet.Avalonia.Theming;

public struct ColorPair(Color color, Color? foreground = null) : IEquatable<ColorPair>
{
    public Color Color { get; set; } = color;

    /// <summary>
    /// Gets or sets the foreground or opposite color. If left null, this will be calculated for you.
    /// Calculated by calling ColorAssist.ContrastingForegroundColor().
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1721:Property names should not match get methods", Justification = "Get nullable value")]
    public Color? ForegroundColor { get; set; } = foreground;

    public static implicit operator ColorPair(Color color) => new(color);

    public readonly Color GetForegroundColor() => ForegroundColor ?? Color.ContrastingForegroundColor();

    public override readonly bool Equals(object? obj) => obj is ColorPair other && Equals(other);

    readonly bool IEquatable<ColorPair>.Equals(ColorPair other) => Equals(other);

    public override readonly int GetHashCode() => HashCode.Combine(Color, ForegroundColor);

    public static bool operator ==(ColorPair left, ColorPair right) => left.Equals(right);

    public static bool operator !=(ColorPair left, ColorPair right) => !(left == right);

    public readonly ColorPair ToColorPair() => new(Color);
}
