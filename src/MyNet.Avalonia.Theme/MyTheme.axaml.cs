// -----------------------------------------------------------------------
// <copyright file="MyTheme.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;
using MyNet.Avalonia.Helpers;
using MyNet.Avalonia.Theme.Palettes;
using MyNet.Avalonia.Theme.Themes;
using MyNet.Avalonia.Theming;
using MyNet.Utilities;

namespace MyNet.Avalonia.Theme;

/// <summary>
/// Represents the main accent engine for the application, providing color palettes, accent management, and resource injection.
/// Supports hot-reload for both accent variants (Dark/Light/HighContrast) and brand colors (Primary/Accent).
/// This class manages three main components:
/// - Theme variant (Dark/Light/HighContrast with semantic colors like Success, Error, Warning)
/// - Primary brand color palette (with automatic shade generation)
/// - Accent brand color palette (with automatic shade generation).
/// </summary>
public class MyTheme(IServiceProvider? serviceProvider) : Styles, IResourceNode, IAvaloniaTheme, IDisposable
{
    #region Constants and Defaults

    private static readonly ColorPalette DefaultPrimary = new(Color.Parse("#2196F3"));
    private static readonly ColorPalette DefaultAccent = new(Color.Parse("#FFC107"));
    private static readonly ThemePalette DefaultTheme = BuiltInThemeProviders.Dark;
    private static readonly Dictionary<ThemeVariant, ThemePalette> RegisteredThemes = new()
    {
        [ThemeVariant.Dark] = BuiltInThemeProviders.Dark,
        [ThemeVariant.Light] = BuiltInThemeProviders.Light,
        [BuiltInThemeProviders.HighContrast.Variant] = BuiltInThemeProviders.HighContrast
    };

    private static MyTheme? _current;

#if NET9_0_OR_GREATER
    private readonly Lock _themeInjectionLock = new();
    private readonly Lock _primaryInjectionLock = new();
    private readonly Lock _accentInjectionLock = new();
#else
    private readonly object _themeInjectionLock = new();
    private readonly object _primaryInjectionLock = new();
    private readonly object _accentInjectionLock = new();
#endif

    private CancellationTokenSource? _themeInjectionCts;
    private CancellationTokenSource? _primaryInjectionCts;
    private CancellationTokenSource? _accentInjectionCts;

    private Task? _currentThemeInjectionTask;
    private Task? _currentPrimaryInjectionTask;
    private Task? _currentAccentInjectionTask;
    private bool _disposedValue;

    #endregion

    #region Static Properties

    /// <summary>
    /// Gets the current accent instance from the application, providing color palettes, accent management, and resource injection.
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

    #endregion

    #region Events

    /// <summary>
    /// Raised when the accent, accent, or accent palette changes.
    /// Useful for components that need to react to accent changes.
    /// </summary>
    public event EventHandler? ThemeChanged;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the duration of color transition animations when accent colors change.
    /// Default is 150 milliseconds.
    /// </summary>
    public TimeSpan ColorTransitionDuration { get; set; } = TimeSpan.FromMilliseconds(150);

    /// <summary>
    /// Gets or sets the easing function used for color transition animations.
    /// Default is SineEaseOut for smooth transitions.
    /// </summary>
    public Easing ColorTransitionEasing { get; set; } = new SineEaseOut();

    #endregion

    #region Theme Property

    /// <summary>
    /// Styled property for the active accent palette.
    /// </summary>
    private static readonly StyledProperty<ThemePalette> ThemeProperty =
        AvaloniaProperty.Register<MyTheme, ThemePalette>(nameof(Theme), DefaultTheme);

    /// <summary>
    /// Gets or sets the active accent palette (Dark, Light, HighContrast, or custom).
    /// Changing this property will update all accent-dependent resources and trigger the ThemeChanged event.
    /// </summary>
    public ThemePalette Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    #endregion

    #region Primary Property

    /// <summary>
    /// Styled property for the accent brand color palette.
    /// </summary>
    private static readonly StyledProperty<ColorPalette> PrimaryProperty =
        AvaloniaProperty.Register<MyTheme, ColorPalette>(nameof(Primary), DefaultPrimary);

