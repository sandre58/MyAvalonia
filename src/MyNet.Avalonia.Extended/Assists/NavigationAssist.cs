// -----------------------------------------------------------------------
// <copyright file="NavigationAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Controls;
using MyNet.UI.Commands;
using MyNet.UI.Locators;
using MyNet.UI.Navigation;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Extended.Assists;

public static class NavigationAssist
{
    static NavigationAssist()
    {
        AttachServiceProperty.Changed.Subscribe(AttachServiceChangedCallback);
        CommandProperty.Changed.Subscribe(CommandChangedCallback);
    }

    #region AttachService

    /// <summary>
    /// Provides AttachService Property for attached NavigationAssist element.
    /// </summary>
    public static readonly AttachedProperty<INavigationService> AttachServiceProperty = AvaloniaProperty.RegisterAttached<StyledElement, INavigationService>("AttachService", typeof(NavigationAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="AttachServiceProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="AttachServiceProperty"/>.</param>
    public static void SetAttachService(StyledElement element, INavigationService value) => element.SetValue(AttachServiceProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="AttachServiceProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static INavigationService GetAttachService(StyledElement element) => element.GetValue(AttachServiceProperty);

    /// <summary>
    /// Attaches the navigation service to the element and sets up the necessary event handlers to synchronize navigation with UI elements like SelectingItemsControl, NavigationMenu, ContentControl, and NavigationPage.
    /// </summary>
    /// <param name="args">The event arguments containing information about the property change.</param>
    private static void AttachServiceChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.NewValue is not INavigationService navigationService)
            return;

        var defaultCommand = CommandsManager.Create<object>(x => NavigateTo(navigationService, x));
        switch (args.Sender)
        {
            case SelectingItemsControl selectingItemsControl:
                {
                    // Synchronise la sélection avec la navigation
                    selectingItemsControl.SelectionChanged += (_, e) =>
                    {
                        if (e.AddedItems.Count > 0)
                        {
                            var selectedItem = e.AddedItems[0];
                            NavigateTo(navigationService, selectedItem);
                        }
                    };

                    // Synchronise la navigation avec la sélection
                    navigationService.Navigated += (_, e) =>
                    {
                        var matchingItem = FindMatchingItem(selectingItemsControl.ItemsSource, e.NewPage);

                        if (matchingItem != null && !Equals(selectingItemsControl.SelectedItem, matchingItem))
                        {
                            selectingItemsControl.SelectedItem = matchingItem;
                        }
                    };

                    break;
                }

            case NavigationMenu menu:
                {
                    RegisterCommand(menu.Items.OfType<NavigationMenuItem>(), defaultCommand, false);
                    menu.Items.CollectionChanged += (_, e) =>
                    {
                        if (e.NewItems != null)
                        {
                            RegisterCommand(e.NewItems.OfType<NavigationMenuItem>(), defaultCommand, false);
                        }
                    };

                    navigationService.Navigated += (_, e) =>
                    {
                        var matchingItem = FindMatchingItem(menu.ItemsSource, e.NewPage);

                        if (matchingItem != null && !Equals(menu.SelectedItem, matchingItem))
                        {
                            menu.SelectedItem = matchingItem;
                        }
                    };

                    break;
                }

            case ContentControl contentControl:
                {
                    navigationService.Navigated += (_, e) => contentControl.Content = e.NewPage;
                    break;
                }

            case NavigationPage navigationPage:
                {
                    if (navigationService is Navigation.NavigationService avaloniaNavigationService)
                        avaloniaNavigationService.AttachNavigationPage(navigationPage);
                    break;
                }
        }
    }

    /// <summary>
    /// Navigates to the specified page or view model using the provided navigation service.
    /// </summary>
    /// <remarks>If the provided object is a type, the method attempts to retrieve the corresponding view or
    /// view model. If a valid navigation page is found, navigation is performed. This method does not return a
    /// value.</remarks>
    /// <param name="navigationService">The navigation service used to perform the navigation operation.</param>
    /// <param name="obj">An object that can be either a navigation page or a type representing a view or view model to navigate to.</param>
    private static void NavigateTo(INavigationService navigationService, object? obj)
    {
        switch (obj)
        {
            case INavigationPage page:
                _ = navigationService.NavigateTo(page);
                break;

            case Type type:
                {
                    var view = ViewManager.Get(type);

                    if (view is INavigationPage pageView)
                    {
                        _ = navigationService.NavigateTo(pageView);
                    }
                    else
                    {
                        var viewModel = ViewModelManager.Get(type);

                        if (viewModel is INavigationPage pageViewModel)
                        {
                            _ = navigationService.NavigateTo(pageViewModel);
                        }
                    }

                    break;
                }
        }
    }

    /// <summary>
    /// Searches the specified collection and returns the first item that matches the given page type.
    /// </summary>
    /// <remarks>An item is considered a match if it is directly of the specified type, implements
    /// INavigationPage with the matching type, has a 'PageType' property equal to the specified type, or is associated
    /// with the specified type via a View or ViewModel. The method does not throw exceptions for null collections and
    /// returns null in such cases.</remarks>
    /// <param name="items">An enumerable collection of items to search. Can be null, in which case the method returns null.</param>
    /// <param name="obj">The object to match against items in the collection. Can be null, in which case the method returns null.</param>
    /// <returns>The first item in the collection that matches the specified object, or null if no matching item is found or
    /// if the collection is null.</returns>
    private static object? FindMatchingItem(System.Collections.IEnumerable? items, object? obj)
    {
        if (items is null) return null;

        var objType = obj?.GetType();
        foreach (var item in items)
        {
            if ((item is Type itemType && itemType == objType) || (item is INavigationPage page && Equals(page, obj)))
            {
                return item;
            }
        }

        return null;
    }

    #endregion

    #region Command

    /// <summary>
    /// Provides Command Property for attached NavigationAssist element.
    /// </summary>
    public static readonly AttachedProperty<ICommand> CommandProperty = AvaloniaProperty.RegisterAttached<StyledElement, ICommand>("Command", typeof(NavigationAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="CommandProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="CommandProperty"/>.</param>
    public static void SetCommand(StyledElement element, ICommand value) => element.SetValue(CommandProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="CommandProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static ICommand GetCommand(StyledElement element) => element.GetValue(CommandProperty);

    private static void CommandChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.NewValue is not ICommand command || args.Sender is not NavigationMenu menu)
            return;

        RegisterCommand(menu.Items.OfType<NavigationMenuItem>(), command, true);
        menu.Items.CollectionChanged += (_, e) =>
        {
            if (e.NewItems != null)
            {
                RegisterCommand(e.NewItems.OfType<NavigationMenuItem>(), command, true);
            }
        };
    }

    private static void RegisterCommand(IEnumerable<NavigationMenuItem> menuItems, ICommand command, bool overrideCommand)
    {
        foreach (var menuItem in menuItems.Where(x => !x.IsSeparator))
        {
            if (menuItem.Command is not null && !overrideCommand) return;

            menuItem.Command = command;
        }
    }

    #endregion

}
