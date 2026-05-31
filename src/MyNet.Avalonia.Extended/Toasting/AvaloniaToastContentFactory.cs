// -----------------------------------------------------------------------
// <copyright file="AvaloniaToastContentFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Media;
using MyNet.UI.Notifications.Models;

namespace MyNet.Avalonia.Extended.Toasting;

/// <summary>
/// Builds Avalonia visual content for toast notifications.
/// </summary>
public static class AvaloniaToastContentFactory
{
    /// <summary>
    /// Gets or sets optional host-specific content resolver checked before the built-in templates.
    /// </summary>
    public static Func<INotification, Control?>? CustomContentFactory { get; set; }

    /// <summary>
    /// Creates display content for the given notification.
    /// </summary>
    /// <param name="notification">The notification to render.</param>
    /// <param name="width">Optional preferred width.</param>
    /// <returns>Visual content passed to <see cref="Avalonia.Controls.Notifications.WindowNotificationManager"/>.</returns>
    public static object Create(INotification notification, double? width = null)
    {
        ArgumentNullException.ThrowIfNull(notification);

        return CustomContentFactory?.Invoke(notification) ?? (object)(notification is MessageNotification message
            ? CreateMessageContent(message, width)
            : new ContentControl
            {
                Content = notification
            });
    }

    private static StackPanel CreateMessageContent(MessageNotification message, double? width)
    {
        var panel = new StackPanel
        {
            Spacing = 4
        };

        if (width is > 0)
            panel.Width = width.Value;

        if (!string.IsNullOrWhiteSpace(message.Title))
        {
            panel.Children.Add(new TextBlock
            {
                Text = message.Title,
                FontWeight = FontWeight.SemiBold,
                TextWrapping = TextWrapping.Wrap
            });
        }

        panel.Children.Add(new TextBlock
        {
            Text = message.Message,
            TextWrapping = TextWrapping.Wrap
        });

        return panel;
    }
}
