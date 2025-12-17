// -----------------------------------------------------------------------
// <copyright file="ControlThemeBuilder.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Styling;
using DynamicData;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.Theme.Palettes;
using MyNet.Utilities;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Demo.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Builder for generating control theme descriptions, layouts, styles, and roles for demo purposes.
/// </summary>
internal sealed class ControlThemeBuilder(string? name = null, ContentType defaultContentType = ContentType.Text)
{
    private static readonly ConcurrentDictionary<(string ControlName, string ThemeName), ControlTheme> ThemeCache = new();

    /// <summary>
    /// Gets the theme name.
    /// </summary>
    public string? Name { get; } = name;

    /// <summary>
    /// Gets the list of layouts.
    /// </summary>
    public List<ControlLayout> Layouts { get; } = [new ControlLayout(string.Empty, defaultContentType)];

    /// <summary>
    /// Gets the list of style combinations.
    /// </summary>
    public List<ControlStyle[]> Styles { get; } = [[new ControlStyle(string.Empty)]];

    /// <summary>
    /// Gets the list of size combinations.
    /// </summary>
    public List<ControlSize> Sizes { get; } = [];

    /// <summary>
    /// Gets the list of theme roles/colors.
    /// </summary>
    public List<ThemeRole> Roles { get; } = [];

    /// <summary>
    /// Gets all combinations of a list of items.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    /// <param name="list">The list of items to combine.</param>
    /// <returns>All possible combinations of the input list.</returns>
    public static List<List<T>> GetCombinations<T>(IEnumerable<T> list)
    {
        var result = new List<List<T>>();
        GenerateCombinations([.. list], 0, [], result);
        return result;
    }

    private static void GenerateCombinations<T>(List<T> list, int index, List<T> current, List<List<T>> result)
    {
        if (index == list.Count)
        {
            result.Add([.. current]);
            return;
        }

        GenerateCombinations(list, index + 1, current, result);

        current.Add(list[index]);
        GenerateCombinations(list, index + 1, current, result);
        current.RemoveAt(current.Count - 1);
    }

    /// <summary>
    /// Adds layouts to the theme section.
    /// </summary>
    /// <param name="layouts">The layout names to add.</param>
    /// <returns>The current builder instance.</returns>
    public ControlThemeBuilder AddLayouts(params string[] layouts)
    {
        Layouts.AddRange(layouts.Select(x => new ControlLayout(x, defaultContentType)));
        return this;
    }

    /// <summary>
    /// Adds layouts to the theme section.
    /// </summary>
    /// <param name="layouts">The layouts to add.</param>
    /// <returns>The current builder instance.</returns>
    public ControlThemeBuilder AddLayouts(params ControlLayout[] layouts)
    {
        Layouts.AddRange(layouts);
        return this;
    }

    /// <summary>
    /// Adds styles to the theme section.
    /// </summary>
    /// <param name="styles">The style names to add.</param>
    /// <returns>The current builder instance.</returns>
    public ControlThemeBuilder AddStyles(params string[] styles)
    {
        Styles.AddRange(styles.Select(x => new List<ControlStyle> { new(x) }.ToArray()));
        return this;
    }

    /// <summary>
    /// Adds cartesian style combinations to the theme section.
    /// </summary>
    /// <param name="styles">The style names to combine.</param>
    /// <returns>The current builder instance.</returns>
    public ControlThemeBuilder AddCartesianStyles(params string[] styles)
    {
        Styles.AddRange(GetCombinations(styles).Where(x => x.Count >= 2).Select(x => x.Select(y => new ControlStyle(y)).ToArray()));
        return this;
    }

    /// <summary>
    /// Adds all default roles to the theme section.
    /// </summary>
    /// <returns>The current builder instance.</returns>
    public ControlThemeBuilder AddDefaultRoles() => AddRoles([ThemeRole.Default, ThemeRole.Primary, ThemeRole.Accent, ThemeRole.Inverse, ThemeRole.Success, ThemeRole.Error, ThemeRole.Warning, ThemeRole.Information]);

    /// <summary>
    /// Adds theme roles to the theme section.
    /// </summary>
    /// <returns>The current builder instance.</returns>
    public ControlThemeBuilder AddThemeRoles() => AddRoles([ThemeRole.Primary, ThemeRole.Accent, ThemeRole.Inverse]);

    /// <summary>
    /// Adds all roles to the theme section.
    /// </summary>
    /// <returns>The current builder instance.</returns>
    public ControlThemeBuilder AddAllRoles() => AddRoles([ThemeRole.Default, ThemeRole.Primary, ThemeRole.Accent, ThemeRole.Inverse, ThemeRole.Dark, ThemeRole.Success, ThemeRole.Error, ThemeRole.Warning, ThemeRole.Information]);

