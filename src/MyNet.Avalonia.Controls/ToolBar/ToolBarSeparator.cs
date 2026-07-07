// -----------------------------------------------------------------------
// <copyright file="ToolBarSeparator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Automation;
using Avalonia.Automation.Peers;
using Avalonia.Controls.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130

/// <summary>
/// Visual divider between groups of items in a <see cref="ToolBar"/>.
/// Non-focusable by design — it is a structural element, not an interactive one.
/// </summary>
public class ToolBarSeparator : TemplatedControl
{
    static ToolBarSeparator()
    {
        FocusableProperty.OverrideDefaultValue<ToolBarSeparator>(false);
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<ToolBarSeparator>(AutomationControlType.Separator);
    }
}
