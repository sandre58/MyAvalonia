// -----------------------------------------------------------------------
// <copyright file="ThemeVariantsRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Styling;
using MyNet.Avalonia.Theme.Themes;
using MyNet.UI.Theming;

namespace MyNet.Avalonia.Extended.Theming;

/// <summary>
/// Registry of Avalonia theme bases available to the application.
/// </summary>
public sealed class ThemeVariantsRegistry : IThemeBaseRegistry
{
    private readonly Dictionary<string, IThemeBase> _themeBases = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeVariantsRegistry"/> class
    /// and registers the built-in light, dark, and high contrast theme bases.
    /// </summary>
    public ThemeVariantsRegistry()
    {
        Register(Light);
        Register(Dark);
        Register(HighContrast);
    }

    /// <inheritdoc />
    public IThemeBase Light { get; } = new ThemeBase(ThemeVariant.Light, isDark: false, isHighContrast: false);

    /// <inheritdoc />
    public IThemeBase Dark { get; } = new ThemeBase(ThemeVariant.Dark, isDark: true, isHighContrast: false);

    /// <inheritdoc />
    public IThemeBase HighContrast { get; } = new ThemeBase(ThemeVariantProvider.HighContrast, isDark: true, isHighContrast: true);

    /// <inheritdoc />
    public IReadOnlyCollection<IThemeBase> AvailableBases => _themeBases.Values.ToList();

    /// <inheritdoc />
    public IThemeBase? Get(string name) => _themeBases.GetValueOrDefault(name);

    /// <inheritdoc />
    public void Register(IThemeBase themeBase)
    {
        ArgumentNullException.ThrowIfNull(themeBase);
        _themeBases[themeBase.Name] = themeBase;
    }

    /// <summary>
    /// Registers a theme base for the specified Avalonia theme variant.
    /// </summary>
    /// <param name="variant">The Avalonia theme variant.</param>
    /// <param name="isDark">True if the theme is dark; otherwise, false.</param>
    /// <param name="isHighContrast">True if the theme is high contrast; otherwise, false.</param>
    public void Register(ThemeVariant variant, bool isDark, bool isHighContrast)
        => Register(new ThemeBase(variant, isDark, isHighContrast));
}
