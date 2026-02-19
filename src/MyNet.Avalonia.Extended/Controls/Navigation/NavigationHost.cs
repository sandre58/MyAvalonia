// -----------------------------------------------------------------------
// <copyright file="NavigationHost.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MyNet.Avalonia.Helpers;
using MyNet.UI.Navigation;
using MyNet.UI.Navigation.Models;
using MyNet.Utilities.Caching;
using MyNet.Utilities.Caching.Policies;
using MyNet.Utilities.Logging;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Navigation host control that displays pages from INavigationService with optional caching support.
/// Supports three cache strategies: None (no caching), ByInstance (cache per page instance), and ByType (cache one view per page type).
/// </summary>
public partial class NavigationHost : ContentControl, IDisposable
{
    private readonly CacheStorage<object, Control> _cache;
    private INavigationService? _navigationService;
    private Control? _currentView;
    private bool _disposed;

    #region Properties

    /// <summary>
    /// Defines the <see cref="NavigationService"/> property.
    /// </summary>
    public static readonly StyledProperty<INavigationService?> NavigationServiceProperty =
        AvaloniaProperty.Register<NavigationHost, INavigationService?>(nameof(NavigationService));

    /// <summary>
    /// Gets or sets the navigation service that manages page navigation.
    /// </summary>
    public INavigationService? NavigationService
    {
        get => GetValue(NavigationServiceProperty);
        set => SetValue(NavigationServiceProperty, value);
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
    public static readonly StyledProperty<bool> EnableCachingProperty =
        AvaloniaProperty.Register<NavigationHost, bool>(nameof(EnableCaching), true);

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets whether caching is enabled. Default is true.
    /// When false, views are always recreated regardless of CacheMode.
    /// </summary>
    public bool EnableCaching
    {
        get => GetValue(EnableCachingProperty);
        set => SetValue(EnableCachingProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="CacheExpiration"/> property.
    /// </summary>
    public static readonly StyledProperty<TimeSpan> CacheExpirationProperty =
        AvaloniaProperty.Register<NavigationHost, TimeSpan>(nameof(CacheExpiration), TimeSpan.Zero);

    /// <summary>
    /// Gets or sets the cache expiration time for views.
    /// Set to TimeSpan.Zero for no expiration (default).
    /// When set, views that haven't been accessed for this duration will be removed from cache.
    /// </summary>
    public TimeSpan CacheExpiration
    {
        get => GetValue(CacheExpirationProperty);
        set => SetValue(CacheExpirationProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="DisposeOnRemoval"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> DisposeOnRemovalProperty =
        AvaloniaProperty.Register<NavigationHost, bool>(nameof(DisposeOnRemoval), true);

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets whether to dispose views when removed from cache. Default is true.
    /// </summary>
    public bool DisposeOnRemoval
    {
        get => GetValue(DisposeOnRemovalProperty);
        set
        {
            SetValue(DisposeOnRemovalProperty, value);
            _cache.DisposeValuesOnRemoval = value;
        }
    }

    /// <summary>
    /// Gets the current number of cached views.
    /// </summary>
    public int CacheSize => _cache.Keys.Count();

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationHost"/> class.
    /// </summary>
    public NavigationHost()
    {
        _cache = new CacheStorage<object, Control>(
            defaultExpirationPolicyInitCode: CacheExpiration != TimeSpan.Zero ? () => ExpirationPolicy.Duration(CacheExpiration)! : null,
            storeNullValues: false)
        {
            DisposeValuesOnRemoval = true
        };

        // Subscribe to cache expiration events to prevent current view from being removed
        _cache.Expiring += OnCacheExpiring;
        _cache.Expired += OnCacheExpired;

        // Subscribe to NavigationService property changes
        this.GetObservable(NavigationServiceProperty).Subscribe(OnNavigationServiceChanged);
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
                    PerformanceMonitor.Debug("[NavigationHost] Displaying current page: {_navigationService.CurrentContext.Page.GetType().Name}", PerformanceCategory.Pages);
                    DisplayPage(_navigationService.CurrentContext.Page);
                }
            }
        }
    }

    private void OnNavigated(object? sender, NavigationEventArgs e)
    {
        using (PerformanceMonitor.Measure($"[NavigationHost] Navigation to {e.NewPage?.GetType().Name ?? "null"}", category: PerformanceCategory.Pages))
        {
            DisplayPage(e.NewPage);
        }
    }

    #endregion

    #region View Management

    private void DisplayPage(INavigationPage? page)
    {
        if (page is null)
        {
            PerformanceMonitor.Debug("[NavigationHost] Clearing content (null page)", PerformanceCategory.Pages);
            _currentView = null;
            Content = null;
            return;
        }

        // If caching is disabled, always create new view
        if (!EnableCaching || CacheStrategy == CacheStrategy.None)
        {
            using (PerformanceMonitor.Measure($"[NavigationHost] Creating new view for {page.GetType().Name} (caching disabled)", category: PerformanceCategory.Pages))
            {
                _currentView = CreateView(page);
                if (_currentView == null)
                {
                    LogManager.Error($"[NavigationHost] No DataTemplate found for {page.GetType().Name}");
                    throw new InvalidOperationException($"No DataTemplate found for {page.GetType().Name}");
                }

                _currentView.DataContext = page;
                Content = _currentView;
            }

            return;
        }

        var cacheKey = GetCacheKey(page);
        var pageTypeName = page.GetType().Name;

        using (PerformanceMonitor.Measure($"[NavigationHost] Total view resolution for {pageTypeName}", category: PerformanceCategory.Pages))
        {
            var isInCache = _cache.Contains(cacheKey);

            if (isInCache)
            {
                PerformanceMonitor.Debug($"[NavigationHost] Cache HIT for {pageTypeName} (CacheMode={CacheStrategy})", PerformanceCategory.Pages);
            }
            else
            {
                PerformanceMonitor.Debug($"[NavigationHost] Cache MISS for {pageTypeName} (CacheMode={CacheStrategy}) - creating new view", PerformanceCategory.Pages);
            }

            // Use CacheStorage.GetFromCacheOrFetch for clean cache-or-create pattern
            var cachedView = _cache.GetFromCacheOrFetch(
                key: cacheKey,
                code: () => CreateView(page) ?? throw new InvalidOperationException($"No DataTemplate found for {pageTypeName}"),
                @override: false);

            if (Content != cachedView)
            {
                _currentView = cachedView;
                _currentView.DataContext = page;
                Content = cachedView;

                PerformanceMonitor.Debug($"[NavigationHost] Displaying view: {pageTypeName}", PerformanceCategory.Pages);
            }
            else
            {
                // Mise à jour du DataContext si la vue est réutilisée
                _currentView?.DataContext = page;
            }

            PerformanceMonitor.Debug($"[NavigationHost] Cache size: {CacheSize} view(s)", PerformanceCategory.Pages);
        }
    }

    private object GetCacheKey(INavigationPage page) =>
        CacheStrategy switch
        {
            CacheStrategy.ByType => page.GetType(),
            CacheStrategy.ByInstance => page,
            _ => page
        };

    private Control? CreateView(object viewModel)
    {
        using (PerformanceMonitor.Measure($"[NavigationHost] DataTemplate.Build for {viewModel.GetType().Name}", category: PerformanceCategory.Pages))
        {
            var template = this.FindDataTemplate(viewModel) ?? ContentTemplate;
            var control = template?.Build(viewModel);

            if (control != null)
            {
                PerformanceMonitor.Debug($"[NavigationHost] View created successfully for {viewModel.GetType().Name}", PerformanceCategory.Pages);
            }

            return control;
        }
    }

    #endregion

    #region Cache Management

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

    /// <summary>
    /// Removes a specific page from the cache.
    /// </summary>
    /// <param name="page">The page to remove from cache.</param>
    public void RemoveFromCache(INavigationPage page)
    {
        if (page == null) return;

        var cacheKey = GetCacheKey(page);
        _cache.Remove(cacheKey);
        PerformanceMonitor.Debug($"[NavigationHost] Removed {page.GetType().Name} from cache", PerformanceCategory.Pages);
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

    private void OnCacheExpired(object? sender, ExpiredEventArgs<object, Control> e) => PerformanceMonitor.Debug("[NavigationHost] View expired and removed from cache", PerformanceCategory.Pages);

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

            // Clear cache (will dispose views if DisposeOnRemoval is true)
            ClearCache();

            PerformanceMonitor.Debug("[NavigationHost] Disposed", PerformanceCategory.Pages);
        }

        _disposed = true;
    }

    #endregion
}
