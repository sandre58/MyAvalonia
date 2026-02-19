// -----------------------------------------------------------------------
// <copyright file="ThemePageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using Avalonia.Media;
using DynamicData;
using MyNet.Avalonia.Extensions;
using MyNet.Observable.Attributes;
using MyNet.UI.Theming;
using MyNet.Utilities.Suspending;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.ViewModels;

#pragma warning disable CS0628 // New protected member declared in sealed type

internal sealed class ThemePageViewModel : PageViewModel
{
    private readonly IThemeService _themeService;
    private readonly Suspender _updateSuspender = new();
    private readonly Suspender _refreshThemePropertiesSuspender = new();

    public ThemePageViewModel(IThemeService themeService, IThemeBaseRegistry themeBaseRegistry)
    {
        _themeService = themeService;

        AvailableThemeVariants.AddRange(themeBaseRegistry.Availables);
        UpdatePropertiesFromCurrentTheme();

        _themeService.ThemeChanged += OnThemeChanged;
    }

    public ObservableCollection<IThemeBase> AvailableThemeVariants { get; } = [];

    [IsRequired]
    public IThemeBase? Base { get; set; }

    [IsRequired]
    public Color? PrimaryColor { get; set; }

    [IsRequired]
    public Color? AccentColor { get; set; }

    public ObservableCollection<BrushGroup> BrushGroups { get; } = [];

    private void UpdatePropertiesFromCurrentTheme()
    {
        using (_updateSuspender.Suspend())
        {
            var currentTheme = _themeService.CurrentTheme;
            Base = currentTheme.Base;
            PrimaryColor = currentTheme.PrimaryColor.ToColor();
            AccentColor = currentTheme.AccentColor.ToColor();
        }
    }

    [SuppressPropertyChangedWarnings]
    private void OnThemeChanged(object? sender, EventArgs e)
    {
        if (_refreshThemePropertiesSuspender.IsSuspended) return;

        UpdatePropertiesFromCurrentTheme();
    }

    [SuppressPropertyChangedWarnings]
    protected void OnBaseChanged()
    {
        if (_updateSuspender.IsSuspended || Base is null) return;

        using (_refreshThemePropertiesSuspender.Suspend())
            _themeService.ApplyBaseTheme(Base);
    }

    [SuppressPropertyChangedWarnings]
    protected void OnPrimaryColorChanged()
    {
        if (_updateSuspender.IsSuspended || !PrimaryColor.HasValue) return;

        using (_refreshThemePropertiesSuspender.Suspend())
            _themeService.ApplyPrimary(PrimaryColor.Value.ToHex());
    }

    [SuppressPropertyChangedWarnings]
    protected void OnAccentColorChanged()
    {
        if (_updateSuspender.IsSuspended || !AccentColor.HasValue) return;

        using (_refreshThemePropertiesSuspender.Suspend())
            _themeService.ApplyAccent(AccentColor.Value.ToHex());
    }

    protected override void Cleanup()
    {
        _themeService.ThemeChanged -= OnThemeChanged;
        base.Cleanup();
    }
}

public record BrushGroup(string Name, Collection<string> BrushKeys, string? Description = null);

#pragma warning restore CS0628 // New protected member declared in sealed type
