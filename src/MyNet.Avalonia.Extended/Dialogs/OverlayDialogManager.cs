// -----------------------------------------------------------------------
// <copyright file="OverlayDialogManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Extended.Controls;
using MyNet.UI.Locators;

namespace MyNet.Avalonia.Extended.Dialogs;

public static class OverlayDialogManager
{
    private static IViewResolver? _viewResolver;

    private static IViewLocator? _viewLocator;

    public static void Initialize(IViewResolver viewResolver, IViewLocator viewLocator)
    {
        _viewResolver = viewResolver;
        _viewLocator = viewLocator;
    }

    public static void Show<TView, TViewModel>(TViewModel vm, string? hostId = null, OverlayDialogOptions? options = null)
        where TView : Control, new()
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var t = new OverlayDialog
        {
            Content = new TView(),
            DataContext = vm
        };
        ConfigureOverlayDialog(t, options);
        host.AddDialog(t);
    }

    public static void Show(Control control, object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var t = new OverlayDialog
        {
            Content = control,
            DataContext = vm
        };
        ConfigureOverlayDialog(t, options);
        host.AddDialog(t);
    }

    public static void Show(object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var view = GetViewFromViewModel(vm?.GetType());
        view ??= new ContentControl { Padding = new(24) };
        view.DataContext = vm;
        var t = new OverlayDialog
        {
            Content = view,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureOverlayDialog(t, options);
        host.AddDialog(t);
    }

    public static Task<TResult?> ShowModal<TView, TViewModel, TResult>(TViewModel vm, string? hostId = null, OverlayDialogOptions? options = null, CancellationToken? token = null)
        where TView : Control, new()
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(default(TResult));
        var t = new OverlayDialog
        {
            Content = new TView(),
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureOverlayDialog(t, options);
        host.AddModalDialog(t);
        return t.ShowAsync<TResult?>(token);
    }

    public static Task<TResult?> ShowModal<TResult>(Control control, object? vm, string? hostId = null, OverlayDialogOptions? options = null, CancellationToken? token = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(default(TResult));
        var t = new OverlayDialog
        {
            Content = control,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureOverlayDialog(t, options);
        host.AddModalDialog(t);
        return t.ShowAsync<TResult?>(token);
    }

    public static Task<TResult?> ShowModal<TResult>(object? vm, string? hostId = null, OverlayDialogOptions? options = null, CancellationToken? token = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(default(TResult));
        var view = GetViewFromViewModel(vm?.GetType());
        view ??= new ContentControl();
        view.DataContext = vm;
        var t = new OverlayDialog
        {
            Content = view,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureOverlayDialog(t, options);
        host.AddModalDialog(t);
        return t.ShowAsync<TResult?>(token);
    }

    private static void ConfigureOverlayDialog(OverlayDialog control, OverlayDialogOptions options)
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
        control.CanLightDismiss = options.CanLightDismiss;
        control.CanResize = options.CanResize;
        control.IsCloseButtonVisible = options.IsCloseButtonVisible;

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

    internal static T? Recall<T>(string? hostId)
        where T : Control
    {
        var host = OverlayDialogHostManager.GetHost(hostId, null);
        var item = host?.Recall<T>();
        return item;
    }

    private static StyledElement? GetViewFromViewModel(Type? viewModelType)
    {
        if (viewModelType is null) return null;
        var type = _viewResolver?.Resolve(viewModelType);
        return type is null ? throw new InvalidOperationException($"{type} has not been resolved.") : GetView(type);
    }

    private static StyledElement? GetView(Type? viewType) => viewType is null ? null : _viewLocator?.Get(viewType) as StyledElement;
}
