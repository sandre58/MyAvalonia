// -----------------------------------------------------------------------
// <copyright file="LayoutClassRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Classes.Registry.States;

namespace MyNet.Avalonia.Theme.Classes.Registry;

/// <summary>
/// Provides utility methods for configuring and managing layout strategies for items within an ItemsControl, enabling
/// flexible arrangement of child elements using stack, uniform, or wrap layouts.
/// </summary>
/// <remarks>LayoutClassRegistry allows developers to register and apply different layout behaviors to ItemsControl
/// elements by associating them with specific layout kinds and orientations. This facilitates dynamic and consistent
/// layout customization across controls, supporting scenarios such as horizontal or vertical stacking, uniform grids,
/// and wrapping panels. The class manages layout state through attached properties to ensure that changes are applied
/// reliably. This utility is intended for use in scenarios where layout flexibility and runtime configuration are
/// required for collections of items.</remarks>
public static class LayoutClassRegistry
{
    #region State

    /// <summary>
    /// Specifies the layout options for arranging items in a collection.
    /// </summary>
    /// <remarks>This enumeration provides different layout styles that can be used to control how items are
    /// displayed. The available options include 'Stack', which arranges items in a single vertical stack; 'Uniform',
    /// which arranges items in a grid with uniform sizes; and 'Wrap', which arranges items in a flowing layout that
    /// wraps to the next line when the space is insufficient.</remarks>
    public enum ItemsLayoutKind
    {
        Stack,

        Uniform,

        Wrap
    }

    /// <summary>
    /// Represents the layout configuration for a collection of items, including the layout kind and orientation.
    /// </summary>
    /// <remarks>This class encapsulates the layout state used to arrange and display items. The default
    /// layout is set to stack items horizontally, but both the layout kind and orientation can be customized as
    /// needed.</remarks>
    private sealed class ControlState
    {
        /// <summary>
        /// Gets or sets the layout style used to arrange items within the container.
        /// </summary>
        /// <remarks>The default value is ItemsLayoutKind.Stack, which arranges items in a vertical stack.
        /// Other layout options may be available depending on the implementation.</remarks>
        public ItemsLayoutKind Layout { get; set; } = ItemsLayoutKind.Stack;

        /// <summary>
        /// Gets or sets the orientation of the layout, which determines whether child elements are arranged
        /// horizontally or vertically.
        /// </summary>
        /// <remarks>The default value is Orientation.Horizontal. Changing this property affects the
        /// layout and visual arrangement of child elements within the container.</remarks>
        public Orientation Orientation { get; set; } = Orientation.Horizontal;
    }

    /// <summary>
    /// Configures the specified ItemsControl to use a layout defined by the provided ItemsLayoutState, updating its
    /// items panel and orientation accordingly.
    /// </summary>
    /// <remarks>This method supports Stack, Uniform, and Wrap layout kinds. The items panel and its
    /// orientation are set based on the layout kind and the orientation specified in the state. Spacing and alignment
    /// properties are also configured as appropriate for each layout.</remarks>
    /// <param name="control">The ItemsControl whose layout will be configured.</param>
    /// <param name="state">An ItemsLayoutState that specifies the layout kind and orientation to apply to the ItemsControl.</param>
    private static void ApplyState(ItemsControl control, ControlState state)
    {
        switch (state.Layout)
        {
            case ItemsLayoutKind.Stack:

                control.ItemsPanel = new FuncTemplate<Panel?>(() =>
                {
                    var panel = new StackPanel
                    {
                        Orientation = state.Orientation
                    };
                    panel.Bind(StackPanel.SpacingProperty, control.GetObservable(ItemsAssist.SpacingProperty));

                    return panel;
                });

                break;

            case ItemsLayoutKind.Uniform:

                control.ItemsPanel = new FuncTemplate<Panel?>(() =>
                {
                    var panel = new UniformGrid();

                    if (state.Orientation == Orientation.Horizontal)
                    {
                        panel.Columns = 0;
                        panel.Rows = 1;
                        panel.Bind(UniformGrid.ColumnSpacingProperty, control.GetObservable(ItemsAssist.SpacingProperty));
                    }
                    else
                    {
                        panel.Columns = 1;
                        panel.Rows = 0;
                        panel.Bind(UniformGrid.RowSpacingProperty, control.GetObservable(ItemsAssist.SpacingProperty));
                    }

                    return panel;
                });

                break;

            case ItemsLayoutKind.Wrap:

                control.ItemsPanel = new FuncTemplate<Panel?>(() =>
                {
                    var panel = new ElasticWrapPanel
                    {
                        Orientation = state.Orientation
                    };

                    panel.Bind(WrapPanel.ItemSpacingProperty, control.GetObservable(ItemsAssist.SpacingProperty));
                    panel.Bind(WrapPanel.LineSpacingProperty, control.GetObservable(ItemsAssist.SpacingProperty));

                    return panel;
                });

                break;
        }
    }

