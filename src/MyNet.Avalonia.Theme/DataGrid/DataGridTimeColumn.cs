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
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Converters;
using MyNet.Utilities.Localization;
using TimePicker = MyNet.Avalonia.Controls.TimePicker;

namespace MyNet.Avalonia.Theme.DataGrid;

public class DataGridTimeColumn : DataGridBoundColumn<TimePicker, ContentControl>
{
    public DataGridTimeColumn()
        : base(TimePicker.SelectedValueProperty, ContentControl.ContentProperty)
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

    protected override void ResetValue(TimePicker control, object uneditedValue) => control.SelectedValue = (TimeSpan?)uneditedValue;

    protected override object? GetValue(TimePicker control) => control.SelectedValue;

    private void SynchronizeDataTemplate(Control element)
    {
        if (element is not ContentControl { ContentTemplate: null } contentControl)
            return;
        var dataTemplate = new FuncDataTemplate<TimeSpan?>((_, _) =>
        {
            var item = new TextBlock
            {
                [!TextBlock.TextProperty] = new Binding { Converter = new DateTimeConverter(DateTimeConverter.DateTimeConverterKind.Default, Humanizer.LetterCasing.Title), ConverterParameter = DisplayFormat }
            };
            applyBinding(null, EventArgs.Empty);

            GlobalizationService.Current.CultureChanged -= applyBinding;
            GlobalizationService.Current.CultureChanged += applyBinding;
            return item;

            void applyBinding(object? sender, EventArgs e) => item[!TextBlock.TextProperty] = new Binding { Converter = new DateTimeConverter(DateTimeConverter.DateTimeConverterKind.Default, Humanizer.LetterCasing.Title), ConverterParameter = DisplayFormat };
        });

        contentControl.ContentTemplate = dataTemplate;
    }
}
