// -----------------------------------------------------------------------
// <copyright file="MessageNotificationControl.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Automation;
using Avalonia.Automation.Peers;
using MyNet.Avalonia.Controls;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Toast and inline notification surface based on <see cref="Banner"/>.
/// </summary>
public sealed class MessageNotificationControl : Banner
{
    static MessageNotificationControl()
    {
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<MessageNotificationControl>(AutomationControlType.Group);
        AutomationProperties.LiveSettingProperty.OverrideDefaultValue<MessageNotificationControl>(AutomationLiveSetting.Polite);
        CanCloseProperty.OverrideDefaultValue<MessageNotificationControl>(false);
        HeaderProperty.Changed.AddClassHandler<MessageNotificationControl, object?>((control, _) => control.UpdateAutomationName());
        ContentProperty.Changed.AddClassHandler<MessageNotificationControl, object?>((control, _) => control.UpdateAutomationName());
    }

    private void UpdateAutomationName()
    {
        var header = Header?.ToString();
        var content = Content?.ToString();

        AutomationProperties.SetName(this, string.IsNullOrEmpty(header) ? content ?? string.Empty : string.IsNullOrEmpty(content) ? header : $"{header}: {content}");
    }
}
