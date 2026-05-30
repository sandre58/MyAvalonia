// -----------------------------------------------------------------------
// <copyright file="DialogButtonHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using MyNet.UI.Dialogs.MessageBox;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Provides shared helper methods for dialog button visibility and default close result logic.
/// </summary>
internal static class DialogButtonHelper
{
    /// <summary>
    /// Sets button visibility based on the specified <see cref="MessageBoxResultOption"/>.
    /// </summary>
    public static void SetButtonVisibility(MessageBoxResultOption buttons, Button? okButton, Button? cancelButton, Button? yesButton, Button? noButton)
    {
        switch (buttons)
        {
            case MessageBoxResultOption.None:
                okButton?.SetValue(Visual.IsVisibleProperty, false);
                cancelButton?.SetValue(Visual.IsVisibleProperty, false);
                yesButton?.SetValue(Visual.IsVisibleProperty, false);
                noButton?.SetValue(Visual.IsVisibleProperty, false);
                break;
            case MessageBoxResultOption.Ok:
                okButton?.SetValue(Visual.IsVisibleProperty, true);
                cancelButton?.SetValue(Visual.IsVisibleProperty, false);
                yesButton?.SetValue(Visual.IsVisibleProperty, false);
                noButton?.SetValue(Visual.IsVisibleProperty, false);
                break;
            case MessageBoxResultOption.OkCancel:
                okButton?.SetValue(Visual.IsVisibleProperty, true);
                cancelButton?.SetValue(Visual.IsVisibleProperty, true);
                yesButton?.SetValue(Visual.IsVisibleProperty, false);
                noButton?.SetValue(Visual.IsVisibleProperty, false);
                break;
            case MessageBoxResultOption.YesNo:
                okButton?.SetValue(Visual.IsVisibleProperty, false);
                cancelButton?.SetValue(Visual.IsVisibleProperty, false);
                yesButton?.SetValue(Visual.IsVisibleProperty, true);
                noButton?.SetValue(Visual.IsVisibleProperty, true);
                break;
            case MessageBoxResultOption.YesNoCancel:
                okButton?.SetValue(Visual.IsVisibleProperty, false);
                cancelButton?.SetValue(Visual.IsVisibleProperty, true);
                yesButton?.SetValue(Visual.IsVisibleProperty, true);
                noButton?.SetValue(Visual.IsVisibleProperty, true);
                break;
        }
    }

    /// <summary>
    /// Gets the default close result for when the dialog is dismissed (close button or escape key).
    /// </summary>
    public static MessageBoxResult GetDefaultCloseResult(MessageBoxResultOption buttons) => buttons switch
    {
        MessageBoxResultOption.None => MessageBoxResult.None,
        MessageBoxResultOption.Ok => MessageBoxResult.Ok,
        MessageBoxResultOption.OkCancel => MessageBoxResult.Cancel,
        MessageBoxResultOption.YesNo => MessageBoxResult.No,
        MessageBoxResultOption.YesNoCancel => MessageBoxResult.Cancel,
        _ => MessageBoxResult.None
    };
}
