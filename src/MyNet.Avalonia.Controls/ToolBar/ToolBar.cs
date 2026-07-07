// -----------------------------------------------------------------------
// <copyright file="ToolBar.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Automation.Peers;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Layout;
using MyNet.Avalonia.Controls.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130

/// <summary>
/// A professional toolbar control with a deterministic C# layout engine.
/// Supports multiple layout modes, adaptive overflow, and responsive resizing.
/// </summary>
/// <remarks>
/// Layout responsibility is fully delegated to <see cref="ToolBarPanel"/> via
/// <see cref="ToolBarLayoutContext"/>. The toolbar itself only owns configuration
/// (LayoutMode, OverflowMode, Orientation, ItemSpacing) and exposes overflow state
/// (OverflowItems, IsOverflowAvailable) as read-only DirectProperties.
/// <para>
/// <see cref="OverflowItems"/> always contains <b>data objects</b> for the popup
/// <see cref="ItemsControl"/> — never live strip <see cref="Control"/> instances.
/// With <c>ItemsSource</c>, entries are the bound data items (e.g. ViewModels).
/// With inline XAML children, entries are <see cref="ToolBarOverflowEntry"/> snapshots.
/// Override <see cref="OverflowItemTemplate"/> to render your data type in the popup.
/// </para>
/// </remarks>
[PseudoClasses(
    PseudoClassName.Compact,
    PseudoClassName.Horizontal,
    PseudoClassName.Vertical,
    PseudoClassName.Overflow)]
[TemplatePart(PartOverflowButton, typeof(ToggleButton))]
[TemplatePart(PartOverflowPopup, typeof(Popup))]
public partial class ToolBar : ItemsControl
{
    public const string PartOverflowButton = "PART_OverflowButton";
    public const string PartOverflowPopup = "PART_OverflowPopup";
    public const string PartItemsPresenter = "PART_ItemsPresenter";

    public static readonly StyledProperty<ToolBarLayoutMode> LayoutModeProperty =
        AvaloniaProperty.Register<ToolBar, ToolBarLayoutMode>(nameof(LayoutMode));

    public static readonly StyledProperty<ToolBarOverflowMode> OverflowModeProperty =
        AvaloniaProperty.Register<ToolBar, ToolBarOverflowMode>(nameof(OverflowMode), ToolBarOverflowMode.Adaptive);

    public static readonly StyledProperty<Orientation> OrientationProperty =
        AvaloniaProperty.Register<ToolBar, Orientation>(nameof(Orientation));

    public static readonly StyledProperty<double> ItemSpacingProperty =
        AvaloniaProperty.Register<ToolBar, double>(nameof(ItemSpacing), 2d);

    public static readonly StyledProperty<IDataTemplate?> OverflowItemTemplateProperty =
        AvaloniaProperty.Register<ToolBar, IDataTemplate?>(nameof(OverflowItemTemplate));

    public static readonly DirectProperty<ToolBar, IReadOnlyList<object>> OverflowItemsProperty =
        AvaloniaProperty.RegisterDirect<ToolBar, IReadOnlyList<object>>(
            nameof(OverflowItems), o => o.OverflowItems);

    public static readonly DirectProperty<ToolBar, bool> IsOverflowAvailableProperty =
        AvaloniaProperty.RegisterDirect<ToolBar, bool>(
            nameof(IsOverflowAvailable), o => o.IsOverflowAvailable);

    private IReadOnlyList<object> _overflowItems = [];
    private bool _isOverflowAvailable;
    private ToolBarPanel? _panel;
    private Popup? _overflowPopup;
    private bool _layoutUpdatedSubscribed;

    static ToolBar()
    {
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<ToolBar>(AutomationControlType.ToolBar);

        LayoutModeProperty.Changed.AddClassHandler<ToolBar>((tb, _) => tb.OnConfigChanged());
        OverflowModeProperty.Changed.AddClassHandler<ToolBar>((tb, _) => tb.OnConfigChanged());
        OrientationProperty.Changed.AddClassHandler<ToolBar>((tb, _) =>
        {
            tb.UpdateOrientationPseudoClasses();
            tb.OnConfigChanged();
        });
        ItemSpacingProperty.Changed.AddClassHandler<ToolBar>((tb, _) => tb.OnConfigChanged());
    }

    public ToolBar() => UpdateOrientationPseudoClasses();

    public ToolBarLayoutMode LayoutMode
    {
        get => GetValue(LayoutModeProperty);
        set => SetValue(LayoutModeProperty, value);
    }

    public ToolBarOverflowMode OverflowMode
    {
        get => GetValue(OverflowModeProperty);
        set => SetValue(OverflowModeProperty, value);
    }

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public double ItemSpacing
    {
        get => GetValue(ItemSpacingProperty);
        set => SetValue(ItemSpacingProperty, value);
    }

    public IDataTemplate? OverflowItemTemplate
    {
        get => GetValue(OverflowItemTemplateProperty);
        set => SetValue(OverflowItemTemplateProperty, value);
    }

    /// <summary>
    /// Gets data objects for items currently in the overflow popup.
    /// Never contains live strip controls — only bound data items or <see cref="ToolBarOverflowEntry"/> snapshots.
    /// </summary>
    public IReadOnlyList<object> OverflowItems
    {
        get => _overflowItems;
        private set => SetAndRaise(OverflowItemsProperty, ref _overflowItems, value);
    }

