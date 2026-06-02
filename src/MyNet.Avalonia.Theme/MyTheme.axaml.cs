// -----------------------------------------------------------------------
// <copyright file="MyTheme.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Runtime;
using MyNet.Avalonia.Theme.Theming;
using MyNet.Avalonia.Theme.Theming.Brushes;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Avalonia.Theme.Theming.Palettes;
using MyNet.Avalonia.Theme.TypeConverters;

namespace MyNet.Avalonia.Theme;

/// <summary>
/// Provides the main theme engine for the application, managing theme variants (Dark, Light, HighContrast), brand color palettes (Primary, Accent), and resource injection.
/// Supports hot-reload for theme changes and dynamic color updates, ensuring consistent styling and smooth transitions across the UI.
/// </summary>
public sealed class MyTheme : Styles, IResourceNode, IThemeBrushService
{
    private static readonly ColorShades DefaultPrimary = new(Color.Parse("#1756BD"));
    private static readonly ColorShades DefaultAccent = new(Color.Parse("#FFAE18"));

    private readonly IServiceProvider? _serviceProvider;
    private readonly BrushManager _brushManager;
    private readonly ThemeChangeCoordinator _changeCoordinator;
    private readonly ThemeVariantCoordinator _variantCoordinator;
    private readonly ThemePaletteInjector _paletteInjector;
    private readonly ThemeResourceStore _resourceStore;
    private readonly ThemeLoadSession _loadSession;
    private bool _applicationThemeSubscribed;

