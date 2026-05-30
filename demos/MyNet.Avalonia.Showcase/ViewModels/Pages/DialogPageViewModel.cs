// -----------------------------------------------------------------------
// <copyright file="DialogPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ViewModels.Base;
using MyNet.Avalonia.Showcase.ViewModels.Playground;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class DialogPageViewModel : ShowcaseViewModel
{
    public DialogPageViewModel()
        : base("Dialogs", [new ControlThemeBuilder()])
    {
    }

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.DockWindow;

    /// <summary>
    /// Gets the group view model for Window-based dialogs (Window Dialog and Window MessageBox).
    /// </summary>
    public WindowDialogGroupViewModel WindowGroup { get; } = new();

    /// <summary>
    /// Gets the group view model for Overlay-based dialogs (Overlay Dialog, Overlay MessageBox and Overlay DialogBox).
    /// </summary>
    public OverlayDialogGroupViewModel OverlayGroup { get; } = new();

    /// <summary>
    /// Performs cleanup operations when the view model is disposed.
    /// </summary>
    protected override void Cleanup()
    {
        WindowGroup.Dispose();
        OverlayGroup.Dispose();
        base.Cleanup();
    }
}
