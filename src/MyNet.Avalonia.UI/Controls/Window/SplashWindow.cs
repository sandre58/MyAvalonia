// -----------------------------------------------------------------------
// <copyright file="SplashWindow.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Threading;
using MyNet.UI.Dialogs.CustomDialogs;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public abstract class SplashWindow : Window
{
    protected override Type StyleKeyOverride => typeof(SplashWindow);

    public static readonly StyledProperty<TimeSpan?> CountDownProperty = AvaloniaProperty.Register<SplashWindow, TimeSpan?>(
        nameof(CountDown));

    public TimeSpan? CountDown
    {
        get => GetValue(CountDownProperty);
        set => SetValue(CountDownProperty, value);
    }

    static SplashWindow() => DataContextProperty.Changed.AddClassHandler<SplashWindow, object?>((window, e) => window.OnDataContextChange(e));

    private void OnDataContextChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if (args.OldValue.Value is IDialogViewModel oldContext) oldContext.CloseRequest -= OnContextRequestClose;

        if (args.NewValue.Value is IDialogViewModel newContext) newContext.CloseRequest += OnContextRequestClose;
    }

    private void OnContextRequestClose(object? sender, object? args)
    {
        DialogResult = args;
        Close();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        if (CountDown != null && CountDown != TimeSpan.Zero)
        {
            _ = DispatcherTimer.RunOnce(Close, CountDown.Value);
        }
    }

    protected object? DialogResult { get; private set; }

    protected virtual Task<bool> CanClose() => Task.FromResult(true);

    protected abstract Task<Window?> CreateNextWindow();

    private bool _canClose;

    protected sealed override async void OnClosing(WindowClosingEventArgs e)
    {
        VerifyAccess();
        if (!_canClose)
        {
            e.Cancel = true;
            _canClose = await CanClose().ConfigureAwait(false);
            if (_canClose)
            {
                var nextWindow = await CreateNextWindow().ConfigureAwait(false);
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime && nextWindow is not null)
                {
                    lifetime.MainWindow = nextWindow;
                }

                nextWindow?.Show();
                Close();
                if (DataContext is IDialogViewModel idc)
                {
                    // unregister in advance in case developer try to raise event again.
                    idc.CloseRequest -= OnContextRequestClose;
                    idc.Close();
                }

                return;
            }
        }

        base.OnClosing(e);
    }
}
