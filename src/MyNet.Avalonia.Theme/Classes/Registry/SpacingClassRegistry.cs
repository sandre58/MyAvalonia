// -----------------------------------------------------------------------
// <copyright file="SpacingClassRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Theme.Classes.Registry;

/// <summary>
/// Provides static utility methods for registering margin and padding properties for layoutable and templated controls
/// using predefined CSS-like prefixes.
/// </summary>
/// <remarks>The SpacingClassRegistry class centralizes the registration of margin and padding settings, enabling
/// consistent application of spacing values across controls. These methods are typically used during application
/// startup or theme initialization to associate spacing sizes with corresponding layout properties. This approach
/// simplifies the management of spacing conventions and ensures uniformity throughout the UI.</remarks>
public static class SpacingClassRegistry
{
    /// <summary>
    /// Registers margin properties for layoutable elements, enabling CSS-style margin adjustments for all sides and
    /// individual edges.
    /// </summary>
    /// <remarks>This method sets up margin utilities for layoutable elements, allowing developers to apply
    /// consistent spacing using predefined CSS prefixes. Margins can be set for all sides simultaneously or for
    /// individual sides (top, bottom, left, right), as well as for horizontal and vertical combinations. This
    /// facilitates flexible and maintainable layout customization in UI development.</remarks>
    public static void RegisterMargins()
    {
        ClassRegistry.RegisterMany<SpacingSize, Layoutable>(CssPrefix.Margin, (x, y) => x.SetProperty(Layoutable.MarginProperty, new Thickness(ThemeResources.Spacing.Get(y).Value)));
        ClassRegistry.RegisterMany<SpacingSize, Layoutable>(CssPrefix.LeftMargin, (x, y) => x.SetProperty(Layoutable.MarginProperty, new Thickness(ThemeResources.Spacing.Get(y).Value, x.Margin.Top, x.Margin.Right, x.Margin.Bottom)));
        ClassRegistry.RegisterMany<SpacingSize, Layoutable>(CssPrefix.RightMargin, (x, y) => x.SetProperty(Layoutable.MarginProperty, new Thickness(x.Margin.Left, x.Margin.Top, ThemeResources.Spacing.Get(y).Value, x.Margin.Bottom)));
        ClassRegistry.RegisterMany<SpacingSize, Layoutable>(CssPrefix.TopMargin, (x, y) => x.SetProperty(Layoutable.MarginProperty, new Thickness(x.Margin.Left, ThemeResources.Spacing.Get(y).Value, x.Margin.Right, x.Margin.Bottom)));
        ClassRegistry.RegisterMany<SpacingSize, Layoutable>(CssPrefix.BottomMargin, (x, y) => x.SetProperty(Layoutable.MarginProperty, new Thickness(x.Margin.Left, x.Margin.Top, x.Margin.Right, ThemeResources.Spacing.Get(y).Value)));
        ClassRegistry.RegisterMany<SpacingSize, Layoutable>(CssPrefix.HorizontalMargin, (x, y) => x.SetProperty(Layoutable.MarginProperty, new Thickness(ThemeResources.Spacing.Get(y).Value, x.Margin.Top, ThemeResources.Spacing.Get(y).Value, x.Margin.Bottom)));
        ClassRegistry.RegisterMany<SpacingSize, Layoutable>(CssPrefix.VerticalMargin, (x, y) => x.SetProperty(Layoutable.MarginProperty, new Thickness(x.Margin.Left, ThemeResources.Spacing.Get(y).Value, x.Margin.Right, ThemeResources.Spacing.Get(y).Value)));
    }

