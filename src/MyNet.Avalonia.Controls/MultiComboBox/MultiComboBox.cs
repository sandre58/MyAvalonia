// -----------------------------------------------------------------------
// <copyright file="MultiComboBox.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Metadata;
using Avalonia.Styling;
using MyNet.Avalonia.Extensions;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// This control inherits from <see cref="SelectingItemsControl"/>, but it only supports MVVM pattern.
/// </summary>
[TemplatePart(PartRootPanel, typeof(Panel))]
[PseudoClasses(PseudoClassName.FlyoutOpen, PseudoClassName.Empty)]
public class MultiComboBox : SelectingItemsControl
{
    public const string PartRootPanel = "PART_RootPanel";

    private static readonly ITemplate<Panel?> DefaultPanel = new FuncTemplate<Panel?>(() => new VirtualizingStackPanel());

    public static readonly StyledProperty<bool> IsDropDownOpenProperty =
        ComboBox.IsDropDownOpenProperty.AddOwner<MultiComboBox>();

    public static readonly StyledProperty<double> MaxDropDownHeightProperty =
        AvaloniaProperty.Register<MultiComboBox, double>(
            nameof(MaxDropDownHeight));

    public static readonly StyledProperty<double> MaxSelectionBoxHeightProperty =
        AvaloniaProperty.Register<MultiComboBox, double>(
            nameof(MaxSelectionBoxHeight));

    public static new readonly StyledProperty<IList?> SelectedItemsProperty =
        AvaloniaProperty.Register<MultiComboBox, IList?>(
            nameof(SelectedItems));

    public static readonly StyledProperty<IDataTemplate?> SelectedItemTemplateProperty =
        AvaloniaProperty.Register<MultiComboBox, IDataTemplate?>(
            nameof(SelectedItemTemplate));

    public static readonly StyledProperty<string?> WatermarkProperty =
        TextBox.WatermarkProperty.AddOwner<MultiComboBox>();

    private Panel? _rootPanel;

    static MultiComboBox()
    {
        FocusableProperty.OverrideDefaultValue<MultiComboBox>(true);
        ItemsPanelProperty.OverrideDefaultValue<MultiComboBox>(DefaultPanel);
        IsDropDownOpenProperty.AffectsPseudoClass<MultiComboBox>(PseudoClassName.FlyoutOpen);
        _ = SelectedItemsProperty.Changed.AddClassHandler<MultiComboBox, IList?>((box, args) => box.OnSelectedItemsChanged(args));
    }

    public MultiComboBox()
    {
        SelectedItems = new AvaloniaList<object>();
        if (SelectedItems is INotifyCollectionChanged c) c.CollectionChanged += OnSelectedItemsCollectionChanged;
    }

    #region SelectedItemContainerTheme

    /// <summary>
    /// Provides SelectedItemContainerTheme Property.
    /// </summary>
    public static readonly StyledProperty<ControlTheme> SelectedItemContainerThemeProperty = AvaloniaProperty.Register<MultiComboBox, ControlTheme>(nameof(SelectedItemContainerTheme));

    /// <summary>
    /// Gets or sets the SelectedItemContainerTheme property.
    /// </summary>
    public ControlTheme SelectedItemContainerTheme
    {
        get => GetValue(SelectedItemContainerThemeProperty);
        set => SetValue(SelectedItemContainerThemeProperty, value);
    }

    #endregion

    #region ShowSelectAll

    /// <summary>
    /// Provides ShowSelectAll Property.
    /// </summary>
    public static readonly StyledProperty<bool> ShowSelectAllProperty = AvaloniaProperty.Register<MultiComboBox, bool>(nameof(ShowSelectAll), true);

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets the ShowSelectAll property.
    /// </summary>
    public bool ShowSelectAll
    {
        get => GetValue(ShowSelectAllProperty);
        set => SetValue(ShowSelectAllProperty, value);
    }

    #endregion

