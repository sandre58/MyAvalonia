// -----------------------------------------------------------------------
// <copyright file="OverlayDialogService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Extended.Controls;
using MyNet.UI.Dialogs.ContentDialogs;

namespace MyNet.Avalonia.Extended.Dialogs;

public class OverlayDialogService : ContentDialogServiceBase
{
    private static readonly OverlayDialogOptions DefaultOptions = new();

    public virtual void Show(object view, IDialogViewModel viewModel, string? hostId, OverlayDialogOptions? options = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;

        var dialog = GetOverlayDialog(view, viewModel, options);
        host.AddDialog(dialog);
    }

    /// <inheritdoc />
    public override Task ShowAsync(object view, IDialogViewModel viewModel)
    {
        Show(view, viewModel, null);
        return Task.CompletedTask;
    }

    public virtual Task<bool?> ShowDialogCoreAsync(object view, IDialogViewModel viewModel, string? hostId, OverlayDialogOptions? options = null, CancellationToken? token = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(default(bool?));

        var dialog = GetOverlayDialog(view, viewModel, options);
        host.AddModalDialog(dialog);
        return dialog.ShowAsync<bool?>(token);
    }

    /// <inheritdoc />
    protected override Task<bool?> ShowDialogCoreAsync(object view, IDialogViewModel viewModel) => ShowDialogCoreAsync(view, viewModel, null);

    private OverlayDialog GetOverlayDialog(object view, IDialogViewModel viewModel, OverlayDialogOptions? options)
    {
        var dialog = CreateOverlayDialog();
        var resolvedOptions = MergeOptions(GetOptions(view), options);
        PrepareOverlayDialog(dialog, resolvedOptions);

        dialog.Content = view;
        dialog.DataContext = viewModel;

        // Load view Model on opening control
        dialog.Loaded += onDialogLoaded;

        // ClosePopup control when view Model request
        viewModel.CloseRequest += onViewModelCloseRequest;

        dialog.Closed += onDialogClosed;

        return dialog;

        // Local functions to avoid lambda allocations
        async void onDialogLoaded(object? sender, RoutedEventArgs e)
        {
            if (viewModel is { LoadWhenDialogOpening: true })
                await viewModel.LoadAsync().ConfigureAwait(false);
        }

        void onViewModelCloseRequest(object? sender, EventArgs e) => dialog.Close();

        void onDialogClosed(object? sender, ResultEventArgs e)
        {
            viewModel.CloseRequest -= onViewModelCloseRequest;
            dialog.Loaded -= onDialogLoaded;
            dialog.Closed -= onDialogClosed;
        }
    }

    protected virtual OverlayDialog CreateOverlayDialog() => new()
    {
        [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
    };

    private static OverlayDialogOptions MergeOptions(OverlayDialogOptions baseOptions, OverlayDialogOptions? overrideOptions)
    {
        if (overrideOptions is null) return baseOptions;

        return new()
        {
            FullScreen = overrideOptions.FullScreen,
            HorizontalAnchor = overrideOptions.HorizontalAnchor,
            VerticalAnchor = overrideOptions.VerticalAnchor,
            HorizontalOffset = overrideOptions.HorizontalOffset ?? baseOptions.HorizontalOffset,
            VerticalOffset = overrideOptions.VerticalOffset ?? baseOptions.VerticalOffset,
            Width = overrideOptions.Width ?? baseOptions.Width,
            Height = overrideOptions.Height ?? baseOptions.Height,
            MinWidth = overrideOptions.MinWidth ?? baseOptions.MinWidth,
            MinHeight = overrideOptions.MinHeight ?? baseOptions.MinHeight,
            MaxWidth = overrideOptions.MaxWidth ?? baseOptions.MaxWidth,
            MaxHeight = overrideOptions.MaxHeight ?? baseOptions.MaxHeight,
            Severity = overrideOptions.Severity,
            Buttons = overrideOptions.Buttons,
            Title = overrideOptions.Title ?? baseOptions.Title,
            IsCloseButtonVisible = overrideOptions.IsCloseButtonVisible ?? baseOptions.IsCloseButtonVisible,
            CanLightDismiss = overrideOptions.CanLightDismiss,
            TopLevelHashCode = overrideOptions.TopLevelHashCode ?? baseOptions.TopLevelHashCode,
            CanResize = overrideOptions.CanResize || baseOptions.CanResize,
            StyleClass = overrideOptions.StyleClass ?? baseOptions.StyleClass
        };
    }

    private static OverlayDialogOptions GetOptions(object view) => view is not ContentDialog contentDialog
            ? DefaultOptions
            : new()
            {
                Title = contentDialog.Header switch
                {
                    string str => str,
                    null => null,
                    var header => header.ToString()
                },
                IsCloseButtonVisible = contentDialog.ShowCloseButton,
                CanResize = contentDialog.CanResize
            };

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
        control.HorizontalOffset = control.HorizontalAnchor == HorizontalPosition.Center ? null : options.HorizontalOffset;
        control.VerticalOffset = options.VerticalAnchor == VerticalPosition.Center ? null : options.VerticalOffset;
        control.IsCloseButtonVisible = options.IsCloseButtonVisible;
        control.CanLightDismiss = options.CanLightDismiss;
        control.CanResize = options.CanResize;

        // Apply sizing options
        if (options.Width.HasValue) control.Width = options.Width.Value;
        if (options.Height.HasValue) control.Height = options.Height.Value;
        if (options.MinWidth.HasValue) control.MinWidth = options.MinWidth.Value;
        if (options.MinHeight.HasValue) control.MinHeight = options.MinHeight.Value;
        if (options.MaxWidth.HasValue) control.MaxWidth = options.MaxWidth.Value;
        if (options.MaxHeight.HasValue) control.MaxHeight = options.MaxHeight.Value;

        if (!string.IsNullOrWhiteSpace(options.StyleClass))
        {
            var styles = options.StyleClass!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            control.Classes.AddRange(styles);
        }
    }
}
