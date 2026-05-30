// -----------------------------------------------------------------------
// <copyright file="ApplicationResources.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;

namespace MyNet.Avalonia.Resources;

/// <summary>
/// Typed accessors for Avalonia application resources.
/// </summary>
public static class ApplicationResources
{
    /// <summary>
    /// Gets the current <see cref="Application"/> when available.
    /// </summary>
    public static Application? CurrentApplication => Application.Current;

    /// <summary>
    /// Gets a resource from the application, or throws when it is missing or has the wrong type.
    /// </summary>
    public static T GetResource<T>(object key, ThemeVariant? themeVariant = null, Application? application = null)
    {
        var app = application ?? Application.Current
            ?? throw new InvalidOperationException("No Avalonia Application is available.");

        return app.FindResource(themeVariant, key) is T resource
            ? resource
            : throw new InvalidOperationException($"Resource '{key}' was not found or is not of type {typeof(T).FullName}.");
    }

    /// <summary>
    /// Tries to get a resource from the application.
    /// </summary>
    public static T? TryGetResource<T>(object key, ThemeVariant? themeVariant = null, Application? application = null)
    {
        var app = application ?? Application.Current;
        return app is not null && app.TryFindResource(key, themeVariant, out var resource) && resource is T typed
            ? typed
            : default;
    }
}