    #endregion

    /// <summary>
    /// Registers default layout configurations for supported controls, enabling consistent orientation and layout
    /// behaviors across StackPanel, WrapPanel, and ItemsControl instances.
    /// </summary>
    /// <remarks>Call this method during application initialization to ensure that flex-related utility
    /// registrations are in place before creating or displaying UI elements. This setup allows controls to respond to
    /// flex-related CSS classes and orientation settings, facilitating uniform styling and alignment throughout the
    /// application.</remarks>
    public static void Register()
    {
        ClassRegistry.RegisterMany<Orientation, Control>(CssPrefix.Layout, setOrientation);
        ClassRegistry.Register<ItemsControl>(CssClass.Uniform, x => new CompositeDisposable
        {
            x.SetProperty(Layoutable.HorizontalAlignmentProperty, HorizontalAlignment.Stretch),
            ClassContext.Create<ItemsControl, ControlState>(x).Update(s => s.Layout = ItemsLayoutKind.Uniform, ApplyState)
        });
        ClassRegistry.Register<ItemsControl>(CssClass.Wrap, x => new CompositeDisposable
                    {
                        x.SetProperty(ItemsAssist.HorizontalAlignmentProperty, HorizontalAlignment.Stretch),
                        ClassContext.Create<ItemsControl, ControlState>(x).Update(s => s.Layout = ItemsLayoutKind.Wrap, ApplyState),
                        Disposable.Create(() => x.ClearValue(ItemsControl.ItemsPanelProperty))
                    });

        ClassRegistry.Register<TemplatedControl>(CssClass.IsStretch, x => new CompositeDisposable
        {
            x.SetProperty(TemplatedControl.CornerRadiusProperty, new CornerRadius(0)),
            x.SetProperty(Layoutable.VerticalAlignmentProperty, VerticalAlignment.Stretch),
            x.SetProperty(TemplatedControl.PaddingProperty, new Thickness(ThemeResources.Spacing.Get(SpacingSize.Sm).Value, 0, ThemeResources.Spacing.Get(SpacingSize.Sm).Value, 0)),
            x.SetProperty(Layoutable.HeightProperty, double.NaN),
            x.SetProperty(Layoutable.WidthProperty, double.NaN)
        });

        static IDisposable setOrientation(Control control, Orientation orientation) => control switch
        {
            StackPanel stackPanel => stackPanel.SetProperty(StackPanel.OrientationProperty, orientation),
            WrapPanel wrapPanel => wrapPanel.SetProperty(StackPanel.OrientationProperty, orientation),
            ItemsControl itemsControl => new CompositeDisposable
                    {
                        itemsControl.SetProperty(ItemsAssist.HorizontalAlignmentProperty, HorizontalAlignment.Stretch),
                        ClassContext.Create<ItemsControl, ControlState>(itemsControl).Update(s => s.Orientation = orientation, ApplyState),
                        Disposable.Create(() => itemsControl.ClearValue(ItemsControl.ItemsPanelProperty))
                    },
            _ => Disposable.Empty,
        };
    }
}
