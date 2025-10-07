// -----------------------------------------------------------------------
// <copyright file="OverlayDialog.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.UI.Controls.Primitives;
using MyNet.UI.Dialogs.CustomDialogs;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class OverlayDialog : OverlayDialogBase
{
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var closeButtonVisible = IsCloseButtonVisible ?? DataContext is IDialogViewModel;
        IsHitTestVisibleProperty.SetValue(closeButtonVisible, CloseButton);
        if (!closeButtonVisible)
        {
            OpacityProperty.SetValue(0, CloseButton);
        }
    }

    public override void Close()
    {
        if (DataContext is IDialogViewModel context)
            context.Close();
        else
            OnElementClosing(this, null);
    }
}
