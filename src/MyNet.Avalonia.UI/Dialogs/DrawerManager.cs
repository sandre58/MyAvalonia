// -----------------------------------------------------------------------
// <copyright file="DrawerManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.UI.Controls;
using MyNet.UI.Dialogs;
using MyNet.UI.Dialogs.MessageBox;
using MyNet.UI.Locators;

namespace MyNet.Avalonia.UI.Dialogs;

public static class DrawerManager
{
    private static IViewResolver? _viewResolver;

    private static IViewLocator? _viewLocator;

    public static void Initialize(IViewResolver viewResolver, IViewLocator viewLocator)
    {
        _viewResolver = viewResolver;
        _viewLocator = viewLocator;
    }

    public static async Task ShowBoxAsync<TView, TViewModel>(TViewModel vm, string? hostId = null, DrawerOptions? options = null)
        where TView : Control, new()
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var drawer = new DrawerBox
        {
            Content = GetView<TView>(),
            DataContext = vm
        };
        ConfigureDrawerBox(drawer, options);
        await host.AddDrawerAsync(drawer).ConfigureAwait(false);
    }

    public static async Task ShowBoxAsync(Control control, object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var drawer = new DrawerBox
        {
            Content = control,
            DataContext = vm
        };
        ConfigureDrawerBox(drawer, options);
        await host.AddDrawerAsync(drawer).ConfigureAwait(false);
    }

    public static async Task ShowBoxAsync(object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var view = GetViewFromViewModel(vm?.GetType());
        view?.DataContext = vm;
        var drawer = new DrawerBox
        {
            Content = view,
            DataContext = vm
        };
        ConfigureDrawerBox(drawer, options);
        await host.AddDrawerAsync(drawer).ConfigureAwait(false);
    }

    public static async Task<MessageBoxResult> ShowBoxModalAsync<TView, TViewModel>(TViewModel vm, string? hostId = null, DrawerOptions? options = null)
        where TView : Control, new()
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return MessageBoxResult.None;
        var drawer = new DrawerBox
        {
            Content = new TView(),
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureDrawerBox(drawer, options);
        await host.AddModalDrawerAsync(drawer).ConfigureAwait(false);
        return await drawer.ShowAsync<MessageBoxResult>().ConfigureAwait(false);
    }

    public static async Task<MessageBoxResult> ShowBoxModalAsync(Control control, object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return MessageBoxResult.None;
        var drawer = new DrawerBox
        {
            Content = control,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureDrawerBox(drawer, options);
        await host.AddModalDrawerAsync(drawer).ConfigureAwait(false);
        return await drawer.ShowAsync<MessageBoxResult>().ConfigureAwait(false);
    }

    public static async Task<MessageBoxResult> ShowBoxModalAsync(object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return MessageBoxResult.None;
        var view = GetViewFromViewModel(vm?.GetType());
        view ??= new ContentControl { Padding = new Thickness(24) };
        view.DataContext = vm;
        var drawer = new DrawerBox
        {
            Content = view,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureDrawerBox(drawer, options);
        await host.AddModalDrawerAsync(drawer).ConfigureAwait(false);
        return await drawer.ShowAsync<MessageBoxResult>().ConfigureAwait(false);
    }

    public static async Task ShowAsync<TView, TViewModel>(TViewModel vm, string? hostId = null, DrawerOptions? options = null)
        where TView : Control, new()
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var dialog = new Drawer
        {
            Content = new TView(),
            DataContext = vm
        };
        ConfigureDrawer(dialog, options);
        await host.AddDrawerAsync(dialog).ConfigureAwait(false);
    }

    public static async Task ShowAsync(Control control, object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var dialog = new Drawer
        {
            Content = control,
            DataContext = vm
        };
        ConfigureDrawer(dialog, options);
        await host.AddDrawerAsync(dialog).ConfigureAwait(false);
    }

    public static async Task ShowAsync(object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var view = GetViewFromViewModel(vm?.GetType());
        view ??= new ContentControl { Padding = new Thickness(24) };
        view.DataContext = vm;
        var dialog = new Drawer
        {
            Content = view,
            DataContext = vm
        };
        ConfigureDrawer(dialog, options);
        await host.AddDrawerAsync(dialog).ConfigureAwait(false);
    }

    public static async Task<TResult?> ShowModalAsync<TView, TViewModel, TResult>(TViewModel vm, string? hostId = null, DrawerOptions? options = null)
        where TView : Control, new()
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return default;
        var dialog = new Drawer
        {
            Content = new TView(),
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureDrawer(dialog, options);
        await host.AddModalDrawerAsync(dialog).ConfigureAwait(false);
        return await dialog.ShowAsync<TResult?>().ConfigureAwait(false);
    }

    public static async Task<TResult?> ShowModalAsync<TResult>(Control control, object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return default;
        var dialog = new Drawer
        {
            Content = control,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureDrawer(dialog, options);
        await host.AddModalDrawerAsync(dialog).ConfigureAwait(false);
        return await dialog.ShowAsync<TResult?>().ConfigureAwait(false);
    }

    public static async Task<TResult?> ShowModalAsync<TResult>(object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return default;
        var view = GetViewFromViewModel(vm?.GetType());
        view ??= new ContentControl();
        view.DataContext = vm;
        var dialog = new Drawer
        {
            Content = view,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureDrawer(dialog, options);
        await host.AddModalDrawerAsync(dialog).ConfigureAwait(false);
        return await dialog.ShowAsync<TResult?>().ConfigureAwait(false);
    }

    private static void ConfigureDrawer(Drawer drawer, DrawerOptions? options)
    {
        options ??= DrawerOptions.Default;
        drawer.Position = options.Position;
        drawer.IsCloseButtonVisible = options.IsCloseButtonVisible;
        drawer.CanLightDismiss = options.CanLightDismiss;
        drawer.CanResize = options.CanResize;
        if (options.Position is Position.Left or Position.Right)
        {
            if (options.MinWidth is not null) drawer.MinWidth = options.MinWidth.Value;
            if (options.MaxWidth is not null) drawer.MaxWidth = options.MaxWidth.Value;
        }

        if (options.Position is Position.Top or Position.Bottom)
        {
            if (options.MinHeight is not null) drawer.MinHeight = options.MinHeight.Value;
            if (options.MaxHeight is not null) drawer.MaxHeight = options.MaxHeight.Value;
        }

        if (!string.IsNullOrWhiteSpace(options.StyleClass))
        {
            var styles = options.StyleClass!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            drawer.Classes.AddRange(styles);
        }
    }

    private static void ConfigureDrawerBox(DrawerBox drawer, DrawerOptions? options)
    {
        options ??= DrawerOptions.Default;
        drawer.Position = options.Position;
        drawer.IsCloseButtonVisible = options.IsCloseButtonVisible;
        drawer.CanLightDismiss = options.CanLightDismiss;
        drawer.Buttons = options.Buttons;
        drawer.Title = options.Title;
        drawer.CanResize = options.CanResize;
        if (options.Position is Position.Left or Position.Right)
        {
            if (options.MinWidth is not null) drawer.MinWidth = options.MinWidth.Value;
            if (options.MaxWidth is not null) drawer.MaxWidth = options.MaxWidth.Value;
        }

        if (options.Position is Position.Top or Position.Bottom)
        {
            if (options.MinHeight is not null) drawer.MinHeight = options.MinHeight.Value;
            if (options.MaxHeight is not null) drawer.MaxHeight = options.MaxHeight.Value;
        }

        if (!string.IsNullOrWhiteSpace(options.StyleClass))
        {
            var styles = options.StyleClass!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            drawer.Classes.AddRange(styles);
        }
    }

    private static StyledElement? GetViewFromViewModel(Type? viewModelType)
    {
        if (viewModelType is null) return new ContentControl();
        var type = _viewResolver?.Resolve(viewModelType);
        return type is null ? throw new InvalidOperationException($"{viewModelType} has not been resolved.") : GetView(type);
    }

    private static StyledElement? GetView<T>() => GetView(typeof(T));

    private static StyledElement? GetView(Type? viewType) => viewType is null ? new ContentControl() : _viewLocator?.Get(viewType) as StyledElement;
}
