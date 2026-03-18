// -----------------------------------------------------------------------
// <copyright file="ControlPlaygroundViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using DynamicData.Binding;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ClassProviders;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ContentProviders;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Observable;
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

/// <summary>
/// View model for the control playground, which provides interactive testing and preview functionality.
/// </summary>
internal sealed class ControlPlaygroundViewModel : ObservableObject
{
    private readonly string _controlName;
    private readonly List<string> _customClasses = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="ControlPlaygroundViewModel"/> class.
    /// </summary>
    /// <param name="controlName">The name of the control being tested.</param>
    /// <param name="themes">The collection of available theme definitions.</param>
    public ControlPlaygroundViewModel(string controlName, IEnumerable<ControlThemeDefinition> themes)
    {
        _controlName = controlName;
        Appearance = new(themes);
        ContentProvider = new();
        IconProvider = new();

        Disposables.AddRange(
            [
                Appearance.WhenPropertyChanged(x => x.SelectedTheme).Subscribe(_ => ContentProvider.SelectedProviderType = Appearance.GetActiveThemeDefinition()?.DefaultContentType ?? ContentProviderType.None),
                Appearance.WhenAnyPropertyChanged(nameof(ControlAppearanceViewModel.ComputedClasses), nameof(ControlAppearanceViewModel.SelectedTheme), nameof(ControlAppearanceViewModel.ActiveRole), nameof(ControlAppearanceViewModel.ActiveItemsRole)).Subscribe(_ => OnPropertyChanged(nameof(PreviewCode))),
                Appearance.WhenPropertyChanged(x => x.ComputedClasses).Subscribe(_ => OnPropertyChanged(nameof(ComputedClasses))),
                IconProvider.WhenPropertyChanged(x => x.ComputedClasses).Subscribe(_ =>
                {
                    OnPropertyChanged(nameof(ComputedClasses));
                    OnPropertyChanged(nameof(PreviewCode));
                }),
                ClassProviders.ToObservableChangeSet().WhenPropertyChanged(x => x.SelectedClass).Subscribe(_ =>
                {
                    OnPropertyChanged(nameof(ComputedClasses));
                    OnPropertyChanged(nameof(PreviewCode));
                })
            ]);
    }

    /// <summary>
    /// Gets the appearance view model for managing theme and visual options.
    /// </summary>
    public ControlAppearanceViewModel Appearance { get; }

    /// <summary>
    /// Gets the behavior view model for managing control content options.
    /// </summary>
    public ControlContentViewModel ContentProvider { get; }

    /// <summary>
    /// Gets the behavior view model for managing control icon options.
    /// </summary>
    public ControlIconViewModel IconProvider { get; }

    /// <summary>
    /// Gets classes providers.
    /// </summary>
    public ObservableCollection<IClassProvider> ClassProviders { get; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether the control is disabled.
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Gets or sets the background theme role.
    /// </summary>
    public ThemeRole BackgroundRole { get; set; }

    /// <summary>
    /// Gets the CSS classes name to apply to the control.
    /// </summary>
    public string[] ComputedClasses
        => [.. Appearance.ComputedClasses.Concat(IconProvider.ComputedClasses).Concat(ClassProviders.Select(x => x.SelectedClass).NotNullOrEmpty().Distinct()).Concat(_customClasses).NotNullOrEmpty().Distinct()];

    /// <summary>
    /// Gets the generated XAML code preview for the current configuration.
    /// </summary>
    public string PreviewCode => GenerateXamlCode();

    /// <summary>
    /// Add or remove a custom class.
    /// </summary>
    /// <param name="class">Class to add.</param>
    /// <param name="add">Value indicates wether class must be added.</param>
    public void AddOrRemoveCustomClass(string @class, bool add)
    {
        if (add)
        {
            if (!_customClasses.Contains(@class))
                _customClasses.Add(@class);
        }
        else
        {
            _customClasses.Remove(@class);
        }

        OnPropertyChanged(nameof(ComputedClasses));
    }

    /// <summary>
    /// Generates the XAML code representation of the control with current settings.
    /// </summary>
    /// <returns>A string containing the generated XAML code.</returns>
    private string GenerateXamlCode()
    {
        var activeTheme = Appearance.GetActiveThemeDefinition();
        var propertyStrings = new[]
        {
            getFullProperty("Theme", !string.IsNullOrEmpty(activeTheme?.FullKey) ? $"{{StaticResource {activeTheme.FullKey}}}" : null),
            getFullProperty("my:ThemeAssist.Role", Appearance.ActiveRole != ThemeRole.Default ? Appearance.ActiveRole.ToString() : null),
            getFullProperty("my:ItemsAssist.Role", Appearance.ActiveItemsRole != ThemeRole.Default ? Appearance.ActiveItemsRole.ToString() : null),
            getFullProperty("Classes", string.Join(" ", ComputedClasses)),
            getFullProperty("IsEnabled", IsDisabled ? "False" : null)
        };

        return $"<{_controlName} {string.Join(" ", propertyStrings.NotNullOrEmpty())} />";

        static string getFullProperty(string key, string? value) => !string.IsNullOrWhiteSpace(value) ? $"{key}=\"{value}\"" : string.Empty;
    }

    /// <summary>
    /// Cleans up resources and disposes the appearance and behavior view models.
    /// </summary>
    protected override void Cleanup()
    {
        base.Cleanup();

        Appearance.Dispose();
        ContentProvider.Dispose();
        IconProvider.Dispose();
    }
}