    /// <summary>
    /// Adds roles to the theme section.
    /// </summary>
    /// <param name="roles">The roles to add.</param>
    /// <returns>The current builder instance.</returns>
    public ControlThemeBuilder AddRoles(params ThemeRole[] roles)
    {
        Roles.AddRange(roles);
        return this;
    }

    /// <summary>
    /// Adds all font sizes to the theme section.
    /// </summary>
    public ControlThemeBuilder AddAllSizes() => AddSizes(Enum.GetValues<FontSize>());

    /// <summary>
    /// Adds font sizes to the theme section.
    /// </summary>
    public ControlThemeBuilder AddSizes(params FontSize[] colors) => AddSizes(colors.Select(x => x.ToString()).ToArray());

    /// <summary>
    /// Adds sizes to the theme section.
    /// </summary>
    public ControlThemeBuilder AddSizes(params string[] sizes)
    {
        Sizes.AddRange(sizes.Select(x => new ControlSize(x)));
        return this;
    }

    /// <summary>
    /// Builds a <see cref="ControlThemeDescription"/> for the specified control name.
    /// </summary>
    /// <param name="controlName">The name of the control.</param>
    /// <returns>A <see cref="ControlThemeDescription"/> instance.</returns>
    public ControlThemeDescription Build(string controlName)
    {
        var theme = GetTheme(controlName, Name);

        return new(Name.OrEmpty(), theme, [.. Layouts.Select(layout => new ControlLayoutDescription(
        layout.Value.OrEmpty(),
        [.. Styles.Select(styles => new ControlStyleDescription(
            styles.Length == 0 ? string.Empty : string.Join(" ", styles.Select(x => x.Value)),
            [.. Roles.Select(x => new ControlDescription(
                theme,
                x,
                Classes: new List<string>([layout.Value]).Concat(styles.Select(x => x.Value)).ToArray(),
                ContentType: layout.ContentType,
                Content: x))]))],
        [.. Sizes.Select(size => new ControlDescription(
            theme,
            ThemeRole.Default,
            Classes: new List<string>([layout.Value, size.Value]).ToArray(),
            ContentType: layout.ContentType,
            Content: size.Value))]))]);
    }

    /// <summary>
    /// Gets the <see cref="ControlTheme"/> for the specified control and theme name.
    /// </summary>
    /// <param name="controlName">The control name.</param>
    /// <param name="themeName">The theme name.</param>
    /// <returns>The resolved <see cref="ControlTheme"/>, or null if not found.</returns>
    internal static ControlTheme? GetTheme(string controlName, string? themeName)
    {
        if (string.IsNullOrWhiteSpace(themeName))
            return null;

        var key = (controlName, themeName);
        if (ThemeCache.TryGetValue(key, out var cachedTheme))
        {
            return cachedTheme;
        }

        var themeKey = ThemeResourceKeyFactory.Theme(controlName, themeName);
        if (MyTheme.Current.TryGetResource(themeKey, null, out var value) && value is ControlTheme theme)
        {
            ThemeCache[key] = theme;
            return theme;
        }

        return null;
    }
}

/// <summary>
/// Describes a control theme, including its display name, theme, and layouts.
/// </summary>
internal sealed record ControlThemeDescription(string DisplayName, ControlTheme? Theme, ControlLayoutDescription[] Layouts);

/// <summary>
/// Describes a control layout, including its display name and styles.
/// </summary>
internal sealed record ControlLayoutDescription(string DisplayName, ControlStyleDescription[] Styles, ControlDescription[] Sizes);

/// <summary>
/// Describes a control style, including its display name and controls.
/// </summary>
internal sealed record ControlStyleDescription(string DisplayName, ControlDescription[] Controls);

/// <summary>
/// Describes a control instance, including its theme, role, classes, and content type.
/// </summary>
internal sealed record ControlDescription(ControlTheme? Theme = null, ThemeRole Role = ThemeRole.Default, string[]? Classes = null, ContentType ContentType = ContentType.Text, object? Content = null);

/// <summary>
/// Represents a control layout value and its content type.
/// </summary>
internal readonly record struct ControlLayout(string Value, ContentType ContentType);

/// <summary>
/// Represents a control style value.
/// </summary>
internal readonly record struct ControlStyle(string Value);

/// <summary>
/// Represents a control size value.
/// </summary>
internal readonly record struct ControlSize(string Value);

/// <summary>
/// Specifies the type of content for a control.
/// </summary>
internal enum ContentType
{
    /// <summary>
    /// Text content.
    /// </summary>
    Text,

    /// <summary>
    /// Icon content.
    /// </summary>
    Icon,

    /// <summary>
    /// Geometry content.
    /// </summary>
    Geometry,

    /// <summary>
    /// Image content.
    /// </summary>
    Image,

    /// <summary>
    /// Custom content.
    /// </summary>
    Custom,
}
