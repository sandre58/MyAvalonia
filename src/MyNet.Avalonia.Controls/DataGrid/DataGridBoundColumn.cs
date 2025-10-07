// -----------------------------------------------------------------------
// <copyright file="DataGridBoundColumn.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using MyNet.Avalonia.Controls.Assists;
using MyNet.Avalonia.Controls.Helpers;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public abstract class DataGridBoundColumn<TEditingControl, TValueControl> : DataGridBoundColumn
    where TEditingControl : Control, new()
    where TValueControl : Control, new()
{
    private readonly Lazy<ControlTheme?> _cellEditingControlTheme;
    private readonly Lazy<ControlTheme?> _cellValueControlTheme;

    protected DataGridBoundColumn(AvaloniaProperty bindingTarget, AvaloniaProperty bindingValue)
        : this($"MyNet.Theme.{typeof(TEditingControl).Name}.Embedded.DataGrid", $"MyNet.Theme.{typeof(TValueControl).Name}.Embedded.DataGrid", bindingTarget, bindingValue) { }

    protected DataGridBoundColumn(string editingControlThemeKey, string valueControlThemeKey, AvaloniaProperty bindingTarget, AvaloniaProperty bindingValue)
    {
        BindingTarget = bindingTarget;
        BindingValue = bindingValue;
        _cellEditingControlTheme = new Lazy<ControlTheme?>(() => !OwningGrid.TryFindResource(editingControlThemeKey, out var value2) ? null : (ControlTheme?)value2);
        _cellValueControlTheme = new Lazy<ControlTheme?>(() => !OwningGrid.TryFindResource(valueControlThemeKey, out var value) ? null : (ControlTheme?)value);
        HorizontalAlignment = HorizontalAlignment.Left;
        VerticalAlignment = VerticalAlignment.Center;
    }

    protected AvaloniaProperty BindingValue { get; set; }

    #region Font

    [SuppressMessage("AvaloniaProperty", "AVP1002", Justification = "Generic avalonia property is expected here.")]
    [SuppressMessage("Roslynator", "RCS1158:Static member in generic type should use a type parameter", Justification = "Generic avalonia property is expected here.")]
    public static readonly AttachedProperty<FontFamily> FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner<DataGridBoundColumn<TEditingControl, TValueControl>>();

    [SuppressMessage("AvaloniaProperty", "AVP1002", Justification = "Generic avalonia property is expected here.")]
    [SuppressMessage("Roslynator", "RCS1158:Static member in generic type should use a type parameter", Justification = "Generic avalonia property is expected here.")]
    public static readonly AttachedProperty<double> FontSizeProperty = TextElement.FontSizeProperty.AddOwner<DataGridBoundColumn<TEditingControl, TValueControl>>();

    [SuppressMessage("AvaloniaProperty", "AVP1002", Justification = "Generic avalonia property is expected here.")]
    [SuppressMessage("Roslynator", "RCS1158:Static member in generic type should use a type parameter", Justification = "Generic avalonia property is expected here.")]
    public static readonly AttachedProperty<FontStyle> FontStyleProperty = TextElement.FontStyleProperty.AddOwner<DataGridBoundColumn<TEditingControl, TValueControl>>();

    [SuppressMessage("AvaloniaProperty", "AVP1002", Justification = "Generic avalonia property is expected here.")]
    [SuppressMessage("Roslynator", "RCS1158:Static member in generic type should use a type parameter", Justification = "Generic avalonia property is expected here.")]
    public static readonly AttachedProperty<FontWeight> FontWeightProperty = TextElement.FontWeightProperty.AddOwner<DataGridBoundColumn<TEditingControl, TValueControl>>();

    [SuppressMessage("AvaloniaProperty", "AVP1002", Justification = "Generic avalonia property is expected here.")]
    [SuppressMessage("Roslynator", "RCS1158:Static member in generic type should use a type parameter", Justification = "Generic avalonia property is expected here.")]
    public static readonly AttachedProperty<FontStretch> FontStretchProperty = TextElement.FontStretchProperty.AddOwner<DataGridBoundColumn<TEditingControl, TValueControl>>();

    [SuppressMessage("AvaloniaProperty", "AVP1002", Justification = "Generic avalonia property is expected here.")]
    [SuppressMessage("Roslynator", "RCS1158:Static member in generic type should use a type parameter", Justification = "Generic avalonia property is expected here.")]
    public static readonly AttachedProperty<IBrush?> ForegroundProperty = TextElement.ForegroundProperty.AddOwner<DataGridBoundColumn<TEditingControl, TValueControl>>();

    public FontFamily FontFamily
    {
        get => GetValue(FontFamilyProperty); set => SetValue(FontFamilyProperty, value);
    }

    [DefaultValue(double.NaN)]
    public double FontSize
    {
        get => GetValue(FontSizeProperty); set => SetValue(FontSizeProperty, value);
    }

    public FontStyle FontStyle
    {
        get => GetValue(FontStyleProperty); set => SetValue(FontStyleProperty, value);
    }

    public FontWeight FontWeight
    {
        get => GetValue(FontWeightProperty); set => SetValue(FontWeightProperty, value);
    }

    public FontStretch FontStretch
    {
        get => GetValue(FontStretchProperty); set => SetValue(FontStretchProperty, value);
    }

    public IBrush? Foreground
    {
        get => GetValue(ForegroundProperty); set => SetValue(ForegroundProperty, value);
    }

    #endregion

    #region Watermark

    /// <summary>
    /// Provides Watermark Property.
    /// </summary>
    [SuppressMessage("AvaloniaProperty", "AVP1002", Justification = "Generic avalonia property is expected here.")]
    [SuppressMessage("Roslynator", "RCS1158:Static member in generic type should use a type parameter", Justification = "Generic avalonia property is expected here.")]
    public static readonly AttachedProperty<string?> WatermarkProperty = TextFieldAssist.WatermarkProperty.AddOwner<DataGridBoundColumn<TEditingControl, TValueControl>>();

    /// <summary>
    /// Gets or sets the Watermark property.
    /// </summary>
    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    #endregion

    #region InnerLeftContent

    /// <summary>
    /// Provides InnerLeftContent Property.
    /// </summary>
    [SuppressMessage("AvaloniaProperty", "AVP1002", Justification = "Generic avalonia property is expected here.")]
    [SuppressMessage("Roslynator", "RCS1158:Static member in generic type should use a type parameter", Justification = "Generic avalonia property is expected here.")]
    public static readonly AttachedProperty<object?> InnerLeftContentProperty = TextFieldAssist.InnerLeftContentProperty.AddOwner<DataGridBoundColumn<TEditingControl, TValueControl>>();

    /// <summary>
    /// Gets or sets the InnerLeftContent property.
    /// </summary>
    public object? InnerLeftContent
    {
        get => GetValue(InnerLeftContentProperty);
        set => SetValue(InnerLeftContentProperty, value);
    }

    #endregion

    #region InnerRightContent

    /// <summary>
    /// Provides InnerRightContent Property.
    /// </summary>
    [SuppressMessage("AvaloniaProperty", "AVP1002", Justification = "Generic avalonia property is expected here.")]
    [SuppressMessage("Roslynator", "RCS1158:Static member in generic type should use a type parameter", Justification = "Generic avalonia property is expected here.")]
    public static readonly AttachedProperty<object?> InnerRightContentProperty = TextFieldAssist.InnerLeftContentProperty.AddOwner<DataGridBoundColumn<TEditingControl, TValueControl>>();

    /// <summary>
    /// Gets or sets the InnerRightContent property.
    /// </summary>
    public object? InnerRightContent
    {
        get => GetValue(InnerRightContentProperty);
        set => SetValue(InnerRightContentProperty, value);
    }

    #endregion

    #region Icon

    /// <summary>
    /// Provides Icon Property.
    /// </summary>
    [SuppressMessage("AvaloniaProperty", "AVP1002", Justification = "Generic avalonia property is expected here.")]
    [SuppressMessage("Roslynator", "RCS1158:Static member in generic type should use a type parameter", Justification = "Generic avalonia property is expected here.")]
    public static readonly StyledProperty<object?> IconProperty = IconAssist.IconProperty.AddOwner<DataGridBoundColumn<TEditingControl, TValueControl>>();

    /// <summary>
    /// Gets or sets the Icon property.
    /// </summary>
    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    #endregion

    #region ShowClearButton

    /// <summary>
    /// Provides ShowClearButton Property.
    /// </summary>
    [SuppressMessage("AvaloniaProperty", "AVP1002", Justification = "Generic avalonia property is expected here.")]
    [SuppressMessage("Roslynator", "RCS1158:Static member in generic type should use a type parameter", Justification = "Generic avalonia property is expected here.")]
    public static readonly StyledProperty<bool> ShowClearButtonProperty = TextFieldAssist.ShowClearButtonProperty.AddOwner<DataGridBoundColumn<TEditingControl, TValueControl>>();

    /// <summary>
    /// Gets or sets a value indicating whether we want show clear button.
    /// </summary>
    public bool ShowClearButton
    {
        get => GetValue(ShowClearButtonProperty);
        set => SetValue(ShowClearButtonProperty, value);
    }

    #endregion

    #region ShowClipboardButton

    /// <summary>
    /// Provides ShowClipboardButton Property.
    /// </summary>
    [SuppressMessage("AvaloniaProperty", "AVP1002", Justification = "Generic avalonia property is expected here.")]
    [SuppressMessage("Roslynator", "RCS1158:Static member in generic type should use a type parameter", Justification = "Generic avalonia property is expected here.")]
    public static readonly StyledProperty<bool> ShowClipboardButtonProperty = TextFieldAssist.ShowClipboardButtonProperty.AddOwner<DataGridBoundColumn<TEditingControl, TValueControl>>();

    /// <summary>
    /// Gets or sets a value indicating whether we want show clipboard button.
    /// </summary>
    public bool ShowClipboardButton
    {
        get => GetValue(ShowClipboardButtonProperty);
        set => SetValue(ShowClipboardButtonProperty, value);
    }

    #endregion

    #region HorizontalAlignment

    /// <summary>
    /// Provides HorizontalAlignment Property.
    /// </summary>
    [SuppressMessage("AvaloniaProperty", "AVP1002", Justification = "Generic avalonia property is expected here.")]
    [SuppressMessage("Roslynator", "RCS1158:Static member in generic type should use a type parameter", Justification = "Generic avalonia property is expected here.")]
    public static readonly StyledProperty<HorizontalAlignment> HorizontalAlignmentProperty = Layoutable.HorizontalAlignmentProperty.AddOwner<DataGridBoundColumn<TEditingControl, TValueControl>>();

    /// <summary>
    /// Gets or sets the HorizontalAlignment property.
    /// </summary>
    public HorizontalAlignment HorizontalAlignment
    {
        get => GetValue(HorizontalAlignmentProperty);
        set => SetValue(HorizontalAlignmentProperty, value);
    }

    #endregion

    #region VerticalAlignment

    /// <summary>
    /// Provides VerticalAlignment Property.
    /// </summary>
    [SuppressMessage("AvaloniaProperty", "AVP1002", Justification = "Generic avalonia property is expected here.")]
    [SuppressMessage("Roslynator", "RCS1158:Static member in generic type should use a type parameter", Justification = "Generic avalonia property is expected here.")]
    public static readonly StyledProperty<VerticalAlignment> VerticalAlignmentProperty = Layoutable.VerticalAlignmentProperty.AddOwner<DataGridBoundColumn<TEditingControl, TValueControl>>();

    /// <summary>
    /// Gets or sets the VerticalAlignment property.
    /// </summary>
    public VerticalAlignment VerticalAlignment
    {
        get => GetValue(VerticalAlignmentProperty);
        set => SetValue(VerticalAlignmentProperty, value);
    }

    #endregion

    #region ContentTemplate

    /// <summary>
    /// Provides ContentTemplate Property.
    /// </summary>
    [SuppressMessage("AvaloniaProperty", "AVP1002", Justification = "Generic avalonia property is expected here.")]
    [SuppressMessage("Roslynator", "RCS1158:Static member in generic type should use a type parameter", Justification = "Generic avalonia property is expected here.")]
    public static readonly StyledProperty<IDataTemplate?> ContentTemplateProperty = AvaloniaProperty.Register<DataGridBoundColumn<TEditingControl, TValueControl>, IDataTemplate?>(nameof(ContentTemplate));

    /// <summary>
    /// Gets or sets the ContentTemplate property.
    /// </summary>
    public IDataTemplate? ContentTemplate
    {
        get => GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }

    #endregion

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == FontFamilyProperty
            || change.Property == FontSizeProperty
            || change.Property == FontStyleProperty
            || change.Property == FontWeightProperty
            || change.Property == ForegroundProperty)
        {
            NotifyPropertyChanged(change.Property.Name);
        }
    }

    protected override void CancelCellEdit(Control editingElement, object uneditedValue)
    {
        if (editingElement is TEditingControl control)
            ResetValue(control, uneditedValue);
    }

    protected override Control GenerateEditingElementDirect(DataGridCell cell, object dataItem)
    {
        var control = CreateEditionControl();
        control.Name = $"Cell{control.GetType().Name}";

        var value = _cellEditingControlTheme.Value;
        if (value != null)
            control.Theme = value;

        SynchronizeEditingControlProperties(control);

        return control;
    }

    protected override Control GenerateElement(DataGridCell cell, object dataItem)
    {
        var control = CreateControl();
        control.Name = $"Cell{control.GetType().Name}";

        var value = _cellValueControlTheme.Value;
        if (value != null)
            control.Theme = value;

        SynchronizeValueProperties(control);

        if (Binding != null)
            _ = control.Bind(BindingValue, Binding);

        return control;
    }

    protected override object? PrepareCellForEdit(Control editingElement, RoutedEventArgs editingEventArgs)
    {
        if (editingElement is not TEditingControl control) return null;

        PrepareEditingControl(control, editingEventArgs);

        return GetValue(control);
    }

    protected virtual void PrepareEditingControl(TEditingControl editingElement, RoutedEventArgs editingEventArgs) { }

    protected abstract void ResetValue(TEditingControl control, object uneditedValue);

    protected abstract object? GetValue(TEditingControl control);

    protected override void RefreshCellContent(Control? element, string propertyName)
    {
        if (element is null)
            return;

        switch (propertyName)
        {
            case "FontFamily":
                DataGridHelper.SynchronizeColumnProperty(this, element, FontFamilyProperty);
                break;
            case "FontSize":
                DataGridHelper.SynchronizeColumnProperty(this, element, FontSizeProperty);
                break;
            case "FontStyle":
                DataGridHelper.SynchronizeColumnProperty(this, element, FontStyleProperty);
                break;
            case "FontWeight":
                DataGridHelper.SynchronizeColumnProperty(this, element, FontWeightProperty);
                break;
            case "Foreground":
                DataGridHelper.SynchronizeColumnProperty(this, element, ForegroundProperty);
                break;
            default:
                break;
        }
    }

    protected virtual TEditingControl CreateEditionControl() => new();

    protected virtual TValueControl CreateControl() => new();

    protected virtual void SynchronizeEditingControlProperties(Control control)
    {
        SynchronizeFontProperties(control);
        DataGridHelper.SynchronizeColumnProperty(this, control, WatermarkProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, InnerLeftContentProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, InnerRightContentProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, IconProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, ShowClearButtonProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, ShowClipboardButtonProperty);
    }

    protected virtual void SynchronizeValueProperties(Control control)
    {
        SynchronizeFontProperties(control);

        DataGridHelper.SynchronizeColumnProperty(this, control, HorizontalAlignmentProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, VerticalAlignmentProperty);

        if (control is ContentControl contentControl)
            DataGridHelper.SynchronizeColumnProperty(this, contentControl, ContentControl.ContentTemplateProperty, ContentTemplateProperty);
    }

    protected virtual void SynchronizeFontProperties(Control control)
    {
        DataGridHelper.SynchronizeColumnProperty(this, control, FontFamilyProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, FontSizeProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, FontStyleProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, FontWeightProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, ForegroundProperty);
    }
}
