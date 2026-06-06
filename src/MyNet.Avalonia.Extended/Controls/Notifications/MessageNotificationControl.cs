// -----------------------------------------------------------------------
// <copyright file="MessageNotificationControl.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Automation;
using Avalonia.Automation.Peers;
using Avalonia.Controls.Primitives;
using MyNet.UI.Notifications.Models;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public sealed class MessageNotificationControl : HeaderedContentControl
{
    static MessageNotificationControl()
    {
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<MessageNotificationControl>(AutomationControlType.Group);
        AutomationProperties.LiveSettingProperty.OverrideDefaultValue<MessageNotificationControl>(AutomationLiveSetting.Polite);
        HeaderProperty.Changed.AddClassHandler<MessageNotificationControl, object?>((control, _) => control.UpdateAutomationName());
        ContentProperty.Changed.AddClassHandler<MessageNotificationControl, object?>((control, _) => control.UpdateAutomationName());
    }

    private void UpdateAutomationName()
    {
        var header = Header?.ToString();
        var content = Content?.ToString();

        AutomationProperties.SetName(this, string.IsNullOrEmpty(header)
            ? content ?? string.Empty
            : string.IsNullOrEmpty(content)
                ? header
                : $"{header}: {content}");
    }

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