    /// <summary>
    /// Registers multiple padding size utilities for templated controls, enabling consistent application of padding
    /// values based on CSS-like prefixes.
    /// </summary>
    /// <remarks>This method sets up mappings between various padding-related CSS prefixes and their
    /// corresponding property setters for templated controls. By calling this method, developers can ensure that
    /// controls support a range of standardized padding options, such as left, right, top, bottom, horizontal, and
    /// vertical padding, which can be referenced throughout the application's styling system.</remarks>
    public static void RegisterPaddings()
    {
        ClassRegistry.RegisterMany<SpacingSize, Control>(CssPrefix.Padding, (x, y) => setPadding(x, new(ThemeResources.Spacing.Get(y).Value)));
        ClassRegistry.RegisterMany<SpacingSize, TemplatedControl>(CssPrefix.LeftPadding, (x, y) => setPadding(x, new(ThemeResources.Spacing.Get(y).Value, x.Padding.Top, x.Padding.Right, x.Padding.Bottom)));
        ClassRegistry.RegisterMany<SpacingSize, TemplatedControl>(CssPrefix.RightPadding, (x, y) => setPadding(x, new(x.Padding.Left, x.Padding.Top, ThemeResources.Spacing.Get(y).Value, x.Padding.Bottom)));
        ClassRegistry.RegisterMany<SpacingSize, TemplatedControl>(CssPrefix.TopPadding, (x, y) => setPadding(x, new(x.Padding.Left, ThemeResources.Spacing.Get(y).Value, x.Padding.Right, x.Padding.Bottom)));
        ClassRegistry.RegisterMany<SpacingSize, TemplatedControl>(CssPrefix.BottomPadding, (x, y) => setPadding(x, new(x.Padding.Left, x.Padding.Top, x.Padding.Right, ThemeResources.Spacing.Get(y).Value)));
        ClassRegistry.RegisterMany<SpacingSize, TemplatedControl>(CssPrefix.HorizontalPadding, (x, y) => setPadding(x, new(ThemeResources.Spacing.Get(y).Value, x.Padding.Top, ThemeResources.Spacing.Get(y).Value, x.Padding.Bottom)));
        ClassRegistry.RegisterMany<SpacingSize, TemplatedControl>(CssPrefix.VerticalPadding, (x, y) => setPadding(x, new(x.Padding.Left, ThemeResources.Spacing.Get(y).Value, x.Padding.Right, ThemeResources.Spacing.Get(y).Value)));

        static IDisposable setPadding(Control control, Thickness thickness) => control switch
        {
            TemplatedControl templatedControl => templatedControl.SetProperty(TemplatedControl.PaddingProperty, thickness),
            Decorator decorator => decorator.SetProperty(Decorator.PaddingProperty, thickness),
            _ => Disposable.Empty
        };
    }

    /// <summary>
    /// Registers multiple spacing size utilities for controls, allowing for consistent application of spacing values
    /// based on CSS-like prefixes.
    /// </summary>
    public static void RegisterSpacings()
    {
        ClassRegistry.RegisterMany<SpacingSize, Control>(CssPrefix.Spacing, (x, y) => setSpacing(x, ThemeResources.Spacing.Get(y).Value, true, true));
        ClassRegistry.RegisterMany<SpacingSize, Control>(CssPrefix.HorizontalSpacing, (x, y) => setSpacing(x, ThemeResources.Spacing.Get(y).Value, true, false));
        ClassRegistry.RegisterMany<SpacingSize, Control>(CssPrefix.VerticalSpacing, (x, y) => setSpacing(x, ThemeResources.Spacing.Get(y).Value, false, true));

        static IDisposable setSpacing(Control control, double spacing, bool isHorizontal, bool isVertical) => control switch
        {
            StackPanel stackPanel => isHorizontal && isVertical ? stackPanel.SetProperty(StackPanel.SpacingProperty, spacing) : Disposable.Empty,
            WrapPanel wrapPanel => new CompositeDisposable
                    {
                        isHorizontal ? wrapPanel.SetProperty(WrapPanel.ItemSpacingProperty, spacing) : Disposable.Empty,
                        isVertical ? wrapPanel.SetProperty(WrapPanel.LineSpacingProperty, spacing) : Disposable.Empty
                    },
            UniformGrid uniformGrid => new CompositeDisposable
                    {
                        isHorizontal ? uniformGrid.SetProperty(UniformGrid.ColumnSpacingProperty, spacing) : Disposable.Empty,
                        isVertical ? uniformGrid.SetProperty(UniformGrid.RowSpacingProperty, spacing) : Disposable.Empty
                    },
            Grid grid => new CompositeDisposable
                    {
                        isHorizontal ? grid.SetProperty(Grid.ColumnSpacingProperty, spacing) : Disposable.Empty,
                        isVertical ? grid.SetProperty(Grid.RowSpacingProperty, spacing) : Disposable.Empty
                    },
            ItemsControl itemsControl => isHorizontal && isVertical ? itemsControl.SetProperty(ItemsAssist.SpacingProperty, spacing) : Disposable.Empty,
            _ => Disposable.Empty
        };
    }
}
