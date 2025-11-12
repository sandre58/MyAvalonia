// -----------------------------------------------------------------------
// <copyright file="MultiComboBox.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Selection;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Metadata;
using Avalonia.Styling;
using Avalonia.VisualTree;
using MyNet.Avalonia.Extensions;
using MyNet.Utilities;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// This control inherits from <see cref="SelectingItemsControl"/>, but it only supports MVVM pattern.
/// </summary>
[TemplatePart(PartPopup, typeof(Popup), IsRequired = true)]
[PseudoClasses(PseudoClassName.FlyoutOpen, PseudoClassName.Empty, PseudoClassName.Pressed)]
public class MultiComboBox : SelectingItemsControl
{
    public const string PartRootPanel = "PART_RootPanel";
    public const string PartPopup = "PART_Popup";

    private static readonly ITemplate<Panel?> DefaultPanel = new FuncTemplate<Panel?>(() => new VirtualizingStackPanel());

    public static readonly StyledProperty<bool> IsDropDownOpenProperty = ComboBox.IsDropDownOpenProperty.AddOwner<MultiComboBox>();

    public static readonly StyledProperty<double> MaxDropDownHeightProperty = AvaloniaProperty.Register<MultiComboBox, double>(nameof(MaxDropDownHeight));

    public static readonly StyledProperty<double> MaxSelectionBoxHeightProperty = AvaloniaProperty.Register<MultiComboBox, double>(nameof(MaxSelectionBoxHeight));

