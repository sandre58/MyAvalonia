// -----------------------------------------------------------------------
// <copyright file="MarkupServiceProvider.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;

namespace MyNet.Avalonia.Markup;

/// <summary>
/// Provides target object and property context for markup extension resolution.
/// </summary>
/// <param name="targetObject">The target object to which the markup extension is being applied.</param>
/// <param name="targetProperty">The target property to which the markup extension is being applied.</param>
public sealed class MarkupServiceProvider(object targetObject, object targetProperty) : IServiceProvider, IProvideValueTarget
{
    /// <summary>
    /// Gets the target object associated with this instance.
    /// </summary>
    public object TargetObject { get; } = targetObject;

    /// <summary>
    /// Gets the target property associated with this instance.
    /// </summary>
    public object TargetProperty { get; } = targetProperty;

    /// <inheritdoc />
    public object? GetService(Type serviceType) => serviceType == typeof(IProvideValueTarget) ? this : null;
}