    /// <summary>
    /// Gets or sets the accent brand color palette with automatic shade generation.
    /// This is typically used for main actions, headers, and accent UI elements.
    /// Changing this property will update all accent-dependent resources and trigger the ThemeChanged event.
    /// </summary>
    public ColorPalette Primary
    {
        get => GetValue(PrimaryProperty);
        set => SetValue(PrimaryProperty, value);
    }

    #endregion

    #region Accent Property

    /// <summary>
    /// Styled property for the accent brand color palette.
    /// </summary>
    private static readonly StyledProperty<ColorPalette> AccentProperty =
        AvaloniaProperty.Register<MyTheme, ColorPalette>(nameof(Accent), DefaultAccent);

    /// <summary>
    /// Gets or sets the accent brand color palette with automatic shade generation.
    /// This is typically used for highlights, floating action buttons, and secondary actions.
    /// Changing this property will update all accent-dependent resources and trigger the ThemeChanged event.
    /// </summary>
    public ColorPalette Accent
    {
        get => GetValue(AccentProperty);
        set => SetValue(AccentProperty, value);
    }

    #endregion

    #region Property Change Handling

    /// <summary>
    /// Handles property changes to update resources when accent or brand colors change.
    /// Automatically injects updated colors into the ResourceDictionary asynchronously and raises the ThemeChanged event.
    /// </summary>
    /// <param name="change">The property change event arguments.</param>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        // Handle Primary color palette change
        if (change.Property == PrimaryProperty)
        {
            _ = InjectPrimaryResourcesAsync();
        }

        // Handle Accent color palette change
        else if (change.Property == AccentProperty)
        {
            _ = InjectAccentResourcesAsync();
        }

