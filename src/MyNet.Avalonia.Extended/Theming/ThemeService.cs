// -----------------------------------------------------------------------
// <copyright file="ThemeService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.UI.Theming;
using UiTheme = MyNet.UI.Theming.Theme;

namespace MyNet.Avalonia.Extended.Theming;

/// <summary>
/// Avalonia implementation of <see cref="IThemeService"/> that applies theme state to <see cref="IThemeBrushService"/>.
/// </summary>
public sealed class ThemeService(IThemeBrushService themeBrushService, IThemeBaseRegistry themeBaseRegistry) : IThemeService
{
    private readonly List<IThemeExtension> _baseExtensions = [];
    private readonly List<IThemeExtension> _primaryExtensions = [];
    private readonly List<IThemeExtension> _accentExtensions = [];

    /// <inheritdoc />
    public event EventHandler<ThemeChangedEventArgs>? ThemeChanged;

    /// <inheritdoc />
    public UiTheme CurrentTheme { get; private set; } = CreateThemeFromBrushService(themeBrushService, themeBaseRegistry);

    /// <inheritdoc />
    public void ApplyTheme(UiTheme theme)
    {
        ArgumentNullException.ThrowIfNull(theme);

        ApplyToBrushService(theme);
        ApplyExtensions(theme);
        CurrentTheme = theme;
        ThemeChanged?.Invoke(this, new(CurrentTheme));
    }

    /// <inheritdoc />
    public void ApplyBaseTheme(IThemeBase baseTheme)
    {
        ArgumentNullException.ThrowIfNull(baseTheme);
        ApplyTheme(CurrentTheme with { Base = baseTheme });
    }

    /// <inheritdoc />
    public void ApplyPrimary(string color, string? foreground = null)
        => ApplyTheme(CurrentTheme with { PrimaryColor = color, PrimaryForegroundColor = foreground });

    /// <inheritdoc />
    public void ApplyAccent(string color, string? foreground = null)
        => ApplyTheme(CurrentTheme with { AccentColor = color, AccentForegroundColor = foreground });

    /// <inheritdoc />
    public void UpdateTheme(Func<UiTheme, UiTheme> update)
    {
        ArgumentNullException.ThrowIfNull(update);
        ApplyTheme(update(CurrentTheme));
    }

    /// <inheritdoc />
    public IThemeService AddBaseExtension(IThemeExtension extension)
    {
        ArgumentNullException.ThrowIfNull(extension);
        _baseExtensions.Add(extension);
        return this;
    }

    /// <inheritdoc />
    public IThemeService AddPrimaryExtension(IThemeExtension extension)
    {
        ArgumentNullException.ThrowIfNull(extension);
        _primaryExtensions.Add(extension);
        return this;
    }

    /// <inheritdoc />
    public IThemeService AddAccentExtension(IThemeExtension extension)
    {
        ArgumentNullException.ThrowIfNull(extension);
        _accentExtensions.Add(extension);
        return this;
    }

    private void ApplyToBrushService(UiTheme theme)
    {
        var primaryColor = theme.PrimaryColor.ToColor();
        var primaryForegroundColor = theme.PrimaryForegroundColor.ToColor();
        var accentColor = theme.AccentColor.ToColor();
        var accentForegroundColor = theme.AccentForegroundColor.ToColor();
        var themeName = theme.Base.Name;

        if (primaryColor.HasValue && accentColor.HasValue)
        {
            themeBrushService.SetTheme(
                themeName,
                primaryColor.Value,
                accentColor.Value,
                primaryForegroundColor,
                accentForegroundColor);
            return;
        }

        themeBrushService.SetTheme(themeName);

        if (primaryColor.HasValue)
            themeBrushService.SetPrimary(primaryColor.Value, primaryForegroundColor);

        if (accentColor.HasValue)
            themeBrushService.SetAccent(accentColor.Value, accentForegroundColor);
    }

    private void ApplyExtensions(UiTheme theme)
    {
        var resources = Application.Current?.Resources;
        if (resources is null)
            return;

        MergeExtensionResources(resources, _baseExtensions, theme);
        MergeExtensionResources(resources, _primaryExtensions, theme);
        MergeExtensionResources(resources, _accentExtensions, theme);
    }

    private static void MergeExtensionResources(
        IResourceDictionary target,
        IReadOnlyList<IThemeExtension> extensions,
        UiTheme theme)
    {
        foreach (var extension in extensions)
        {
            foreach (var (key, value) in extension.GetResources(theme))
                target[key] = value!;
        }
    }

    private static UiTheme CreateThemeFromBrushService(IThemeBrushService brushService, IThemeBaseRegistry registry)
    {
        var primary = brushService.GetPrimary();
        var accent = brushService.GetAccent();
        var baseTheme = ResolveBase(registry, brushService.GetTheme());

        return new(
            baseTheme,
            primary?.Base.ToHex() ?? "#2563EB",
            accent?.Base.ToHex() ?? "#F59E0B",
            primary?.Foreground.ToHex(),
            accent?.Foreground.ToHex());
    }

    private static IThemeBase ResolveBase(IThemeBaseRegistry registry, string? themeName) => !string.IsNullOrWhiteSpace(themeName) && registry.Get(themeName) is { } registered ? registered : registry.Dark;
}
