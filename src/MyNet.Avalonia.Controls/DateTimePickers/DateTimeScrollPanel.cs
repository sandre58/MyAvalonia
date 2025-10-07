// -----------------------------------------------------------------------
// <copyright file="DateTimeScrollPanel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class DateTimeScrollPanel : DateTimePickerPanel
{
    protected override Size MeasureOverride(Size availableSize)
    {
        var size = base.MeasureOverride(availableSize);
        var width = Children.Max(a => a.DesiredSize.Width);
        width = Math.Max(width, MinWidth);
        return new Size(width, size.Height);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var width = Children.Max(a => a.DesiredSize.Width);
        width = Math.Max(width, MinWidth);
        finalSize = new Size(width, finalSize.Height);
        return base.ArrangeOverride(finalSize);
    }
}
