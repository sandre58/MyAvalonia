// -----------------------------------------------------------------------
// <copyright file="DataGridCheckBoxColumn.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Styling;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class DataGridCheckBoxColumn : global::Avalonia.Controls.DataGridCheckBoxColumn
{
    private readonly Lazy<ControlTheme?> _cellCheckBoxTheme;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataGridCheckBoxColumn" /> class.
    /// </summary>
    public DataGridCheckBoxColumn() => _cellCheckBoxTheme = new Lazy<ControlTheme?>(() => OwningGrid.TryFindResource("MyNet.Theme.CheckBox.Embedded.DataGrid", out var theme) ? (ControlTheme?)theme : null);

    protected override Control GenerateElement(DataGridCell cell, object dataItem)
    {
        var control = base.GenerateElement(cell, dataItem);

        if (_cellCheckBoxTheme.Value is { } theme)
            control.Theme = theme;

        return control;
    }

    protected override Control GenerateEditingElementDirect(DataGridCell cell, object dataItem)
    {
        var control = base.GenerateEditingElementDirect(cell, dataItem);

        if (_cellCheckBoxTheme.Value is { } theme)
            control.Theme = theme;

        return control;
    }
}
