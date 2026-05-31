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
        _cache = new(
            defaultExpirationPolicyInitCode: CacheExpiration != TimeSpan.Zero ? () => ExpirationPolicy.Duration(CacheExpiration)! : null,
            storeNullValues: false)
        {
            DisposeValuesOnRemoval = DisposeOnRemoval
        };

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
            UpdateView(change.NewValue);
    }

    private void UpdateView(object? newContent)
    {
        if (newContent == null)
            return;

        if (!EnableCaching)
            return;

        var cacheKey = GetCacheKey(newContent);

        var cachedView = _cache.GetFromCacheOrFetch(
            key: cacheKey,
            code: () => CreateView(newContent)!,
            @override: false);

        cachedView.DataContext = newContent;

        if (!ReferenceEquals(Content, cachedView))
            Content = cachedView;
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
        var template = this.FindDataTemplate(content) ?? ContentTemplate;

        var newView = template?.Build(content);
        if (newView == null)
            return null;

        newView.DataContext = content;
        return newView;
    }

    private void OnCacheExpiring(object? sender, ExpiringEventArgs<object, Control> e)
    {
        var currentKey = Content != null ? GetCacheKey(Content) : null;
        if (currentKey?.Equals(e.Key) == true)
            e.Cancel = true;
    }

    private static void OnCacheExpired(object? sender, ExpiredEventArgs<object, Control> e)
    {
    }

    /// <summary>
    /// Clears all cached views.
    /// </summary>
    public void ClearCache() => _cache.Clear();

    /// <summary>
    /// Gets the current cache size.
    /// </summary>
    public int CacheSize => _cache.Keys.Count();

    /// <summary>
    /// Removes a specific view from the cache.
    /// </summary>
    /// <param name="key">The view model key to remove.</param>
    public void RemoveFromCache(object key) => _cache.Remove(key);
}
