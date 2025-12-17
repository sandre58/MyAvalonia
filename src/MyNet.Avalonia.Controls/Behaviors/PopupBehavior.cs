// -----------------------------------------------------------------------
// <copyright file="PopupBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Transformation;
using MyNet.Avalonia.Controls.Extensions;
using MyNet.Utilities;

namespace MyNet.Avalonia.Controls.Behaviors;

public static class PopupBehavior
{
    static PopupBehavior()
    {
        PlacementProperty.Changed.Subscribe(PlacementPropertyChangedCallback);
        OpenOnFocusProperty.Changed.Subscribe(OpenOnFocusChangedCallback);
        EnableShortcutKeysProperty.Changed.Subscribe(OnEnableShortcutKeyChanged);
        AutoFocusOnOpeningProperty.Changed.Subscribe(OnAutoFocusOnOpeningChanged);
        EnableSlideProperty.Changed.AddClassHandler<Popup>((popup, e) =>
        {
            if ((bool)e.NewValue!)
            {
                popup.Opened += OnPopupOpenedCallback;
                popup.Closed += OnPopupClosedCallback;
                popup.PropertyChanged += OnPopupPropertyChanged;

                // Initialize child state if it already exists
                if (popup.Child is Control child)
                {
                    InitializeControlForSlide(child);
                }
            }
            else
            {
                popup.Opened -= OnPopupOpenedCallback;
                popup.Closed -= OnPopupClosedCallback;
                popup.PropertyChanged -= OnPopupPropertyChanged;
            }
        });

        // Support for ContextMenu
        EnableSlideProperty.Changed.AddClassHandler<ContextMenu>((contextMenu, e) =>
        {
            if ((bool)e.NewValue!)
            {
                // Initialize state immediately when slide is enabled
                InitializeControlForSlide(contextMenu);
                contextMenu.Opened += OnContextMenuOpenedCallback;
                contextMenu.Closed += OnContextMenuClosedCallback;
            }
            else
            {
                contextMenu.Opened -= OnContextMenuOpenedCallback;
                contextMenu.Closed -= OnContextMenuClosedCallback;
            }
        });

        // Support for MenuFlyoutPresenter (used by MenuFlyout)
        EnableSlideProperty.Changed.AddClassHandler<MenuFlyoutPresenter>((flyoutPresenter, e) =>
        {
            if ((bool)e.NewValue!)
            {
                // Initialize state immediately when slide is enabled
                InitializeControlForSlide(flyoutPresenter);
                flyoutPresenter.AttachedToVisualTree += OnFlyoutPresenterAttachedToVisualTree;
                flyoutPresenter.DetachedFromVisualTree += OnFlyoutPresenterDetachedFromVisualTree;
            }
            else
            {
                flyoutPresenter.AttachedToVisualTree -= OnFlyoutPresenterAttachedToVisualTree;
                flyoutPresenter.DetachedFromVisualTree -= OnFlyoutPresenterDetachedFromVisualTree;
            }
        });

        // Support for FlyoutPresenter (used by Flyout)
        EnableSlideProperty.Changed.AddClassHandler<FlyoutPresenter>((flyoutPresenter, e) =>
        {
            if ((bool)e.NewValue!)
            {
                // Initialize state immediately when slide is enabled
                InitializeControlForSlide(flyoutPresenter);
                flyoutPresenter.AttachedToVisualTree += OnFlyoutPresenterAttachedToVisualTree;
                flyoutPresenter.DetachedFromVisualTree += OnFlyoutPresenterDetachedFromVisualTree;
            }
            else
            {
                flyoutPresenter.AttachedToVisualTree -= OnFlyoutPresenterAttachedToVisualTree;
                flyoutPresenter.DetachedFromVisualTree -= OnFlyoutPresenterDetachedFromVisualTree;
            }
        });
    }

    #region Placement

