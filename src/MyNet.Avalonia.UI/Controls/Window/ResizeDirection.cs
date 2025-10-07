// -----------------------------------------------------------------------
// <copyright file="ResizeDirection.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[Flags]
public enum ResizeDirection
{
    None = 0,
    Top = 1,
    Bottom = 2,
    Left = 4,
    Right = 8,

    Sides = Top | Bottom | Left | Right,
    TopLeft = 16,
    TopRight = 32,
    BottomLeft = 64,
    BottomRight = 128,
    Corners = TopLeft | TopRight | BottomLeft | BottomRight,
    All = Sides | Corners,
}
