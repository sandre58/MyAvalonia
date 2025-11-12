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
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.UI.Controls;
using MyNet.UI.Dialogs.ContentDialogs;

namespace MyNet.Avalonia.UI.Dialogs;

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
        PrepareWindow(dialog, view as ContentDialog, viewModel);

        dialog.Content = view;
        dialog.DataContext = viewModel;

        if (!string.IsNullOrEmpty(viewModel.Title))
            dialog.Title = viewModel.Title;

        if (view is ContentDialog contentDialog)
        {
            dialog.TitleBarContent = contentDialog.Header;
        }

        // Load view Model on openning control
        dialog.Loaded += OnWindowLoaded;

        // Manage control closing by view Model
        dialog.Closing += OnWindowClosingAsync;

        // Hide Control
        dialog.Closed += OnWindowClosed;

        return dialog;
    }

    protected virtual WindowDialog CreateWindow() => new();

    private void OnWindowLoaded(object? sender, RoutedEventArgs e)
    {
        if ((sender as Window)!.DataContext is IDialogViewModel dialogViewModel && dialogViewModel.LoadWhenDialogOpening)
            dialogViewModel.Load();
    }

    private async void OnWindowClosingAsync(object? sender, WindowClosingEventArgs e)
        => e.Cancel = !await ((sender as Window)!.DataContext as IDialogViewModel)!.CanCloseAsync().ConfigureAwait(false);

    private void OnWindowClosed(object? sender, EventArgs e)
    {
        (sender as Window)!.Loaded -= OnWindowLoaded;
        (sender as Window)!.Closing -= OnWindowClosingAsync;
        (sender as Window)!.Closed -= OnWindowClosed;
    }

    protected virtual void PrepareWindow(WindowDialog window, ContentDialog? content, IDialogViewModel? dialogViewModel)
    {
        window.WindowState = WindowState.Normal;

        if (content is null) return;

        window.WindowStartupLocation = content.StartupLocation;
        window.TitleBarContent = content.Header ?? dialogViewModel?.Title;
        window.Title = dialogViewModel?.Title ?? (content.Header is string ? content.Header.ToString() : null);
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
