// -----------------------------------------------------------------------
// <copyright file="PlaygroundBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using MyNet.Avalonia.Showcase.ThemeBuilder.Rendering;
using MyNet.Avalonia.Showcase.ViewModels.Playground;

namespace MyNet.Avalonia.Showcase.Behaviors;

internal static class PlaygroundBehavior
{
    private static readonly ConditionalWeakTable<Control, IDisposable> Subscriptions = [];

    static PlaygroundBehavior()
    {
        AttachStyleProviderProperty.Changed.Subscribe(OnAttachStyleProviderChanged);
        AttachStyleProperty.Changed.Subscribe(OnAttachStyleChanged);
    }

    /// <summary>
    /// Identifies the Source attached property. Setting it to a <see cref="PlaygroundViewModel"/>
    /// immediately applies the style state and begins reactive synchronization.
    /// </summary>
    public static readonly AttachedProperty<IStyleProvider?> AttachStyleProviderProperty =
        AvaloniaProperty.RegisterAttached<Control, IStyleProvider?>("AttachStyleProvider", typeof(PlaygroundBehavior));

    public static IStyleProvider? GetAttachStyleProvider(AvaloniaObject element) =>
        element.GetValue(AttachStyleProviderProperty);

    public static void SetAttachStyleProvider(AvaloniaObject element, IStyleProvider? value) =>
        element.SetValue(AttachStyleProviderProperty, value);

    private static void OnAttachStyleProviderChanged(AvaloniaPropertyChangedEventArgs<IStyleProvider?> args)
    {
        if (args.Sender is not Control control)
            return;

        UnregisterVisualTreeHandlers(control);
        DisposeSubscription(control);

        if (args.NewValue.Value is not { } styleProvider)
            return;

        SubscribeStyleProvider(control, styleProvider);
        RegisterVisualTreeHandlers(control);
    }

    private static void SubscribeStyleProvider(Control control, IStyleProvider styleProvider)
    {
        DisposeSubscription(control);

        var styler = new StyleRenderer();
        styler.Apply(control, styleProvider.BuildStyle());

        var subscription = System.Reactive.Linq.Observable.FromEventPattern<ControlStyle>(
                h => styleProvider.StyleChanged += h,
                h => styleProvider.StyleChanged -= h)
            .Subscribe(x => styler.Apply(control, x.EventArgs));

        Subscriptions.Add(control, new CompositeDisposable(subscription, styler));
    }

    private static void RegisterVisualTreeHandlers(Control control)
    {
        control.DetachedFromVisualTree -= OnControlDetachedFromVisualTree;
        control.AttachedToVisualTree -= OnControlAttachedToVisualTree;
        control.DetachedFromVisualTree += OnControlDetachedFromVisualTree;
        control.AttachedToVisualTree += OnControlAttachedToVisualTree;
    }

    private static void UnregisterVisualTreeHandlers(Control control)
    {
        control.DetachedFromVisualTree -= OnControlDetachedFromVisualTree;
        control.AttachedToVisualTree -= OnControlAttachedToVisualTree;
    }

    private static void OnControlDetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is not Control control)
            return;

        DisposeSubscription(control);
    }

    private static void OnControlAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is not Control control)
            return;

        if (GetAttachStyleProvider(control) is { } provider)
            SubscribeStyleProvider(control, provider);
    }

    private static void DisposeSubscription(Control control)
    {
        if (!Subscriptions.TryGetValue(control, out var sub)) return;

        sub.Dispose();
        Subscriptions.Remove(control);
    }

    #region AttachStyle

    /// <summary>
    /// Provides AttachStyle Property for attached PlaygroundBehavior element.
    /// </summary>
    public static readonly AttachedProperty<ControlStyle> AttachStyleProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, ControlStyle>("AttachStyle", typeof(PlaygroundBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="AttachStyleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="AttachStyleProperty"/>.</param>
    public static void SetAttachStyle(StyledElement element, ControlStyle value) =>
        element.SetValue(AttachStyleProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="AttachStyleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static ControlStyle GetAttachStyle(StyledElement element) => element.GetValue(AttachStyleProperty);

    private static void OnAttachStyleChanged(AvaloniaPropertyChangedEventArgs<ControlStyle> args)
    {
        if (args.Sender is not Control control)
            return;

        if (args.NewValue.Value is not { } style)
            return;

        var styler = new StyleRenderer();
        styler.Apply(control, style);
    }

    #endregion
}
