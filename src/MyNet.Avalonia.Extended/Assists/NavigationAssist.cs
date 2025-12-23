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

    private static void AttachServiceChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.NewValue is not INavigationService navigationService)
            return;

        var defaultCommand = CommandsManager.Create<object>(x => NavigateTo(navigationService, x));
        switch (args.Sender)
        {
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

                    navigationService.Navigated += (_, e) => menu.SelectedItem = menu.Items.OfType<NavigationMenuItem>().FirstOrDefault(x => Equals(x.CommandParameter, e.NewPage.GetType()));

                    break;
                }

            case ContentControl contentControl:
                {
                    navigationService.Navigated += (_, e) => contentControl.Content = e.NewPage;
                    break;
                }
        }
    }

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
