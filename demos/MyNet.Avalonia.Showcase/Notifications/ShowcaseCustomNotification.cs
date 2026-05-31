// -----------------------------------------------------------------------
// <copyright file="ShowcaseCustomNotification.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.Notifications.Models;

namespace MyNet.Avalonia.Showcase.Notifications;

/// <summary>
/// Sample notification with custom toast content in the showcase app.
/// </summary>
internal sealed class ShowcaseCustomNotification : NotificationBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ShowcaseCustomNotification"/> class.
    /// </summary>
    public ShowcaseCustomNotification()
        : base(string.Empty, severity: NotificationSeverity.None)
    {
    }
}
