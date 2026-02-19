// -----------------------------------------------------------------------
// <copyright file="WindowDialogService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using MyNet.Avalonia.Extended.Controls;
using MyNet.Avalonia.Extensions;
using MyNet.UI.Dialogs.ContentDialogs;

namespace MyNet.Avalonia.Extended.Dialogs;

public class WindowDialogService : ContentDialogServiceBase
{
    /// <inheritdoc />
    public override Task ShowAsync(object view, IDialogViewModel viewModel)
    {
        var window = GetWindow(view, viewModel);
        var owner = GetMainWindow();

        if (owner is null)
        {
            window.Show();
        }
        else
        {
            window.Icon = owner.Icon;
            window.Show(owner);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override Task<bool?> ShowDialogCoreAsync(object view, IDialogViewModel viewModel)
    {
        var window = GetWindow(view, viewModel);
        var owner = GetMainWindow();

        if (owner is null)
        {
            window.Show();
            return Task.FromResult((bool?)null);
        }

        window.Icon = owner.Icon;
        return window.ShowDialog<bool?>(owner);
    }

    private static Window? GetMainWindow()
    {
        var lifetime = Application.Current?.ApplicationLifetime;
        return lifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } w } ? w : null;
    }

    private WindowDialog GetWindow(object view, IDialogViewModel viewModel)
    {
        var dialog = CreateWindow();
        var contentDialog = view as ContentDialog;

        PrepareWindow(dialog, contentDialog, viewModel);

        dialog.Content = view;
        dialog.DataContext = viewModel;

        if (!string.IsNullOrEmpty(viewModel.Title))
            dialog.Title = viewModel.Title;

        if (contentDialog is not null)
        {
            dialog.TitleBarContent = contentDialog.Header;
        }

        // Load view Model on opening control
        dialog.Loaded += onWindowLoaded;

        // Manage control closing by view Model
        dialog.Closing += onWindowClosingAsync;

        // Hide Control
        dialog.Closed += onWindowClosed;

        return dialog;

        // Local functions to avoid lambda allocations and improve performance
        async void onWindowLoaded(object? sender, RoutedEventArgs e)
        {
            if (sender is Window { DataContext: IDialogViewModel { LoadWhenDialogOpening: true } dialogVm })
                await dialogVm.LoadAsync().ConfigureAwait(false);
        }

        async void onWindowClosingAsync(object? sender, WindowClosingEventArgs e)
        {
            if (sender is Window { DataContext: IDialogViewModel dialogVm })
                e.Cancel = !await dialogVm.CanCloseAsync().ConfigureAwait(false);
        }

        void onWindowClosed(object? sender, EventArgs e)
        {
            if (sender is not Window window) return;

            window.Loaded -= onWindowLoaded;
            window.Closing -= onWindowClosingAsync;
            window.Closed -= onWindowClosed;
        }
    }

    protected virtual WindowDialog CreateWindow() => new();

    protected virtual void PrepareWindow(WindowDialog window, ContentDialog? content, IDialogViewModel? dialogViewModel)
    {
        window.WindowState = WindowState.Normal;

        if (content is null) return;

        window.WindowStartupLocation = content.StartupLocation;
        window.TitleBarContent = content.Header ?? dialogViewModel?.Title;

        // Optimize ToString() call - avoid boxing
        window.Title = dialogViewModel?.Title ?? content.Header switch
        {
            string str => str,
            null => null,
            var header => header.ToString()
        };

        window.IsCloseButtonVisible = content.ShowCloseButton;
        window.CanDragMove = content.CanDragMove;
        window.CanResize = content.CanResize;
        window.IsManagedResizerVisible = content.CanResize;
        window.ShowInTaskbar = content.ShowInTaskBar;

        if (content.StartupLocation == WindowStartupLocation.Manual)
        {
            if (content.Position is not null)
                window.Position = content.Position.Value;
            else
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        if (!string.IsNullOrWhiteSpace(content.ParentClasses))
        {
            window.AddClasses(content.ParentClasses);
        }
    }
}
