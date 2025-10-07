// -----------------------------------------------------------------------
// <copyright file="ThemeViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Media;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theme;
using MyNet.Observable;
using MyNet.Observable.Attributes;
using MyNet.UI.Theming;
using MyNet.UI.ViewModels.Workspace;
using MyNet.Utilities;
using MyNet.Utilities.Suspending;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.ViewModels;

public class ThemeViewModel : NavigableWorkspaceViewModel
{
    private readonly Suspender _applyThemeSuspender = new();

    private readonly ObservableCollection<BrushData> _accentBrushes = [];
    private readonly ObservableCollection<BrushData> _themeBrushes = [];

    public DataGridCollectionView AccentBrushes { get; }

    public DataGridCollectionView ThemeBrushes { get; }

    [IsRequired]
    public ThemeBase? CurrentBase { get; set; }

    [IsRequired]
    public Color? CurrentPrimaryColor { get; set; }

    [IsRequired]
    public Color? CurrentAccentColor { get; set; }

    public ThemeViewModel()
    {
        AccentBrushes = new DataGridCollectionView(_accentBrushes);
        AccentBrushes.GroupDescriptions.Add(new DataGridPathGroupDescription("Category"));

        ThemeBrushes = new DataGridCollectionView(_themeBrushes);
        ThemeBrushes.GroupDescriptions.Add(new DataGridPathGroupDescription("Category"));

        UpdatePropertiesFromCurrentTheme();

        ThemeManager.ThemeChanged += OnThemeChanged;

        UpdateBrushes();
    }

    private void UpdatePropertiesFromCurrentTheme()
    {
        using (_applyThemeSuspender.Suspend())
        {
            CurrentBase = ThemeManager.CurrentTheme?.Base;
            CurrentPrimaryColor = ThemeManager.CurrentTheme?.PrimaryColor.ToColor();
            CurrentAccentColor = ThemeManager.CurrentTheme?.AccentColor.ToColor();
        }
    }

    [SuppressPropertyChangedWarnings]
    private void OnThemeChanged(object? sender, ThemeChangedEventArgs e)
    {
        UpdatePropertiesFromCurrentTheme();
        UpdateBrushes();
    }

    private void UpdateBrushes()
    {
        _accentBrushes.Set(MyTheme.Current.Resources.Where(x => x.Key.ToString().OrEmpty().Contains(ThemeResources.BrushKey, System.StringComparison.OrdinalIgnoreCase))
                                                            .Select(x => new BrushData(x.Key.ToString().OrEmpty())));
        _themeBrushes.Set(((ResourceDictionary)MyTheme.Current.Resources.ThemeDictionaries[global::Avalonia.Application.Current!.ActualThemeVariant])
                                                       .Where(x => x.Key.ToString().OrEmpty().Contains(ThemeResources.BrushKey, System.StringComparison.OrdinalIgnoreCase))
                                                       .Select(x => new BrushData(x.Key.ToString().OrEmpty()))
                                                       .Where(x => x.Category != "Code"));
    }

    protected virtual void OnCurrentBaseChanged()
    {
        if (_applyThemeSuspender.IsSuspended) return;

        if (CurrentBase.HasValue)
            ThemeManager.ApplyBase(CurrentBase.Value);
    }

    protected virtual void OnCurrentPrimaryColorChanged()
    {
        if (_applyThemeSuspender.IsSuspended) return;

        if (CurrentPrimaryColor.HasValue)
            ThemeManager.ApplyPrimaryColor(CurrentPrimaryColor.Value.ToHex());
    }

    protected virtual void OnCurrentAccentColorChanged()
    {
        if (_applyThemeSuspender.IsSuspended) return;

        if (CurrentAccentColor.HasValue)
            ThemeManager.ApplyAccentColor(CurrentAccentColor.Value.ToHex());
    }

    protected override void Cleanup()
    {
        base.Cleanup();

        ThemeManager.ThemeChanged -= OnThemeChanged;
    }
}

internal sealed class BrushData : ObservableObject
{
    public BrushData(string fullName)
    {
        Brush = ResourceLocator.GetResource<IBrush>(fullName);
        FullName = fullName;
        ColorFullName = fullName.Replace(ThemeResources.BrushKey, ThemeResources.ColorKey, System.StringComparison.OrdinalIgnoreCase);
        Category = GetCategory(fullName);

        var stringToReplace = !string.IsNullOrEmpty(Category) ? $"{ThemeResources.ResourcePrefix}.{ThemeResources.BrushKey}.{Category}." : $"{ThemeResources.ResourcePrefix}.{ThemeResources.BrushKey}.";
        Name = fullName.Replace(stringToReplace, string.Empty, System.StringComparison.OrdinalIgnoreCase);
        Color = (Brush as SolidColorBrush)?.Color;
        Opacity = Brush.Opacity;
    }

    public string Name { get; }

    public string FullName { get; }

    public string ColorFullName { get; }

    public string Category { get; }

    public IBrush Brush { get; }

    public Color? Color { get; private set; }

    public double Opacity { get; private set; }

    private static string GetCategory(string fullName)
    {
        var strings = fullName.Split('.');

        return strings.Length <= 3 && !new List<string> { "Transparency", "Primary", "Accent" }.Contains(strings[2]) ? "Others" : strings[2];
    }
}

internal sealed class OpacityData : ObservableObject
{
    private readonly string _brushName;

    public OpacityData(string displayName, string brushName)
    {
        DisplayName = displayName;
        Name = ThemeResources.GetOpacityKey(displayName);
        _brushName = brushName;
        Brush = ThemeResources.GetBrush(brushName);
        Opacity = ThemeResources.GetOpacity(Name);

        ThemeManager.ThemeChanged += ThemeManager_ThemeChanged;
    }

    private void ThemeManager_ThemeChanged(object? sender, ThemeChangedEventArgs e) => Brush = ThemeResources.GetBrush(_brushName);

    public string DisplayName { get; }

    public string Name { get; }

    public double Opacity { get; }

    public IBrush Brush { get; private set; }
}
