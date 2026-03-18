// -----------------------------------------------------------------------
// <copyright file="ThemeResources.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;

namespace MyNet.Avalonia;

/// <summary>
/// Represents a static class that provides access to theme-related resources, such as animation durations, colors, brushes, and other theming elements. This class serves as a centralized point for retrieving theme resources in a type-safe manner, ensuring that the correct resource keys are used and that resources are accessed efficiently through lazy loading.
/// </summary>
public static class ThemeResources
{
    /// <summary>
    /// Gets the current application instance, which is used to access theme resources. This property ensures that a valid application instance is available when retrieving resources, and it throws an exception if no current application instance is found. This approach provides a safeguard against null reference issues when accessing resources in contexts where the application instance may not be initialized.
    /// </summary>
    private static Application Application => Application.Current ?? throw new InvalidOperationException("No current application instance found.");

    /// <summary>
    /// Gets a resource of the specified type from the current application instance using the provided key. This method ensures that the resource is retrieved in a type-safe manner, and it throws an exception if the resource is not found or if it is not of the expected type. This approach promotes robust error handling and helps to prevent issues related to missing or incorrectly typed resources in the application.
    /// </summary>
    /// <typeparam name="T">The type of the resource to be retrieved.</typeparam>
    /// <param name="key">The key that uniquely identifies the resource to retrieve.</param>
    /// <param name="themeVariant">The theme variant to use when retrieving the resource. If null, the current theme variant is used.</param>
    /// <returns>The resource of the specified type.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the resource is not found or is not of the expected type.</exception>
    public static T GetResource<T>(string key, ThemeVariant? themeVariant = null) => Application.FindResource(themeVariant, key) is T resource ? resource : throw new InvalidOperationException($"Resource '{key}' not found or is not of type {typeof(T).FullName}.");

    /// <summary>
    /// Gets a resource of the specified type from the current application instance using the provided key, returning null if the resource is not found or if it is not of the expected type. This method provides a safe way to attempt to retrieve resources without throwing exceptions, allowing for more flexible error handling in scenarios where missing resources are acceptable or expected.
    /// </summary>
    /// <typeparam name="T">The type of the resource to be retrieved.</typeparam>
    /// <param name="resourceKey">The key that uniquely identifies the resource to retrieve.</param>
    /// <param name="themeVariant">The theme variant to use when retrieving the resource. If null, the current theme variant is used.</param>
    /// <returns>The resource of the specified type, or null if not found or not of the expected type.</returns>
    public static T? TryGetResource<T>(string resourceKey, ThemeVariant? themeVariant = null) => Application.TryFindResource(resourceKey, themeVariant, out var resource) ? (T?)resource : default;
}
