// -----------------------------------------------------------------------
// <copyright file="AvaloniaDialogResultMapper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Dialogs.MessageBox;

namespace MyNet.Avalonia.Extended.Dialogs.Internal;

internal static class AvaloniaDialogResultMapper
{
    public static DialogResult<bool> MapBool(object? result)
        => result switch
        {
            true => DialogResult.Ok(),
            false => DialogResult.Cancel(),
            _ => DialogResult.Dismiss()
        };

    public static void ApplyMessageBoxResult(MessageBoxViewModel messageBox, object? result)
    {
        if (result is MessageBoxResult messageBoxResult)
            messageBox.ApplyResult(messageBoxResult);
    }
}
