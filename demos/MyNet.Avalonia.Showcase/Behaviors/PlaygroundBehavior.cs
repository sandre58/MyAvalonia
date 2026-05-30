// -----------------------------------------------------------------------
// <copyright file="PlaygroundBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
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

        if (args.NewValue.Value is not { } styleProvider)
            return;

        DisposeSubscription(control);

        // Re-apply whenever any style-affecting property changes on the view model.
        Subscribe(control, styleProvider);

        new StyleRenderer().Apply(control, styleProvider.BuildStyle());

        // Clean up when the control is detached from the visual tree.
        control.DetachedFromVisualTree += onDetached;
        control.AttachedToVisualTree += onAttached;

        void onDetached(object? sender, VisualTreeAttachmentEventArgs e)
        {
            control.DetachedFromVisualTree -= onDetached;
            control.AttachedToVisualTree += onAttached;
            DisposeSubscription(control);
        }

        void onAttached(object? sender, VisualTreeAttachmentEventArgs e)
        {
            control.AttachedToVisualTree -= onAttached;
            control.DetachedFromVisualTree += onDetached;
            Subscribe(control, GetAttachStyleProvider(control));
        }
    }

    private static void DisposeSubscription(Control control)
    {
        if (!Subscriptions.TryGetValue(control, out var sub)) return;

        sub.Dispose();
        Subscriptions.Remove(control);
    }

    private static void Subscribe(Control control, IStyleProvider styleProvider)
    {
        if (Subscriptions.TryGetValue(control, out _)) return;

        var styler = new StyleRenderer();
        var subscription = System.Reactive.Linq.Observable.FromEventPattern<ControlStyle>(
                h => styleProvider.StyleChanged += h,
                h => styleProvider.StyleChanged -= h)
            .Subscribe(x => styler.Apply(control, x.EventArgs));

        Subscriptions.Add(control, subscription);
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
