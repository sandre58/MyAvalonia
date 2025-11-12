// -----------------------------------------------------------------------
// <copyright file="OverlayDialogService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.UI.Controls;
using MyNet.Avalonia.UI.Controls.Primitives;
using MyNet.UI.Dialogs.ContentDialogs;

namespace MyNet.Avalonia.UI.Dialogs;

public class OverlayDialogService : ContentDialogServiceBase
{
    public virtual void Show(object view, IDialogViewModel viewModel, string? hostId, OverlayDialogOptions? options = null, CancellationToken? token = default)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var dialog = GetOverlayDialog(view, viewModel);

        host.AddDialog(dialog);
    }

    /// <inheritdoc />
    public override Task ShowAsync(object view, IDialogViewModel viewModel)
    {
        Show(view, viewModel, null);

        return Task.CompletedTask;
    }

    public virtual Task<bool?> ShowDialogCoreAsync(object view, IDialogViewModel viewModel, string? hostId, OverlayDialogOptions? options = null, CancellationToken? token = default)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(default(bool?));
        var dialog = GetOverlayDialog(view, viewModel);

        host.AddModalDialog(dialog);
        return dialog.ShowAsync<bool?>(token);
    }

    /// <inheritdoc />
    protected override Task<bool?> ShowDialogCoreAsync(object view, IDialogViewModel viewModel) => ShowDialogCoreAsync(view, viewModel, null);

    private OverlayDialog GetOverlayDialog(object view, IDialogViewModel viewModel)
    {
        var dialog = CreateOverlayDialog();
        PrepareOverlayDialog(dialog, GetOptions(view));

        dialog.Content = view;
        dialog.DataContext = viewModel;

        // Load view Model on openning control
        dialog.Loaded += OnDialogLoaded;

        // Close control when view Model request
        viewModel.CloseRequest += (sender, e) => dialog.Close();

        // Hide Control
        dialog.Closed += OnDialogClosed;

        return dialog;
    }

    protected virtual OverlayDialog CreateOverlayDialog() => new()
    {
        [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
    };

    private static OverlayDialogOptions GetOptions(object view) => view is ContentDialog contentDialog
            ? new OverlayDialogOptions
            {
                Title = contentDialog.Header?.ToString(),
                IsCloseButtonVisible = contentDialog.ShowCloseButton,
                CanDragMove = contentDialog.CanDragMove,
                CanResize = contentDialog.CanResize
            }
            : new OverlayDialogOptions();

    private void OnDialogLoaded(object? sender, RoutedEventArgs e)
    {
        if ((sender as OverlayDialog)!.DataContext is IDialogViewModel { LoadWhenDialogOpening: true } dialogViewModel)
            dialogViewModel.Load();
    }

    private void OnDialogClosed(object? sender, EventArgs e)
    {
        (sender as Window)!.Loaded -= OnDialogLoaded;
        (sender as Window)!.Closed -= OnDialogClosed;
    }

    protected virtual void PrepareOverlayDialog(OverlayDialog control, OverlayDialogOptions options)
    {
        control.IsFullScreen = options.FullScreen;
        if (options.FullScreen)
        {
            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            control.VerticalAlignment = VerticalAlignment.Stretch;
        }

        control.HorizontalAnchor = options.HorizontalAnchor;
        control.VerticalAnchor = options.VerticalAnchor;
        control.ActualHorizontalAnchor = options.HorizontalAnchor;
        control.ActualVerticalAnchor = options.VerticalAnchor;
        control.HorizontalOffset =
            control.HorizontalAnchor == HorizontalPosition.Center ? null : options.HorizontalOffset;
        control.VerticalOffset =
            options.VerticalAnchor == VerticalPosition.Center ? null : options.VerticalOffset;
        control.IsCloseButtonVisible = options.IsCloseButtonVisible;
        control.CanLightDismiss = options.CanLightDismiss;
        control.CanResize = options.CanResize;
        if (!string.IsNullOrWhiteSpace(options.StyleClass))
        {
            var styles = options.StyleClass!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            control.Classes.AddRange(styles);
        }

        OverlayDialogBase.SetCanDragMove(control, options.CanDragMove);
    }
}
