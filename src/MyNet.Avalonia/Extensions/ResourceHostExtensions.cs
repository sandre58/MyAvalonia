// -----------------------------------------------------------------------
// <copyright file="ResourceHostExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Typed resource lookup on Avalonia resource hosts.
/// </summary>
public static class ResourceHostExtensions
{
    extension(StyledElement host)
    {
        /// <summary>
        /// Gets a resource from the host, or throws when it is missing or has the wrong type.
        /// </summary>
        public T GetResource<T>(object key, ThemeVariant? themeVariant = null) => host.TryGetResource<T>(key, themeVariant) ?? throw new InvalidOperationException($"Resource '{key}' was not found on '{host.GetType().Name}' or is not of type {typeof(T).FullName}.");

        /// <summary>
        /// Tries to get a resource from the host.
        /// </summary>
        public T? TryGetResource<T>(object key, ThemeVariant? themeVariant = null)
            => host.TryFindResource(key, themeVariant, out var resource) && resource is T typed ? typed : default;
    }
}