    public bool IsOverflowAvailable
    {
        get => _isOverflowAvailable;
        private set => SetAndRaise(IsOverflowAvailableProperty, ref _isOverflowAvailable, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _ = e.NameScope.Find<ToggleButton>(PartOverflowButton);
        _overflowPopup = e.NameScope.Find<Popup>(PartOverflowPopup);

        EnsurePanelConnected(e.NameScope);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        EnsurePanelConnected();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        EnsurePanelConnected();
        EnsureLayoutUpdatedSubscription();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        RemoveLayoutUpdatedSubscription();
        base.OnDetachedFromVisualTree(e);
    }

    private void EnsurePanelConnected(INameScope? nameScope = null)
    {
        var panel = ItemsPanelRoot as ToolBarPanel
                    ?? (nameScope?.Find<ItemsPresenter>(PartItemsPresenter) ?? Presenter)?.Panel as ToolBarPanel;
        if (panel is null)
            return;

        _panel = panel;
        _panel.LayoutContext = BuildLayoutContext();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        if (item is ToolBarSeparatorItem)
            return NeedsContainer<ToolBarSeparator>(item, out recycleKey);

        if (item is ToolBarSeparator)
        {
            recycleKey = null;
            return false;
        }

        return NeedsContainer<ToolBarItem>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
        => item is ToolBarSeparatorItem ? new ToolBarSeparator() : new ToolBarItem();

    /// <summary>
    /// Called from <see cref="OnLayoutUpdated"/> after the panel caches an overflow result.
    /// Updates the public overflow state and pseudo-classes.
    /// </summary>
    internal void UpdateOverflowState(ToolBarOverflowResult result)
    {
        var overflowData = result.OverflowItems
            .Select(s => s.PopupItem)
            .Where(i => i is not null)
            .Cast<object>()
            .ToList();

        if (result.HasOverflow == _isOverflowAvailable && OverflowItemsEqual(_overflowItems, overflowData))
            return;

        var overflowAvailabilityChanged = result.HasOverflow != _isOverflowAvailable;

        OverflowItems = overflowData;
        IsOverflowAvailable = result.HasOverflow;
        PseudoClasses.Set(PseudoClassName.Overflow, result.HasOverflow);
        PseudoClasses.Set(PseudoClassName.Compact, LayoutMode == ToolBarLayoutMode.Compact);

        if (overflowAvailabilityChanged)
            _panel?.LayoutContext = BuildLayoutContext();
    }

    private static bool OverflowItemsEqual(IReadOnlyList<object> current, IReadOnlyList<object> next)
    {
        if (current.Count != next.Count)
            return false;

        for (var i = 0; i < current.Count; i++)
        {
            if (!Equals(current[i], next[i]))
                return false;
        }

        return true;
    }

    private void EnsureLayoutUpdatedSubscription()
    {
        if (_layoutUpdatedSubscribed)
            return;

        LayoutUpdated += OnLayoutUpdated;
        _layoutUpdatedSubscribed = true;
    }

    private void RemoveLayoutUpdatedSubscription()
    {
        if (!_layoutUpdatedSubscribed)
            return;

        LayoutUpdated -= OnLayoutUpdated;
        _layoutUpdatedSubscribed = false;
    }

    private void OnLayoutUpdated(object? sender, EventArgs e)
    {
        if (_panel?.TryTakePendingOverflowResult(out var result) == true && result is not null)
            UpdateOverflowState(result);
    }

    private object? ResolvePopupItem(Control container)
    {
        var index = IndexFromContainer(container);
        if (index >= 0 && index < ItemCount)
        {
            return Items[index] switch
            {
                null => null,
                ToolBarItem directItem when ReferenceEquals(directItem, container)
                    => ToolBarOverflowEntry.FromItem(directItem),
                ToolBarSeparator when ReferenceEquals(Items[index], container)
                    => ToolBarOverflowEntry.Separator,
                ToolBarSeparatorItem => ToolBarOverflowEntry.Separator,
                ToolBarSeparator => null,
                var item => item,
            };
        }

        return container switch
        {
            ToolBarItem directItem => ToolBarOverflowEntry.FromItem(directItem),
            ToolBarSeparator => ToolBarOverflowEntry.Separator,
            _ => null,
        };
    }

    private void OnConfigChanged() => _panel?.LayoutContext = BuildLayoutContext();

    private const double DefaultOverflowButtonReserveWidth = 32;

    private ToolBarLayoutContext BuildLayoutContext()
        => new()
        {
            Engine = ToolBarLayoutEngineFactory.Create(LayoutMode),
            OverflowEngine = new DefaultToolBarOverflowEngine(),
            Orientation = Orientation,
            ItemSpacing = ItemSpacing,
            OverflowMode = OverflowMode,
            OverflowButtonReserveWidth = OverflowMode == ToolBarOverflowMode.None || _isOverflowAvailable
                ? 0
                : DefaultOverflowButtonReserveWidth,
            ResolvePopupItem = ResolvePopupItem,
        };

    private void UpdateOrientationPseudoClasses()
    {
        PseudoClasses.Set(PseudoClassName.Horizontal, Orientation == Orientation.Horizontal);
        PseudoClasses.Set(PseudoClassName.Vertical, Orientation == Orientation.Vertical);
    }
}
