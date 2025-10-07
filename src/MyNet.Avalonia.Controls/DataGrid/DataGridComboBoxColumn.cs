// -----------------------------------------------------------------------
// <copyright file="DataGridComboBoxColumn.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Metadata;
using MyNet.Avalonia.Controls.Helpers;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class DataGridComboBoxColumn : DataGridBoundColumn<ComboBox, ContentControl>
{
    private IBinding? _selectedValueBinding;

    public DataGridComboBoxColumn()
        : base(global::Avalonia.Controls.Primitives.SelectingItemsControl.SelectedItemProperty, ContentControl.ContentProperty) { }

    public virtual IBinding? SelectedValueBinding
    {
        get => _selectedValueBinding;
        set
        {
            if (_selectedValueBinding == value)
                return;
            _selectedValueBinding = value;
            BindingTarget = global::Avalonia.Controls.Primitives.SelectingItemsControl.SelectedValueProperty;
        }
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
    public static readonly StyledProperty<IBinding?> DisplayMemberBindingProperty = ItemsControl.DisplayMemberBindingProperty.AddOwner<DataGridComboBoxColumn>();

    /// <summary>
    /// Gets or sets the DisplayMemberBinding property.
    /// </summary>
    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? DisplayMemberBinding
    {
        get => GetValue(DisplayMemberBindingProperty);
        set => SetValue(DisplayMemberBindingProperty, value);
    }

    #endregion

    protected override void PrepareEditingControl(ComboBox editingElement, RoutedEventArgs editingEventArgs)
    {
        base.PrepareEditingControl(editingElement, editingEventArgs);

        editingElement.IsDropDownOpen = true;
    }

    protected override void SynchronizeEditingControlProperties(Control control)
    {
        base.SynchronizeEditingControlProperties(control);

        DataGridHelper.SynchronizeColumnProperty(this, control, ComboBox.PlaceholderTextProperty, WatermarkProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, ItemsControl.ItemTemplateProperty, ContentTemplateProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, ComboBox.SelectionBoxItemTemplateProperty, ContentTemplateProperty);

        DataGridHelper.SynchronizeColumnProperty(this, control, ItemsSourceProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, DisplayMemberBindingProperty);

        if (control is ComboBox comboBox && SelectedValueBinding is not null)
            comboBox.SelectedValueBinding = SelectedValueBinding;
    }

    protected override void ResetValue(ComboBox control, object uneditedValue)
    {
        if (SelectedValueBinding != null)
        {
            control.SelectedValue = uneditedValue;
        }
        else
        {
            control.SelectedItem = uneditedValue;
        }
    }

    protected override object? GetValue(ComboBox control) => SelectedValueBinding is not null ? control.SelectedValue : control.SelectedItem;
}
