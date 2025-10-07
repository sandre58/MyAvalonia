// -----------------------------------------------------------------------
// <copyright file="ThicknessExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Enums;

namespace MyNet.Avalonia.MarkupExtensions;

public class ThicknessExtension : MarkupExtension
{
    public ThicknessExtension()
    {
    }

    public ThicknessExtension(ThicknessSize size) => Size = size;

    public ThicknessExtension(ThicknessSize size, ThicknessDirection direction)
    {
        Size = size;
        Direction = direction;
    }

    [ConstructorArgument("size")]
    public ThicknessSize Size { get; set; } = ThicknessSize.None;

    [ConstructorArgument("direction")]
    public ThicknessDirection Direction { get; set; } = ThicknessDirection.All;

    public double? Left { get; set; }

    public double? Top { get; set; }

    public double? Right { get; set; }

    public double? Bottom { get; set; }

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
