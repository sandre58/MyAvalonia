// -----------------------------------------------------------------------
// <copyright file="DataGridNumericColumn.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Assists;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Controls.Helpers;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class DataGridNumericColumn : DataGridBoundColumn<NumericUpDown, TextBlock>
{
    public DataGridNumericColumn()
        : base(NumericUpDown.ValueProperty, TextBlock.TextProperty)
    {
        HorizontalAlignment = HorizontalAlignment.Center;
        TextAlignment = TextAlignment.Center;
        Layout = SpinnerLayout.Vertical;
    }

    #region FormatString

    /// <summary>
    /// Provides FormatString Property.
    /// </summary>
    public static readonly StyledProperty<string> FormatStringProperty = NumericUpDown.FormatStringProperty.AddOwner<DataGridNumericColumn>();

    /// <summary>
    /// Gets or sets the FormatString property.
    /// </summary>
    public string FormatString
    {
        get => GetValue(FormatStringProperty);
        set => SetValue(FormatStringProperty, value);
    }

    #endregion

    #region Minimum

    /// <summary>
    /// Provides Minimum Property.
    /// </summary>
    public static readonly StyledProperty<decimal> MinimumProperty = NumericUpDown.MinimumProperty.AddOwner<DataGridNumericColumn>();

    /// <summary>
    /// Gets or sets the Minimum property.
    /// </summary>
    public decimal Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    #endregion

    #region Maximum

    /// <summary>
    /// Provides Maximum Property.
    /// </summary>
    public static readonly StyledProperty<decimal> MaximumProperty = NumericUpDown.MaximumProperty.AddOwner<DataGridNumericColumn>();

    /// <summary>
    /// Gets or sets the Maximum property.
    /// </summary>
    public decimal Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    #endregion

    #region Interval

    /// <summary>
    /// Provides Add Property.
    /// </summary>
    public static readonly StyledProperty<decimal> IncrementProperty = NumericUpDown.IncrementProperty.AddOwner<DataGridNumericColumn>();

    /// <summary>
    /// Gets or sets the Add property.
    /// </summary>
    public decimal Increment
    {
        get => GetValue(IncrementProperty);
        set => SetValue(IncrementProperty, value);
    }

    #endregion

    #region ButtonSpinnerLocation

    /// <summary>
    /// Provides ButtonSpinnerLocation Property.
    /// </summary>
    public static readonly StyledProperty<Location> ButtonSpinnerLocationProperty = NumericUpDown.ButtonSpinnerLocationProperty.AddOwner<DataGridNumericColumn>();

    /// <summary>
    /// Gets or sets the ButtonSpinnerLocation property.
    /// </summary>
    public Location ButtonSpinnerLocation
    {
        get => GetValue(ButtonSpinnerLocationProperty);
        set => SetValue(ButtonSpinnerLocationProperty, value);
    }

    #endregion

    #region ShowButtonSpinner

    /// <summary>
    /// Provides ShowButtonSpinner Property.
    /// </summary>
    public static readonly StyledProperty<bool> ShowButtonSpinnerProperty = NumericUpDown.ShowButtonSpinnerProperty.AddOwner<DataGridNumericColumn>();

    public bool ShowButtonSpinner
    {
        get => GetValue(ShowButtonSpinnerProperty);
        set => SetValue(ShowButtonSpinnerProperty, value);
    }

    #endregion

    #region TextAlignment

    /// <summary>
    /// Provides TextAlignment Property.
    /// </summary>
    public static readonly StyledProperty<TextAlignment> TextAlignmentProperty = AvaloniaProperty.Register<DataGridNumericColumn, TextAlignment>(nameof(TextAlignment));

    /// <summary>
    /// Gets or sets the TextAlignment property.
    /// </summary>
    public TextAlignment TextAlignment
    {
        get => GetValue(TextAlignmentProperty);
        set => SetValue(TextAlignmentProperty, value);
    }

    #endregion

    #region Layout

    /// <summary>
    /// Provides Layout Property.
    /// </summary>
    public static readonly StyledProperty<SpinnerLayout> LayoutProperty = SpinnerAssist.LayoutProperty.AddOwner<DataGridNumericColumn>();

    /// <summary>
    /// Gets or sets the Layout property.
    /// </summary>
    public SpinnerLayout Layout
    {
        get => GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    #endregion

    #region DecreaseContent

    /// <summary>
    /// Provides DecreaseContent Property.
    /// </summary>
    public static readonly StyledProperty<object> DecreaseContentProperty = SpinnerAssist.DecreaseContentProperty.AddOwner<DataGridNumericColumn>();

    /// <summary>
    /// Gets or sets the DecreaseContent property.
    /// </summary>
    public object DecreaseContent
    {
        get => GetValue(DecreaseContentProperty);
        set => SetValue(DecreaseContentProperty, value);
    }

    #endregion

    #region IncreaseContent

    /// <summary>
    /// Provides IncreaseContent Property.
    /// </summary>
    public static readonly StyledProperty<object> IncreaseContentProperty = SpinnerAssist.IncreaseContentProperty.AddOwner<DataGridNumericColumn>();

    /// <summary>
    /// Gets or sets the IncreaseContent property.
    /// </summary>
    public object IncreaseContent
    {
        get => GetValue(IncreaseContentProperty);
        set => SetValue(IncreaseContentProperty, value);
    }

    #endregion

    protected override void SynchronizeEditingControlProperties(Control control)
    {
        base.SynchronizeEditingControlProperties(control);

        DataGridHelper.SynchronizeColumnProperty(this, control, NumericUpDown.WatermarkProperty, WatermarkProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, NumericUpDown.InnerRightContentProperty, InnerRightContentProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, NumericUpDown.InnerLeftContentProperty, InnerLeftContentProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, NumericUpDown.TextAlignmentProperty, TextAlignmentProperty);

        DataGridHelper.SynchronizeColumnProperty(this, control, FormatStringProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, MinimumProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, MaximumProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, IncrementProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, FormatStringProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, ButtonSpinnerLocationProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, ShowButtonSpinnerProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, DecreaseContentProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, IncreaseContentProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, LayoutProperty);
    }

    protected override void ResetValue(NumericUpDown control, object uneditedValue) => control.Value = (decimal?)uneditedValue;

    protected override object? GetValue(NumericUpDown control) => control.Value;
}
