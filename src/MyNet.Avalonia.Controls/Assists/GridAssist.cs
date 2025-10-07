// -----------------------------------------------------------------------
// <copyright file="GridAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia;
using Avalonia.Controls;

namespace MyNet.Avalonia.Controls.Assists;

public static class GridAssist
{
    static GridAssist()
    {
        _ = ColumnDefinitionsProperty.Changed.AddClassHandler<Grid, ColumnDefinitions>((grid, e) =>
        {
            grid.ColumnDefinitions.Clear();

            if (e.NewValue.GetValueOrDefault() is { } columns)
            {
                grid.ColumnDefinitions.AddRange(columns.Select(o => new ColumnDefinition
                {
                    Width = o.Width,
                    SharedSizeGroup = o.SharedSizeGroup
                }));
            }
        });

        _ = RowDefinitionsProperty.Changed.AddClassHandler<Grid, RowDefinitions>((grid, e) =>
        {
            grid.RowDefinitions.Clear();

            if (e.NewValue.GetValueOrDefault() is { } rows)
            {
                grid.RowDefinitions.AddRange(rows.Select(o => new RowDefinition
                {
                    Height = o.Height,
                    SharedSizeGroup = o.SharedSizeGroup
                }));
            }
        });
    }

    #region RowDefinitions

    /// <summary>
    /// Provides RowDefinitions Property for attached GridAssist element.
    /// </summary>
    public static readonly AttachedProperty<RowDefinitions> RowDefinitionsProperty = AvaloniaProperty.RegisterAttached<Grid, RowDefinitions>("RowDefinitions", typeof(GridAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="RowDefinitionsProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="RowDefinitionsProperty"/>.</param>
    public static void SetRowDefinitions(Grid element, RowDefinitions value) => element.SetValue(RowDefinitionsProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="RowDefinitionsProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static RowDefinitions GetRowDefinitions(Grid element) => element.GetValue(RowDefinitionsProperty);

    #endregion

    #region ColumnDefinitions

    /// <summary>
    /// Provides ColumnDefinitions Property for attached GridAssist element.
    /// </summary>
    public static readonly AttachedProperty<ColumnDefinitions> ColumnDefinitionsProperty = AvaloniaProperty.RegisterAttached<Grid, ColumnDefinitions>("ColumnDefinitions", typeof(GridAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ColumnDefinitionsProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ColumnDefinitionsProperty"/>.</param>
    public static void SetColumnDefinitions(Grid element, ColumnDefinitions value) => element.SetValue(ColumnDefinitionsProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ColumnDefinitionsProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static ColumnDefinitions GetColumnDefinitions(Grid element) => element.GetValue(ColumnDefinitionsProperty);

    #endregion

}
