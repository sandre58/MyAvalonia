// -----------------------------------------------------------------------
// <copyright file="ThemeResources.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Media;
using MyNet.Utilities;

namespace MyNet.Avalonia.Theme;

public static class ThemeResources
{
    public const string ResourcePrefix = "MyNet";

    public const string ThemePattern = $"{ResourcePrefix}.{{0}}.{{1}}";
    public const string IconPattern = "{{my:Icon {0}}}";
    public const string IconPathPattern = $"<PathIcon Data=\"{{{{StaticResource {ResourcePrefix}.{GeometryKey}.{{0}}}}}}\" />";

    public const string ColorKey = "Color";
    public const string BrushKey = "Brush";
    public const string OpacityKey = "Opacity";
    public const string GeometryKey = "Geometry";
    public const string PrimaryKey = "Primary";
    public const string AccentKey = "Accent";

    public static string GetThemeKey(string type, string name) => ThemePattern.FormatWith(type, name);

    public static string GetColorKey(string name) => GetThemeKey(ColorKey, name);

    public static string GetBrushKey(string name) => GetThemeKey(BrushKey, name);

    public static string GetGeometryKey(string name) => GetThemeKey(GeometryKey, name);

    public static string GetOpacityKey(string name) => GetThemeKey(OpacityKey, name);

    public static string GetPattern(string type) => GetThemeKey(type, "{0}");

    public static IBrush GetBrush(string name) => ResourceLocator.GetResource<IBrush>(GetBrushKey(name));

    public static double GetOpacity(string name) => ResourceLocator.GetResource<double>(GetOpacityKey(name));
}
