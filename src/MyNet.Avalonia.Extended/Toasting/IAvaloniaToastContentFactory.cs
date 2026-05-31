// -----------------------------------------------------------------------
// <copyright file="IAvaloniaToastContentFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.Notifications.Models;

namespace MyNet.Avalonia.Extended.Toasting;

/// <summary>
/// Creates Avalonia visual content for toast notifications.
/// </summary>
public interface IAvaloniaToastContentFactory
{
    /// <summary>
    /// Creates display content for the given notification.
    /// </summary>
    /// <param name="notification">The notification to render.</param>
    /// <param name="width">Optional preferred width.</param>
    /// <returns>Visual content passed to <see cref="Avalonia.Controls.Notifications.WindowNotificationManager"/>.</returns>
    object CreateContent(INotification notification, double? width = null);
}
