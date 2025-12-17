// -----------------------------------------------------------------------
// <copyright file="ControlThemeViewer.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using PropertyChanged;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Demo.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// A control for displaying themed controls with multiple layouts, styles, and roles.
/// </summary>
[DoNotNotify]
public class ControlThemeViewer : TemplatedControl
{
    public static readonly StyledProperty<IEnumerable?> ItemsSourceProperty = AvaloniaProperty.Register<ControlThemeViewer, IEnumerable?>(nameof(ItemsSource));

    public static readonly StyledProperty<IDataTemplate?> ControlTemplateProperty = AvaloniaProperty.Register<ControlThemeViewer, IDataTemplate?>(nameof(ControlTemplate));

    public static readonly StyledProperty<object?> CustomContentProperty = AvaloniaProperty.Register<ControlThemeViewer, object?>(nameof(CustomContent));

    public static readonly StyledProperty<bool> IsActiveProperty = AvaloniaProperty.Register<ControlThemeViewer, bool>(nameof(IsActive), defaultValue: true);

    public static readonly StyledProperty<double> MinItemWidthProperty = AvaloniaProperty.Register<ControlThemeViewer, double>(nameof(MinItemWidth), defaultValue: 140.0);

    public static readonly StyledProperty<double> MinItemHeightProperty = AvaloniaProperty.Register<ControlThemeViewer, double>(nameof(MinItemHeight), defaultValue: 48.0);

    public static readonly StyledProperty<IEnumerable?> CombinedItemsProperty = AvaloniaProperty.Register<ControlThemeViewer, IEnumerable?>(nameof(CombinedItems));

    static ControlThemeViewer()
    {
        ItemsSourceProperty.Changed.AddClassHandler<ControlThemeViewer>((x, _) => x.UpdateCombinedItems());
        CustomContentProperty.Changed.AddClassHandler<ControlThemeViewer>((x, _) => x.UpdateCombinedItems());
    }

    /// <summary>
    /// Gets or sets the source collection of control theme descriptions.
    /// </summary>
    public IEnumerable? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    /// <summary>
    /// Gets or sets the data template for rendering individual controls.
    /// </summary>
    public IDataTemplate? ControlTemplate
    {
        get => GetValue(ControlTemplateProperty);
        set => SetValue(ControlTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the custom content to display in an additional tab.
    /// If null or empty, the custom tab will not be displayed.
    /// </summary>
    public object? CustomContent
    {
        get => GetValue(CustomContentProperty);
        set => SetValue(CustomContentProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the displayed controls are enabled.
    /// When false, all controls in the viewer will be disabled.
    /// </summary>
    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum width for each item in the grid layout.
    /// </summary>
    public double MinItemWidth
    {
        get => GetValue(MinItemWidthProperty);
        set => SetValue(MinItemWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum height for each item in the grid layout.
    /// </summary>
    public double MinItemHeight
    {
        get => GetValue(MinItemHeightProperty);
        set => SetValue(MinItemHeightProperty, value);
    }

    private IEnumerable? CombinedItems
    {
        get => GetValue(CombinedItemsProperty);
        set => SetValue(CombinedItemsProperty, value);
    }

    private void UpdateCombinedItems()
    {
        var items = new List<object>();

        if (ItemsSource != null)
        {
            items.AddRange(ItemsSource.Cast<object>());
        }

        if (CustomContent != null)
        {
            items.Add(new CustomTabItem(CustomContent));
        }

        CombinedItems = items;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        UpdateCombinedItems();
    }
}

/// <summary>
/// Represents a custom tab item with content.
/// </summary>
public sealed record CustomTabItem(object Content);
