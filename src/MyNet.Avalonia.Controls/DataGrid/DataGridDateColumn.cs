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
using MyNet.Avalonia.Controls.Helpers;
using MyNet.Avalonia.Converters;
using MyNet.Utilities;
using MyNet.Utilities.Helpers;
using MyNet.Utilities.Localization;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class DataGridDateColumn : DataGridBoundColumn<CalendarDatePicker, ContentControl>
{
    public DataGridDateColumn()
        : base(CalendarDatePicker.SelectedDateProperty, ContentControl.ContentProperty) => Format = nameof(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);

    #region FirstDayOfWeek

    /// <summary>
    /// Provides FirstDayOfWeek Property.
    /// </summary>
    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty = Primitives.DatePickerBase.FirstDayOfWeekProperty.AddOwner<DataGridDateColumn>();

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
    public static readonly StyledProperty<string?> FormatProperty = Primitives.DatePickerBase.DisplayFormatProperty.AddOwner<DataGridDateColumn>();

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

        DataGridHelper.SynchronizeColumnProperty(this, control, Primitives.DatePickerBase.WatermarkProperty, WatermarkProperty);

        DataGridHelper.SynchronizeColumnProperty(this, control, FirstDayOfWeekProperty);
        ((CalendarDatePicker)control).DisplayFormat = DateTimeHelper.TranslateDatePattern(Format.OrEmpty(), CultureInfo.CurrentCulture);
    }

    protected override void ResetValue(CalendarDatePicker control, object uneditedValue) => control.SelectedDate = (DateTime?)uneditedValue;

    protected override object? GetValue(CalendarDatePicker control) => control.SelectedDate;

    private void SynchronizeDataTemplate(Control element)
    {
        if (element is not ContentControl { ContentTemplate: null } contentControl)
            return;
        var dataTemplate = new FuncDataTemplate<DateTime?>((_, _) =>
        {
            var item = new TextBlock
            {
                [!TextBlock.TextProperty] = new Binding { Converter = new DateTimeConverter(DateTimeConverter.DateTimeConverterKind.Default, Humanizer.LetterCasing.Title), ConverterParameter = Format }
            };
            applyBinding(null, EventArgs.Empty);

            GlobalizationService.Current.CultureChanged -= applyBinding;
            GlobalizationService.Current.CultureChanged += applyBinding;
            return item;

            void applyBinding(object? sender, EventArgs e) => item[!TextBlock.TextProperty] = new Binding { Converter = new DateTimeConverter(DateTimeConverter.DateTimeConverterKind.Default, Humanizer.LetterCasing.Title), ConverterParameter = Format };
        });

        contentControl.ContentTemplate = dataTemplate;
    }
}
