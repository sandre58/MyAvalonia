// -----------------------------------------------------------------------
// <copyright file="MarkupServiceProvider.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;

namespace MyNet.Avalonia.Helpers;

/// <summary>
/// Represents a service provider that provides the target object and property for a markup extension. This class is used to provide the necessary context for a markup extension to resolve its value when it is being applied to a property in XAML. It implements the IProvideValueTarget interface, which allows it to provide the target object and property to the markup extension when requested.
/// </summary>
/// <param name="targetObject">The target object to which the markup extension is being applied.</param>
/// <param name="targetProperty">The target property to which the markup extension is being applied.</param>
public sealed class MarkupServiceProvider(object targetObject, object targetProperty) : IServiceProvider, IProvideValueTarget
{
    /// <summary>
    /// Gets the target object associated with this instance.
    /// </summary>
    /// <remarks>This property provides access to the underlying object that this instance is designed to
    /// interact with. The target object is set during initialization and cannot be modified afterward.</remarks>
    public object TargetObject { get; } = targetObject;

    /// <summary>
    /// Gets the target property associated with this instance.
    /// </summary>
    /// <remarks>This property provides access to the underlying property that this instance is designed to
    /// interact with. The target property is set during initialization and cannot be modified afterward.</remarks>
    public object TargetProperty { get; } = targetProperty;

    /// <summary>
    /// Retrieves a service object of the specified type, if available.
    /// </summary>
    /// <param name="serviceType">The type of service to retrieve. If the type is <see cref="IProvideValueTarget"/>, the current instance is
    /// returned.</param>
    /// <returns>The current instance if <paramref name="serviceType"/> is <see cref="IProvideValueTarget"/>; otherwise, <see
    /// langword="null"/>.</returns>
    public object? GetService(Type serviceType) => serviceType == typeof(IProvideValueTarget) ? this : (object?)null;
}
