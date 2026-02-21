// -----------------------------------------------------------------------
// <copyright file="ThemeVariantsRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Styling;
using MyNet.Avalonia.Theme.Themes;
using MyNet.UI.Theming;

namespace MyNet.Avalonia.Extended.Theming;

public class ThemeVariantsRegistry : IThemeBaseRegistry
{
    private static readonly ThemeBase DefaultDark = new(ThemeVariant.Dark, true, false);
    private static readonly ThemeBase DefaultLight = new(ThemeVariant.Light, false, false);
    private static readonly ThemeBase DefaultHighContrast = new(ThemeVariantProvider.HighContrast, true, true);
    private readonly Dictionary<string, IThemeBase> _themeBases = [];

    /// <summary>
    /// Gets the light theme base, which represents the standard light theme variant in the theming system. This theme base is used to provide the default light theme configuration for the application.
    /// </summary>
    public IThemeBase Light => DefaultLight;

    /// <summary>
    /// Gets the dark theme base, which represents the standard dark theme variant in the theming system. This theme base is used to provide the default dark theme configuration for the application.
    /// </summary>
    public IThemeBase Dark => DefaultDark;

    /// <summary>
    /// Gets the high contrast theme base, which represents the high contrast theme variant in the theming system. This theme base is used to provide an enhanced visibility and accessibility theme configuration for users with visual impairments.
    /// </summary>
    public IThemeBase HighContrast => DefaultHighContrast;

    /// <summary>
    /// Gets the collection of available theme bases registered in the theming system, including the default light, dark, and high contrast theme bases, as well as any additional theme bases that have been registered. This collection is used to provide access to all theme bases that can be applied to the application.
    /// </summary>
    public IReadOnlyCollection<IThemeBase> Availables => _themeBases.Values;

    /// <summary>
    /// Gets a theme base by its name from the registry. This method allows retrieving a specific theme base based on its unique name, which is derived from the associated theme variant key. If the theme base with the specified name exists in the registry, it is returned; otherwise, null is returned. This method is used to access specific theme bases for applying them to the application or for other theming operations.
    /// </summary>
    /// <param name="name">The unique name of the theme base to retrieve.</param>
    /// <returns>The theme base associated with the specified name, or null if not found.</returns>
    public IThemeBase? Get(string name) => _themeBases.GetValueOrDefault(name);

    /// <summary>
    /// Registers a new theme base in the registry. This method allows adding a custom theme base to the theming system, making it available for retrieval and application. The theme base is identified by its unique name, which is derived from the associated theme variant key. If a theme base with the same name already exists in the registry, it will be overwritten with the new theme base. This method is used to expand the theming capabilities of the application by allowing developers to define and register their own custom theme bases.
    /// </summary>
    /// <param name="themeBase">The theme base to register.</param>
    public void Register(IThemeBase themeBase) => _themeBases[themeBase.Name] = themeBase;

    /// <summary>
    /// Registers a new theme base in the registry. This method allows adding a custom theme base to the theming system, making it available for retrieval and application. The theme base is identified by its unique name, which is derived from the associated theme variant key. If a theme base with the same name already exists in the registry, it will be overwritten with the new theme base. This method is used to expand the theming capabilities of the application by allowing developers to define and register their own custom theme bases.
    /// </summary>
    /// <param name="variant">The theme base to register.</param>
    /// <param name="isDark">True if the theme is dark, false otherwise.</param>
    /// <param name="isHighContrast">True if the theme is high contrast, false otherwise.</param>
    public void Register(ThemeVariant variant, bool isDark, bool isHighContrast) => Register(new ThemeBase(variant, isDark, isHighContrast));
}
