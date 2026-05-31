// -----------------------------------------------------------------------
// <copyright file="NavigationAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Extended.Navigation;
using MyNet.UI.Navigation;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Extended.Assists;

/// <summary>
/// Attached properties that bind Avalonia navigation controls to <see cref="INavigationService"/>.
/// </summary>
public static class NavigationAssist
{
    static NavigationAssist()
    {
        AttachServiceProperty.Changed.Subscribe(AttachServiceChangedCallback);
        CommandProperty.Changed.Subscribe(CommandChangedCallback);
    }

    #region AttachService

    /// <summary>
    /// Identifies the <see cref="AttachService"/> attached property.
    /// </summary>
    public static readonly AttachedProperty<INavigationService?> AttachServiceProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, INavigationService?>("AttachService", typeof(NavigationAssist));

    /// <summary>
    /// Sets the navigation service attached to the target element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The navigation service.</param>
    public static void SetAttachService(StyledElement element, INavigationService? value) =>
        element.SetValue(AttachServiceProperty, value);

    /// <summary>
    /// Gets the navigation service attached to the target element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The attached navigation service, if any.</returns>
    public static INavigationService? GetAttachService(StyledElement element) =>
        element.GetValue(AttachServiceProperty);

    private static void AttachServiceChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.NewValue is not INavigationService navigationService)
            return;

        var defaultCommand = AvaloniaNavigationContext.CommandFactory.Create<object?>(
            parameter => NavigateTo(navigationService, parameter));

        switch (args.Sender)
        {
            case SelectingItemsControl selectingItemsControl:
                selectingItemsControl.SelectionChanged += (_, e) =>
                {
                    if (e.AddedItems.Count > 0)
                        NavigateTo(navigationService, e.AddedItems[0]);
                };

                navigationService.StateChanged += (_, e) =>
                {
                    var matchingItem = FindMatchingItem(selectingItemsControl.ItemsSource, e.CurrentContext?.To);

                    if (matchingItem != null && !Equals(selectingItemsControl.SelectedItem, matchingItem))
                        selectingItemsControl.SelectedItem = matchingItem;
                };

                break;

            case NavigationMenu menu:
                RegisterCommand(menu.Items.OfType<NavigationMenuItem>(), defaultCommand, false);
                menu.Items.CollectionChanged += (_, e) =>
                {
                    if (e.NewItems != null)
                        RegisterCommand(e.NewItems.OfType<NavigationMenuItem>(), defaultCommand, false);
                };

                navigationService.StateChanged += (_, e) =>
                {
                    var matchingItem = FindMatchingItem(menu.ItemsSource, e.CurrentContext?.To);

                    if (matchingItem != null && !Equals(menu.SelectedItem, matchingItem))
                        menu.SelectedItem = matchingItem;
                };

                break;

            case ContentControl contentControl:
                navigationService.StateChanged += (_, e) => contentControl.Content = e.CurrentContext?.To;
                break;

            case NavigationPage navigationPage:
                AvaloniaNavigationContext.NavigationHost.Attach(navigationPage);
                break;
        }
    }

    private static void NavigateTo(INavigationService navigationService, object? obj)
    {
        switch (obj)
        {
            case INavigationPage page:
                _ = navigationService.NavigateToAsync(page);
                break;

            case Type type:
                {
                    var viewModel = AvaloniaNavigationContext.ViewModelLocator.Get(type);

                    if (viewModel is INavigationPage navigationPage)
                        _ = navigationService.NavigateToAsync(navigationPage);

                    break;
                }
        }
    }

    private static object? FindMatchingItem(IEnumerable? items, object? obj)
    {
        if (items is null)
            return null;

        var objType = obj?.GetType();

        foreach (var item in items)
        {
            if ((item is Type itemType && itemType == objType) || (item is INavigationPage page && Equals(page, obj)))
                return item;
        }

        return null;
    }

    #endregion

    #region Command

    /// <summary>
    /// Identifies the <see cref="Command"/> attached property.
    /// </summary>
    public static readonly AttachedProperty<ICommand?> CommandProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, ICommand?>("Command", typeof(NavigationAssist));

    /// <summary>
    /// Sets the navigation command attached to the target element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The command.</param>
    public static void SetCommand(StyledElement element, ICommand? value) => element.SetValue(CommandProperty, value);

    /// <summary>
    /// Gets the navigation command attached to the target element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The attached command, if any.</returns>
    public static ICommand? GetCommand(StyledElement element) => element.GetValue(CommandProperty);

    private static void CommandChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.NewValue is not ICommand command || args.Sender is not NavigationMenu menu)
            return;

        RegisterCommand(menu.Items.OfType<NavigationMenuItem>(), command, true);
        menu.Items.CollectionChanged += (_, e) =>
        {
            if (e.NewItems != null)
                RegisterCommand(e.NewItems.OfType<NavigationMenuItem>(), command, true);
        };
    }

    private static void RegisterCommand(IEnumerable<NavigationMenuItem> menuItems, ICommand command, bool overrideCommand)
    {
        foreach (var menuItem in menuItems.Where(x => !x.IsSeparator))
        {
            if (menuItem.Command is not null && !overrideCommand)
                return;

            menuItem.Command = command;
        }
    }

    #endregion
}
