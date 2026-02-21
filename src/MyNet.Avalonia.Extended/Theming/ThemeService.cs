// -----------------------------------------------------------------------
// <copyright file="ThemeService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theme.Infrastructure;
using MyNet.Avalonia.Theme.Palettes;
using MyNet.UI.Theming;
using MyNet.Utilities;

namespace MyNet.Avalonia.Extended.Theming;

/// <summary>
/// Service for managing application myTheme (Dark/Light/HighContrast) and brand colors (Primary/Accent).
/// Integrates with MyNet.Avalonia.Theme.MyTheme for hot-reload support.
/// </summary>
public class ThemeService(IMyTheme myTheme, IThemeBaseRegistry themeBaseRegistry) : IThemeService
{
    /// <summary>
    /// Event raised when the myTheme changes.
    /// </summary>
    public event EventHandler<ThemeChangedEventArgs>? ThemeChanged;

    /// <summary>
    /// Gets the current myTheme configuration.
    /// </summary>
    public UI.Theming.Theme CurrentTheme
    {
        get
        {
            var primary = myTheme.Primary;
            var accent = myTheme.Accent;
            return new(themeBaseRegistry.Get(myTheme.Theme.OrEmpty()) ?? themeBaseRegistry.Dark, primary.Base.ToHex(), accent.Base.ToHex(), primary.Foreground.ToHex(), accent.Foreground.ToHex());
        }
    }

    /// <summary>
    /// Applies a myTheme configuration to the application.
    /// </summary>
    /// <param name="theme">The myTheme to apply.</param>
    public void ApplyTheme(UI.Theming.Theme theme)
    {
        myTheme.Theme = theme.Base.ToString();

        var primaryColor = theme.PrimaryColor.ToColor();
        var primaryForegroundColor = theme.PrimaryForegroundColor.ToColor();
        if (primaryColor.HasValue)
        {
            myTheme.Primary = new ColorShades(primaryColor.Value, primaryForegroundColor);
        }

        var accentColor = theme.AccentColor.ToColor();
        var accentForegroundColor = theme.AccentForegroundColor.ToColor();
        if (accentColor.HasValue)
        {
            myTheme.Accent = new ColorShades(accentColor.Value, accentForegroundColor);
        }

        ThemeChanged?.Invoke(this, new ThemeChangedEventArgs(CurrentTheme));
    }

    /// <summary>
    /// Applies a base myTheme (Dark/Light/HighContrast) to the application, keeping existing brand colors. This allows changing the overall theme mode while preserving the current primary and accent colors.
    /// </summary>
    /// <param name="baseTheme">The base theme to apply.</param>
    public void ApplyBaseTheme(IThemeBase baseTheme)
    {
        var currentTheme = CurrentTheme;

        ApplyTheme(currentTheme with { Base = baseTheme });
    }

    /// <summary>
    /// Applies a primary color to the application, keeping existing base myTheme and accent color. This allows changing the primary brand color while preserving the overall theme mode and accent color.
    /// </summary>
    /// <param name="color">The primary color to apply.</param>
    /// <param name="foreground">The primary foreground color to apply.</param>
    public void ApplyPrimary(string color, string? foreground = null)
    {
        var currentTheme = CurrentTheme;
        ApplyTheme(currentTheme with { PrimaryColor = color, PrimaryForegroundColor = foreground });
    }

    /// <summary>
    /// Applies an accent color to the application, keeping existing base myTheme and primary color. This allows changing the accent color while preserving the overall theme mode and primary brand color.
    /// </summary>
    /// <param name="color">The accent color to apply.</param>
    /// <param name="foreground">The accent foreground color to apply.</param>
    public void ApplyAccent(string color, string? foreground = null)
    {
        var currentTheme = CurrentTheme;
        ApplyTheme(currentTheme with { AccentColor = color, AccentForegroundColor = foreground });
    }

    /// <summary>
    /// Updates the current myTheme configuration using a provided update action, allowing for flexible modifications to the theme properties. The update action receives the current theme as a parameter, and any changes made to the theme within the action will be applied when the method completes. This allows for complex theme updates that may involve multiple properties or conditional logic while ensuring that the updated theme is applied correctly to the application.
    /// </summary>
    /// <param name="update">The update action to apply to the current theme.</param>
    public void UpdateTheme(Action<UI.Theming.Theme> update)
    {
        var currentTheme = CurrentTheme;
        update(currentTheme);
        ApplyTheme(currentTheme);
    }

    /// <summary>
    /// Adds a base myTheme extension (not implemented - for future use).
    /// </summary>
    public IThemeService AddBaseExtension(IThemeExtension extension) => this;

    /// <summary>
    /// Adds a primary color extension (not implemented - for future use).
    /// </summary>
    public IThemeService AddPrimaryExtension(IThemeExtension extension) => this;

    /// <summary>
    /// Adds an accent color extension (not implemented - for future use).
    /// </summary>
    public IThemeService AddAccentExtension(IThemeExtension extension) => this;
}
