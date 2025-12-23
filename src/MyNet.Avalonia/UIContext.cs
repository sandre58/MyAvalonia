// -----------------------------------------------------------------------
// <copyright file="UIContext.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Observable.Globalization;
using MyNet.Utilities.Localization;

namespace MyNet.Avalonia;

/// <summary>
/// Provides global UI context objects for Avalonia applications.
/// </summary>
/// <remarks>
/// This static class exposes shared, observable context objects such as globalization settings
/// that can be used for data binding and localization throughout the UI.
/// </remarks>
public static class UIContext
{
    /// <summary>
    /// Gets the observable globalization context, which exposes the current culture and time zone.
    /// This can be used for data binding to automatically update UI elements when the application's
    /// culture or time zone changes.
    /// </summary>
    public static ObservableGlobalization Globalization { get; } = new(GlobalizationService.Current);
}
