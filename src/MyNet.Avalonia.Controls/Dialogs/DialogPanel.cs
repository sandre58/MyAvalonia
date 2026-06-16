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
/// Standard dialog body layout: optional header band, leading slot, content, and actions.
/// </summary>
/// <remarks>
/// <para><see cref="RegionControl.Header"/> is the dialog title. Hide the internal header band with
/// <c>HeaderAssist.IsVisible="False"</c> when a window or overlay shell already displays the title.</para>
/// <para>Use <see cref="ContentDialog"/> for modal dialogs. Compose <see cref="DialogPanel"/> directly
/// when embedding dialog layout inside another control (for example a message box preset).</para>
/// </remarks>
[PseudoClasses(PseudoClassName.HeaderEmpty)]
public class DialogPanel : RegionControl
{
    static DialogPanel()
    {
        HeaderProperty.Changed.AddClassHandler<DialogPanel, object?>((panel, _) =>
        {
            UpdateAutomationName(panel);
            panel.UpdateHeaderEmptyPseudoClass();
        });
    }

    public DialogPanel() => UpdateHeaderEmptyPseudoClass();

    private static void UpdateAutomationName(DialogPanel panel) => AutomationProperties.SetName(panel, panel.Header?.ToString() ?? string.Empty);
}
