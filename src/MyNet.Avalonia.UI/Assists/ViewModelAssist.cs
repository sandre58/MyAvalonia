// -----------------------------------------------------------------------
// <copyright file="ViewModelAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using MyNet.UI.Locators;
using MyNet.Utilities;

namespace MyNet.Avalonia.UI.Assists;

public static class ViewModelAssist
{
    static ViewModelAssist() => AutoWireProperty.Changed.Subscribe(AutoWireChangedCallback);

    #region AutoWire

    /// <summary>
    /// Provides AutoWire Property for attached ViewModelAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> AutoWireProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("AutoWire", typeof(ViewModelAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="AutoWireProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="AutoWireProperty"/>.</param>
    public static void SetAutoWire(StyledElement element, bool value) => element.SetValue(AutoWireProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="AutoWireProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetAutoWire(StyledElement element) => element.GetValue(AutoWireProperty);

    private static void AutoWireChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not StyledElement element || !((bool?)args.NewValue).IsTrue())
            return;
        var viewModel = ViewModelManager.GetViewModel(element.GetType());

        Bind(element, viewModel);
    }

    /// <summary>
    /// Sets the DataContext of a View.
    /// </summary>
    /// <param name="view">The View to set the DataContext on.</param>
    /// <param name="viewModel">The object to use as the DataContext for the View.</param>
    private static void Bind(object view, object? viewModel)
    {
        if (view is StyledElement element)
        {
            element.DataContext = viewModel;
        }
    }

    #endregion
}