    public bool IsDropDownOpen
    {
        get => GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    public double MaxDropDownHeight
    {
        get => GetValue(MaxDropDownHeightProperty);
        set => SetValue(MaxDropDownHeightProperty, value);
    }

    public double MaxSelectionBoxHeight
    {
        get => GetValue(MaxSelectionBoxHeightProperty);
        set => SetValue(MaxSelectionBoxHeightProperty, value);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Used for binding")]
    public new IList? SelectedItems
    {
        get => GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }

    [InheritDataTypeFromItems(nameof(SelectedItems))]
    public IDataTemplate? SelectedItemTemplate
    {
        get => GetValue(SelectedItemTemplateProperty);
        set => SetValue(SelectedItemTemplateProperty, value);
    }

    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    private void OnSelectedItemsChanged(AvaloniaPropertyChangedEventArgs<IList?> args)
    {
        if (args.OldValue.Value is INotifyCollectionChanged old)
            old.CollectionChanged -= OnSelectedItemsCollectionChanged;
        if (args.NewValue.Value is INotifyCollectionChanged @new)
            @new.CollectionChanged += OnSelectedItemsCollectionChanged;

        RaiseEvent(new SelectionChangedEventArgs(SelectionChangedEvent, args.OldValue.Value!, args.NewValue.Value!) { RoutedEvent = SelectionChangedEvent, Source = this });
    }

    private void OnSelectedItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        PseudoClasses.Set(PseudoClassName.Empty, SelectedItems?.Count is null or 0);
        var containers = Presenter?.Panel?.Children;
        if (containers is null) return;
        foreach (var container in containers)
        {
            if (container is MultiComboBoxItem i)
            {
                i.UpdateSelection();
            }
        }

        RaiseEvent(new SelectionChangedEventArgs(SelectionChangedEvent, e.OldItems!, e.NewItems!) { RoutedEvent = SelectionChangedEvent, Source = this });
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        recycleKey = item;
        return item is not MultiComboBoxItem;
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey) => new MultiComboBoxItem();

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        PointerPressedEvent.RemoveHandler(OnBackgroundPointerPressed, _rootPanel);
        _rootPanel = e.NameScope.Find<Panel>(PartRootPanel);
        PointerPressedEvent.AddHandler(OnBackgroundPointerPressed, _rootPanel);
        PseudoClasses.Set(PseudoClassName.Empty, SelectedItems?.Count == 0);
    }

    private void OnBackgroundPointerPressed(object? sender, PointerPressedEventArgs e) => SetCurrentValue(IsDropDownOpenProperty, !IsDropDownOpen);

    internal void ItemFocused(MultiComboBoxItem dropDownItem)
    {
        if (IsDropDownOpen && dropDownItem.IsFocused && dropDownItem.IsArrangeValid) dropDownItem.BringIntoView();
    }

    public void Remove(object? o)
    {
        if (o is StyledElement s)
        {
            var data = s.DataContext;
            SelectedItems?.Remove(data);
            var item = Items.FirstOrDefault(a => ReferenceEquals(a, data));
            if (item is not null)
            {
                var container = ContainerFromItem(item);
                if (container is MultiComboBoxItem t) t.IsSelected = false;
            }
        }
    }

    public void Clear()
    {
        SelectedItems?.Clear();
        var containers = Presenter?.Panel?.Children;
        if (containers is null) return;
        foreach (var container in containers)
        {
            if (container is MultiComboBoxItem t)
                t.IsSelected = false;
        }
    }

    public void SelectAll()
    {
        SelectedItems = new AvaloniaList<object?>(Items);
        var containers = Presenter?.Panel?.Children;
        if (containers is null) return;
        foreach (var container in containers)
        {
            if (container is MultiComboBoxItem t)
                t.IsSelected = true;
        }
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        if (SelectedItems is INotifyCollectionChanged c) c.CollectionChanged -= OnSelectedItemsCollectionChanged;
    }
}
