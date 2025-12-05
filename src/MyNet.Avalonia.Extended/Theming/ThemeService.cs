// -----------------------------------------------------------------------
// <copyright file="ThemeService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Media;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theme.Palettes;
using MyNet.Avalonia.Theme.Theming;
using MyNet.UI.Theming;

namespace MyNet.Avalonia.Extended.Theming;

/// <summary>
/// Service for managing application myTheme (Dark/Light/HighContrast) and brand colors (Primary/Accent).
/// Integrates with MyNet.Avalonia.Theme.MyTheme for hot-reload support.
/// </summary>
public class ThemeService(IMyTheme myTheme) : IThemeService
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
            return new()
            {
                Base = Enum.TryParse<ThemeBase>(myTheme.Theme, out var baseTheme) ? baseTheme : ThemeBase.Inherit,
                PrimaryColor = primary.Base.ToHex(),
                PrimaryForegroundColor = primary.Foreground.ToHex(),
                AccentColor = accent.Foreground.ToHex(),
                AccentForegroundColor = accent.Foreground.ToHex()
            };
        }
    }

    /// <summary>
    /// Applies a myTheme configuration to the application.
    /// </summary>
    /// <param name="theme">The myTheme to apply.</param>
    public void ApplyTheme(UI.Theming.Theme theme)
    {
        if (theme.Base is not null)
        {
            myTheme.Theme = theme.Base.ToString();
        }

        if (theme.PrimaryColor is not null)
        {
            var primaryColor = theme.PrimaryColor.ToColor();
            var foregroundColor = theme.PrimaryForegroundColor.ToColor();
            if (primaryColor.HasValue)
            {
                myTheme.Primary = new ColorShades(primaryColor.Value, foregroundColor);
            }
        }

        if (theme.AccentColor is not null)
        {
            var accentColor = theme.AccentColor.ToColor();
            var foregroundColor = theme.AccentForegroundColor.ToColor();
            if (accentColor.HasValue)
            {
                myTheme.Accent = new ColorShades(accentColor.Value, foregroundColor);
            }
        }

        ThemeChanged?.Invoke(this, new ThemeChangedEventArgs(CurrentTheme));
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
