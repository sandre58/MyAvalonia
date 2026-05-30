// -----------------------------------------------------------------------
// <copyright file="DataGridComboBoxColumn.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Metadata;

namespace MyNet.Avalonia.Theme.DataGrid;

public class DataGridComboBoxColumn() : DataGridBoundColumn<ComboBox, ContentControl>(global::Avalonia.Controls.Primitives.SelectingItemsControl.SelectedValueProperty, ContentControl.ContentProperty)
{
    private const string FallbackSelectedValuePath = ".";

    public virtual BindingBase? SelectedValueBinding
    {
        get;
        set;
    }

    #region ItemsSource

    /// <summary>
    /// Provides ItemsSource Property.
    /// </summary>
    public static readonly StyledProperty<IEnumerable?> ItemsSourceProperty = ItemsControl.ItemsSourceProperty.AddOwner<DataGridComboBoxColumn>();

    /// <summary>
    /// Gets or sets the ItemsSource property.
    /// </summary>
    public IEnumerable? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    #endregion

    #region DisplayMemberBinding

    /// <summary>
    /// Provides DisplayMemberBinding Property.
    /// </summary>
    public static readonly StyledProperty<BindingBase?> DisplayMemberBindingProperty = ItemsControl.DisplayMemberBindingProperty.AddOwner<DataGridComboBoxColumn>();

    /// <summary>
    /// Gets or sets the DisplayMemberBinding property.
    /// </summary>
    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public BindingBase? DisplayMemberBinding
    {
        get => GetValue(DisplayMemberBindingProperty);
        set => SetValue(DisplayMemberBindingProperty, value);
    }

    #endregion

    protected override void PrepareEditingControl(ComboBox editingElement, RoutedEventArgs editingEventArgs)
    {
        base.PrepareEditingControl(editingElement, editingEventArgs);

        OwningGrid.CellEditEnding += onCellEditEnding;
        editingElement.DropDownClosed += onDropDownClosed;

        // Open the dropdown directly.
        editingElement.IsDropDownOpen = true;

        // Guard: prevent DataGrid from ending edit while dropdown is open.
        void onCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
        {
            if (editingElement.IsDropDownOpen)
                e.Cancel = true;
        }

        void onDropDownClosed(object? sender, EventArgs e)
        {
            OwningGrid.CellEditEnding -= onCellEditEnding;
            editingElement.DropDownClosed -= onDropDownClosed;
        }
    }

    protected override void SynchronizeEditingControlProperties(Control control)
    {
        base.SynchronizeEditingControlProperties(control);

        DataGridHelper.SynchronizeColumnProperty(this, control, ComboBox.PlaceholderTextProperty, PlaceholderTextProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, ItemsControl.ItemTemplateProperty, ContentTemplateProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, ComboBox.SelectionBoxItemTemplateProperty, ContentTemplateProperty);

        DataGridHelper.SynchronizeColumnProperty(this, control, ItemsSourceProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, DisplayMemberBindingProperty);

        if (control is ComboBox comboBox)
            comboBox.SelectedValueBinding = SelectedValueBinding ?? CreateFallbackSelectedValueBinding();
    }

    protected override void ResetValue(ComboBox control, object uneditedValue) => control.SelectedValue = uneditedValue;

    protected override object? GetValue(ComboBox control) => control.SelectedValue;

    private static Binding CreateFallbackSelectedValueBinding() => new(FallbackSelectedValuePath);
}