    /// <summary>
    /// Provides Placement Property for attached PopupBehavior element.
    /// </summary>
    public static readonly AttachedProperty<PlacementMode> PlacementProperty = AvaloniaProperty.RegisterAttached<StyledElement, PlacementMode>("Placement", typeof(PopupBehavior), PlacementMode.Custom);

    /// <summary>
    /// Accessor for Attached  <see cref="PlacementProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PlacementProperty"/>.</param>
    public static void SetPlacement(StyledElement element, PlacementMode value) => element.SetValue(PlacementProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PlacementProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static PlacementMode GetPlacement(StyledElement element) => element.GetValue(PlacementProperty);

    private static void PlacementPropertyChangedCallback(AvaloniaPropertyChangedEventArgs obj)
    {
        const int largeShadowOffset = 6;
        const int smallShadowOffset = 2;
        const int largeMargin = 8;
        const int smallMargin = 2;

        var flyout = obj.Sender switch
        {
            Button button => button.Flyout,
            ToggleSplitButton toggleSplitButton => toggleSplitButton.Flyout,
            SplitButton splitButton => splitButton.Flyout,
            _ => null
        };
        if (flyout is not PopupFlyoutBase popupFlyout || obj.NewValue is not PlacementMode placement || placement == PlacementMode.Custom)
            return;
        popupFlyout.Placement = placement;

        switch (placement)
        {
            case PlacementMode.Bottom:
            case PlacementMode.Right:
                break;
            case PlacementMode.Left:
                popupFlyout.HorizontalOffset = largeMargin + (largeShadowOffset / 2.0);
                break;
            case PlacementMode.Top:
                popupFlyout.VerticalOffset = largeMargin + (largeShadowOffset / 2.0);
                break;
            case PlacementMode.TopEdgeAlignedLeft:
                popupFlyout.VerticalOffset = largeMargin + (largeShadowOffset / 2.0);
                popupFlyout.HorizontalOffset = -(smallMargin + smallShadowOffset);
                break;
            case PlacementMode.TopEdgeAlignedRight:
                popupFlyout.VerticalOffset = largeMargin + (largeShadowOffset / 2.0);
                popupFlyout.HorizontalOffset = largeMargin + largeShadowOffset;
                break;
            case PlacementMode.BottomEdgeAlignedLeft:
                popupFlyout.HorizontalOffset = -(smallMargin + smallShadowOffset);
                break;
            case PlacementMode.BottomEdgeAlignedRight:
                popupFlyout.HorizontalOffset = largeMargin + largeShadowOffset;
                break;
            case PlacementMode.LeftEdgeAlignedTop:
                popupFlyout.VerticalOffset = -(smallMargin + (smallShadowOffset / 2.0));
                popupFlyout.HorizontalOffset = largeMargin + (largeShadowOffset / 2.0);
                break;
            case PlacementMode.LeftEdgeAlignedBottom:
                popupFlyout.HorizontalOffset = largeMargin + (largeShadowOffset / 2.0);
                popupFlyout.VerticalOffset = largeMargin + largeShadowOffset;
                break;
            case PlacementMode.RightEdgeAlignedTop:
                popupFlyout.VerticalOffset = -(smallMargin + (smallShadowOffset / 2.0));
                break;
            case PlacementMode.RightEdgeAlignedBottom:
                popupFlyout.VerticalOffset = largeMargin + largeShadowOffset;
                break;
        }
    }
    #endregion

    #region OpenOnFocus

    /// <summary>
    /// Provides OpenOnFocus Property for attached ProxyBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> OpenOnFocusProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("OpenOnFocus", typeof(PopupBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="OpenOnFocusProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="OpenOnFocusProperty"/>.</param>
    public static void SetOpenOnFocus(StyledElement element, bool value) => element.SetValue(OpenOnFocusProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="OpenOnFocusProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetOpenOnFocus(StyledElement element) => element.GetValue(OpenOnFocusProperty);

    private static void OpenOnFocusChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not Control control) return;

        if (((bool?)args.NewValue).IsTrue())
        {
            control.GotFocus += onGotFocus;
        }
        else
        {
            control.GotFocus -= onGotFocus;
        }

        static void onGotFocus(object? sender, EventArgs e) => (sender as Control)?.OpenPopup();
    }

