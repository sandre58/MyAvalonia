// -----------------------------------------------------------------------
// <copyright file="DialogResultMapper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Dialogs.MessageBox;

namespace MyNet.Avalonia.Extended.Dialogs.Internal;

internal static class DialogResultMapper
{
    public static DialogResult<bool> Map(object? result)
        => result switch
        {
            MessageBoxResult.Ok or MessageBoxResult.Yes => DialogResult.Ok(),
            MessageBoxResult.Cancel or MessageBoxResult.No => DialogResult.Cancel(),
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
