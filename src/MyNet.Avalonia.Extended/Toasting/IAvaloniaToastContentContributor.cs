// -----------------------------------------------------------------------
// <copyright file="IAvaloniaToastContentContributor.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using MyNet.UI.Notifications.Models;

namespace MyNet.Avalonia.Extended.Toasting;

/// <summary>
/// Optional toast content resolver registered in dependency injection.
/// </summary>
public interface IAvaloniaToastContentContributor
{
    /// <summary>
    /// Gets the evaluation order. Lower values run first.
    /// </summary>
    int Order { get; }

    /// <summary>
    /// Attempts to create toast content for the given notification.
    /// </summary>
    /// <param name="notification">The notification to render.</param>
    /// <param name="width">Optional preferred width.</param>
    /// <param name="content">The created content when this method returns <see langword="true"/>.</param>
    /// <returns><see langword="true"/> when this contributor handled the notification.</returns>
    bool TryCreateContent(INotification notification, double? width, [NotNullWhen(true)] out object? content);
}
