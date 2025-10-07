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
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.UI.Controls;
using MyNet.Avalonia.UI.Controls.Primitives;
using MyNet.UI.Dialogs;
using MyNet.UI.Dialogs.MessageBox;
using MyNet.UI.Locators;

namespace MyNet.Avalonia.UI.Dialogs;

public static class OverlayDialogManager
{
    private static IViewResolver? _viewResolver;

    private static IViewLocator? _viewLocator;

    public static void Initialize(IViewResolver viewResolver, IViewLocator viewLocator)
    {
        _viewResolver = viewResolver;
        _viewLocator = viewLocator;
    }

    public static void ShowBox<TView, TViewModel>(TViewModel vm, string? hostId = null, OverlayDialogOptions? options = null)
        where TView : Control, new()
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var t = new OverlayDialogBox()
        {
            Content = new TView(),
            DataContext = vm,
        };
        ConfigureOverlayDialogBox(t, options);
        host.AddDialog(t);
    }

    public static void ShowBox(Control control, object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var t = new OverlayDialogBox()
        {
            Content = control,
            DataContext = vm,
        };
        ConfigureOverlayDialogBox(t, options);
        host.AddDialog(t);
    }

    public static void ShowBox(object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var view = GetViewFromViewModel(vm?.GetType());
        view ??= new ContentControl();
        view.DataContext = vm;
        var t = new OverlayDialogBox()
        {
            Content = view,
            DataContext = vm,
        };
        ConfigureOverlayDialogBox(t, options);
        host.AddDialog(t);
    }

    public static void Show<TView, TViewModel>(TViewModel vm, string? hostId = null, OverlayDialogOptions? options = null)
        where TView : Control, new()
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var t = new OverlayDialog()
        {
            Content = new TView(),
            DataContext = vm,
        };
        ConfigureOverlayDialog(t, options);
        host.AddDialog(t);
    }

    public static void Show(Control control, object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var t = new OverlayDialog()
        {
            Content = control,
            DataContext = vm,
        };
        ConfigureOverlayDialog(t, options);
        host.AddDialog(t);
    }

    public static void Show(object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var view = GetViewFromViewModel(vm?.GetType());
        view ??= new ContentControl() { Padding = new Thickness(24) };
        view.DataContext = vm;
        var t = new OverlayDialog()
        {
            Content = view,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureOverlayDialog(t, options);
        host.AddDialog(t);
    }

    public static Task<MessageBoxResult> ShowBoxModal<TView, TViewModel>(TViewModel vm, string? hostId = null, OverlayDialogOptions? options = null, CancellationToken? token = default)
        where TView : Control, new()
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(MessageBoxResult.None);
        var t = new OverlayDialogBox()
        {
            Content = new TView(),
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureOverlayDialogBox(t, options);
        host.AddModalDialog(t);
        return t.ShowAsync<MessageBoxResult>(token);
    }

    public static Task<MessageBoxResult> ShowBoxModal(Control control, object? vm, string? hostId = null, OverlayDialogOptions? options = null, CancellationToken? token = default)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(MessageBoxResult.None);
        var t = new OverlayDialogBox()
        {
            Content = control,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureOverlayDialogBox(t, options);
        host.AddModalDialog(t);
        return t.ShowAsync<MessageBoxResult>(token);
    }

    public static Task<TResult?> ShowModal<TView, TViewModel, TResult>(TViewModel vm, string? hostId = null, OverlayDialogOptions? options = null, CancellationToken? token = default)
        where TView : Control, new()
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(default(TResult));
        var t = new OverlayDialog()
        {
            Content = new TView(),
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureOverlayDialog(t, options);
        host.AddModalDialog(t);
        return t.ShowAsync<TResult?>(token);
    }

    public static Task<TResult?> ShowModal<TResult>(Control control, object? vm, string? hostId = null, OverlayDialogOptions? options = null, CancellationToken? token = default)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(default(TResult));
        var t = new OverlayDialog()
        {
            Content = control,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureOverlayDialog(t, options);
        host.AddModalDialog(t);
        return t.ShowAsync<TResult?>(token);
    }

    public static Task<TResult?> ShowModal<TResult>(object? vm, string? hostId = null, OverlayDialogOptions? options = null, CancellationToken? token = default)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(default(TResult));
        var view = GetViewFromViewModel(vm?.GetType());
        view ??= new ContentControl();
        view.DataContext = vm;
        var t = new OverlayDialog()
        {
            Content = view,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureOverlayDialog(t, options);
        host.AddModalDialog(t);
        return t.ShowAsync<TResult?>(token);
    }

    private static void ConfigureOverlayDialog(OverlayDialog control, OverlayDialogOptions? options)
    {
        options ??= OverlayDialogOptions.Default;
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

    private static void ConfigureOverlayDialogBox(OverlayDialogBox control, OverlayDialogOptions? options)
    {
        options ??= new OverlayDialogOptions();
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
        control.Severity = options.Severity;
        control.Buttons = options.Buttons;
        control.Title = options.Title;
        control.CanLightDismiss = options.CanLightDismiss;
        control.IsCloseButtonVisible = options.IsCloseButtonVisible;
        control.CanResize = options.CanResize;
        if (!string.IsNullOrWhiteSpace(options.StyleClass))
        {
            var styles = options.StyleClass!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            control.Classes.AddRange(styles);
        }

        OverlayDialogBase.SetCanDragMove(control, options.CanDragMove);
    }

    internal static T? Recall<T>(string? hostId)
        where T : Control
    {
        var host = OverlayDialogHostManager.GetHost(hostId, null);
        if (host is null) return null;
        var item = host.Recall<T>();
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