        // Handle Theme variant change
        else if (change.Property == ThemeProperty)
        {
            _ = InjectThemeResourcesAsync();
        }
    }

    #endregion

    #region Theme Provider Management

    /// <summary>
    /// Registers a custom accent variant palette.
    /// This allows extending the accent system with custom themes beyond Dark/Light/HighContrast.
    /// </summary>
    /// <param name="theme">The accent palette to register.</param>
    /// <exception cref="ArgumentNullException">Thrown when accent is null.</exception>
    /// <example>
    /// <code>
    /// var neonTheme = new ThemePalette(new ThemeVariant("Neon", ThemeVariantKind.Custom))
    /// {
    ///     Base = new BaseThemePalette { ... },
    ///     Success = new ColorPalette(Color.Parse("#00FF00"))y
    ///     // ... other palettes
    /// };
    /// MyTheme.RegisterThemeProvider(neonTheme);
    /// </code>
    /// </example>
    public static void RegisterThemeProvider(ThemePalette theme)
    {
        ArgumentNullException.ThrowIfNull(theme);

        lock (RegisteredThemes)
        {
            RegisteredThemes[theme.Variant] = theme;
        }
    }

    /// <summary>
    /// Unregisters a custom accent variant palette.
    /// Built-in themes (Dark, Light, HighContrast) cannot be unregistered.
    /// </summary>
    /// <param name="themeVariant">The accent variant to unregister.</param>
    /// <returns>True if the accent was successfully unregistered; false if it was a built-in accent or not found.</returns>
    public static bool UnregisterThemeProvider(ThemeVariant themeVariant)
    {
        // Prevent unregistering built-in themes
        if (themeVariant == ThemeVariant.Dark ||
            themeVariant == ThemeVariant.Light ||
            themeVariant == BuiltInThemeProviders.HighContrast.Variant)
        {
            return false;
        }

        lock (RegisteredThemes)
        {
            return RegisteredThemes.Remove(themeVariant);
        }
    }

    /// <summary>
    /// Gets a accent palette by its variant key.
    /// </summary>
    /// <param name="variant">The accent variant to retrieve.</param>
    /// <returns>The accent palette if found; otherwise, the default Dark accent.</returns>
    public static ThemePalette GetThemeByVariant(ThemeVariant variant)
    {
        lock (RegisteredThemes)
        {
            return RegisteredThemes.TryGetValue(variant, out var theme) ? theme : DefaultTheme;
        }
    }

    #endregion

    #region Resource Injection (Async)

    /// <summary>
    /// Injects all resources (accent, accent, accent) asynchronously in parallel.
    /// Each palette injection is independent and will not cancel the others.
    /// </summary>
    /// <returns>A task representing the asynchronous injection operation.</returns>
    private async Task InjectAllResourcesAsync() => await Task.WhenAll(
            InjectThemeResourcesAsync(),
            InjectPrimaryResourcesAsync(),
            InjectAccentResourcesAsync()).ConfigureAwait(false);

    /// <summary>
    /// Injects all accent palette resources (Base, Success, Warning, Error, etc.) into the ResourceDictionary asynchronously.
    /// Each color is injected both as a Color resource and as a SolidColorBrush resource with transitions.
    /// </summary>
    /// <returns>A task representing the asynchronous injection operation.</returns>
    private async Task InjectThemeResourcesAsync()
    {
        CancellationToken cancellationToken;

        lock (_themeInjectionLock)
        {
            // Cancel only the previous injection of THIS palette type
            if (_themeInjectionCts is not null)
            {
                _ = _themeInjectionCts.CancelAsync();
                _themeInjectionCts.Dispose();
            }

            _themeInjectionCts = new CancellationTokenSource();
            cancellationToken = _themeInjectionCts.Token;
        }

        try
        {
            // Wait for previous injection of THIS palette type to complete
            if (_currentThemeInjectionTask is not null)
            {
                try
                {
                    await _currentThemeInjectionTask.ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    // Expected when we cancelled the previous operation
                }
            }

            // Start new injection
            _currentThemeInjectionTask = Task.Run(async () =>
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    var theme = DispatcherHelper.Invoke(() => Theme);
                    var resources = theme.ToResourceDictionary();
                    await InjectColorDictionaryAsync(resources).ConfigureAwait(false);
                    await RaiseThemeChangedAsync().ConfigureAwait(false);
                }
            },
            cancellationToken);

            await _currentThemeInjectionTask.ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Expected when a new injection cancels this one
        }
    }

    /// <summary>
    /// Injects all accent brand palette resources into the ResourceDictionary asynchronously.
    /// Includes base color, foreground, and all shades (50-900, Light, Dark, etc.).
    /// </summary>
    /// <returns>A task representing the asynchronous injection operation.</returns>
    private async Task InjectPrimaryResourcesAsync()
    {
        CancellationToken cancellationToken;

        lock (_primaryInjectionLock)
        {
            // Cancel only the previous injection of THIS palette type
            if (_primaryInjectionCts is not null)
            {
                _ = _primaryInjectionCts.CancelAsync();
                _primaryInjectionCts.Dispose();
            }

            _primaryInjectionCts = new CancellationTokenSource();
            cancellationToken = _primaryInjectionCts.Token;
        }

        try
        {
            // Wait for previous injection of THIS palette type to complete
            if (_currentPrimaryInjectionTask is not null)
            {
                try
                {
                    await _currentPrimaryInjectionTask.ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    // Expected when we cancelled the previous operation
                }
            }

            // Start new injection
            _currentPrimaryInjectionTask = Task.Run(async () =>
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    var primary = DispatcherHelper.Invoke(() => Primary);
                    var resources = primary.ToResourceDictionary(nameof(Primary));
                    await InjectColorDictionaryAsync(resources).ConfigureAwait(false);
                    await RaiseThemeChangedAsync().ConfigureAwait(false);
                }
            },
            cancellationToken);

            await _currentPrimaryInjectionTask.ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Expected when a new injection cancels this one
        }
    }

    /// <summary>
    /// Injects all accent brand palette resources into the ResourceDictionary asynchronously.
    /// Includes base color, foreground, and all shades (50-900, Light, Dark, etc.).
    /// </summary>
    /// <returns>A task representing the asynchronous injection operation.</returns>
    private async Task InjectAccentResourcesAsync()
    {
        CancellationToken cancellationToken;

        lock (_accentInjectionLock)
        {
            // Cancel only the previous injection of THIS palette type
            if (_accentInjectionCts is not null)
            {
                _ = _accentInjectionCts.CancelAsync();
                _accentInjectionCts.Dispose();
            }

            _accentInjectionCts = new CancellationTokenSource();
            cancellationToken = _accentInjectionCts.Token;
        }

        try
        {
            // Wait for previous injection of THIS palette type to complete
            if (_currentAccentInjectionTask is not null)
            {
                try
                {
                    await _currentAccentInjectionTask.ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    // Expected when we cancelled the previous operation
                }
            }

            // Start new injection
            _currentAccentInjectionTask = Task.Run(async () =>
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    var accent = DispatcherHelper.Invoke(() => Accent);
                    var resources = accent.ToResourceDictionary(nameof(Accent));
                    await InjectColorDictionaryAsync(resources).ConfigureAwait(false);
                    await RaiseThemeChangedAsync().ConfigureAwait(false);
                }
            },
            cancellationToken);

            await _currentAccentInjectionTask.ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Expected when a new injection cancels this one
        }
    }

    /// <summary>
    /// Injects a dictionary of colors into the ResourceDictionary asynchronously.
    /// Each entry is injected on the UI thread.
    /// </summary>
    /// <param name="colorDictionary">The dictionary of color keys and values to inject.</param>
    /// <returns>A task representing the asynchronous injection operation.</returns>
    private Task InjectColorDictionaryAsync(IReadOnlyDictionary<string, Color> colorDictionary)
        => DispatcherHelper.InvokeAsync(() =>
        {
            foreach (var (key, color) in colorDictionary)
            {
                InjectColorAndBrushResource(key, color);
            }
        });

    /// <summary>
    /// Injects a single color into the ResourceDictionary both as a Color and as a SolidColorBrush with transitions.
    /// If the brush already exists, it updates the color (triggering the transition animation).
    /// If it doesn't exist, it creates a new brush with color transition animations.
    /// </summary>
    /// <param name="colorKey">The base resource key (will be prefixed with "MyNet.Color." and "MyNet.Brush.").</param>
    /// <param name="newColor">The color to inject.</param>
    private void InjectColorAndBrushResource(string colorKey, Color newColor)
    {
        var rd = Resources;

        // Generate full resource keys
        var fullColorKey = ThemeResourceKeyFactory.Color(colorKey);
        var fullBrushKey = ThemeResourceKeyFactory.Brush(colorKey);

        // Inject or update Color resource
        rd.AddOrUpdate(fullColorKey, newColor);

        // Inject or update Brush resource with transition
        if (rd.TryGetValue(fullBrushKey, out var existing) && existing is SolidColorBrush brush)
        {
            // Update existing brush color (will animate via ColorTransition)
            brush.Color = newColor;
        }
        else
        {
            // Create new brush with color transition animation
            var newBrush = new SolidColorBrush(newColor)
            {
                Transitions =
                [
                    new ColorTransition
                        {
                            Duration = ColorTransitionDuration,
                            Easing = ColorTransitionEasing,
                            Property = SolidColorBrush.ColorProperty
                        }
                ]
            };

            rd.AddOrUpdate(fullBrushKey, newBrush);
        }
    }

    /// <summary>
    /// Raises the ThemeChanged event on the UI thread asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private Task RaiseThemeChangedAsync() => DispatcherHelper.InvokeAsync(() => ThemeChanged?.Invoke(this, EventArgs.Empty), DispatcherPriority.Normal);

    #endregion

    #region IResourceNode Implementation

    private bool _isResourcedAccessed;

    /// <summary>
    /// Tries to get a resource from the accent's resource dictionary.
    /// Implements IResourceNode to integrate with Avalonia's resource lookup system.
    /// </summary>
    /// <param name="key">The resource key to look up.</param>
    /// <param name="theme">The accent variant context (not used in this implementation).</param>
    /// <param name="value">The resource value if found.</param>
    /// <returns>True if the resource was found; otherwise, false.</returns>
    bool IResourceNode.TryGetResource(object key, ThemeVariant? theme, out object? value) => TryGetResource(key, theme, out value);

    protected new virtual bool TryGetResource(object key, ThemeVariant? theme, out object? value)
    {
        if (_isResourcedAccessed)
            return base.TryGetResource(key, theme, out value) || Resources.TryGetResource(key, theme, out value);
        _isResourcedAccessed = true;
        OnResourcedAccessed();

        return base.TryGetResource(key, theme, out value) || Resources.TryGetResource(key, theme, out value);
    }

    private void OnResourcedAccessed()
    {
        _ = InjectAllResourcesAsync();

        AvaloniaXamlLoader.Load(serviceProvider, this);
    }

    #endregion

    #region IAvaloniaTheme Implementation

    /// <summary>
    /// Sets the accent brand color and optional foreground color.
    /// Creates a new ColorPalette with automatic shade generation.
    /// </summary>
    /// <param name="color">The base accent color.</param>
    /// <param name="foreground">Optional foreground color (auto-calculated if null).</param>
    public void SetPrimary(Color color, Color? foreground) => SetCurrentValue(PrimaryProperty, new ColorPalette(color, foreground));

    /// <summary>
    /// Sets the accent brand color and optional foreground color.
    /// Creates a new ColorPalette with automatic shade generation.
    /// </summary>
    /// <param name="color">The base accent color.</param>
    /// <param name="foreground">Optional foreground color (auto-calculated if null).</param>
    public void SetAccent(Color color, Color? foreground) => SetCurrentValue(AccentProperty, new ColorPalette(color, foreground));

    /// <summary>
    /// Sets the active accent by name or variant.
    /// If name is null or empty, uses the system's ActualThemeVariant.
    /// </summary>
    /// <param name="name">The accent name (e.g., "Dark", "Light", "HighContrast") or null for system default.</param>
    public void SetTheme(string? name)
    {
        ThemePalette? targetTheme = null;

        // If no name specified, use system accent
        if (string.IsNullOrWhiteSpace(name))
        {
            var systemVariant = Application.Current?.ActualThemeVariant ?? ThemeVariant.Dark;
            targetTheme = GetThemeByVariant(systemVariant);
        }
        else
        {
            // Try to find accent by name (case-insensitive)
            lock (RegisteredThemes)
            {
                targetTheme = RegisteredThemes.Values.FirstOrDefault(t =>
                    t.Variant.Key?.ToString()?.Equals(name, StringComparison.OrdinalIgnoreCase) == true);
            }
        }

        // Apply accent if found, otherwise keep current
        if (targetTheme is not null)
        {
            SetCurrentValue(ThemeProperty, targetTheme);
        }
    }

    /// <summary>
    /// Gets the current accent variant name.
    /// </summary>
    /// <returns>The accent variant key as a string (e.g., "Dark", "Light").</returns>
    public string? GetThemeName() => Theme.Variant.Key?.ToString();

    /// <summary>
    /// Gets the current accent color pair (base color and foreground).
    /// </summary>
    /// <returns>A ColorPair containing the accent base and foreground colors.</returns>
    public ColorPair GetPrimaryPair() => new(Primary.Base, Primary.Foreground);

    /// <summary>
    /// Gets the current accent color pair (base color and foreground).
    /// </summary>
    /// <returns>A ColorPair containing the accent base and foreground colors.</returns>
    public ColorPair GetAccentPair() => new(Accent.Base, Accent.Foreground);

    #endregion

    #region IDisposable

    /// <summary>
    /// Disposes of resources used by the accent.
    /// Cancels any ongoing async injection operations for all palette types.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // Cancel and dispose Theme injection
                _themeInjectionCts?.Cancel();
                _themeInjectionCts?.Dispose();
                _themeInjectionCts = null;

                // Cancel and dispose Primary injection
                _primaryInjectionCts?.Cancel();
                _primaryInjectionCts?.Dispose();
                _primaryInjectionCts = null;

                // Cancel and dispose Accent injection
                _accentInjectionCts?.Cancel();
                _accentInjectionCts?.Dispose();
                _accentInjectionCts = null;
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
