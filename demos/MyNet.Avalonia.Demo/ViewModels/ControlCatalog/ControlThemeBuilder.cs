// -----------------------------------------------------------------------
// <copyright file="ControlThemeBuilder.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Styling;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ContentProviders;
using MyNet.Avalonia.Theme.Theming;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Humanizer;
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

/// <summary>
/// Builder class for creating control theme definitions with fluent API.
/// </summary>
/// <param name="themeName">The name of the theme.</param>
/// <param name="kind">The kind identifier.</param>
/// <param name="defaultContentType">The default content type for previews.</param>
internal sealed class ControlThemeBuilder(string? themeName = null, string? kind = null, ContentProviderType defaultContentType = ContentProviderType.Text)
{
    private static readonly ConcurrentDictionary<string, ControlTheme> ThemeCache = new();

    private readonly List<ShapeDefinition> _shapes = [];
    private readonly List<VariantDefinition> _variants = [];
    private readonly List<SizeDefinition> _sizes = [];
    private readonly List<RoleDefinition> _roles = [];
    private readonly List<RoleDefinition> _itemsRoles = [];

    #region Fluent API

    /// <summary>
    /// Adds shape definitions to the builder.
    /// </summary>
    /// <param name="values">The shape class names to add.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public ControlThemeBuilder AddShapes(params string[] values)
    {
        foreach (var v in values)
        {
            if (string.IsNullOrWhiteSpace(v)) continue;
            if (_shapes.Any(x => x.Class == v)) continue;
            _shapes.Add(new ShapeDefinition(v));
        }

        return this;
    }

    /// <summary>
    /// Adds variant definitions to the builder, one class per variant.
    /// </summary>
    /// <param name="values">The variant class names to add.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public ControlThemeBuilder AddVariants(params string[] values)
    {
        foreach (var v in values)
        {
            if (string.IsNullOrWhiteSpace(v)) continue;
            if (_variants.Any(x => x.Classes.Length == 1 && x.Classes[0] == v)) continue;
            _variants.Add(new VariantDefinition([v]));
        }

        return this;
    }

    /// <summary>
    /// Adds size definitions to the builder.
    /// </summary>
    /// <param name="values">The size class names to add.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public ControlThemeBuilder AddSizes(params string[] values)
    {
        foreach (var v in values)
        {
            if (string.IsNullOrWhiteSpace(v)) continue;
            if (_sizes.Any(x => x.Class == v)) continue;
            _sizes.Add(new SizeDefinition(v));
        }

        return this;
    }

    /// <summary>
    /// Adds default size definitions (size-sm, size-md, size-lg).
    /// </summary>
    /// <returns>The current builder instance for method chaining.</returns>
    public ControlThemeBuilder AddDefaultSizes() => AddSizes("size-sm", "size-md", "size-lg");

    /// <summary>
    /// Adds all size definitions.
    /// </summary>
    /// <returns>The current builder instance for method chaining.</returns>
    public ControlThemeBuilder AddAllSizes() => AddSizes("size-xs", "size-sm", "size-md", "size-lg", "size-xl");

    /// <summary>
    /// Adds role definitions to the builder.
    /// </summary>
    /// <param name="roles">The theme roles to add.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public ControlThemeBuilder AddRoles(params ThemeRole[] roles)
    {
        foreach (var role in roles)
        {
            if (_roles.Any(r => r.Role == role)) continue;
            _roles.Add(new RoleDefinition(role, false));
        }

        return this;
    }

    /// <summary>
    /// Adds default role definitions (Primary, Accent, Inverse, Success, Warning, Error, Information).
    /// </summary>
    /// <returns>The current builder instance for method chaining.</returns>
    public ControlThemeBuilder AddAllRoles() => AddRoles([.. Enum.GetValues<ThemeRole>()]);

    /// <summary>
    /// Adds default role definitions (Primary, Accent, Contrast, Success, Warning, Error, Information).
    /// </summary>
    /// <returns>The current builder instance for method chaining.</returns>
    public ControlThemeBuilder AddDefaultRoles() => AddRoles([.. Enum.GetValues<ThemeRole>().Except([ThemeRole.Neutral, ThemeRole.Inverse])]);

    /// <summary>
    /// Adds theme role definitions (Primary, Accent, Contrast).
    /// </summary>
    /// <returns>The current builder instance for method chaining.</returns>
    public ControlThemeBuilder AddThemeRoles() => AddRoles(ThemeRole.Default, ThemeRole.Primary, ThemeRole.Accent, ThemeRole.Contrast);

    /// <summary>
    /// Adds role definitions to the builder for items.
    /// </summary>
    /// <param name="roles">The theme roles to add.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public ControlThemeBuilder AddItemsRoles(params ThemeRole[] roles)
    {
        foreach (var role in roles)
        {
            if (_itemsRoles.Any(r => r.Role == role)) continue;
            _itemsRoles.Add(new RoleDefinition(role, true));
        }

        return this;
    }

    /// <summary>
    /// Adds default role definitions (Primary, Accent, Inverse, Success, Warning, Error, Information) for items.
    /// </summary>
    /// <returns>The current builder instance for method chaining.</returns>
    public ControlThemeBuilder AddItemsAllRoles() => AddItemsRoles([.. Enum.GetValues<ThemeRole>()]);

