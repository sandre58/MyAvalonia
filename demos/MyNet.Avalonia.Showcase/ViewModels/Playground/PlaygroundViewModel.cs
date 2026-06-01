// -----------------------------------------------------------------------
// <copyright file="PlaygroundViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using Avalonia.Media;
using DynamicData;
using DynamicData.Binding;
using Material.Icons;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Showcase.ThemeBuilder.Rendering;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Choices;
using MyNet.Avalonia.Showcase.ViewModels.Playground.ContentProviders;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Options;
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Theming;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Avalonia.Theme.Theming.Palettes;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground;

/// <summary>
/// View model for the control playground, which provides interactive testing and preview functionality.
/// </summary>
internal sealed class PlaygroundViewModel : ObservableObject, IStyleProvider
{
    private readonly string _controlName;

    [SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Disposed in Cleanup method")]
    private CompositeDisposable _optionDisposables = [];

    /// <summary>
    /// Occurs when the style configuration has changed, providing the new configuration as an argument to event handlers. Subscribers to this event will be notified whenever there is a change in the style settings, allowing them to react accordingly, such as updating the user interface or applying the new styles to controls. The event handlers receive a ControlStyle object that encapsulates the current theme, classes, properties, and actions based on the user's selections in the playground.
    /// </summary>
    public event EventHandler<ControlStyle>? StyleChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaygroundViewModel"/> class with the specified control name and available.
    /// themes.
    /// </summary>
    /// <remarks>This constructor sets the initial background context, subscribes to property changes to
    /// update styles dynamically, and resets the view model to its default state upon initialization.</remarks>
    /// <param name="controlName">The name of the control that this view model represents. This value determines the styling and behavior
    /// associated with the view model.</param>
    /// <param name="themes">An observable collection of ControlThemeViewModel instances that represent the available themes for the control.
    /// Cannot be null.</param>
    public PlaygroundViewModel(string controlName, ObservableCollection<ControlThemeViewModel> themes)
    {
        _controlName = controlName;
        AvailableThemes = new(themes);
        BackgroundContexts.AddRange(CreateBackgroundContexts());

        SelectedBackgroundContext = BackgroundContexts[0];

        Reset();

        Disposables.AddRange(
            [
                this.WhenPropertyChanged(x => x.SelectedTheme).Subscribe(_ => ResetFrom(SelectedTheme)),
                SelectedVariants.ToObservableChangeSet().Subscribe(_ => OnStyleChanged())
            ]);

        PropertyChanged += OnPlaygroundPropertyChanged;

        foreach (var provider in AvailableContentProviders)
            provider.PropertyChanged += (_, _) => OnStyleChanged();
    }

    private void OnPlaygroundPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(SelectedRole):
            case nameof(SelectedSubItemsRole):
            case nameof(SelectedSize):
            case nameof(SelectedShape):
            case nameof(SelectedContentProvider):
            case nameof(SelectedIcon):
            case nameof(UseIcon):
            case nameof(IconPosition):
                OnStyleChanged();
                break;
        }
    }

    /// <summary>
    /// Gets the collection of available themes.
    /// </summary>
    public ReadOnlyObservableCollection<ControlThemeViewModel> AvailableThemes { get; }

    /// <summary>
    /// Gets the collection of available content providers for the control.
    /// </summary>
    public ReadOnlyObservableCollection<IContentProviderViewModel> AvailableContentProviders { get; } = new(
    [
        new NoContentProviderViewModel(),
        new IconProviderViewModel(),
        new TextProviderViewModel()
    ]);

    /// <summary>
    /// Gets or sets the currently selected theme.
    /// </summary>
    public ControlThemeViewModel? SelectedTheme
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <summary>
    /// Gets or sets the collection of currently selected variants.
    /// </summary>
    public ObservableCollection<ClassChoiceViewModel> SelectedVariants { get; set; } = [];

    /// <summary>
    /// Gets or sets the currently selected role.
    /// </summary>
    public ThemeRole SelectedRole
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <summary>
    /// Gets or sets the currently selected role for items.
    /// </summary>
    public ThemeRole SelectedSubItemsRole
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <summary>
    /// Gets or sets the currently selected size.
    /// </summary>
    public string? SelectedSize
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <summary>
    /// Gets or sets the currently selected shape.
    /// </summary>
    public string? SelectedShape
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <summary>
    /// Gets or sets the currently selected content provider for the view model.
    /// </summary>
    /// <remarks>Changing this property updates the active content provider, which may affect the data or
    /// functionality presented in the associated view. Assigning a new value typically triggers updates in the user
    /// interface to reflect the selected provider.</remarks>
    public IContentProviderViewModel? SelectedContentProvider
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the control is disabled.
    /// </summary>
    public bool IsDisabled
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether to show an icon in the control. When set to true, an icon will be displayed based on the specified data or randomly generated if no data is provided. When set to false, no icon will be shown in the control preview.
    /// </summary>
    public bool UseIcon
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <summary>
    /// Gets or sets the icon data to provide. This property can be used to specify a specific icon, but it is not required for the random icon generation.
    /// </summary>
    public MaterialIconKind? SelectedIcon
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <summary>
    /// Gets or sets the icon position.
    /// </summary>
    public Position IconPosition
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <summary>
    /// Gets backgrounds contexts.
    /// </summary>
    public ObservableCollection<BackgroundContext> BackgroundContexts { get; } = [];

    /// <summary>
    /// Gets or sets the background context.
    /// </summary>
    public BackgroundContext? SelectedBackgroundContext
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <summary>
    /// Gets the preview code associated with the current context, which may be null if no preview is available.
    /// </summary>
    /// <remarks>This property is intended for use in scenarios where a preview representation of code is
    /// needed, such as in code editors or preview windows.</remarks>
    public string? PreviewCode
    {
        get;
        private set => SetProperty(ref field, value);
    }

    /// <summary>
    /// Gets the CSS classes name to apply to the control.
    /// </summary>
    private string[] ComputeClasses()
    {
        if (SelectedTheme is null) return [];

        var classes = new List<string>();

        // Theme
        var themeClass = SelectedTheme.Definition.Key?.ToLower(CultureInfo.CurrentCulture).Replace(ThemeResourceKeyFactory.Theme(_controlName), "theme", StringComparison.OrdinalIgnoreCase).Replace(".", "-", StringComparison.OrdinalIgnoreCase);
        classes.Add(!string.IsNullOrEmpty(themeClass) ? themeClass : "theme-default");

        // Kind
        if (SelectedTheme.Definition.Kind is not null)
            classes.Add(SelectedTheme.Definition.Kind);

        // Shape
        if (!string.IsNullOrEmpty(SelectedShape))
            classes.Add(SelectedShape);

        // Size
        if (!string.IsNullOrEmpty(SelectedSize))
            classes.Add(SelectedSize);

        // Variants
        classes.AddRange(SelectedVariants.Select(x => x.Value?.ToString()));

        // Icon
        if (UseIcon)
            classes.Add(CssClass.Icon(IconPosition.ToString()).ToString());

        // Custom classes
        classes.AddRange(SelectedTheme.ComputeClasses());

        return [.. classes.NotNullOrEmpty().Distinct()];
    }

    /// <summary>
    /// Computes the control properties based on the selected options and available properties.
    /// </summary>
    /// <returns>An array of <see cref="StyleProperty"/> representing the computed properties for the control.</returns>
    private StyleProperty[] ComputeProperties()
    {
        if (SelectedTheme is null) return [];

        var properties = new List<StyleProperty>();

        // Roles
        if (SelectedRole != ThemeRole.Default)
            properties.Add(StyleProperty.FromProperty(ThemeAssist.RoleProperty, SelectedRole));
        if (SelectedSubItemsRole != ThemeRole.Default)
            properties.Add(StyleProperty.FromProperty(ItemsAssist.RoleProperty, SelectedSubItemsRole));

        // Content
        if (SelectedTheme.Definition.ContentDefinition is not null)
            properties.Add(StyleProperty.FromProperty(SelectedTheme.Definition.ContentDefinition.Property, SelectedContentProvider?.ProvideContent()));

        // Icon
        if (UseIcon)
            properties.Add(StyleProperty.FromProperty(SelectedTheme.Definition.IconDefinition.Property, SelectedIcon ?? RandomGenerator.Current.Enum<MaterialIconKind>()));

        // Custom properties
        properties.AddRange(SelectedTheme.ComputeStyleProperties());

        return [.. properties];
    }

    /// <summary>
    /// Computes and retrieves an array of style actions based on the currently selected theme.
    /// </summary>
    /// <remarks>This method filters the available options in the selected theme to create style actions for
    /// control definitions and custom options that have value change handlers. It ensures that only non-null and
    /// distinct actions are returned.</remarks>
    /// <returns>An array of unique style actions derived from the available options in the selected theme. The array will be
    /// empty if no theme is selected or if no actions are available.</returns>
    private StyleAction[] ComputeActions() => [.. SelectedTheme?.ComputeActions() ?? []];

    /// <summary>
    /// Generates an XML markup string for a control based on the specified configuration settings.
    /// </summary>
    /// <remarks>The generated XML includes only those attributes that differ from their default values. The
    /// output is formatted with indentation for readability when multiple attributes are present.</remarks>
    /// <param name="config">The configuration that defines the control's theme, classes, and property values to be included in the generated
    /// XML.</param>
    /// <returns>A string containing the XML representation of the control, including any specified attributes. If no attributes
    /// are provided, a self-closing tag is returned.</returns>
    private string GenerateCode(ControlStyle config)
    {
        var attributes = new List<string>();

        // Theme
        if (!string.IsNullOrEmpty(config.ThemeKey))
            attributes.Add($"Theme=\"{{StaticResource {config.ThemeKey}}}\"");

        // Classes
        if (config.Classes.Count > 0)
            attributes.Add($"Classes=\"{string.Join(" ", config.Classes)}\"");

        // Custom Properties
        attributes.AddRange(config.Properties.Select(prop => $"{prop.XamlKey}=\"{prop.XamlValue}\""));

        // Build attributes
        var sb = new StringBuilder();
        var indent = new string(' ', _controlName.Length + 2);

        if (attributes.Count == 0)
        {
            sb.Append(CultureInfo.CurrentCulture, $"<{_controlName} />");
        }
        else
        {
            sb.Append(CultureInfo.CurrentCulture, $"<{_controlName} {attributes[0]}");
            for (var i = 1; i < attributes.Count; i++)
                sb.Append(CultureInfo.CurrentCulture, $"\n{indent}{attributes[i]}");
            sb.Append(" />");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Builds and returns a new ControlConfiguration instance that reflects the currently selected theme, computed
    /// classes, and properties for the control.
    /// </summary>
    /// <remarks>The Theme property of the returned configuration is set based on the currently selected
    /// theme. The Classes and Properties collections are determined by invoking their respective computation methods.
    /// Use this method to obtain a configuration that matches the current state of the control's settings.</remarks>
    /// <returns>A ControlConfiguration object that encapsulates the current theme, classes, and properties for the control.</returns>
    public ControlStyle BuildStyle() => new()
    {
        Theme = SelectedTheme?.Definition.Theme,
        ThemeKey = SelectedTheme?.Definition.Key,
        Classes = ComputeClasses(),
        Properties = ComputeProperties(),
        Actions = ComputeActions()
    };

    /// <summary>
    /// Handles changes to the style configuration, updating the preview code and notifying subscribers of the
    /// configuration change.
    /// </summary>
    /// <remarks>Call this method when the style is modified to ensure that the preview and any listeners are
    /// updated to reflect the latest configuration. This method triggers the ConfigurationChanged event with the new
    /// configuration.</remarks>
    private void OnStyleChanged()
    {
        var configuration = BuildStyle();
        PreviewCode = GenerateCode(configuration);
        StyleChanged?.Invoke(this, configuration);
    }

    /// <summary>
    /// Resets the appearance settings to default values.
    /// </summary>
    public void Reset()
    {
        SelectedTheme = AvailableThemes.FirstOrDefault();
        ResetFrom(SelectedTheme);
    }

    /// <summary>
    /// Cleans up resources used by the view model, including disposing of any option-related disposables and invoking the base cleanup method. This method should be called when the view model is no longer needed to ensure that all resources are properly released and to prevent memory leaks. The cleanup process includes disposing of any subscriptions or resources associated with the options, as well as performing any additional cleanup defined in the base class implementation.
    /// </summary>
    protected override void DisposeManagedResources()
    {
        _optionDisposables.Dispose();
        base.DisposeManagedResources();
    }

    /// <summary>
    /// Resets the appearance settings based on a specific theme definition.
    /// </summary>
    /// <param name="theme">The theme definition to reset from.</param>
    private void ResetFrom(ControlThemeViewModel? theme)
    {
        SelectedVariants.Clear();
        SelectedSize = null;
        SelectedShape = null;
        SelectedRole = theme?.AvailableRoles.FirstOrDefault()?.Value ?? ThemeRole.Default;
        SelectedSubItemsRole = theme?.AvailableItemsRoles.FirstOrDefault()?.Value ?? ThemeRole.Default;
        SelectedContentProvider = theme?.Definition.ContentDefinition is not null ?
                                    AvailableContentProviders.GetById(theme.Definition.ContentDefinition.ContentProviderType)
                                    : null;
        UseIcon = false;
        SelectedIcon = null;
        IconPosition = Position.Left;

        _optionDisposables.Dispose();
        _optionDisposables = [];
        foreach (var option in theme?.AvailableOptions.Concat(theme.AvailableGroups.SelectMany(x => x.Options)).OfType<ValueOptionViewModel>() ?? [])
            _optionDisposables.Add(option.ValueChangedSubject.Subscribe(_ => OnStyleChanged()));
    }

    /// <summary>
    /// Retrieves a collection of background contexts representing different visual themes for the application UI.
    /// </summary>
    /// <remarks>Each <see cref="BackgroundContext"/> in the returned collection is initialized with brushes
    /// corresponding to a particular theme element, ensuring consistent theming across various UI components. Override
    /// this method to customize or extend the available background contexts for derived classes.</remarks>
    /// <returns>An enumerable collection of <see cref="BackgroundContext"/> instances, each configured with specific theme
    /// brushes for accent, primary, and surface backgrounds.</returns>
    private static IEnumerable<BackgroundContext> CreateBackgroundContexts()
    {
        var theme = MyTheme.Current;
        var accent = theme.GetBrush(nameof(MyTheme.Accent));
        var accentForeground = theme.GetBrush($"{nameof(MyTheme.Accent)}.{nameof(ColorShades.Foreground)}");
        var primary = theme.GetBrush(nameof(MyTheme.Primary));
        var primaryForeground = theme.GetBrush($"{nameof(MyTheme.Primary)}.{nameof(ColorShades.Foreground)}");
        var surface = theme.GetBrush("Surface.Level2");
        var surfaceForeground = theme.GetBrush(ThemeResourceKeyFactory.PrimaryForeground);
        return [
            new(surface, surfaceForeground, ThemeContext.Default),
            new(primary, primaryForeground, ThemeContext.Contrast),
            new(accent, accentForeground, ThemeContext.Contrast)
        ];
    }
}

/// <summary>
/// Represents the background context, including brush and theme information, used for rendering visual elements.
/// </summary>
/// <param name="Brush">The brush used to render the background of the visual element.</param>
/// <param name="Foreground">The brush used to render the foreground content, such as text or icons, within the visual element.</param>
/// <param name="Context">The theme context that provides additional styling and theming information for rendering.</param>
internal sealed record BackgroundContext(IBrush Brush, IBrush Foreground, ThemeContext Context);
