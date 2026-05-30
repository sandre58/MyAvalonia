// -----------------------------------------------------------------------
// <copyright file="GlobalizationBindingSource.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
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

    private bool _isSubscribed;

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

    private void EnsureSubscribed()
    {
        if (_isSubscribed) return;

        var service = GlobalizationServices.Current;
        service.CultureChanged += OnCultureChanged;
        service.TimeZoneChanged += OnTimeZoneChanged;
        _isSubscribed = true;
    }

    private void OnCultureChanged(object? sender, EventArgs e) => OnPropertyChanged(nameof(Culture));

    private void OnTimeZoneChanged(object? sender, EventArgs e) => OnPropertyChanged(nameof(TimeZone));

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new(propertyName));
}
