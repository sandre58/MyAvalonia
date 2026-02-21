// -----------------------------------------------------------------------
// <copyright file="CachedContentControl.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MyNet.Avalonia.Helpers;
using MyNet.Utilities.Caching;
using MyNet.Utilities.Caching.Policies;

namespace MyNet.Avalonia.Controls;

/// <summary>
/// A content control that caches created views to avoid recreating them on each content change.
/// Dramatically improves navigation performance by reusing existing visual trees.
/// Uses CacheStorage for automatic expiration and thread-safe operations.
/// </summary>
public class CachedContentControl : ContentControl
{
    private readonly CacheStorage<object, Control> _cache;

    /// <summary>
    /// Defines the cache key strategy.
    /// </summary>
    public enum CacheKeyStrategy
    {
        /// <summary>
        /// Cache by view model instance (default).
        /// Each view model instance gets its own cached view.
        /// </summary>
        ByInstance,

        /// <summary>
        /// Cache by view model type.
        /// All instances of the same view model type share the same view.
        /// The view's DataContext is updated when switching between instances.
        /// </summary>
        ByType
    }

    public CachedContentControl()
    {
        // Initialize cache with optional expiration for memory management
        _cache = new CacheStorage<object, Control>(
            defaultExpirationPolicyInitCode: CacheExpiration != TimeSpan.Zero ? () => ExpirationPolicy.Duration(CacheExpiration)! : null,
            storeNullValues: false)
        {
            DisposeValuesOnRemoval = DisposeOnRemoval
        };

        // Subscribe to cache events for cleanup and monitoring
        _cache.Expiring += OnCacheExpiring;
        _cache.Expired += OnCacheExpired;
    }

    /// <summary>
    /// Defines the <see cref="CacheBy"/> property.
    /// </summary>
    public static readonly StyledProperty<CacheKeyStrategy> CacheByProperty =
        AvaloniaProperty.Register<CachedContentControl, CacheKeyStrategy>(nameof(CacheBy), CacheKeyStrategy.ByType);

    /// <summary>
    /// Gets or sets the cache key strategy.
    /// Default is ByType (all instances of same ViewModel type share one view).
    /// </summary>
    public CacheKeyStrategy CacheBy
    {
        get => GetValue(CacheByProperty);
        set => SetValue(CacheByProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="CacheExpiration"/> property.
    /// </summary>
    public static readonly StyledProperty<TimeSpan> CacheExpirationProperty =
        AvaloniaProperty.Register<CachedContentControl, TimeSpan>(nameof(CacheExpiration), TimeSpan.Zero);

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
    public static readonly StyledProperty<bool> DisposeOnRemovalProperty = AvaloniaProperty.Register<CachedContentControl, bool>(nameof(DisposeOnRemoval), true);

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets whether to dispose views when removed from cache.
    /// Default is true.
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
    /// Gets or sets whether to enable caching. Default is true.
    /// </summary>
    public static readonly StyledProperty<bool> EnableCachingProperty = AvaloniaProperty.Register<CachedContentControl, bool>(nameof(EnableCaching), true);

    public bool EnableCaching
    {
        get => GetValue(EnableCachingProperty);
        set => SetValue(EnableCachingProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ContentProperty)
        {
            using (PerformanceMonitor.Measure($"[CachedContentControl] Content changed to {change.NewValue?.GetType().Name}", category: PerformanceCategory.Controls))
            {
                UpdateView(change.NewValue);
            }
        }
    }

    private void UpdateView(object? newContent)
    {
        if (newContent == null)
        {
            // Let the base class handle null content
            return;
        }

        // If caching is disabled, use default behavior
        if (!EnableCaching)
        {
            // Base ContentControl will handle content presentation through its template
            return;
        }

        // Determine cache key based on strategy
        var cacheKey = GetCacheKey(newContent);

        // Measure the ENTIRE view creation process (including cache lookup)
        using (PerformanceMonitor.Measure($"[CachedContentControl] Total view resolution for {newContent.GetType().Name}", category: PerformanceCategory.Pages))
        {
            // Check if already in cache
            var isInCache = _cache.Contains(cacheKey);

            PerformanceMonitor.Debug(isInCache ? $"[CachedContentControl] Cache HIT for {newContent.GetType().Name}" : $"[CachedContentControl] Cache MISS for {newContent.GetType().Name} - creating new view", PerformanceCategory.Pages);

            // Use CacheStorage.GetFromCacheOrFetch for clean cache-or-create pattern
            var cachedView = _cache.GetFromCacheOrFetch(
                key: cacheKey,
                code: () => CreateView(newContent)!,
                @override: false);

            // Always update DataContext (important for ByType strategy)
            cachedView.DataContext = newContent;

            // Set the cached or newly created control as content
            SetCurrentValue(ContentProperty, cachedView);
        }
    }

    /// <summary>
    /// Gets the cache key for the given content based on the CacheBy strategy.
    /// </summary>
    private object GetCacheKey(object content) => CacheBy switch
    {
        CacheKeyStrategy.ByType => content.GetType(),
        _ => content
    };

    private Control? CreateView(object content)
    {
        // Resolve via DataTemplate
        var template = this.FindDataTemplate(content) ?? ContentTemplate;
        if (template == null)
            return null;

        using (PerformanceMonitor.Measure($"[CachedContentControl] DataTemplate.Build for {content.GetType().Name}", category: PerformanceCategory.Pages))
        {
            var newView = template.Build(content);
            if (newView != null)
            {
                newView.DataContext = content;
                PerformanceMonitor.Debug($"[CachedContentControl] View type created: {newView.GetType().Name}", PerformanceCategory.Pages);
                return newView;
            }
        }

        return null;
    }

    private void OnCacheExpiring(object? sender, ExpiringEventArgs<object, Control> e)
    {
        // Allow cancellation of expiration if needed
        var keyDescription = e.Key is Type type ? type.Name : e.Key.GetType().Name;
        PerformanceMonitor.Debug($"[CachedContentControl] View for {keyDescription} is expiring", PerformanceCategory.Controls);

        // Prevent expiration of currently visible view
        var currentKey = Content != null ? GetCacheKey(Content) : null;
        if (currentKey?.Equals(e.Key) == true)
        {
            e.Cancel = true;
            PerformanceMonitor.Debug("[CachedContentControl] Prevented expiration of currently visible view", PerformanceCategory.Controls);
        }
    }

    private static void OnCacheExpired(object? sender, ExpiredEventArgs<object, Control> e)
    {
        var keyDescription = e.Key is Type type ? type.Name : e.Key.GetType().Name;
        PerformanceMonitor.Debug($"[CachedContentControl] View for {keyDescription} has expired and been removed from cache", PerformanceCategory.Controls);
    }

    /// <summary>
    /// Clears all cached views.
    /// </summary>
    public void ClearCache()
    {
        PerformanceMonitor.Debug($"[CachedContentControl] Clearing cache ({CacheSize} views)", PerformanceCategory.Controls);
        _cache.Clear();
    }

    /// <summary>
    /// Gets the current cache size.
    /// </summary>
    public int CacheSize => _cache.Keys.Count();

    /// <summary>
    /// Removes a specific view from the cache.
    /// </summary>
    /// <param name="key">The view model key to remove.</param>
    public void RemoveFromCache(object key)
    {
        _cache.Remove(key);
        PerformanceMonitor.Debug($"[CachedContentControl] Removed view for {key.GetType().Name} from cache", PerformanceCategory.Controls);
    }
}
