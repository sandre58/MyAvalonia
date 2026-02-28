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
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.Theme.Palettes;
using MyNet.Observable.Attributes;
using MyNet.UI.Theming;
using MyNet.Utilities;
using MyNet.Utilities.Suspending;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.ViewModels;

[SuppressMessage("ReSharper", "UnusedMember.Local", Justification = "Used by Fody")]
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

        var currentTheme = MyTheme.Current;
        var currentThemeColorVariant = currentTheme.GetCurrentThemeVariantColors();

        if (currentThemeColorVariant is not null)
        {
            var baseKeys = currentThemeColorVariant.Base.ToResourceDictionary().Keys.ToList();
            var opacityLevels = currentThemeColorVariant.Opacity.ToResourceDictionary(string.Empty).Keys;

            Primary.AddRange(GetBrushDefinitions(currentTheme, nameof(Primary), MyTheme.Current.Primary));
            Accent.AddRange(GetBrushDefinitions(currentTheme, nameof(Accent), MyTheme.Current.Accent));

            Surfaces.AddRange(GetBrushDefinitions([.. baseKeys.Where(x => x.Contains("Surface", StringComparison.OrdinalIgnoreCase) && !x.Contains("Border", StringComparison.OrdinalIgnoreCase))], currentTheme));
            Borders.AddRange(GetBrushDefinitions([.. baseKeys.Where(x => x.Contains("Border", StringComparison.OrdinalIgnoreCase) || x.Contains("Divider", StringComparison.OrdinalIgnoreCase))], currentTheme));
            Foregrounds.AddRange(GetBrushDefinitions([.. baseKeys.Where(x => x.Contains("Foreground", StringComparison.OrdinalIgnoreCase))], currentTheme));

            Semantic.Add(GetBrushDefinitions(currentTheme, nameof(ThemeVariantColors.Success), currentThemeColorVariant.Success));
            Semantic.Add(GetBrushDefinitions(currentTheme, nameof(ThemeVariantColors.Error), currentThemeColorVariant.Error));
            Semantic.Add(GetBrushDefinitions(currentTheme, nameof(ThemeVariantColors.Warning), currentThemeColorVariant.Warning));
            Semantic.Add(GetBrushDefinitions(currentTheme, nameof(ThemeVariantColors.Information), currentThemeColorVariant.Information));
            Semantic.Add(GetBrushDefinitions(currentTheme, nameof(ThemeVariantColors.Neutral), currentThemeColorVariant.Neutral));

            OpacityLevels.AddRange(opacityLevels.Select(x => new OpacityDefinition(x, currentTheme.GetOpacity(x) ?? 0.0)));
        }
    }

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

    private static ObservableCollection<BrushDefinition> GetBrushDefinitions(MyTheme theme, string prefix, ColorShades shades)
        => shades.ToResourceDictionary(prefix)
            .Where(x => !x.Key.Contains(nameof(ColorShades.Foreground), StringComparison.OrdinalIgnoreCase))
            .Select(x => new BrushDefinition(x.Key, (ISolidColorBrush)theme.GetBrush(x.Key), (ISolidColorBrush)theme.GetBrush(x.Key, contrast: true)))
            .ToObservableCollection();

    private static ObservableCollection<BrushDefinition> GetBrushDefinitions(IReadOnlyCollection<string> resourceKeys, MyTheme currentTheme)
        => resourceKeys
            .Select(x => new BrushDefinition(x, (ISolidColorBrush)currentTheme.GetBrush(x), (ISolidColorBrush)currentTheme.GetBrush(x, contrast: true)))
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
