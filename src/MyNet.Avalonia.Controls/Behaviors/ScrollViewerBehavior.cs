// -----------------------------------------------------------------------
// <copyright file="ScrollViewerBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using MyNet.Primitives;

namespace MyNet.Avalonia.Controls.Behaviors;

public static class ScrollViewerBehavior
{
    static ScrollViewerBehavior() => RefreshOnScrollProperty.Changed.Subscribe(RefreshOnScrollChangedCallback);

    #region RefreshOnScroll

    /// <summary>
    /// Provides RefreshOnScroll Property for attached ScrollViewerBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> RefreshOnScrollProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("RefreshOnScroll", typeof(ScrollViewerBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="RefreshOnScrollProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="RefreshOnScrollProperty"/>.</param>
    public static void SetRefreshOnScroll(StyledElement element, bool value) => element.SetValue(RefreshOnScrollProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="RefreshOnScrollProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetRefreshOnScroll(StyledElement element) => element.GetValue(RefreshOnScrollProperty);

    private static void RefreshOnScrollChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not ScrollViewer scrollViewer) return;

        var refreshContainer = scrollViewer.FindAncestorOfType<RefreshContainer>() ?? ((Visual?)scrollViewer.TemplatedParent)?.FindAncestorOfType<RefreshContainer>();

        if (refreshContainer is null) return;

        if (((bool?)args.NewValue).IsTrue())
        {
            scrollViewer.AddHandler(ScrollViewer.ScrollChangedEvent, onScrollChanged, RoutingStrategies.Bubble);
        }
        else
        {
            scrollViewer.RemoveHandler(ScrollViewer.ScrollChangedEvent, onScrollChanged);
        }

        void onScrollChanged(object? sender, ScrollChangedEventArgs e)
        {
            var canRefresh = refreshContainer.PullDirection switch
            {
                PullDirection.TopToBottom => scrollViewer.Offset.Y >= scrollViewer.Extent.Height - scrollViewer.Viewport.Height,
                PullDirection.BottomToTop => scrollViewer.Offset.Y <= 0,
                PullDirection.LeftToRight => scrollViewer.Offset.X >= scrollViewer.Extent.Width - scrollViewer.Viewport.Width,
                PullDirection.RightToLeft => scrollViewer.Offset.X <= 0,
                _ => false
            };

            if (canRefresh)
            {
                refreshContainer.RequestRefresh();
            }
        }
    }

    #endregion
}
