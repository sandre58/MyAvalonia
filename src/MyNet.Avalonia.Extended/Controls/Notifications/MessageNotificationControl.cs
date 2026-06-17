// -----------------------------------------------------------------------
// <copyright file="MessageNotificationControl.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Automation;
using Avalonia.Automation.Peers;
using Avalonia.Controls.Metadata;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Chromeless notification body layout (leading icon, title, message) hosted inside <c>NotificationCard</c>.
/// </summary>
/// <remarks>
/// Visual severity (surface, colors, close button) is owned by the toast shell — not this control.
/// </remarks>
[PseudoClasses(PseudoClassName.HeaderEmpty)]
public sealed class MessageNotificationControl : RegionControl
{
    static MessageNotificationControl()
    {
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<MessageNotificationControl>(AutomationControlType.Group);
        AutomationProperties.LiveSettingProperty.OverrideDefaultValue<MessageNotificationControl>(AutomationLiveSetting.Polite);
        HeaderProperty.Changed.AddClassHandler<MessageNotificationControl, object?>((control, _) =>
        {
            control.UpdateHeaderEmptyPseudoClass();
            control.UpdateAutomationName();
        });
        ContentProperty.Changed.AddClassHandler<MessageNotificationControl, object?>((control, _) => control.UpdateAutomationName());
    }

    public MessageNotificationControl() => UpdateHeaderEmptyPseudoClass();

    private void UpdateAutomationName()
    {
        var header = Header?.ToString();
        var content = Content?.ToString();

        AutomationProperties.SetName(this, string.IsNullOrEmpty(header) ? content ?? string.Empty : string.IsNullOrEmpty(content) ? header : $"{header}: {content}");
    }
}
