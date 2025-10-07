// -----------------------------------------------------------------------
// <copyright file="CaptionButtons.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Extensions;
using CaptionButtonsBase = Avalonia.Controls.Chrome.CaptionButtons;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartCloseButton, typeof(Button))]
[TemplatePart(PartRestoreButton, typeof(Button))]
[TemplatePart(PartMinimizeButton, typeof(Button))]
[TemplatePart(PartFullScreenButton, typeof(Button))]
[PseudoClasses(":minimized", ":normal", ":maximized", ":fullscreen")]
public class CaptionButtons : CaptionButtonsBase
{
    private const string PartCloseButton = "PART_CloseButton";
    private const string PartRestoreButton = "PART_RestoreButton";
    private const string PartMinimizeButton = "PART_MinimizeButton";
    private const string PartFullScreenButton = "PART_FullScreenButton";

    private Button? _closeButton;
    private Button? _restoreButton;
    private Button? _minimizeButton;
    private Button? _fullScreenButton;

    private IDisposable? _windowStateSubscription;
    private IDisposable? _fullScreenSubscription;
    private IDisposable? _minimizeSubscription;
    private IDisposable? _restoreSubscription;
    private IDisposable? _closeSubscription;

    private WindowState? _oldWindowState;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _closeButton = e.NameScope.Get<Button>(PartCloseButton);
        _restoreButton = e.NameScope.Get<Button>(PartRestoreButton);
        _minimizeButton = e.NameScope.Get<Button>(PartMinimizeButton);
        _fullScreenButton = e.NameScope.Get<Button>(PartFullScreenButton);
        Button.ClickEvent.AddHandler((_, _) => OnClose(), _closeButton);
        Button.ClickEvent.AddHandler((_, _) => OnRestore(), _restoreButton);
        Button.ClickEvent.AddHandler((_, _) => OnMinimize(), _minimizeButton);
        Button.ClickEvent.AddHandler((_, _) => OnToggleFullScreen(), _fullScreenButton);

        _ = Window.WindowStateProperty.Changed.AddClassHandler<Window, WindowState>(WindowStateChanged);
        if (HostWindow?.CanResize == false)
        {
            _restoreButton.IsEnabled = false;
        }

        UpdateVisibility();
    }

    private void WindowStateChanged(Window window, AvaloniaPropertyChangedEventArgs<WindowState> e)
    {
        if (window != HostWindow) return;
        if (e.NewValue.Value == WindowState.FullScreen)
        {
            _oldWindowState = e.OldValue.Value;
        }
    }

    protected override void OnToggleFullScreen() => HostWindow?.WindowState = HostWindow.WindowState != WindowState.FullScreen
                ? WindowState.FullScreen
                : _oldWindowState ?? WindowState.Normal;

    public override void Attach(Window? hostWindow)
    {
        if (hostWindow is null) return;
        base.Attach(hostWindow);
        _windowStateSubscription = HostWindow?.GetObservable(Window.WindowStateProperty).Subscribe(_ => UpdateVisibility());
        _fullScreenSubscription = HostWindow?.GetObservable(ExtendedWindow.IsFullScreenButtonVisibleProperty).Subscribe(_ => UpdateVisibility());
        _minimizeSubscription = HostWindow?.GetObservable(ExtendedWindow.IsMinimizeButtonVisibleProperty).Subscribe(_ => UpdateVisibility());
        _restoreSubscription = HostWindow?.GetObservable(ExtendedWindow.IsRestoreButtonVisibleProperty).Subscribe(_ => UpdateVisibility());
        _closeSubscription = HostWindow?.GetObservable(ExtendedWindow.IsCloseButtonVisibleProperty).Subscribe(_ => UpdateVisibility());
    }

    private void UpdateVisibility()
    {
        if (HostWindow is not ExtendedWindow u)
        {
            return;
        }

        IsVisibleProperty.SetValue(u.IsCloseButtonVisible, _closeButton);
        IsVisibleProperty.SetValue(u.WindowState != WindowState.FullScreen && u.IsRestoreButtonVisible,
            _restoreButton);
        IsVisibleProperty.SetValue(u.WindowState != WindowState.FullScreen && u.IsMinimizeButtonVisible,
            _minimizeButton);
        IsVisibleProperty.SetValue(u.IsFullScreenButtonVisible, _fullScreenButton);
    }

    public override void Detach()
    {
        base.Detach();
        _windowStateSubscription?.Dispose();
        _fullScreenSubscription?.Dispose();
        _minimizeSubscription?.Dispose();
        _restoreSubscription?.Dispose();
        _closeSubscription?.Dispose();
    }
}
