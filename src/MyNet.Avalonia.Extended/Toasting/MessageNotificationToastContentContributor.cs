// -----------------------------------------------------------------------
// <copyright file="MessageNotificationToastContentContributor.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Extended.Controls;
using MyNet.Avalonia.Theme.Assists;
using MyNet.UI.Notifications.Models;

namespace MyNet.Avalonia.Extended.Toasting;

/// <summary>
/// Renders <see cref="MessageNotification"/> with the themed <see cref="MessageNotificationControl"/>.
/// </summary>
public sealed class MessageNotificationToastContentContributor : IAvaloniaToastContentContributor
{
    /// <inheritdoc />
    public int Order => 100;

    /// <inheritdoc />
    public bool TryCreateContent(INotification notification, double? width, [NotNullWhen(true)] out object? content)
    {
        if (notification is not MessageNotification message)
        {
            content = null;
            return false;
        }

        var control = new MessageNotificationControl
        {
            Header = message.Title,
            Content = message.Message,
            Severity = MapSeverity(message.Severity)
        };

        if (width is > 0)
            control.Width = width.Value;

        content = control;
        return true;
    }

    private static Severity MapSeverity(NotificationSeverity severity) => severity switch
    {
        NotificationSeverity.Success => Severity.Success,
        NotificationSeverity.Warning => Severity.Warning,
        NotificationSeverity.Error => Severity.Error,
        NotificationSeverity.Information => Severity.Information,
        _ => Severity.Custom
    };
}