    /// <summary>
    /// Gets the current theme instance from the application, providing color palettes, theme management, and resource injection.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when MyTheme is not found in Application.Styles.</exception>
    public static MyTheme Current
    {
        get
        {
            if (field is not null) return field;
            field = Application.Current?.Styles.OfType<MyTheme>().FirstOrDefault()
                    ?? throw new InvalidOperationException("Cannot locate MyTheme in Avalonia application styles. Ensure MyTheme is included in your App.axaml in Application.Styles section.");
            return field;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MyTheme"/> class.
    /// </summary>
    /// <param name="serviceProvider">Optional service provider for resource loading.</param>
    public MyTheme(IServiceProvider? serviceProvider)
        : this(serviceProvider, new ThemeXamlLoader())
    {
    }

    internal MyTheme(IServiceProvider? serviceProvider, IThemeXamlLoader xamlLoader)
    {
        _serviceProvider = serviceProvider;
        _brushManager = new(ColorTransitionDuration, ColorTransitionEasing);
        _variantCoordinator = new(() => (ResourceDictionary)Resources);
        _resourceStore = new();
        _paletteInjector = new(
            () => (ResourceDictionary)Resources,
            _brushManager,
            _variantCoordinator,
            _resourceStore.Invalidate,
            GetBrush);
        _loadSession = new(xamlLoader, _paletteInjector);
        _changeCoordinator = new(this, () => ThemeVersion++);
        _changeCoordinator.ThemeChanged += (_, e) => ThemeChanged?.Invoke(this, e);

        ClassesBootstrapper.Initialize();

        if (Application.Current is not null)
            Theme = Application.Current.ActualThemeVariant.Key.ToString();
    }

    /// <summary>
    /// Raised when the theme or color palette changes. Useful for components that need to react to theme changes.
    /// </summary>
    public event EventHandler? ThemeChanged;

    #region Transition Properties

    /// <summary>
    /// Gets or sets the easing function used for color transition animations. Default is SineEaseOut for smooth transitions.
    /// </summary>
    public Easing ColorTransitionEasing { get; set; } = new SineEaseOut();

    private static readonly StyledProperty<TimeSpan> ColorTransitionDurationProperty =
        AvaloniaProperty.Register<MyTheme, TimeSpan>(nameof(ColorTransitionDuration), TimeSpan.FromMilliseconds(150));

    /// <summary>
    /// Gets or sets the duration of color transition animations when theme colors change. Default is 150 milliseconds.
    /// </summary>
    public TimeSpan ColorTransitionDuration
    {
        get => GetValue(ColorTransitionDurationProperty);
        set => SetValue(ColorTransitionDurationProperty, value);
    }

    private static readonly StyledProperty<bool> TransitionIsEnabledProperty =
        AvaloniaProperty.Register<MyTheme, bool>(nameof(TransitionIsEnabled), true);

    /// <summary>
    /// Gets or sets a value indicating whether color transition animations are enabled. Default is true.
    /// When disabled, color changes happen instantly without animation.
    /// </summary>
    public bool TransitionIsEnabled
    {
        get => GetValue(TransitionIsEnabledProperty);
        set => SetValue(TransitionIsEnabledProperty, value);
    }

    #endregion

    #region ThemeVersion Property

    private static readonly StyledProperty<int> ThemeVersionProperty = AvaloniaProperty.Register<MyTheme, int>(nameof(ThemeVersion));

    /// <summary>
    /// Gets a counter that increments each time the theme changes, after all brushes have been updated.
    /// Can be used in bindings to force re-evaluation when the theme changes.
    /// </summary>
    public int ThemeVersion
    {
        get => GetValue(ThemeVersionProperty);
        private set => SetValue(ThemeVersionProperty, value);
    }

    #endregion

    #region Primary Property

    private static readonly StyledProperty<ColorShades> PrimaryProperty =
        AvaloniaProperty.Register<MyTheme, ColorShades>(nameof(Primary), DefaultPrimary);

    /// <summary>
    /// Gets or sets the primary brand color palette with automatic shade generation. Used for main actions, headers, and accent UI elements.
    /// Changing this property updates all primary-dependent resources and triggers the ThemeChanged event.
    /// Can be set from XAML using a hex color string (e.g., Primary="#124378").
    /// </summary>
    [TypeConverter(typeof(ColorShadesTypeConverter))]
    public ColorShades Primary
    {
        get => GetValue(PrimaryProperty);
        set => SetValue(PrimaryProperty, value);
    }

    #endregion

    #region Accent Property

    private static readonly StyledProperty<ColorShades> AccentProperty =
        AvaloniaProperty.Register<MyTheme, ColorShades>(nameof(Accent), DefaultAccent);

    /// <summary>
    /// Gets or sets the accent brand color palette with automatic shade generation. Used for highlights, floating action buttons, and secondary actions.
    /// Changing this property updates all accent-dependent resources and triggers the ThemeChanged event.
    /// Can be set from XAML using a hex color string (e.g., Accent="#FFAE18").
    /// </summary>
    [TypeConverter(typeof(ColorShadesTypeConverter))]
    public ColorShades Accent
    {
        get => GetValue(AccentProperty);
        set => SetValue(AccentProperty, value);
    }

    #endregion

    #region Theme

    /// <summary>
    /// Provides Theme Property.
    /// </summary>
    public static readonly StyledProperty<string?> ThemeProperty = AvaloniaProperty.Register<MyTheme, string?>(nameof(Theme));

    /// <summary>
    /// Gets or sets the Theme property.
    /// </summary>
    public string? Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    #endregion

    #region Property Change Handling

    /// <inheritdoc />
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == PrimaryProperty)
        {
            _paletteInjector.AddOrUpdatePrimaryShades(Primary);
            _changeCoordinator.NotifyChange();
        }
        else if (change.Property == AccentProperty)
        {
            _paletteInjector.AddOrUpdateAccentShades(Accent);
            _changeCoordinator.NotifyChange();
        }
        else if (change.Property == ThemeProperty)
        {
            _variantCoordinator.SyncApplicationThemeVariant(Theme);
        }
    }

    private void OnActualThemeVariantChanged()
    {
        using (_changeCoordinator.Defer())
        {
            Theme = Application.Current?.ActualThemeVariant.Key.ToString();
            _resourceStore.Invalidate();
            _paletteInjector.UpdateBrushesFromCurrentTheme();
        }
    }

    #endregion

    #region Theme Provider Management

    /// <summary>
    /// Registers a custom theme variant palette, allowing extension beyond built-in variants.
    /// </summary>
    /// <param name="theme">The theme variant palette to register.</param>
    /// <exception cref="ArgumentNullException">Thrown when theme is null.</exception>
    public void RegisterThemeProvider(ThemeVariantPalette theme) => _variantCoordinator.RegisterThemeProvider(theme);

    /// <summary>
    /// Applies a complete theme to MyTheme, updating Primary, Accent, and all theme variant colors.
    /// </summary>
    /// <param name="completeTheme">The complete theme to apply.</param>
    /// <exception cref="ArgumentNullException">Thrown when completeTheme is null.</exception>
    public void ApplyTheme(ThemePalette completeTheme)
    {
        ArgumentNullException.ThrowIfNull(completeTheme);

        using (_changeCoordinator.Defer())
        {
            Primary = completeTheme.Primary;
            Accent = completeTheme.Accent;
            Theme = completeTheme.ThemeVariant.Variant.Key.ToString();
            _paletteInjector.UpdateBrushesFromCurrentTheme();
        }
    }

