// -----------------------------------------------------------------------
// <copyright file="MessageNotificationControl.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls.Primitives;
using MyNet.UI.Notifications;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class MessageNotificationControl : HeaderedContentControl
{
    #region Severity

    /// <summary>
    /// Provides Severity Property.
    /// </summary>
    public static readonly StyledProperty<NotificationSeverity> SeverityProperty = AvaloniaProperty.Register<MessageNotificationControl, NotificationSeverity>(nameof(Severity));

    /// <summary>
    /// Gets or sets the Severity property.
    /// </summary>
    public NotificationSeverity Severity
    {
        get => GetValue(SeverityProperty);
        set => SetValue(SeverityProperty, value);
    }

    #endregion
}
