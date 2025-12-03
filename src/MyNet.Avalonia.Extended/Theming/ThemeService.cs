// -----------------------------------------------------------------------
// <copyright file="ThemeService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Media;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theming;
using MyNet.UI.Theming;

namespace MyNet.Avalonia.Extended.Theming;

/// <summary>
/// Service for managing application avaloniaTheme (Dark/Light/HighContrast) and brand colors (Primary/Accent).
/// Integrates with MyNet.Avalonia.Theme.MyTheme for hot-reload support.
/// </summary>
public class ThemeService(IAvaloniaTheme avaloniaTheme) : IThemeService
{
    /// <summary>
    /// Event raised when the avaloniaTheme changes.
    /// </summary>
    public event EventHandler<ThemeChangedEventArgs>? ThemeChanged;

    /// <summary>
    /// Gets the current avaloniaTheme configuration.
    /// </summary>
    public UI.Theming.Theme CurrentTheme
    {
        get
        {
            var primary = avaloniaTheme.GetPrimaryPair();
            var accent = avaloniaTheme.GetAccentPair();
            return new()
            {
                Base = avaloniaTheme.GetThemeName() switch
                {
                    "Dark" => ThemeBase.Dark,
                    "Light" => ThemeBase.Light,
                    "HighContrast" => ThemeBase.Dark, // Map HighContrast to Dark for legacy compatibility
                    _ => ThemeBase.Inherit
                },
                PrimaryColor = primary.Color.ToHex(),
                PrimaryForegroundColor = primary.ForegroundColor?.ToHex(),
                AccentColor = accent.Color.ToHex(),
                AccentForegroundColor = accent.ForegroundColor?.ToHex()
            };
        }
    }

    /// <summary>
    /// Applies a avaloniaTheme configuration to the application.
    /// </summary>
    /// <param name="theme">The avaloniaTheme to apply.</param>
    public void ApplyTheme(UI.Theming.Theme theme)
    {
        if (theme.Base is not null)
        {
            avaloniaTheme.SetTheme(theme.Base switch
            {
                ThemeBase.Dark => nameof(ThemeBase.Dark),
                ThemeBase.Light => nameof(ThemeBase.Light),
                _ => null
            });
        }

        if (theme.PrimaryColor is not null)
        {
            var primaryColor = theme.PrimaryColor.ToColor();
            var foregroundColor = theme.PrimaryForegroundColor;
            if (primaryColor.HasValue)
            {
                avaloniaTheme.SetPrimary(primaryColor.Value, foregroundColor?.ToColor());
            }
        }

        if (theme.AccentColor is not null)
        {
            var accentColor = theme.AccentColor.ToColor();
            var foregroundColor = theme.AccentForegroundColor;
            if (accentColor.HasValue)
            {
                avaloniaTheme.SetAccent(accentColor.Value, foregroundColor?.ToColor());
            }
        }

        ThemeChanged?.Invoke(this, new ThemeChangedEventArgs(CurrentTheme));
    }

    /// <summary>
    /// Adds a base avaloniaTheme extension (not implemented - for future use).
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
