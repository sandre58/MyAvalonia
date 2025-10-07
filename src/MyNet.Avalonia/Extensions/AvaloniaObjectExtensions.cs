// -----------------------------------------------------------------------
// <copyright file="AvaloniaObjectExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;

namespace MyNet.Avalonia.Extensions;

public static class AvaloniaObjectExtensions
{
    public static ResultDisposable TryBind(this AvaloniaObject obj, AvaloniaProperty property, IBinding? binding)
        => binding == null
            ? new ResultDisposable(Disposable.Empty, result: false)
            : new ResultDisposable(obj.Bind(property, binding), result: true);

    public static void OnLoading<T>(this AvaloniaObject? avaloniaObject, Action<T> onLoadAction, Action<T>? onUnloadAction = null)
            where T : Control
    {
        if (avaloniaObject is not T element) return;

        if (element.IsLoaded)
        {
            onLoadAction(element);
            element.Unloaded -= onUnloaded;
            element.Unloaded += onUnloaded;
        }
        else
        {
            element.Loaded -= onLoaded;
            element.Loaded += onLoaded;
        }

        void onLoaded(object? sender, RoutedEventArgs e)
        {
            onLoadAction(element);
            element.Loaded -= onLoaded;
            element.Unloaded -= onUnloaded;
            element.Unloaded += onUnloaded;
        }

        void onUnloaded(object? sender, RoutedEventArgs e)
        {
            onUnloadAction?.Invoke(element);
            element.Unloaded -= onUnloaded;
            element.Loaded -= onLoaded;
            element.Loaded += onLoaded;
        }
    }
}

public sealed class ResultDisposable(IDisposable? disposable, bool result) : IDisposable
{
    [CompilerGenerated]
    private readonly IDisposable? _disposable = disposable;

    public bool Result { get; } = result;

    public void Dispose() => _disposable?.Dispose();
}
