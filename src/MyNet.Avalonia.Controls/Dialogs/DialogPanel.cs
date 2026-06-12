// -----------------------------------------------------------------------
// <copyright file="DialogPanel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Automation;
using MyNet.Avalonia.Controls.Primitives;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130

/// <summary>
/// Standard dialog body layout: optional header band, leading slot, content, and actions.
/// </summary>
/// <remarks>
/// Use <see cref="ContentDialog"/> for modal dialogs. Compose <see cref="DialogPanel"/> directly
/// when embedding dialog layout inside another control (for example a message box preset).
/// </remarks>
public class DialogPanel : RegionControl
{
    static DialogPanel() => HeaderProperty.Changed.AddClassHandler<DialogPanel, object?>((panel, _) => UpdateAutomationName(panel));

    private static void UpdateAutomationName(DialogPanel panel) => AutomationProperties.SetName(panel, panel.Header?.ToString() ?? string.Empty);
}
