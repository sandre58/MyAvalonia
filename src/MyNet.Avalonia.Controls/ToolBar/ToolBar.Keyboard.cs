// -----------------------------------------------------------------------
// <copyright file="ToolBar.Keyboard.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Input;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130

public partial class ToolBar
{
    /// <inheritdoc/>
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Handled) return;

        // Phase 1 — minimal: Escape closes the overflow popup.
        // Full roving focus keyboard navigation is Phase 1.1.
        if (e.Key == Key.Escape && _overflowPopup?.IsOpen == true)
        {
            _overflowPopup.IsOpen = false;
            e.Handled = true;
        }
    }
}