    [System.Diagnostics.CodeAnalysis.SuppressMessage("AvaloniaProperty", "AVP1010", Justification = "This property is owned by SelectingItemsControl, but protected there. ListBox changes its visibility.")]
    public static new readonly DirectProperty<SelectingItemsControl, IList?> SelectedItemsProperty = SelectingItemsControl.SelectedItemsProperty;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("AvaloniaProperty", "AVP1010", Justification = "This property is owned by SelectingItemsControl, but protected there. ListBox changes its visibility.")]
    public static new readonly DirectProperty<SelectingItemsControl, ISelectionModel> SelectionProperty = SelectingItemsControl.SelectionProperty;

    public static readonly StyledProperty<IDataTemplate?> SelectedItemTemplateProperty = AvaloniaProperty.Register<MultiComboBox, IDataTemplate?>(nameof(SelectedItemTemplate));

    public static readonly StyledProperty<string?> WatermarkProperty = TextBox.WatermarkProperty.AddOwner<MultiComboBox>();

    private readonly CompositeDisposable _subscriptionsOnOpen = [];
    private Popup? _popup;

    static MultiComboBox()
    {
        SelectionModeProperty.OverrideDefaultValue<MultiComboBox>(SelectionMode.Toggle | SelectionMode.Multiple);
        FocusableProperty.OverrideDefaultValue<MultiComboBox>(true);
        ItemsPanelProperty.OverrideDefaultValue<MultiComboBox>(DefaultPanel);
        IsDropDownOpenProperty.AffectsPseudoClass<MultiComboBox>(PseudoClassName.FlyoutOpen);
        KeyboardNavigation.TabNavigationProperty.OverrideDefaultValue<MultiComboBox>(KeyboardNavigationMode.Once);
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
        get => base.SelectedItems;
        set => base.SelectedItems = value;
    }

    public new ISelectionModel Selection
    {
        get => base.Selection;
        set => base.Selection = value;
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

    public event EventHandler? DropDownClosed;

    public event EventHandler? DropDownOpened;

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey) => NeedsContainer<MultiComboBoxItem>(item, out recycleKey);

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey) => new MultiComboBoxItem();

    /// <inheritdoc/>
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (!e.Handled && e.Source is Visual source)
        {
            if (_popup?.IsInsidePopup(source) == true)
            {
                e.Handled = true;
                return;
            }
        }

        if (IsDropDownOpen)
        {
            // When a drop-down is open with OverlayDismissEventPassThrough enabled and the control
            // is pressed, close the drop-down
            SetCurrentValue(IsDropDownOpenProperty, false);
            e.Handled = true;
        }
        else
        {
            PseudoClasses.Set(PseudoClassName.Pressed, true);
        }
    }

    /// <inheritdoc/>
    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        if (!e.Handled && e.Source is Visual source)
        {
            if (_popup?.IsInsidePopup(source) == true)
            {
                if (UpdateSelectionFromEventSource(e.Source))
                {
                    e.Handled = true;
                }
            }
            else if (PseudoClasses.Contains(PseudoClassName.Pressed))
            {
                SetCurrentValue(IsDropDownOpenProperty, !IsDropDownOpen);
                e.Handled = true;
            }
        }

        PseudoClasses.Set(PseudoClassName.Pressed, false);
        base.OnPointerReleased(e);
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        if (_popup != null)
        {
            _popup.Opened -= PopupOpened;
            _popup.Closed -= PopupClosed;
        }

        _popup = e.NameScope.Get<Popup>("PART_Popup");
        _popup.Opened += PopupOpened;
        _popup.Closed += PopupClosed;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Handled)
            return;

        if (!IsDropDownOpen && e.Key is Key.F4 or Key.Down or Key.Enter or Key.Space)
        {
            SetCurrentValue(IsDropDownOpenProperty, true);
            e.Handled = true;
        }
        else if (IsDropDownOpen)
        {
            var hotkeys = Application.Current!.PlatformSettings?.HotkeyConfiguration;
            var ctrl = hotkeys is not null && e.KeyModifiers.HasFlag(hotkeys.CommandModifiers);

            if (e.Key is Key.Escape or Key.Tab or Key.F4)
            {
                SetCurrentValue(IsDropDownOpenProperty, false);
                e.Handled = true;
            }

            // This part of code is needed just to acquire initial focus, subsequent focus navigation will be done by ItemsControl.
            else if (SelectedIndex < 0 && ItemCount > 0 && (e.Key == Key.Up || e.Key == Key.Down) && IsFocused)
            {
                var firstChild = Presenter?.Panel?.Children.FirstOrDefault(CanFocus);
                if (firstChild != null)
                {
                    e.Handled = firstChild.Focus(NavigationMethod.Directional);
                }
            }
            else if (!ctrl && e.Key.ToNavigationDirection() is { } direction && direction.IsDirectional())
            {
                e.Handled |= MoveSelection(direction, WrapSelection, e.KeyModifiers.HasFlag(KeyModifiers.Shift));
            }
            else if (SelectionMode.HasFlag(SelectionMode.Multiple) && hotkeys?.SelectAll.Any(x => x.Matches(e)) == true)
            {
                Selection.SelectAll();
                e.Handled = true;
            }
            else if (e.Key == Key.Space || e.Key == Key.Enter)
            {
                UpdateSelectionFromEventSource(e.Source, true, e.KeyModifiers.HasFlag(KeyModifiers.Shift), ctrl);
            }
        }
    }

    internal void ItemFocused(MultiComboBoxItem dropDownItem)
    {
        if (IsDropDownOpen && dropDownItem.IsFocused && dropDownItem.IsArrangeValid) dropDownItem.BringIntoView();
    }

    public void SelectAll() => Selection.SelectAll();

    public void UnselectAll() => Selection.Clear();

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

    private void PopupClosed(object? sender, EventArgs e)
    {
        _subscriptionsOnOpen.Clear();

        if (CanFocus(this))
        {
            Focus();
        }

        DropDownClosed?.Invoke(this, EventArgs.Empty);
    }

    private void PopupOpened(object? sender, EventArgs e)
    {
        TryFocusSelectedItem();

        _subscriptionsOnOpen.Clear();

        this.GetObservable(IsVisibleProperty).Subscribe(IsVisibleChanged).DisposeWith(_subscriptionsOnOpen);

        foreach (var parent in this.GetVisualAncestors().OfType<Control>())
        {
            parent.GetObservable(IsVisibleProperty).Subscribe(IsVisibleChanged).DisposeWith(_subscriptionsOnOpen);
        }

        DropDownOpened?.Invoke(this, EventArgs.Empty);
    }

    private void IsVisibleChanged(bool isVisible)
    {
        if (!isVisible && IsDropDownOpen)
        {
            SetCurrentValue(IsDropDownOpenProperty, false);
        }
    }

    private void TryFocusSelectedItem()
    {
        var selectedIndex = SelectedIndex;
        if (IsDropDownOpen && selectedIndex != -1)
        {
            var container = ContainerFromIndex(selectedIndex);

            if (container == null && SelectedIndex != -1)
            {
                ScrollIntoView(Selection.SelectedIndex);
                container = ContainerFromIndex(selectedIndex);
            }

            if (container != null && CanFocus(container))
            {
                container.Focus();
            }
        }
    }

    private bool CanFocus(Control control) => control.Focusable && control.IsEffectivelyEnabled && control.IsVisible;
}
