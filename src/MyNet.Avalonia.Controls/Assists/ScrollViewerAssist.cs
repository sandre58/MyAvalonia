// -----------------------------------------------------------------------
// <copyright file="ScrollViewerAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using MyNet.Utilities;

namespace MyNet.Avalonia.Controls.Assists;

public static class ScrollViewerAssist
{
    static ScrollViewerAssist() => RefreshOnScrollProperty.Changed.Subscribe(RefreshOnScrollChangedCallback);

    #region ButtonsIsVisible

    /// <summary>
    /// Provides ButtonsIsVisible Property for attached ScrollBarAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> ButtonsIsVisibleProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("ButtonsIsVisible", typeof(ScrollViewerAssist), true);

    /// <summary>
    /// Accessor for Attached  <see cref="ButtonsIsVisibleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ButtonsIsVisibleProperty"/>.</param>
    public static void SetButtonsIsVisible(StyledElement element, bool value) => element.SetValue(ButtonsIsVisibleProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ButtonsIsVisibleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetButtonsIsVisible(StyledElement element) => element.GetValue(ButtonsIsVisibleProperty);

    #endregion

    #region ActiveThumbThickness

    /// <summary>
    /// Provides ActiveThumbThickness Property for attached ScrollBarAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> ActiveThumbThicknessProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("ActiveThumbThickness", typeof(ScrollViewerAssist), 10.0);

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveThumbThicknessProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ActiveThumbThicknessProperty"/>.</param>
    public static void SetActiveThumbThickness(StyledElement element, double value) => element.SetValue(ActiveThumbThicknessProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveThumbThicknessProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetActiveThumbThickness(StyledElement element) => element.GetValue(ActiveThumbThicknessProperty);

    #endregion

    #region InactiveThumbThickness

    /// <summary>
    /// Provides InactiveThumbThickness Property for attached ScrollBarAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> InactiveThumbThicknessProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("InactiveThumbThickness", typeof(ScrollViewerAssist), 6.0);

    /// <summary>
    /// Accessor for Attached  <see cref="InactiveThumbThicknessProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="InactiveThumbThicknessProperty"/>.</param>
    public static void SetInactiveThumbThickness(StyledElement element, double value) => element.SetValue(InactiveThumbThicknessProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="InactiveThumbThicknessProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetInactiveThumbThickness(StyledElement element) => element.GetValue(InactiveThumbThicknessProperty);

    #endregion

    #region RefreshOnScroll

    /// <summary>
    /// Provides RefreshOnScroll Property for attached ScrollViewerAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> RefreshOnScrollProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("RefreshOnScroll", typeof(ScrollViewerAssist));

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
