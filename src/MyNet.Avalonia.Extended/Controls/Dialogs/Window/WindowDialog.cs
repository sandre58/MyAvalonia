// -----------------------------------------------------------------------
// <copyright file="WindowDialog.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Automation.Peers;
using Avalonia.Controls;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Window shell for content dialogs. Close requests are handled via <see cref="MyNet.UI.Dialogs.ContentDialogs.IDialog.CloseRequested"/> in <see cref="MyNet.Avalonia.Extended.Dialogs.Internal.WindowDialogBuilder"/>.
/// </summary>
public class WindowDialog : Window
{
    static WindowDialog()
    {
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<WindowDialog>(AutomationControlType.Window);
        TitleProperty.Changed.AddClassHandler<WindowDialog, string?>((dialog, _) => UpdateAutomationName(dialog));
    }

    protected override Type StyleKeyOverride { get; } = typeof(WindowDialog);

    /// <summary>
    /// Gets the result passed to the last <see cref="CloseWithResult"/> call.
    /// </summary>
    internal object? LastCloseResult { get; private set; }

    /// <summary>
    /// Closes the window and records <paramref name="result"/> for non-modal presentation.
    /// </summary>
    internal void CloseWithResult(object? result)
    {
        LastCloseResult = result;
        Close(result);
    }

    private static void UpdateAutomationName(WindowDialog dialog) => AutomationProperties.SetName(dialog, dialog.Title ?? string.Empty);
}
