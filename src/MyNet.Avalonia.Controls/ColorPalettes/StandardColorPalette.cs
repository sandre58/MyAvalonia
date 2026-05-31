// -----------------------------------------------------------------------
// <copyright file="StandardColorPalette.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Media;
using MyNet.Primitives;

namespace MyNet.Avalonia.Controls.ColorPalettes;

public class StandardColorPalette : IColorPalette
{
    private static readonly Color[][] Colors =
    [
        [
            global::Avalonia.Media.Colors.White
        ],
        [
            global::Avalonia.Media.Colors.Black
        ],
        [
            global::Avalonia.Media.Colors.Gray
        ],
        [
            global::Avalonia.Media.Colors.Brown
        ],
        [
            global::Avalonia.Media.Colors.Red
        ],
        [
            global::Avalonia.Media.Colors.Orange
        ],
        [
            global::Avalonia.Media.Colors.Yellow
        ],
        [
            global::Avalonia.Media.Colors.Green
        ],
        [
            global::Avalonia.Media.Colors.Blue
        ],
        [
            global::Avalonia.Media.Colors.Purple
        ],
        [
            global::Avalonia.Media.Colors.Pink
        ]
    ];

    public int ColorCount => Colors.Length;

    public int ShadeCount => Colors[0].Length;

    public Color GetColor(int colorIndex, int shadeIndex)
    {
        var safeColorIndex = colorIndex.SafeClamp(0, Colors.Length - 1);
        var shades = Colors[safeColorIndex];
        var safeShadeIndex = shadeIndex.SafeClamp(0, shades.Length - 1);
        return shades[safeShadeIndex];
    }
}
