// -----------------------------------------------------------------------
// <copyright file="Tag.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Automation.Peers;
using Avalonia.Controls;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class Tag : Label
{
    public static readonly StyledProperty<ICommand?> CloseCommandProperty = AvaloniaProperty.Register<Tag, ICommand?>(nameof(CloseCommand));

    static Tag()
    {
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<Tag>(AutomationControlType.ListItem);
        _ = ContentProperty.Changed.AddClassHandler<Tag>((tag, _) => tag.UpdateAutomationName());
    }

    public Tag() => UpdateAutomationName();

    private void UpdateAutomationName() => AutomationProperties.SetName(this, Content?.ToString() ?? string.Empty);

    public ICommand? CloseCommand
    {
        get => GetValue(CloseCommandProperty);
        set => SetValue(CloseCommandProperty, value);
    }
}
