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
using MyNet.Humanizer;
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
        contentControl.ContentTemplate = new FuncDataTemplate<TimeSpan?>((_, _) => new TextBlock
        {
            [!TextBlock.TextProperty] = new MultiBinding
            {
                Converter = new DateTimeConverter(DateTimeConverterKind.Default, LetterCasing.Title),
                ConverterParameter = DisplayFormat,
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
