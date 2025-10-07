// -----------------------------------------------------------------------
// <copyright file="DataGridTextColumn.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Helpers;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class DataGridTextColumn : DataGridBoundColumn<TextBox, TextBlock>
{
    public DataGridTextColumn()
        : base(TextBox.TextProperty, TextBlock.TextProperty) { }

    #region TextAlignment

    /// <summary>
    /// Provides TextAlignment Property.
    /// </summary>
    public static readonly StyledProperty<TextAlignment> TextAlignmentProperty = AvaloniaProperty.Register<DataGridTextColumn, TextAlignment>(nameof(TextAlignment));

    /// <summary>
    /// Gets or sets the TextAlignment property.
    /// </summary>
    public TextAlignment TextAlignment
    {
        get => GetValue(TextAlignmentProperty);
        set => SetValue(TextAlignmentProperty, value);
    }

    #endregion

    protected override void SynchronizeEditingControlProperties(Control control)
    {
        base.SynchronizeEditingControlProperties(control);

        DataGridHelper.SynchronizeColumnProperty(this, control, TextBox.WatermarkProperty, WatermarkProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, TextBox.InnerLeftContentProperty, InnerLeftContentProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, TextBox.InnerRightContentProperty, InnerRightContentProperty);
        DataGridHelper.SynchronizeColumnProperty(this, control, TextBox.TextAlignmentProperty, TextAlignmentProperty);
    }

    protected override void PrepareEditingControl(TextBox editingElement, RoutedEventArgs editingEventArgs)
    {
        base.PrepareEditingControl(editingElement, editingEventArgs);

        var obj = editingElement.Text ?? string.Empty;
        var length = obj.Length;
        if (editingEventArgs is KeyEventArgs { Key: Key.F2 })
        {
            editingElement.SelectionStart = length;
            editingElement.SelectionEnd = length;
        }

        editingElement.SelectionStart = 0;
        editingElement.SelectionEnd = length;
        editingElement.CaretIndex = length;
    }

    protected override void ResetValue(TextBox control, object uneditedValue) => control.Text = uneditedValue as string;

    protected override object? GetValue(TextBox control) => control.Text;
}
