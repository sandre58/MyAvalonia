// -----------------------------------------------------------------------
// <copyright file="NavigationHost.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Threading;
using MyNet.Avalonia.Extended.WarmUp;
using MyNet.Avalonia.Helpers;
using MyNet.UI.Navigation;
using MyNet.Utilities.Caching;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Navigation host control that displays pages from INavigationService with caching support.
/// Caches views to improve navigation performance on subsequent visits.
/// </summary>
public class NavigationHost : TransitioningContentControl, IDisposable
{
    private readonly CacheStorage<object, Control> _cache;
    private INavigationService? _navigationService;
    private IWarmUpService? _warmUpService;
    private Control? _currentView;
    private bool _disposed;

    #region Properties

    /// <summary>
    /// Defines the <see cref="NavigationService"/> property.
    /// </summary>
    public static readonly StyledProperty<INavigationService?> NavigationServiceProperty = AvaloniaProperty.Register<NavigationHost, INavigationService?>(nameof(NavigationService));

    /// <summary>
    /// Gets or sets the navigation service that manages page navigation.
    /// </summary>
    public INavigationService? NavigationService
    {
        get => GetValue(NavigationServiceProperty);
        set => SetValue(NavigationServiceProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="WarmUpService"/> property.
    /// </summary>
    public static readonly StyledProperty<IWarmUpService?> WarmUpServiceProperty = AvaloniaProperty.Register<NavigationHost, IWarmUpService?>(nameof(WarmUpService));

    /// <summary>
    /// Gets or sets the warm-up service that manages page warm-up.
    /// </summary>
    public IWarmUpService? WarmUpService
    {
        get => GetValue(WarmUpServiceProperty);
        set => SetValue(WarmUpServiceProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="CacheStrategy"/> property.
    /// </summary>
    public static readonly StyledProperty<CacheStrategy> CacheStrategyProperty = AvaloniaProperty.Register<NavigationHost, CacheStrategy>(nameof(CacheStrategy), CacheStrategy.ByType);

    /// <summary>
    /// Gets or sets the cache strategy. Default is ByType.
    /// </summary>
    public CacheStrategy CacheStrategy
    {
        get => GetValue(CacheStrategyProperty);
        set => SetValue(CacheStrategyProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="EnableCaching"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> EnableCachingProperty = AvaloniaProperty.Register<NavigationHost, bool>(nameof(EnableCaching), true);

    /// <summary>
    /// Gets or sets a value indicating whether caching is enabled. Default is true.
    /// </summary>
    public bool EnableCaching
    {
        get => GetValue(EnableCachingProperty);
        set => SetValue(EnableCachingProperty, value);
    }

    /// <summary>
    /// Gets the current number of cached views.
    /// </summary>
    public int CacheSize => _cache.Keys.Count();

    /// <summary>
    /// Defines the <see cref="MemorySafeMode"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> MemorySafeModeProperty = AvaloniaProperty.Register<NavigationHost, bool>(nameof(MemorySafeMode));

    /// <summary>
    /// Gets or sets a value indicating whether memory safe mode is enabled. When enabled, the control will attempt to minimize memory usage by allowing views to be garbage collected when not in use. This may result in slightly slower navigation performance on subsequent visits due to view re-creation, but can help reduce memory footprint in applications with many pages or limited resources.
    /// </summary>
    public bool MemorySafeMode
    {
        get => GetValue(MemorySafeModeProperty);
        set => SetValue(MemorySafeModeProperty, value);
    }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationHost"/> class.
    /// </summary>
    public NavigationHost()
    {
        _cache = new CacheStorage<object, Control>(storeNullValues: false) { DisposeValuesOnRemoval = true };

        // Subscribe to cache events
        _cache.Expiring += OnCacheExpiring;
        _cache.Expired += OnCacheExpired;

        PageTransition = new CrossFade(TimeSpan.FromMilliseconds(1500));

        // Subscribe to NavigationService property changes
        this.GetObservable(NavigationServiceProperty).Subscribe(OnNavigationServiceChanged);
        this.GetObservable(WarmUpServiceProperty).Subscribe(OnWarmUpServiceChanged);
    }

    #region Navigation Service Management

    private void OnNavigationServiceChanged(INavigationService? service)
    {
        using (PerformanceMonitor.Measure("[NavigationHost] NavigationService changed", category: PerformanceCategory.Pages))
        {
            // Unsubscribe from old service
            if (_navigationService != null)
            {
                _navigationService.Navigated -= OnNavigated;
                PerformanceMonitor.Debug("[NavigationHost] Unsubscribed from previous NavigationService", PerformanceCategory.Pages);
            }

            _navigationService = service;

            // Subscribe to new service and display current page if exists
            if (_navigationService != null)
            {
                _navigationService.Navigated += OnNavigated;
                PerformanceMonitor.Debug("[NavigationHost] Subscribed to new NavigationService", PerformanceCategory.Pages);

                if (_navigationService.CurrentContext?.Page != null)
                {
                    DisplayPage(_navigationService.CurrentContext.Page);
                }
            }
        }
    }

    private void OnNavigated(object? sender, NavigationEventArgs e)
    {
        using (PerformanceMonitor.Measure($"[NavigationHost] Navigation to {e.NewPage.GetType().Name}", category: PerformanceCategory.Pages))
        {
            Dispatcher.UIThread.Post(() => DisplayPage(e.NewPage));
        }
    }

    #endregion

    #region WarmUp Service Management

    private void OnWarmUpServiceChanged(IWarmUpService? service)
    {
        using (PerformanceMonitor.Measure("[NavigationHost] WarmUp service changed", category: PerformanceCategory.Pages))
        {
            // Unsubscribe from old service
            if (_warmUpService != null)
            {
                _warmUpService.WarmUpProgress -= OnWarmUpProgress;
            }

            _warmUpService = service;

            // Subscribe to new service and display current page if exists
            if (_warmUpService != null)
            {
                _warmUpService.WarmUpProgress += OnWarmUpProgress;
            }
        }
    }

    private void OnWarmUpProgress(object? sender, WarmUpProgressEventArgs e)
    {
        using (PerformanceMonitor.Measure($"[NavigationHost] WarmUp to {e.CurrentType.Name}", category: PerformanceCategory.Pages))
        {
            if (e.CurrentObject is not null)
                Dispatcher.UIThread.Post(() => GetOrCreateView(e.CurrentObject));
        }
    }

    #endregion

    #region View Display

    private void DisplayPage(object? page)
    {
        if (page is null)
        {
            PerformanceMonitor.Debug("[NavigationHost] Clearing content (null page)", PerformanceCategory.Pages);
            Content = null;
            return;
        }

        var pageTypeName = page.GetType().Name;

        using (PerformanceMonitor.Measure($"[NavigationHost] Displaying {pageTypeName}", category: PerformanceCategory.Pages))
        {
            var oldView = _currentView;
            var view = GetOrCreateView(page);

            _currentView = view;
            Content = page; // view;

            CleanupOldView(oldView, view);

            PerformanceMonitor.Debug($"[NavigationHost] Content set to {pageTypeName} - Cache size: {CacheSize}", PerformanceCategory.Pages);
        }
    }

    private Control? GetOrCreateView(object page)
    {
        if (!EnableCaching || CacheStrategy == CacheStrategy.None)
        {
            var view = CreateView(page);
            view?.DataContext = page;

            return view;
        }

        var pageTypeName = page.GetType().Name;

        // Use cache
        var cacheKey = GetCacheKey(page);
        var isInCache = _cache.Contains(cacheKey);

        PerformanceMonitor.Debug(isInCache ? $"[NavigationHost] Cache HIT for {pageTypeName} (CacheStrategy={CacheStrategy})" : $"[NavigationHost] Cache MISS for {pageTypeName} (CacheStrategy={CacheStrategy}) - creating view", PerformanceCategory.Pages);

        // Get or create the view
        var newView = _cache.GetFromCacheOrFetch(
            key: cacheKey,
            code: () => CreateView(page) ?? throw new InvalidOperationException($"Failed to create view for {pageTypeName}"),
            @override: false);
        newView.DataContext = page;
        return newView;
    }

    private object GetCacheKey(object page) =>
        CacheStrategy switch
        {
            CacheStrategy.ByType => page.GetType(),
            _ => page
        };

    private Control? CreateView(object? viewModel)
    {
        using (PerformanceMonitor.Measure($"[NavigationHost] Creating view for {viewModel?.GetType().Name}", category: PerformanceCategory.Pages))
        {
            var template = this.FindDataTemplate(viewModel) ?? ContentTemplate;
            var control = template?.Build(viewModel);

            if (control != null)
            {
                PerformanceMonitor.Debug($"[NavigationHost] View created for {viewModel?.GetType().Name}", PerformanceCategory.Pages);
            }

            return control;
        }
    }

    #endregion

    #region Cache Management

    private void CleanupOldView(Control? oldView, Control? newView)
    {
        if (oldView == null || oldView == newView)
            return;

        if (!EnableCaching || MemorySafeMode)
        {
            if (oldView is IReusableView reusable)
                reusable.Reset();

            oldView.DataContext = null;
        }
    }

    /// <summary>
    /// Clears all cached views.
    /// </summary>
    public void ClearCache()
    {
        using (PerformanceMonitor.Measure("[NavigationHost] ClearCache", category: PerformanceCategory.Pages))
        {
            var count = CacheSize;
            _cache.Clear();
            PerformanceMonitor.Debug($"[NavigationHost] Cleared {count} cached view(s)", PerformanceCategory.Pages);
        }
    }

    private void OnCacheExpiring(object? sender, ExpiringEventArgs<object, Control> e)
    {
        // Prevent current view from being expired
        if (_currentView != null && ReferenceEquals(e.Value, _currentView))
        {
            e.Cancel = true;
            PerformanceMonitor.Debug("[NavigationHost] Prevented current view from expiring", PerformanceCategory.Pages);
        }
        else
        {
            PerformanceMonitor.Debug("[NavigationHost] View expiring from cache", PerformanceCategory.Pages);
        }
    }

    private static void OnCacheExpired(object? sender, ExpiredEventArgs<object, Control> e) =>
        PerformanceMonitor.Debug("[NavigationHost] View expired and removed from cache", PerformanceCategory.Pages);

    #endregion

    #region IDisposable

    /// <summary>
    /// Disposes the NavigationHost and cleans up resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the NavigationHost and cleans up resources.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // Unsubscribe from navigation service
            if (_navigationService != null)
            {
                _navigationService.Navigated -= OnNavigated;
            }

            // Unsubscribe from cache events
            _cache.Expiring -= OnCacheExpiring;
            _cache.Expired -= OnCacheExpired;

            // Clear cache
            ClearCache();

            PerformanceMonitor.Debug("[NavigationHost] Disposed", PerformanceCategory.Pages);
        }

        _disposed = true;
    }

    #endregion
}
