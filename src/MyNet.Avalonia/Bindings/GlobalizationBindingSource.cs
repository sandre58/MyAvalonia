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

    static GlobalizationBindingSource()
    {
        var service = GlobalizationServices.Current;
        service.CultureChanged += (_, _) => Instance.OnPropertyChanged(nameof(Culture));
        service.TimeZoneChanged += (_, _) => Instance.OnPropertyChanged(nameof(TimeZone));
    }

    private GlobalizationBindingSource() { }

    /// <summary>
    /// Gets the current UI culture.
    /// </summary>
    public CultureInfo Culture => GlobalizationServices.Current.CurrentCulture;

    /// <summary>
    /// Gets the current application time zone.
    /// </summary>
    public TimeZoneInfo TimeZone => GlobalizationServices.Current.CurrentTimeZone;

    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new(propertyName));
}
