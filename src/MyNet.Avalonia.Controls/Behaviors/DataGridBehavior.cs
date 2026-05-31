// -----------------------------------------------------------------------
// <copyright file="DataGridBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using MyNet.Primitives;
using MyNet.Utilities.Suspending;

namespace MyNet.Avalonia.Controls.Behaviors;

public static class DataGridBehavior
{
    static DataGridBehavior()
    {
        _ = UseAreAllSelectedProperty.Changed.Subscribe(UseAreAllSelectedChangedCallback);
        _ = AreAllSelectedProperty.Changed.Subscribe(AreAllSelectedChangedCallback);
    }

    #region UseAreAllSelected

    private static readonly Suspender AreAllSelectedSuspender = new();

    /// <summary>
    /// Provides UseAreAllSelected Property for attached DataGridBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> UseAreAllSelectedProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("UseAreAllSelected", typeof(DataGridBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="UseAreAllSelectedProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="UseAreAllSelectedProperty"/>.</param>
    public static void SetUseAreAllSelected(StyledElement element, bool value) => element.SetValue(UseAreAllSelectedProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="UseAreAllSelectedProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetUseAreAllSelected(StyledElement element) => element.GetValue(UseAreAllSelectedProperty);

    private static void UseAreAllSelectedChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not DataGrid dataGrid)
            return;

        dataGrid.SelectionChanged -= onSelectionChanged;
        dataGrid.SelectionChanged += onSelectionChanged;

        void onSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            using (AreAllSelectedSuspender.Suspend())
                SetAreAllSelected(dataGrid, dataGrid.SelectedItems.Count == 0 ? false : dataGrid.SelectedItems.Count == dataGrid.CollectionView.SourceCollection.OfType<object>().Count() ? true : null);
        }
    }

    #endregion

    #region AreAllSelected

    /// <summary>
    /// Provides AreAllSelected Property for attached DataGridBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool?> AreAllSelectedProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool?>("AreAllSelected", typeof(DataGridBehavior), false);

    /// <summary>
    /// Accessor for Attached  <see cref="AreAllSelectedProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="AreAllSelectedProperty"/>.</param>
    public static void SetAreAllSelected(StyledElement element, bool? value) => element.SetValue(AreAllSelectedProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="AreAllSelectedProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool? GetAreAllSelected(StyledElement element) => element.GetValue(AreAllSelectedProperty);

    private static void AreAllSelectedChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (AreAllSelectedSuspender.IsSuspended || args.Sender is not DataGrid dataGrid)
            return;

        var value = (bool?)args.NewValue;
        if (value.IsTrue())
            dataGrid.SelectAll();
        else if (value.IsFalse())
            dataGrid.SelectedItems.Clear();
    }

    #endregion
}
