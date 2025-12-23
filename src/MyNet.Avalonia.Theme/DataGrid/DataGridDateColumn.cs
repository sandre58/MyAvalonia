// -----------------------------------------------------------------------
// <copyright file="DataGridDateColumn.cs" company="Stéphane ANDRE">
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
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Converters;
using MyNet.Humanizer;
using MyNet.Utilities;
using MyNet.Utilities.Helpers;
using MyNet.Utilities.Localization;
using CalendarDatePicker = MyNet.Avalonia.Controls.CalendarDatePicker;

namespace MyNet.Avalonia.Theme.DataGrid;

public class DataGridDateColumn : DataGridBoundColumn<CalendarDatePicker, ContentControl>
{
    public DataGridDateColumn()
        : base(CalendarDatePicker.SelectedValueProperty, ContentControl.ContentProperty) => Format = nameof(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);

    #region FirstDayOfWeek

    /// <summary>
    /// Provides FirstDayOfWeek Property.
    /// </summary>
    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty = DatePickerBase.FirstDayOfWeekProperty.AddOwner<DataGridDateColumn>();

    /// <summary>
    /// Gets or sets the FirstDayOfWeek property.
    /// </summary>
    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    #endregion

    #region Format

    /// <summary>
    /// Provides Format Property.
    /// </summary>
    public static readonly StyledProperty<string?> FormatProperty = DatePickerBase.DisplayFormatProperty.AddOwner<DataGridDateColumn>();

    /// <summary>
    /// Gets or sets the Format property.
    /// </summary>
    public string? Format
    {
        get => GetValue(FormatProperty);
        set => SetValue(FormatProperty, value);
    }

    #endregion

    protected override void PrepareEditingControl(CalendarDatePicker editingElement, RoutedEventArgs editingEventArgs)
    {
        base.PrepareEditingControl(editingElement, editingEventArgs);

        editingElement.IsDropDownOpen = true;
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

        DataGridHelper.SynchronizeColumnProperty(this, control, DatePickerBase.WatermarkProperty, WatermarkProperty);

        DataGridHelper.SynchronizeColumnProperty(this, control, FirstDayOfWeekProperty);
        ((CalendarDatePicker)control).DisplayFormat = DateTimeHelper.TranslateDatePattern(Format.OrEmpty(), CultureInfo.CurrentCulture);
    }

    protected override void ResetValue(CalendarDatePicker control, object uneditedValue) => control.SelectedValue = (DateTime?)uneditedValue;

    protected override object? GetValue(CalendarDatePicker control) => control.SelectedValue;

    private void SynchronizeDataTemplate(Control element)
    {
        if (element is not ContentControl { ContentTemplate: null } contentControl)
            return;
        contentControl.ContentTemplate = new FuncDataTemplate<TimeSpan?>((_, _) => new TextBlock
        {
            [!TextBlock.TextProperty] = new MultiBinding
            {
                Converter = new DateTimeConverter(DateTimeConverterKind.Default, LetterCasing.Title),
                ConverterParameter = Format,
                Mode = BindingMode.OneWay,
                Bindings =
                {
                    new Binding
                    {
                        Mode = BindingMode.OneWay,
                    },
                    new Binding
                    {
                        Source = UIContext.Globalization,
                        Path = nameof(UIContext.Globalization.Culture),
                        Mode = BindingMode.OneWay
                    },
                    new Binding
                    {
                        Source = UIContext.Globalization,
                        Path = nameof(UIContext.Globalization.TimeZone),
                        Mode = BindingMode.OneWay
                    }
                }
            }
        });
    }
}
