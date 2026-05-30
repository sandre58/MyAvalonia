// -----------------------------------------------------------------------
// <copyright file="ThemePageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Avalonia.Media;
using DynamicData;
using Material.Icons;
using MyNet.Avalonia.Colors;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Showcase.ViewModels.Base;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Avalonia.Theme.Theming.Palettes;
using MyNet.Observable.Attributes;
using MyNet.UI.Theming;
using MyNet.Utilities;
using MyNet.Utilities.Suspending;
using PropertyChanged;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

[SuppressMessage("ReSharper", "UnusedMember.Local", Justification = "Used by Fody")]
internal sealed class ThemePageViewModel : PageViewModel
{
    private readonly IThemeService _themeService;
    private readonly IThemeBrushService _themeBrushService;
    private readonly Suspender _updateSuspender = new();
    private readonly Suspender _refreshThemePropertiesSuspender = new();

    public ThemePageViewModel(IThemeService themeService, IThemeBrushService themeBrushService, IThemeBaseRegistry themeBaseRegistry)
    {
        _themeService = themeService;
        _themeBrushService = themeBrushService;

        AvailableThemeVariants.AddRange(themeBaseRegistry.AvailableBases);

        UpdatePropertiesFromCurrentTheme();

        _themeService.ThemeChanged += OnThemeChanged;

        var currentPalette = _themeBrushService.GetThemePalette();

        if (currentPalette is not null)
        {
            var baseKeys = currentPalette.Base.ToResourceDictionary().Keys.ToList();
            var opacityLevels = currentPalette.Opacity.ToResourceDictionary(string.Empty).Keys;

            Primary.AddRange(GetBrushDefinitions(nameof(Primary), _themeBrushService.GetPrimary()));
            Accent.AddRange(GetBrushDefinitions(nameof(Accent), _themeBrushService.GetAccent()));

            Surfaces.AddRange(GetBrushDefinitions([.. baseKeys.Where(x => x.Contains("Surface", StringComparison.OrdinalIgnoreCase) && !x.Contains("Border", StringComparison.OrdinalIgnoreCase))]));
            Borders.AddRange(GetBrushDefinitions([.. baseKeys.Where(x => x.Contains("Border", StringComparison.OrdinalIgnoreCase) || x.Contains("Divider", StringComparison.OrdinalIgnoreCase))]));
            Foregrounds.AddRange(GetBrushDefinitions([.. baseKeys.Where(x => x.Contains("Foreground", StringComparison.OrdinalIgnoreCase))]));

            Semantic.Add(GetBrushDefinitions(nameof(ThemeVariantPalette.Success), currentPalette.Success));
            Semantic.Add(GetBrushDefinitions(nameof(ThemeVariantPalette.Error), currentPalette.Error));
            Semantic.Add(GetBrushDefinitions(nameof(ThemeVariantPalette.Warning), currentPalette.Warning));
            Semantic.Add(GetBrushDefinitions(nameof(ThemeVariantPalette.Information), currentPalette.Information));
            Semantic.Add(GetBrushDefinitions(nameof(ThemeVariantPalette.Neutral), currentPalette.Neutral));

            OpacityLevels.AddRange(opacityLevels.Select(x => new OpacityDefinition(x, themeBrushService.GetOpacity(x) ?? 0.0)));
        }
    }

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.PaletteSwatchVariant;

    public ObservableCollection<IThemeBase> AvailableThemeVariants { get; } = [];

    [IsRequired]
    public IThemeBase? Base { get; set; }

    [IsRequired]
    public Color? PrimaryColor { get; set; }

    [IsRequired]
    public Color? AccentColor { get; set; }

    public ObservableCollection<BrushDefinition> Primary { get; } = [];

    public ObservableCollection<BrushDefinition> Accent { get; } = [];

    public ObservableCollection<BrushDefinition> Surfaces { get; } = [];

    public ObservableCollection<BrushDefinition> Borders { get; } = [];

    public ObservableCollection<BrushDefinition> Foregrounds { get; } = [];

    public ObservableCollection<ObservableCollection<BrushDefinition>> Semantic { get; } = [];

    public ObservableCollection<OpacityDefinition> OpacityLevels { get; } = [];

    private ObservableCollection<BrushDefinition> GetBrushDefinitions(string prefix, ColorShades shades)
        => shades.ToResourceDictionary(prefix)
            .Where(x => !x.Key.Contains(nameof(ColorShades.Foreground), StringComparison.OrdinalIgnoreCase))
            .Select(x => new BrushDefinition(x.Key, (ISolidColorBrush)_themeBrushService.GetBrush(x.Key), (ISolidColorBrush)_themeBrushService.GetBrush(x.Key, contrast: true)))
            .ToObservableCollection();

    private ObservableCollection<BrushDefinition> GetBrushDefinitions(IReadOnlyCollection<string> resourceKeys)
        => resourceKeys
            .Select(x => new BrushDefinition(x, (ISolidColorBrush)_themeBrushService.GetBrush(x), (ISolidColorBrush)_themeBrushService.GetBrush(x, contrast: true)))
            .ToObservableCollection();

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
    private void OnBaseChanged()
    {
        if (_updateSuspender.IsSuspended || Base is null) return;

        using (_refreshThemePropertiesSuspender.Suspend())
            _themeService.ApplyBaseTheme(Base);
    }

    [SuppressPropertyChangedWarnings]
    private void OnPrimaryColorChanged()
    {
        if (_updateSuspender.IsSuspended || !PrimaryColor.HasValue) return;

        using (_refreshThemePropertiesSuspender.Suspend())
            _themeService.ApplyPrimary(PrimaryColor.Value.ToHex());
    }

    [SuppressPropertyChangedWarnings]
    private void OnAccentColorChanged()
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

public record BrushDefinition(string Key, ISolidColorBrush Brush, ISolidColorBrush Foreground);

public record OpacityDefinition(string Key, double Value);
