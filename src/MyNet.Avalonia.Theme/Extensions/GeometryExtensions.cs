// -----------------------------------------------------------------------
// <copyright file="GeometryExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Media;
using MyNet.Avalonia.Enums;

namespace MyNet.Avalonia.Theme.Extensions;

public static class GeometryExtensions
{
    public static StreamGeometry ToGeometry(this IconData icon) => ResourceLocator.GetResource<StreamGeometry>(ThemeResources.GetGeometryKey(icon.ToString()));

    public static PathIcon ToIcon(this IconData icon, double? size = null)
    {
        var item = new PathIcon
        {
            Data = icon.ToGeometry(),
            Focusable = false,
            Opacity = 1
        };

        if (!size.HasValue)
        {
            return item;
        }

        item.Width = size.Value;
        item.Height = size.Value;

        return item;
    }
}