    #endregion

    #region IThemeBrushService Implementation

    /// <inheritdoc />
    public ThemeVariantPalette? GetThemePalette() => _variantCoordinator.GetThemePalette();

    string? IThemeBrushService.GetTheme() => Theme;

    ColorShades IThemeBrushService.GetPrimary() => Primary;

    ColorShades IThemeBrushService.GetAccent() => Accent;

    /// <inheritdoc />
    public void SetTheme(string theme) => Theme = theme;

    /// <inheritdoc />
    public void SetPrimary(Color color, Color? foreground) => Primary = new(color, foreground);

    /// <inheritdoc />
    public void SetAccent(Color color, Color? foreground) => Accent = new(color, foreground);

    /// <inheritdoc />
    public void SetTheme(string theme, Color primary, Color accent, Color? primaryForeground = null, Color? accentForeground = null)
    {
        using (_changeCoordinator.Defer())
        {
            Theme = theme;
            Primary = new(primary, primaryForeground);
            Accent = new(accent, accentForeground);
        }
    }

    /// <inheritdoc />
    public IBrush GetBrush(string path, string? opacityKey = null, bool contrast = false, double? darken = null, double? lighten = null)
    {
        var opacity = GetOpacity(opacityKey);
        return _brushManager.Get(ThemeResourceKeyFactory.Brush(path), new(opacity, contrast, darken, lighten));
    }

    /// <inheritdoc />
    public IBrush GetBrush(IBrush brush, string? opacityKey = null, bool contrast = false, double? darken = null, double? lighten = null)
    {
        var opacity = GetOpacity(opacityKey);
        return _brushManager.Get(brush, new(opacity, contrast, darken, lighten));
    }

    /// <inheritdoc />
    public double? GetOpacity(string? opacityKey)
    {
        if (double.TryParse(opacityKey, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
            return result;

        if (string.IsNullOrEmpty(opacityKey))
            return null;

        var fullOpacityKey = ThemeResourceKeyFactory.Opacity(opacityKey);
        return TryGetResource(fullOpacityKey, Application.Current?.ActualThemeVariant, out var obj) && obj is double d
            ? d
            : null;
    }

    #endregion

    #region IResourceNode Implementation

    bool IResourceNode.TryGetResource(object key, ThemeVariant? theme, out object? value)
        => TryGetResource(key, theme, out value);

    private new bool TryGetResource(object key, ThemeVariant? theme, out object? value)
        => _resourceStore.TryGetResource(
            key,
            theme,
            OnResourcedAccessed,
            LookupResource,
            out value);

    private (bool Found, object? Value) LookupResource(object key, ThemeVariant? theme)
    {
        var found = Resources.TryGetResource(key, theme, out var value) || base.TryGetResource(key, theme, out value);
        return (found, value);
    }

    /// <summary>
    /// Eagerly loads all theme resources, palettes, and brushes. Call this at application startup
    /// (e.g., behind a splash screen) to avoid a freeze on first resource access.
    /// This method is idempotent — subsequent calls are no-ops for base resources; variant brushes are refreshed each time.
    /// </summary>
    public void EnsureLoaded()
    {
        EnsureApplicationThemeSubscription();
        _resourceStore.EnsureLoaded(OnResourcedAccessed);
        ApplyVariantBrushes();
    }

    /// <summary>
    /// Synchronizes semantic brushes from the active theme variant dictionary.
    /// Call after the application theme variant is known (e.g. before showing the main window).
    /// </summary>
    public void ApplyVariantBrushes() => _loadSession.ApplyVariantBrushes();

    private void OnResourcedAccessed()
        => _loadSession.LoadBaseResources(_serviceProvider, this, Primary, Accent);

    private void EnsureApplicationThemeSubscription()
    {
        if (_applicationThemeSubscribed || Application.Current is null)
            return;

        Application.Current.ActualThemeVariantChanged += (_, _) => OnActualThemeVariantChanged();
        _applicationThemeSubscribed = true;
    }

    /// <summary>
    /// Loads compiled theme XAML. Called by <see cref="ThemeXamlLoader"/> (satisfies Avalonia XAML source generator).
    /// </summary>
    internal void LoadXamlCore(IServiceProvider? serviceProvider)
        => AvaloniaXamlLoader.Load(serviceProvider, this);

    #endregion
}
