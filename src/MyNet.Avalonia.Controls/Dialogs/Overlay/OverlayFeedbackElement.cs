// -----------------------------------------------------------------------
// <copyright file="OverlayFeedbackElement.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public abstract class OverlayFeedbackElement : ContentControl
{
    static OverlayFeedbackElement()
    {
        FocusableProperty.OverrideDefaultValue<OverlayFeedbackElement>(false);
        _ = ClosedEvent.AddClassHandler<OverlayFeedbackElement>((o, _) => o.OnClosed());
    }

    public static readonly StyledProperty<bool> IsClosedProperty = AvaloniaProperty.Register<OverlayFeedbackElement, bool>(nameof(IsClosed), true);

    public static readonly RoutedEvent<ResultEventArgs> ClosedEvent = RoutedEvent.Register<OverlayFeedbackElement, ResultEventArgs>(nameof(Closed), RoutingStrategies.Bubble);

    protected Panel? ContainerPanel { get; set; }

    public bool IsClosed
    {
        get => GetValue(IsClosedProperty);
        set => SetValue(IsClosedProperty, value);
    }

    public event EventHandler<ResultEventArgs> Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        Content = null;
    }

    private void OnClosed() => SetCurrentValue(IsClosedProperty, true);

    [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "Used by children classes")]
    protected virtual void OnElementClosing(object? sender, object? args) => RaiseEvent(new ResultEventArgs(ClosedEvent, args));

    public Task<T?> ShowAsync<T>(CancellationToken? token = null)
    {
        var tcs = new TaskCompletionSource<T?>();
        _ = token?.Register(() => Dispatcher.UIThread.Invoke(Close));

        AddHandler(ClosedEvent, onCloseHandler);
        return tcs.Task;

        void onCloseHandler(object? sender, ResultEventArgs? args)
        {
            if (args?.Result is T result)
                tcs.SetResult(result);
            else
                tcs.SetResult(default);
            RemoveHandler(ClosedEvent, onCloseHandler);
        }
    }

    public abstract void Close();

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        ContainerPanel = this.FindAncestorOfType<Panel>();
    }

    protected internal abstract void AnchorAndUpdatePositionInfo();
}
