// -----------------------------------------------------------------------
// <copyright file="DialogPanel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls.Metadata;
using MyNet.Avalonia.Controls.Primitives;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130

/// <summary>
/// Layout primitive for dialog regions (header, leading, content, actions).
/// </summary>
/// <remarks>
/// <para>No default theme — use <see cref="ContentDialog"/> or the Extended
/// <c>MessageBoxContent</c> control for themed templates.</para>
/// <para><see cref="RegionControl.Header"/> is the dialog title. Hide the header band with
/// <c>HeaderAssist.IsVisible="False"</c> when a window shell shows the native title bar.</para>
/// </remarks>
[PseudoClasses(PseudoClassName.HeaderEmpty)]
public class DialogPanel : RegionControl
{
    static DialogPanel() => HeaderProperty.Changed.AddClassHandler<DialogPanel, object?>((panel, _) =>
    {
        UpdateAutomationName(panel);
        panel.UpdateHeaderEmptyPseudoClass();
    });

    public DialogPanel() => UpdateHeaderEmptyPseudoClass();

    private static void UpdateAutomationName(DialogPanel panel) => AutomationProperties.SetName(panel, panel.Header?.ToString() ?? string.Empty);
}
