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
using MyNet.UI.Dialogs.CustomDialogs;

namespace MyNet.Avalonia.UI.Dialogs;

public class WindowDialogService : DialogServiceBase
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
        PrepareWindow(dialog, GetOptions(view));

        dialog.Content = view;
        dialog.DataContext = viewModel;

        if (!string.IsNullOrEmpty(viewModel.Title))
            dialog.Title = viewModel.Title;

        // Load view Model on openning control
        dialog.Loaded += OnWindowLoaded;

        // Manage control closing by view Model
        dialog.Closing += OnWindowClosingAsync;

        // Hide Control
        dialog.Closed += OnWindowClosed;

        return dialog;
    }

    protected virtual WindowDialog CreateWindow() => new();

    private static WindowDialogOptions GetOptions(object view) => view is ContentDialog contentDialog
            ? new WindowDialogOptions
            {
                Title = contentDialog.Header?.ToString(),
                StartupLocation = contentDialog.StartupLocation,
                IsCloseButtonVisible = contentDialog.ShowCloseButton,
                CanDragMove = contentDialog.CanDragMove,
                CanResize = contentDialog.CanResize,
                ShowInTaskBar = contentDialog.ShowInTaskBar,
                Position = contentDialog.Position,
                Classes = contentDialog.ParentClasses
            }
            : new WindowDialogOptions();

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

    protected virtual void PrepareWindow(WindowDialog window, WindowDialogOptions options)
    {
        window.WindowStartupLocation = options.StartupLocation;
        window.Title = options.Title;
        window.IsCloseButtonVisible = options.IsCloseButtonVisible;
        window.WindowState = WindowState.Normal;
        window.CanDragMove = options.CanDragMove;
        window.CanResize = options.CanResize;
        window.IsManagedResizerVisible = options.CanResize;
        window.ShowInTaskbar = options.ShowInTaskBar;

        if (options.StartupLocation == WindowStartupLocation.Manual)
        {
            if (options.Position is not null)
                window.Position = options.Position.Value;
            else
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        if (!string.IsNullOrWhiteSpace(options.Classes))
        {
            window.AddClasses(options.Classes);
        }
    }
}
