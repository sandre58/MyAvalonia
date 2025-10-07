// -----------------------------------------------------------------------
// <copyright file="ThemeService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Styling;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theming;
using MyNet.UI.Theming;

namespace MyNet.Avalonia.UI.Theming;

public class ThemeService(IAvaloniaTheme theme) : IThemeService
{
    public event EventHandler<ThemeChangedEventArgs>? ThemeChanged;

    private IAvaloniaTheme Theme { get; } = theme;

    public Theme CurrentTheme => new()
    {
        Base = Application.Current?.RequestedThemeVariant?.Key switch
        {
            nameof(ThemeVariant.Dark) => ThemeBase.Dark,
            nameof(ThemeVariant.Light) => ThemeBase.Light,
            _ => ThemeBase.Inherit
        },
        PrimaryColor = Theme.PrimaryColor.ToHex(),
        PrimaryForegroundColor = Theme.PrimaryForegroundColor?.ToHex(),
        AccentColor = Theme.AccentColor.ToHex(),
        AccentForegroundColor = Theme.AccentForegroundColor?.ToHex()
    };

    public IThemeService AddBaseExtension(IThemeExtension extension) => this;

    public IThemeService AddPrimaryExtension(IThemeExtension extension) => this;

    public IThemeService AddAccentExtension(IThemeExtension extension) => this;

    public void ApplyTheme(Theme theme)
    {
        if (theme.Base is not null && Application.Current is not null)
        {
            Application.Current.RequestedThemeVariant = theme.Base switch
            {
                ThemeBase.Dark => ThemeVariant.Dark,
                ThemeBase.Light => ThemeVariant.Light,
                ThemeBase.Inherit => ThemeVariant.Default,
                ThemeBase.HighContrast => ThemeVariant.Default,
                _ => throw new InvalidOperationException()
            };
        }

        if (theme.PrimaryColor is not null)
        {
            Theme.PrimaryColor = theme.PrimaryColor.ToColor() ?? default;
            Theme.PrimaryForegroundColor = theme.PrimaryForegroundColor.ToColor();
        }

        if (theme.AccentColor is not null)
        {
            Theme.AccentColor = theme.AccentColor.ToColor() ?? default;
            Theme.AccentForegroundColor = theme.AccentForegroundColor.ToColor();
        }

        ThemeChanged?.Invoke(this, new ThemeChangedEventArgs(CurrentTheme));
    }
}