    /// <summary>
    /// Adds default role definitions (Primary, Accent, Inverse, Success, Warning, Error, Information) for items.
    /// </summary>
    /// <returns>The current builder instance for method chaining.</returns>
    public ControlThemeBuilder AddItemsDefaultRoles() => AddItemsRoles([.. Enum.GetValues<ThemeRole>().Except([ThemeRole.Neutral, ThemeRole.Inverse])]);

    /// <summary>
    /// Adds theme role definitions (Primary, Accent, Inverse) for items.
    /// </summary>
    /// <returns>The current builder instance for method chaining.</returns>
    public ControlThemeBuilder AddItemsThemeRoles() => AddItemsRoles(ThemeRole.Default, ThemeRole.Primary, ThemeRole.Accent, ThemeRole.Contrast);

    #endregion

    #region Build

    /// <summary>
    /// Builds the control theme definition with all configured settings.
    /// </summary>
    /// <param name="controlName">The name of the control.</param>
    /// <returns>The constructed control theme definition.</returns>
    public ControlThemeDefinition Build(string controlName)
    {
        var fullKey = ResolveKey(controlName, themeName);
        var theme = ResolveTheme(fullKey);

        var definition = new ControlThemeDefinition(theme, kind, fullKey, themeName, themeName, defaultContentType);
        definition.Variants.AddRange(_variants);
        definition.Sizes.AddRange(_sizes);
        definition.Roles.AddRange(_roles);
        definition.ItemsRoles.AddRange(_itemsRoles);
        definition.Shapes.AddRange(_shapes);

        return definition;
    }

    #endregion

    #region Theme resolution

    /// <summary>
    /// Resolves the theme resource key based on control name and theme name.
    /// </summary>
    /// <param name="control">The control name.</param>
    /// <param name="themeName">The theme name.</param>
    /// <returns>The resolved theme resource key, or null if no theme name is provided.</returns>
    private static string? ResolveKey(string control, string? themeName) => string.IsNullOrEmpty(themeName) ? null : ThemeResourceKeyFactory.Theme(control, themeName);

    /// <summary>
    /// Resolves and caches the control theme from the theme resource key.
    /// </summary>
    /// <param name="themeKey">The theme resource key.</param>
    /// <returns>The resolved control theme, or null if not found.</returns>
    private static ControlTheme? ResolveTheme(string? themeKey)
    {
        if (string.IsNullOrEmpty(themeKey))
            return null;

        if (ThemeCache.TryGetValue(themeKey, out var cached))
            return cached;

        if (Application.Current?.TryGetResource(themeKey, null, out var value) == true && value is ControlTheme theme)
        {
            ThemeCache[themeKey] = theme;
            return theme;
        }

        return null;
    }

    #endregion
}

/// <summary>
/// Represents a theme definition for a control with associated variants, roles, sizes, and shapes.
/// </summary>
/// <param name="Theme">The control theme.</param>
/// <param name="Kind">The kind identifier for the theme.</param>
/// <param name="FullKey">The resource key for the theme.</param>
/// <param name="Key">The theme name.</param>
/// <param name="DisplayName">The display name for the theme.</param>
/// <param name="DefaultContentType">The default content to display.</param>
internal sealed record ControlThemeDefinition(ControlTheme? Theme, string? Kind, string? FullKey, string? Key, string? DisplayName, ContentProviderType DefaultContentType = ContentProviderType.None)
    : AppearanceDefinition(DisplayName + (!string.IsNullOrEmpty(Kind) ? $" [{Kind}]" : string.Empty))
{
    /// <summary>
    /// Gets the collection of available variants for this theme.
    /// </summary>
    public ObservableCollection<VariantDefinition> Variants { get; } = [];

    /// <summary>
    /// Gets the collection of available roles for this theme.
    /// </summary>
    public ObservableCollection<RoleDefinition> Roles { get; } = [];

    /// <summary>
    /// Gets the collection of available roles for items for this theme.
    /// </summary>
    public ObservableCollection<RoleDefinition> ItemsRoles { get; } = [];

    /// <summary>
    /// Gets the collection of available sizes for this theme.
    /// </summary>
    public ObservableCollection<SizeDefinition> Sizes { get; } = [];

    /// <summary>
    /// Gets the collection of available shapes for this theme.
    /// </summary>
    public ObservableCollection<ShapeDefinition> Shapes { get; } = [];
}

/// <summary>
/// Represents a variant definition with one or more CSS classes.
/// </summary>
/// <param name="Classes">The array of CSS classes for this variant.</param>
internal sealed record VariantDefinition(string[] Classes) : AppearanceDefinition(Classes.Humanize(", "));

/// <summary>
/// Represents a role definition with a specific theme role.
/// </summary>
/// <param name="Role">The theme role.</param>
/// <param name="IsItemsRole">Whether this role is for items.</param>
internal sealed record RoleDefinition(ThemeRole Role, bool IsItemsRole) : AppearanceDefinition(Role.ToString());

/// <summary>
/// Represents a size definition with a CSS class.
/// </summary>
/// <param name="Class">The CSS class for this size.</param>
internal sealed record SizeDefinition(string Class) : AppearanceDefinition(Class);

/// <summary>
/// Represents a shape definition with a CSS class.
/// </summary>
/// <param name="Class">The CSS class for this shape.</param>
internal sealed record ShapeDefinition(string Class) : AppearanceDefinition(Class);

/// <summary>
/// Represents a shape definition with a CSS class.
/// </summary>
/// <param name="DisplayName">The display name for this definition.</param>
internal abstract record AppearanceDefinition(string DisplayName);
