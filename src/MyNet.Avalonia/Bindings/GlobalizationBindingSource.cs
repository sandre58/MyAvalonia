// -----------------------------------------------------------------------
// <copyright file="GlobalizationBindingSource.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using MyNet.Globalization;
using MyNet.Globalization.Facade;

namespace MyNet.Avalonia.Bindings;

/// <summary>
/// Observable binding source for culture and time zone changes from <see cref="GlobalizationServices.Current"/>.
/// </summary>
public sealed class GlobalizationBindingSource : INotifyPropertyChanged
{
    /// <summary>
    /// Gets the shared instance used as a binding source in XAML multi-bindings.
    /// </summary>
    public static GlobalizationBindingSource Instance { get; } = new();

    private IGlobalizationService? _subscribedService;

    private GlobalizationBindingSource() { }

    /// <summary>
    /// Gets the current UI culture.
    /// </summary>
    public CultureInfo Culture
    {
        get
        {
            EnsureSubscribed();
            return GlobalizationServices.Current.CurrentCulture;
        }
    }

    /// <summary>
    /// Gets the current application time zone.
    /// </summary>
    public TimeZoneInfo TimeZone
    {
        get
        {
            EnsureSubscribed();
            return GlobalizationServices.Current.CurrentTimeZone;
        }
    }

    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Reconnects culture/time zone change handlers to <see cref="GlobalizationServices.Current"/>.
    /// Call after <c>UseGlobalization()</c> when bindings may have subscribed to the default service instance.
    /// </summary>
    internal static void ReconnectToCurrentService() => Instance.EnsureSubscribed(force: true);

    private void EnsureSubscribed(bool force = false)
    {
        var service = GlobalizationServices.Current;
        if (!force && ReferenceEquals(_subscribedService, service))
            return;

        if (_subscribedService is not null)
        {
            _subscribedService.CultureChanged -= OnCultureChanged;
            _subscribedService.TimeZoneChanged -= OnTimeZoneChanged;
        }

        service.CultureChanged += OnCultureChanged;
        service.TimeZoneChanged += OnTimeZoneChanged;
        _subscribedService = service;
    }

    private void OnCultureChanged(object? sender, EventArgs e) => OnPropertyChanged(nameof(Culture));

    private void OnTimeZoneChanged(object? sender, EventArgs e) => OnPropertyChanged(nameof(TimeZone));

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new(propertyName));
}
