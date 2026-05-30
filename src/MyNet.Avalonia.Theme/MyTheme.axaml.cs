// -----------------------------------------------------------------------
// <copyright file="MyTheme.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using MyNet.Avalonia.Theme.Diagnostics;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Theming;
using MyNet.Avalonia.Theme.Theming.Brushes;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Avalonia.Theme.Theming.Palettes;
using MyNet.Avalonia.Theme.TypeConverters;
using MyNet.Utilities;
using MyNet.Utilities.Deferring;

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
    private readonly Deferrer _themeChangedDeferrer;

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
    {
        _serviceProvider = serviceProvider;
        _brushManager = new(ColorTransitionDuration, ColorTransitionEasing);
        _themeChangedDeferrer = new(RaiseThemeChanged);

        ClassesBootstrapper.Initialize();

        if (Application.Current is not null)
        {
            Theme = Application.Current.ActualThemeVariant.Key.ToString();
            Application.Current.ActualThemeVariantChanged += (_, _) => OnActualThemeVariantChanged();
        }
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

    /// <summary>
    /// Styled property for duration of color transition animations when theme colors change. Default is 150 milliseconds.
    /// </summary>
    private static readonly StyledProperty<TimeSpan> ColorTransitionDurationProperty = AvaloniaProperty.Register<MyTheme, TimeSpan>(nameof(ColorTransitionDuration), TimeSpan.FromMilliseconds(150));

    /// <summary>
    /// Gets or sets the duration of color transition animations when theme colors change. Default is 150 milliseconds.
    /// </summary>
    public TimeSpan ColorTransitionDuration
    {
        get => GetValue(ColorTransitionDurationProperty);
        set => SetValue(ColorTransitionDurationProperty, value);
    }

    /// <summary>
    /// Styled property for enabling or disabling color transitions.
    /// </summary>
    private static readonly StyledProperty<bool> TransitionIsEnabledProperty = AvaloniaProperty.Register<MyTheme, bool>(nameof(TransitionIsEnabled), true);

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

    /// <summary>
    /// Styled property for the primary brand color palette.
    /// </summary>
    private static readonly StyledProperty<ColorShades> PrimaryProperty = AvaloniaProperty.Register<MyTheme, ColorShades>(nameof(Primary), DefaultPrimary);

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

    /// <summary>
    /// Styled property for the accent brand color palette.
    /// </summary>
    private static readonly StyledProperty<ColorShades> AccentProperty = AvaloniaProperty.Register<MyTheme, ColorShades>(nameof(Accent), DefaultAccent);

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

    /// <summary>
    /// Handles property changes to update resources when primary or accent colors change.
    /// Automatically injects updated colors into the ResourceDictionary and raises the ThemeChanged event.
    /// </summary>
    /// <param name="change">The property change event arguments.</param>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == PrimaryProperty)
        {
            AddOrUpdatePrimaryShades();
            RaiseThemeChanged();
        }
        else if (change.Property == AccentProperty)
        {
            AddOrUpdateAccentShades();
            RaiseThemeChanged();
        }
        else if (change.Property == ThemeProperty)
        {
            if (Resources.ThemeDictionaries.Count > 0)
                Application.Current?.RequestedThemeVariant = Resources.ThemeDictionaries.Keys.FirstOrDefault(x => x.Key.Equals(Theme));
        }
    }

    /// <summary>
    /// Handles theme variant changes (e.g., switching between Dark/Light/HighContrast).
    /// Updates brushes and raises the ThemeChanged event.
    /// </summary>
    private void OnActualThemeVariantChanged()
    {
        using (_themeChangedDeferrer.Defer())
        {
            Theme = Application.Current?.ActualThemeVariant.Key.ToString();
            InvalidateResourceCache();
            UpdateBrushesFromCurrentTheme();
        }
    }

    #endregion

    #region Theme Provider Management

    /// <summary>
    /// Registers a custom theme variant palette, allowing extension beyond built-in variants.
    /// </summary>
    /// <param name="theme">The theme variant palette to register.</param>
    /// <exception cref="ArgumentNullException">Thrown when theme is null.</exception>
    /// <example>
    /// <code>
    /// var neonTheme = new ThemeVariantColors(new ThemeVariant("Neon", ThemeVariantKind.Custom))
    /// {
    ///     Base = new ThemePalette { ... },
    ///     Success = new ColorShades(Type.Parse("#00FF00")),
    ///     // ... other palettes
    /// };
    /// MyTheme.RegisterThemeProvider(neonTheme);
    /// </code>
    /// </example>
    public void RegisterThemeProvider(ThemeVariantPalette theme)
    {
        ArgumentNullException.ThrowIfNull(theme);
        var rd = new ResourceDictionary();
        theme.ToResourceDictionary().ForEach(kv => rd.Add(kv.Key, kv.Value));
        Resources.ThemeDictionaries.AddOrUpdate(theme.Variant, rd);
    }

    /// <summary>
    /// Applies a complete theme to MyTheme, updating Primary, Accent, and all theme variant colors.
    /// </summary>
    /// <param name="completeTheme">The complete theme to apply.</param>
    /// <exception cref="ArgumentNullException">Thrown when completeTheme is null.</exception>
    public void ApplyTheme(ThemePalette completeTheme)
    {
        ArgumentNullException.ThrowIfNull(completeTheme);

        using (_themeChangedDeferrer.Defer())
        {
            // Apply brand colors
            Primary = completeTheme.Primary;
            Accent = completeTheme.Accent;
            Theme = completeTheme.ThemeVariant.Variant.Key.ToString();

            // Force update of brushes
            UpdateBrushesFromCurrentTheme();
        }
    }

    #endregion

    #region Resource Injection

    /// <summary>
    /// Injects all accent brand palette resources into the ResourceDictionary, including base color, foreground, and all shades.
    /// </summary>
    private void AddOrUpdatePrimaryShades()
    {
        using (PerformanceMonitor.Measure("AddOrUpdatePrimaryShades", category: PerformanceCategory.Theme))
            AddOrUpdateColorShades(Primary, nameof(Primary));
        InvalidateResourceCache();
    }

    /// <summary>
    /// Injects all primary brand palette resources into the ResourceDictionary, including base color, foreground, and all shades.
    /// </summary>
    private void AddOrUpdateAccentShades()
    {
        using (PerformanceMonitor.Measure("AddOrUpdateAccentShades", category: PerformanceCategory.Theme))
            AddOrUpdateColorShades(Accent, nameof(Accent));
        InvalidateResourceCache();
    }

    /// <summary>
    /// Injects a dictionary of colors into the ResourceDictionary.
    /// </summary>
    private void UpdateBrushesFromCurrentTheme()
    {
        const string transparencyKey = "Transparency";
        const string transparencySmallKey = "Transparency.Small";

        using (PerformanceMonitor.Measure("UpdateBrushesFromCurrentTheme", category: PerformanceCategory.Theme))
        {
            var count = 0;
            var activeTheme = GetActiveThemeDictionary();

            foreach (var (key, obj) in activeTheme)
            {
                if (obj is Color color)
                {
                    var colorKey = key.ToString()?.Replace(ThemeResourceKeyFactory.Pattern(ThemeResourceKeyFactory.ColorKey).FormatWith(string.Empty), string.Empty, StringComparison.OrdinalIgnoreCase);
                    if (!string.IsNullOrEmpty(colorKey))
                    {
                        var contrastedColor = GetContrastedColorForKey(colorKey, activeTheme);

                        if (new List<string>
                            {
                                nameof(ThemeVariantPalette.Success),
                                nameof(ThemeVariantPalette.Error),
                                nameof(ThemeVariantPalette.Warning),
                                nameof(ThemeVariantPalette.Information),
                                nameof(ThemeVariantPalette.Neutral)
                            }.Contains(colorKey))
                        {
                            var shades = new ColorShades(color, contrastedColor);
                            shades.ToResourceDictionary(colorKey).ForEach(x =>
                            {
                                AddOrUpdateBrush(x.Key, x.Value, shades.Foreground);
                                count++;
                            });
                        }
                        else
                        {
                            AddOrUpdateBrush(colorKey, color, contrastedColor);
                            count++;
                        }
                    }
                }
            }

            if (!Resources.ContainsKey(ThemeResourceKeyFactory.Brush(transparencyKey)))
                Resources.Add(ThemeResourceKeyFactory.Brush(transparencyKey), createTransparencyBrush(20));

            if (!Resources.ContainsKey(ThemeResourceKeyFactory.Brush(transparencySmallKey)))
                Resources.Add(ThemeResourceKeyFactory.Brush(transparencySmallKey), createTransparencyBrush(8));

            PerformanceMonitor.Debug($"UpdateBrushesFromCurrentTheme processed {count + 3} brushes", category: PerformanceCategory.Theme);
        }

        InvalidateResourceCache();

        VisualBrush createTransparencyBrush(double size) => new(new Image
        {
            Height = size,
            Width = size,
            Source = new DrawingImage
            {
                Drawing = new DrawingGroup
                {
                    Children =
                    [
                        new GeometryDrawing { Brush = Brushes.Transparent, Geometry = PathGeometry.Parse("M0,0 L2,0 2,2, 0,2Z") },
                        new GeometryDrawing { Brush = GetBrush("Foreground.Primary", nameof(Opacity.Scrim)), Geometry = PathGeometry.Parse("M0,1 L2,1 2,2, 1,2 1,0 0,0Z") }
                    ]
                }
            }
        }) { DestinationRect = new(0, 0, size, size, RelativeUnit.Absolute), Stretch = Stretch.Uniform, TileMode = TileMode.Tile };
    }

    /// <summary>
    /// Determines the appropriate contrasted color for a given color key based on theme conventions.
    /// </summary>
    /// <param name="colorKey">The color key to find a contrasted color for.</param>
    /// <param name="themeDictionary">The active theme dictionary containing color definitions.</param>
    /// <returns>The contrasted color if found; otherwise, null.</returns>
    private static Color? GetContrastedColorForKey(string colorKey, ResourceDictionary themeDictionary)
    {
        var contrastedColorKey = ThemeResourceKeyFactory.ContrastedColor(colorKey);

        return contrastedColorKey is null
            ? null
            : themeDictionary.TryGetResource(contrastedColorKey, null, out var obj) && obj is Color color
                ? color
                : null;
    }

    /// <summary>
    /// Injects a dictionary of colors into the ResourceDictionary. Each entry is injected on the UI thread.
    /// </summary>
    /// <param name="shades">Shades of the color to inject.</param>
    /// <param name="name">The name of the color group.</param>
    private void AddOrUpdateColorShades(ColorShades shades, string name)
    {
        using (PerformanceMonitor.Measure(category: PerformanceCategory.Theme))
        {
            var count = 0;

            foreach (var (key, color) in shades.ToResourceDictionary(name))
            {
                AddOrUpdateColorAndBrush(key, color, !key.Contains(nameof(ColorShades.Foreground), StringComparison.OrdinalIgnoreCase) ? shades.Foreground : null);
                count++;
            }

            PerformanceMonitor.Debug($"AddOrUpdateColorShades({name}) processed {count} shades", category: PerformanceCategory.Theme);
        }
    }

    /// <summary>
    /// Injects a single color into the ResourceDictionary both as a Type and as a SolidColorBrush with transitions.
    /// If the brush already exists, it updates the color (triggering the transition animation). If it doesn't exist, it creates a new brush with color transition animations.
    /// </summary>
    /// <param name="colorKey">The base resources key (will be prefixed with "MyNet.Type." and "MyNet.Brush.").</param>
    /// <param name="newColor">The color to inject.</param>
    /// <param name="contrastedColor">The color to use for the contrast brush (optional; defaults to the contrasting color of the current color).</param>
    private void AddOrUpdateColorAndBrush(string colorKey, Color newColor, Color? contrastedColor)
    {
        AddOrUpdateColor(colorKey, newColor);
        AddOrUpdateBrush(colorKey, newColor, contrastedColor);
    }

    /// <summary>
    /// Injects a color into the ResourceDictionary.
    /// </summary>
    /// <param name="key">The key identifying the color resource.</param>
    /// <param name="color">The color to inject.</param>
    private void AddOrUpdateColor(string key, Color color)
    {
        var fullColorKey = ThemeResourceKeyFactory.Color(key);
        Resources.AddOrUpdate(fullColorKey, color);
    }

    /// <summary>
    /// Injects a brush into the ResourceDictionary, creating or updating it with color transition support.
    /// </summary>
    /// <param name="key">The key identifying the color resource.</param>
    /// <param name="color">The color to inject.</param>
    /// <param name="contrastedColor">The color to use for the contrast brush (optional; defaults to the contrasting color of the current color).</param>
    private void AddOrUpdateBrush(string key, Color color, Color? contrastedColor)
    {
        var fullBrushKey = ThemeResourceKeyFactory.Brush(key);
        var brush = _brushManager.Register(fullBrushKey, color, contrastedColor);
        Resources.AddOrUpdate(fullBrushKey, brush);
    }

    /// <summary>
    /// Raises the ThemeChanged event asynchronously.
    /// </summary>
    private void RaiseThemeChanged()
    {
        if (_themeChangedDeferrer.IsDeferred) return;

        ThemeVersion++;
        ThemeChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Retrieves actual resources dictionary from current theme.
    /// </summary>
    /// <returns>The active resource dictionary.</returns>
    private ResourceDictionary GetActiveThemeDictionary()
    {
        var current = Application.Current?.ActualThemeVariant ?? ThemeVariant.Default;
        return Resources.ThemeDictionaries.TryGetValue(current, out var rd) && rd is ResourceDictionary resources ? resources : [];
    }

    #endregion

    #region IThemeBrushService Implementation

    /// <summary>
    /// Retrieves the set of theme variant colors associated with the application's current theme variant.
    /// </summary>
    /// <remarks>This method obtains the current theme variant from the application and attempts to retrieve
    /// its corresponding resource dictionary. If the resource dictionary is available, it is converted into a <see
    /// cref="ThemeVariantPalette"/> instance. If the theme variant is not set or the resource dictionary is missing, the
    /// method returns <see langword="null"/>.</remarks>
    /// <returns>A <see cref="ThemeVariantPalette"/> object containing the colors for the current theme variant, or <see
    /// langword="null"/> if the variant colors cannot be determined.</returns>
    public ThemeVariantPalette? GetThemePalette()
    {
        var currentVariant = Application.Current?.ActualThemeVariant ?? ThemeVariant.Default;
        return Resources.ThemeDictionaries.TryGetValue(currentVariant, out var rd) && rd is ResourceDictionary resourceDict
            ? ThemeVariantPalette.FromResourceDictionary(currentVariant, resourceDict.ToDictionary(x => x.Key.ToString().OrEmpty().Replace(ThemeResourceKeyFactory.Pattern(ThemeResourceKeyFactory.ColorKey).FormatWith(string.Empty), string.Empty, StringComparison.OrdinalIgnoreCase), x => x.Value!))
            : null;
    }

    /// <summary>
    /// Gets the current theme as a string, which defines the visual style of the application.
    /// </summary>
    /// <returns>A string representing the current theme. Returns null if no theme is set.</returns>
    string? IThemeBrushService.GetTheme() => Theme;

    /// <summary>
    /// Gets the primary color shade used in the theme.
    /// </summary>
    /// <returns>A nullable <see cref="ColorShades"/> representing the primary color shade. Returns null if no primary color is
    /// set.</returns>
    ColorShades IThemeBrushService.GetPrimary() => Primary;

    /// <summary>
    /// Gets the current accent color used in the theme.
    /// </summary>
    /// <returns>A nullable <see cref="ColorShades"/> representing the accent color. Returns null if no accent color is set.</returns>
    ColorShades IThemeBrushService.GetAccent() => Accent;

    /// <summary>
    /// Sets the current theme for the application.
    /// </summary>
    /// <param name="theme">The name of the theme to apply. This value must not be null or empty.</param>
    public void SetTheme(string theme) => Theme = theme;

    /// <summary>
    /// Sets the primary color and an optional foreground color for the primary color shades.
    /// </summary>
    /// <remarks>This method updates the <c>Primary</c> property with a new <c>ColorShades</c> instance using
    /// the specified colors. Ensure that the provided colors are appropriate for the intended visual theme.</remarks>
    /// <param name="color">The base color used to generate the primary color shades. This value must be a valid color.</param>
    /// <param name="foreground">An optional foreground color to be associated with the primary color shades. If <see langword="null"/>, no
    /// foreground color is set.</param>
    public void SetPrimary(Color color, Color? foreground) => Primary = new(color, foreground);

    /// <summary>
    /// Sets the application's accent color and an optional foreground color for the theme.
    /// </summary>
    /// <remarks>Use colors that provide sufficient contrast for accessibility. The method updates the
    /// application's color scheme immediately.</remarks>
    /// <param name="color">The accent color to apply to the application's theme.</param>
    /// <param name="foreground">An optional foreground color to use with the accent color. If <see langword="null"/>, a default foreground color
    /// is selected.</param>
    public void SetAccent(Color color, Color? foreground) => Accent = new(color, foreground);

    /// <summary>
    /// Sets the application's theme along with optional primary and accent colors and their respective foreground colors.
    /// </summary>
    /// <param name="theme">The name of the theme to apply. This value must correspond to a valid theme identifier.</param>
    /// <param name="primary">An optional primary color to apply to the theme.</param>
    /// <param name="accent">An optional accent color to apply to the theme.</param>
    /// <param name="primaryForeground">An optional foreground color for the primary color.</param>
    /// <param name="accentForeground">An optional foreground color for the accent color.</param>
    public void SetTheme(string theme, Color primary, Color accent, Color? primaryForeground = null, Color? accentForeground = null)
    {
        using (_themeChangedDeferrer.Defer())
        {
            Theme = theme;
            Primary = new(primary, primaryForeground);
            Accent = new(accent, accentForeground);
        }
    }

    /// <summary>
    /// Gets a brush from the theme resources by path.
    /// </summary>
    /// <param name="path">The resource path for the brush.</param>
    /// <param name="opacityKey">Optional opacity key or value.</param>
    /// <param name="contrast">If true, returns the contrast brush for accessibility.</param>
    /// <param name="darken">Optional darken factor (value between 0.0 and 1.0).</param>
    /// <param name="lighten">Optional lighten factor (value between 0.0 and 1.0).</param>
    /// <returns>The brush instance.</returns>
    public IBrush GetBrush(string path, string? opacityKey = null, bool contrast = false, double? darken = null, double? lighten = null)
    {
        var opacity = GetOpacity(opacityKey);

        return _brushManager.Get(ThemeResourceKeyFactory.Brush(path), new(opacity, contrast, darken, lighten));
    }

    /// <summary>
    /// Gets a brush from the theme resources by brush instance, optionally with a specific opacity or contrast.
    /// </summary>
    /// <param name="brush">The brush instance to search for.</param>
    /// <param name="opacityKey">Optional opacity key or value.</param>
    /// <param name="contrast">If true, returns the contrast brush for accessibility.</param>
    /// <param name="darken">Optional darken factor (value between 0.0 and 1.0).</param>
    /// <param name="lighten">Optional lighten factor (value between 0.0 and 1.0).</param>
    /// <returns>The brush instance with the specified opacity or contrast.</returns>
    public IBrush GetBrush(IBrush brush, string? opacityKey = null, bool contrast = false, double? darken = null, double? lighten = null)
    {
        var opacity = GetOpacity(opacityKey);

        return _brushManager.Get(brush, new(opacity, contrast, darken, lighten));
    }

    /// <summary>
    /// Gets an opacity value from the theme resources or parses it directly.
    /// </summary>
    /// <param name="opacityKey">Value or key for the opacity.</param>
    /// <returns>Value for the opacity.</returns>
    public double? GetOpacity(string? opacityKey)
    {
        double? opacity = null;
        if (double.TryParse(opacityKey, CultureInfo.InvariantCulture, out var result))
        {
            opacity = result;
        }
        else if (!string.IsNullOrEmpty(opacityKey))
        {
            var fullOpacityKey = ThemeResourceKeyFactory.Opacity(opacityKey);
            opacity = TryGetResource(fullOpacityKey, Application.Current?.ActualThemeVariant, out var obj) && obj is double d ? d : null;
        }

        return opacity;
    }

    #endregion

    #region IResourceNode Implementation

    private bool _isResourcedAccessed;
    private static readonly object NotFoundSentinel = new();
    private Dictionary<object, object?>? _resourceCache;
    private ThemeVariant? _cachedThemeVariant;

    /// <summary>
    /// Tries to get a resource from the theme's resources dictionary. Implements IResourceNode to integrate with Avalonia's resources lookup system.
    /// </summary>
    /// <param name="key">The resource key to look up.</param>
    /// <param name="theme">The theme variant context (not used in this implementation).</param>
    /// <param name="value">The resource value if found.</param>
    /// <returns>True if the resource was found; otherwise, false.</returns>
    bool IResourceNode.TryGetResource(object key, ThemeVariant? theme, out object? value) => TryGetResource(key, theme, out value);

    /// <summary>
    /// Tries to get a resource from the theme's resources dictionary, loading resources if accessed for the first time.
    /// Uses a per-theme-variant cache to avoid repeated dictionary walks through merged dictionaries and child styles.
    /// </summary>
    /// <param name="key">The resource key to look up.</param>
    /// <param name="theme">The theme variant context.</param>
    /// <param name="value">The resource value if found.</param>
    /// <returns>True if the resource was found; otherwise, false.</returns>
    private new bool TryGetResource(object key, ThemeVariant? theme, out object? value)
    {
        if (!_isResourcedAccessed)
        {
            _isResourcedAccessed = true;
            OnResourcedAccessed();
        }

        // Fast path: check cache for the current theme variant
        if (_resourceCache is not null && theme == _cachedThemeVariant && _resourceCache.TryGetValue(key, out var cached))
        {
            if (ReferenceEquals(cached, NotFoundSentinel))
            {
                value = null;
                return false;
            }

            value = cached;
            return true;
        }

        // Slow path: walk dictionaries and child styles
        var found = Resources.TryGetResource(key, theme, out value) || base.TryGetResource(key, theme, out value);

        // Populate cache (invalidate if theme variant changed)
        if (theme != _cachedThemeVariant)
        {
            _resourceCache?.Clear();
            _cachedThemeVariant = theme;
        }

        _resourceCache ??= new(256);
        _resourceCache[key] = found ? value : NotFoundSentinel;

        return found;
    }

    /// <summary>
    /// Invalidates the resource lookup cache. Must be called whenever theme resources are modified.
    /// </summary>
    private void InvalidateResourceCache() => _resourceCache?.Clear();

    /// <summary>
    /// Eagerly loads all theme resources, palettes, and brushes. Call this at application startup
    /// (e.g., behind a splash screen) to avoid a freeze on first resource access.
    /// This method is idempotent — subsequent calls are no-ops.
    /// </summary>
    public void EnsureLoaded()
    {
        if (!_isResourcedAccessed)
        {
            _isResourcedAccessed = true;
            OnResourcedAccessed();
        }
    }

    /// <summary>
    /// Loads theme resources and injects palettes and brushes when accessed for the first time.
    /// </summary>
    private void OnResourcedAccessed()
    {
        using (PerformanceMonitor.Measure(category: PerformanceCategory.Theme))
            AvaloniaXamlLoader.Load(_serviceProvider, this);

        AddOrUpdateAccentShades();
        AddOrUpdatePrimaryShades();
        UpdateBrushesFromCurrentTheme();
    }

    #endregion
}
