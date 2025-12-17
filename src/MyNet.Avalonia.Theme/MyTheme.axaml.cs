// -----------------------------------------------------------------------
// <copyright file="MyTheme.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using MyNet.Avalonia.Helpers;
using MyNet.Avalonia.Theme.Palettes;
using MyNet.Avalonia.Theme.Theming;
using MyNet.Utilities;

namespace MyNet.Avalonia.Theme;

/// <summary>
/// Provides the main theme engine for the application, managing theme variants (Dark, Light, HighContrast), brand color palettes (Primary, Accent), and resource injection.
/// Supports hot-reload for theme changes and dynamic color updates, ensuring consistent styling and smooth transitions across the UI.
/// </summary>
public class MyTheme : Styles, IResourceNode, IMyTheme
{
    private static readonly ColorShades DefaultPrimary = new(Color.Parse("#2196F3"));
    private static readonly ColorShades DefaultAccent = new(Color.Parse("#FFC107"));

    private static MyTheme? _current;
    private readonly IServiceProvider? _serviceProvider;
    private readonly BrushManager _brushManager;

    /// <summary>
    /// Gets the current theme instance from the application, providing color palettes, theme management, and resource injection.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when MyTheme is not found in Application.Styles.</exception>
    public static MyTheme Current
    {
        get
        {
            if (_current is not null) return _current;
            _current = Application.Current?.Styles.OfType<MyTheme>().FirstOrDefault()
                ?? throw new InvalidOperationException("Cannot locate MyTheme in Avalonia application styles. Ensure MyTheme is included in your App.axaml in Application.Styles section.");
            return _current;
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

    #region Properties

    /// <summary>
    /// Gets or sets the duration of color transition animations when theme colors change. Default is 150 milliseconds.
    /// </summary>
    public TimeSpan ColorTransitionDuration { get; set; } = TimeSpan.FromMilliseconds(150);

    /// <summary>
    /// Gets or sets the easing function used for color transition animations. Default is SineEaseOut for smooth transitions.
    /// </summary>
    public Easing ColorTransitionEasing { get; set; } = new SineEaseOut();

    #endregion

    #region Primary Property

    /// <summary>
    /// Styled property for the primary brand color palette.
    /// </summary>
    private static readonly StyledProperty<ColorShades> PrimaryProperty =
        AvaloniaProperty.Register<MyTheme, ColorShades>(nameof(Primary), DefaultPrimary);

    /// <summary>
    /// Gets or sets the primary brand color palette with automatic shade generation. Used for main actions, headers, and accent UI elements.
    /// Changing this property updates all primary-dependent resources and triggers the ThemeChanged event.
    /// </summary>
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
    private static readonly StyledProperty<ColorShades> AccentProperty =
        AvaloniaProperty.Register<MyTheme, ColorShades>(nameof(Accent), DefaultAccent);

    /// <summary>
    /// Gets or sets the accent brand color palette with automatic shade generation. Used for highlights, floating action buttons, and secondary actions.
    /// Changing this property updates all accent-dependent resources and triggers the ThemeChanged event.
    /// </summary>
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
        Theme = Application.Current?.ActualThemeVariant.Key?.ToString();
        UpdateBrushesFromCurrentTheme();
        RaiseThemeChanged();
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
    public void RegisterThemeProvider(ThemeVariantColors theme)
    {
        ArgumentNullException.ThrowIfNull(theme);
        var rd = new ResourceDictionary();
        theme.ToResourceDictionary().ForEach(kv => rd.Add(kv.Key, kv.Value));
        Resources.ThemeDictionaries.AddOrUpdate(theme.Variant, rd);
    }

    #endregion

    #region Resource Injection

    /// <summary>
    /// Injects all accent brand palette resources into the ResourceDictionary, including base color, foreground, and all shades.
    /// </summary>
    private void AddOrUpdatePrimaryShades()
    {
        using (PerformanceMonitor.Measure())
            AddOrUpdateColorShades(Primary, nameof(Primary));
    }

    /// <summary>
    /// Injects all primary brand palette resources into the ResourceDictionary, including base color, foreground, and all shades.
    /// </summary>
    private void AddOrUpdateAccentShades()
    {
        using (PerformanceMonitor.Measure())
            AddOrUpdateColorShades(Accent, nameof(Accent));
    }

    /// <summary>
    /// Injects a dictionary of colors into the ResourceDictionary.
    /// </summary>
    private void UpdateBrushesFromCurrentTheme()
    {
        const string transparencyKey = "Transparency";
        const string transparencySmallKey = "Transparency.Small";

        using (PerformanceMonitor.Measure())
        {
            var count = 0;

            foreach (var (key, obj) in GetActiveThemeDictionary())
            {
                if (obj is Color color)
                {
                    var colorKey = key?.ToString()?.Replace(ThemeResourceKeyFactory.Pattern(ThemeResourceKeyFactory.ColorKey).FormatWith(string.Empty), string.Empty, StringComparison.OrdinalIgnoreCase);
                    if (!string.IsNullOrEmpty(colorKey))
                    {
                        AddOrUpdateBrush(colorKey, color, null);
                        count++;
                    }
                }
            }

            if (!Resources.ContainsKey(ThemeResourceKeyFactory.Brush(transparencyKey)))
                Resources.Add(ThemeResourceKeyFactory.Brush(transparencyKey), createTransparencyBrush(20));

            if (!Resources.ContainsKey(ThemeResourceKeyFactory.Brush(transparencySmallKey)))
                Resources.Add(ThemeResourceKeyFactory.Brush(transparencySmallKey), createTransparencyBrush(8));

            PerformanceMonitor.Debug($"UpdateBrushesFromCurrentTheme processed {count + 2} brushes");
        }

        VisualBrush createTransparencyBrush(double size) => new(new Image
        {
            Height = size,
            Width = size,
            Source = new DrawingImage
            {
                Drawing = new DrawingGroup
                {
                    Children = [
                                new GeometryDrawing() { Brush = Brushes.Transparent, Geometry = PathGeometry.Parse("M0,0 L2,0 2,2, 0,2Z") },
                                new GeometryDrawing() { Brush = GetBrush("Application.Foreground", nameof(Opacity.Scrim)), Geometry = PathGeometry.Parse("M0,1 L2,1 2,2, 1,2 1,0 0,0Z") },
                            ]
                }
            }
        })
        {
            DestinationRect = new RelativeRect(0, 0, size, size, RelativeUnit.Absolute),
            Stretch = Stretch.Uniform,
            TileMode = TileMode.Tile,
        };
    }

    /// <summary>
    /// Injects a dictionary of colors into the ResourceDictionary. Each entry is injected on the UI thread.
    /// </summary>
    /// <param name="shades">Shades of the color to inject.</param>
    /// <param name="name">The name of the color group.</param>
    private void AddOrUpdateColorShades(ColorShades shades, string name)
    {
        using (PerformanceMonitor.Measure())
        {
            var count = 0;

            foreach (var (key, color) in shades.ToResourceDictionary(name))
            {
                AddOrUpdateColorAndBrush(key, color, shades.Foreground);
                count++;
            }

            PerformanceMonitor.Debug($"AddOrUpdateColorShades({name}) processed {count} shades");
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
    private void RaiseThemeChanged() => ThemeChanged?.Invoke(this, EventArgs.Empty);

    private ResourceDictionary GetActiveThemeDictionary()
    {
        var current = Application.Current?.ActualThemeVariant ?? ThemeVariant.Default;
        return Resources.ThemeDictionaries.TryGetValue(current, out var rd) && rd is ResourceDictionary resources ? resources : [];
    }

    #endregion

    #region IResourceNode Implementation

    private bool _isResourcedAccessed;

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
    /// </summary>
    /// <param name="key">The resource key to look up.</param>
    /// <param name="theme">The theme variant context.</param>
    /// <param name="value">The resource value if found.</param>
    /// <returns>True if the resource was found; otherwise, false.</returns>
    protected new virtual bool TryGetResource(object key, ThemeVariant? theme, out object? value)
    {
        if (_isResourcedAccessed)
            return Resources.TryGetResource(key, theme, out value) || base.TryGetResource(key, theme, out value);
        _isResourcedAccessed = true;
        OnResourcedAccessed();
        return Resources.TryGetResource(key, theme, out value) || base.TryGetResource(key, theme, out value);
    }

    /// <summary>
    /// Gets a brush from the theme resources by path.
    /// </summary>
    /// <param name="path">The resource path for the brush.</param>
    /// <param name="opacityKey">Optional opacity key or value.</param>
    /// <param name="contrast">If true, returns the contrast brush for accessibility.</param>
    /// <returns>The brush instance.</returns>
    public IBrush GetBrush(string path, string? opacityKey = null, bool contrast = false)
    {
        var opacity = GetOpacity(opacityKey);
        return _brushManager.Get(ThemeResourceKeyFactory.Brush(path), opacity, contrast);
    }

    /// <summary>
    /// Gets a brush from the theme resources by brush instance, optionally with a specific opacity or contrast.
    /// </summary>
    /// <param name="brush">The brush instance to search for.</param>
    /// <param name="opacityKey">Optional opacity key or value.</param>
    /// <param name="contrast">If true, returns the contrast brush for accessibility.</param>
    /// <returns>The brush instance with the specified opacity or contrast.</returns>
    public IBrush GetBrush(IBrush brush, string? opacityKey = null, bool contrast = false)
    {
        var opacity = GetOpacity(opacityKey);
        return _brushManager.Get(brush, opacity, contrast);
    }

    /// <summary>
    /// Gets an opacity value from the theme resources or parses it directly.
    /// </summary>
    /// <param name="opacityKey">Value or key for the opacity.</param>
    /// <returns>Value for the opacity.</returns>
    public double? GetOpacity(string? opacityKey)
    {
        double? opacity = null;
        if (double.TryParse(opacityKey, out var result))
        {
            opacity = result;
        }
        else if (!string.IsNullOrEmpty(opacityKey))
        {
            var fullOpacityKey = ThemeResourceKeyFactory.Opacity(opacityKey);
            opacity = Resources.TryGetResource(fullOpacityKey, Application.Current?.ActualThemeVariant, out var obj) && obj is double d ? d : null;
        }

        return opacity;
    }

    /// <summary>
    /// Loads theme resources and injects palettes and brushes when accessed for the first time.
    /// </summary>
    private void OnResourcedAccessed()
    {
        using (PerformanceMonitor.Measure())
            AvaloniaXamlLoader.Load(_serviceProvider, this);

        AddOrUpdateAccentShades();
        AddOrUpdatePrimaryShades();
        UpdateBrushesFromCurrentTheme();
    }

    #endregion
}