    #endregion

    #region EnableShortcutKeys

    /// <summary>
    /// Provides EnableShortcutKeys Property for attached PopupBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> EnableShortcutKeysProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("EnableShortcutKeys", typeof(PopupBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="EnableShortcutKeysProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="EnableShortcutKeysProperty"/>.</param>
    public static void SetEnableShortcutKeys(StyledElement element, bool value) => element.SetValue(EnableShortcutKeysProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="EnableShortcutKeysProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetEnableShortcutKeys(StyledElement element) => element.GetValue(EnableShortcutKeysProperty);

    private static void OnEnableShortcutKeyChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (args.Sender is TemplatedControl tc)
        {
            if (args.NewValue.GetValueOrDefault<bool>())
                AttachOnEnableShortcutKey(tc);
            else
                DetachOnEnableShortcutKey(tc);
        }
    }

    private static void AttachOnEnableShortcutKey(TemplatedControl dp) => dp.AddHandler(InputElement.KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);

    private static void DetachOnEnableShortcutKey(TemplatedControl dp) => dp.RemoveHandler(InputElement.KeyDownEvent, OnKeyDown);

    private static void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is not TemplatedControl tc)
            return;

        var popupOpen = tc.IsPopupOpen();

        if (!popupOpen)
        {
            if ((e.Key == Key.Down || e.Key == Key.Up) && e.KeyModifiers == KeyModifiers.Alt)
            {
                tc.OpenPopup();
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                tc.OpenPopup();
                e.Handled = true;
            }
        }
    }

    #endregion

    #region AutoFocusOnOpening

    /// <summary>
    /// Provides AutoFocusOnOpening Property for attached PopupBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> AutoFocusOnOpeningProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("AutoFocusOnOpening", typeof(PopupBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="AutoFocusOnOpeningProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="AutoFocusOnOpeningProperty"/>.</param>
    public static void SetAutoFocusOnOpening(StyledElement element, bool value) => element.SetValue(AutoFocusOnOpeningProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="AutoFocusOnOpeningProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetAutoFocusOnOpening(StyledElement element) => element.GetValue(AutoFocusOnOpeningProperty);

    private static void OnAutoFocusOnOpeningChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (args.Sender is TemplatedControl tc)
        {
            if (args.NewValue.GetValueOrDefault<bool>())
                AttachOnAutoFocusOnOpening(tc);
            else
                DetachOnAutoFocusOnOpening(tc);
        }
    }

    private static void AttachOnAutoFocusOnOpening(TemplatedControl dp) => dp.TemplateApplied += OnTemplateApplied;

    private static void DetachOnAutoFocusOnOpening(TemplatedControl dp) => dp.TemplateApplied -= OnTemplateApplied;

    private static void OnTemplateApplied(object? sender, TemplateAppliedEventArgs e)
    {
        if (sender is not TemplatedControl tc)
            return;

        var popup = e.NameScope.Find<Popup>("PART_Popup");
        if (popup != null)
        {
            popup.Opened += (_, __) =>
            {
                var focusable = popup?.Child?.GetFirstFocusableControl();
                focusable?.Focus();
            };
        }
    }

    #endregion

    #region EnableSlide

    /// <summary>
    /// Provides EnableSlide Property for attached PopupAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> EnableSlideProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("EnableSlide", typeof(PopupBehavior), false);

    /// <summary>
    /// Accessor for Attached  <see cref="EnableSlideProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="EnableSlideProperty"/>.</param>
    public static void SetEnableSlide(StyledElement element, bool value) => element.SetValue(EnableSlideProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="EnableSlideProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetEnableSlide(StyledElement element) => element.GetValue(EnableSlideProperty);

    private static void OnPopupPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (sender is not Popup popup || e.Property.Name != nameof(Popup.Child) || popup.Child is not Control child)
            return;

        // When Child is set, immediately hide it to prevent flash
        InitializeControlForSlide(child);
    }

    private static void OnPopupClosedCallback(object? sender, EventArgs e)
    {
        if (sender is not Popup popup || popup.Child is not Control child)
            return;

        ResetControlState(child);
    }

    private static void OnPopupOpenedCallback(object? sender, EventArgs e)
    {
        if (sender is not Popup popup || popup.Child is not Control child)
            return;

        ApplySlideAnimation(child, popup.Placement);
    }

    private static void OnContextMenuOpenedCallback(object? sender, EventArgs e)
    {
        if (sender is not ContextMenu contextMenu)
            return;

        ApplySlideAnimation(contextMenu, contextMenu.Placement);
    }

    private static void OnContextMenuClosedCallback(object? sender, EventArgs e)
    {
        if (sender is not ContextMenu contextMenu)
            return;

        ResetControlState(contextMenu);
    }

    private static void OnFlyoutPresenterAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is not Control flyoutPresenter)
            return;

        // For flyout presenters, try to get placement from parent flyout
        var placement = GetFlyoutPlacement(flyoutPresenter);
        ApplySlideAnimation(flyoutPresenter, placement);
    }

    private static void OnFlyoutPresenterDetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is not Control flyoutPresenter)
            return;

        ResetControlState(flyoutPresenter);
    }

    private static PlacementMode GetFlyoutPlacement(Control flyoutPresenter)
    {
        // Try to find the parent flyout to get placement information
        var parent = flyoutPresenter.Parent;
        while (parent != null)
        {
            if (parent is Popup popup)
                return popup.Placement;

            parent = parent.Parent;
        }

        // Default to Custom if we can't determine placement
        return PlacementMode.Custom;
    }

    private static void InitializeControlForSlide(Control control)
    {
        // Set initial hidden state for controls that don't have PropertyChanged events
        control.Opacity = 0;
        control.RenderTransform = TransformOperations.Parse("translateY(-20px)");
    }

    private static void ApplySlideAnimation(Control control, PlacementMode placement)
    {
        // Determine slide direction based on Placement property
        var slideDirection = GetSlideDirectionFromPlacement(placement);
        var offsetY = slideDirection == SlideDirection.Up ? 20 : -20;

        // Set initial state immediately
        control.Opacity = 0;
        control.RenderTransform = TransformOperations.Parse($"translateY({offsetY}px)");

        // Animate to final position using AXAML transitions
        control.Opacity = 1;
        control.RenderTransform = TransformOperations.Parse("translateY(0px)");
    }

    private static void ResetControlState(Control control)
    {
        // Reset to hidden state when control closes to prepare for next opening
        control.Opacity = 0;
        control.RenderTransform = TransformOperations.Parse("translateY(-20px)");
    }

    private static SlideDirection GetSlideDirectionFromPlacement(PlacementMode placement) => placement switch
    {
        // Top placements: popup appears above target, slide from top
        PlacementMode.Top or
        PlacementMode.TopEdgeAlignedLeft or
        PlacementMode.TopEdgeAlignedRight => SlideDirection.Up,

        // Bottom placements: popup appears below target, slide from bottom
        PlacementMode.Bottom or
        PlacementMode.BottomEdgeAlignedLeft or
        PlacementMode.BottomEdgeAlignedRight => SlideDirection.Down,

        // Side placements: default to slide down
        PlacementMode.Left or
        PlacementMode.LeftEdgeAlignedTop or
        PlacementMode.LeftEdgeAlignedBottom or
        PlacementMode.Right or
        PlacementMode.RightEdgeAlignedTop or
        PlacementMode.RightEdgeAlignedBottom => SlideDirection.Down,

        // Auto/Custom/etc: default to slide down
        _ => SlideDirection.Down
    };

    private enum SlideDirection
    {
        Up,
        Down
    }
}

#endregion
