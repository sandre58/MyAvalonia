// -----------------------------------------------------------------------
// <copyright file="DataGridTimeColumn.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Interactivity;
using MyNet.Avalonia.Bindings;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Converters;
using MyNet.Text.TextCasing;
using TimePickerEx = MyNet.Avalonia.Controls.TimePickerEx;

namespace MyNet.Avalonia.Theme.Controls.DataGrid;

public class DataGridTimeColumn : DataGridBoundColumn<TimePickerEx, ContentControl>
{
    public DataGridTimeColumn()
        : base(TimePickerEx.SelectedValueProperty, ContentControl.ContentProperty)
    {
        DisplayFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
        PanelFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.Replace(":", " ", StringComparison.OrdinalIgnoreCase);
    }

    #region DisplayFormat

    /// <summary>
    /// Provides DisplayFormat Property.
    /// </summary>
    public static readonly StyledProperty<string?> DisplayFormatProperty = TimePickerBase.DisplayFormatProperty.AddOwner<DataGridTimeColumn>();

    public string? DisplayFormat
    {
        get => GetValue(DisplayFormatProperty);
        set => SetValue(DisplayFormatProperty, value);
    }

    #endregion

    #region PanelFormat

    /// <summary>
    /// Provides DisplayFormat Property.
    /// </summary>
    public static readonly StyledProperty<string> PanelFormatProperty = TimePickerBase.PanelFormatProperty.AddOwner<DataGridTimeColumn>();

    public string PanelFormat
    {
        get => GetValue(PanelFormatProperty);
        set => SetValue(PanelFormatProperty, value);
    }

    #endregion

    protected override void PrepareEditingControl(TimePickerEx editingElement, RoutedEventArgs editingEventArgs)
    {
        base.PrepareEditingControl(editingElement, editingEventArgs);

        OwningGrid.CellEditEnding += onCellEditEnding;
        editingElement.DetachedFromVisualTree += cleanup;

        // Open the dropdown directly.
        editingElement.IsDropDownOpen = true;

        // Guard: prevent DataGrid from ending edit while dropdown is open.
        void onCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
        {
            if (editingElement.IsDropDownOpen)
                e.Cancel = true;
        }

        void cleanup(object? sender, VisualTreeAttachmentEventArgs e)
        {
            OwningGrid.CellEditEnding -= onCellEditEnding;
            editingElement.DetachedFromVisualTree -= cleanup;
        }
    }

    protected override Control GenerateElement(DataGridCell cell, object dataItem)
    {
        var element = base.GenerateElement(cell, dataItem);
        SynchronizeDataTemplate(element);

        return element;
    }

    protected override void RefreshCellContent(Control? element, string propertyName)
    {
        base.RefreshCellContent(element, propertyName);

        if (element is not null && propertyName == nameof(ContentTemplate))
            SynchronizeDataTemplate(element);
    }

    protected override void SynchronizeEditingControlProperties(Control control)
    {
        base.SynchronizeEditingControlProperties(control);

        DataGridHelper.SynchronizeColumnProperty(this, control, PanelFormatProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, DisplayFormatProperty);
    }

    protected override void ResetValue(TimePickerEx control, object uneditedValue) => control.SelectedValue = (TimeSpan?)uneditedValue;

    protected override object? GetValue(TimePickerEx control) => control.SelectedValue;

    private void SynchronizeDataTemplate(Control element)
    {
        if (element is not ContentControl { ContentTemplate: null } contentControl)
            return;
        contentControl.ContentTemplate = new FuncDataTemplate<TimeSpan?>((_, _) => new TextBlock { [!TextBlock.TextProperty] = new MultiBinding { Converter = new DateTimeConverter(DateTimeConverterKind.Default, LetterCasing.Title), ConverterParameter = DisplayFormat, Mode = BindingMode.OneWay, Bindings = { CreateObjectBinding(), CreateCultureBinding(), CreateTimeZoneBinding() } } });
    }

    private static CompiledBinding CreateCultureBinding() => GlobalizationBinding.CreateCultureBinding();

    private static CompiledBinding CreateTimeZoneBinding() => GlobalizationBinding.CreateTimeZoneBinding();

    private static CompiledBinding CreateObjectBinding()
    {
        var binding = CompiledBinding.Create<object, object?>(x => x);
        binding.Mode = BindingMode.OneWay;
        return binding;
    }
}
