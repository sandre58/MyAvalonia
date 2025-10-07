// -----------------------------------------------------------------------
// <copyright file="NavigationAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using MyNet.Avalonia.Controls;
using MyNet.UI.Commands;
using MyNet.UI.Locators;
using MyNet.UI.Navigation;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.UI.Assists;

public static class NavigationAssist
{
    static NavigationAssist() => AttachServiceProperty.Changed.Subscribe(AttachServiceChangedCallback);

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
        switch (args.Sender)
        {
            case NavigationMenu menu:
                {
                    RegisterCommand(navigationService, menu.Items.OfType<NavigationMenuItem>());
                    menu.Items.CollectionChanged += (_, e) =>
                    {
                        if (e.NewItems != null)
                        {
                            RegisterCommand(navigationService, e.NewItems.OfType<NavigationMenuItem>());
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

            default:
                break;
        }
    }

    private static void RegisterCommand(INavigationService navigationService, IEnumerable<NavigationMenuItem> menuItems)
    {
        foreach (var menuItem in menuItems.Where(x => !x.IsSeparator))
        {
            menuItem.Command = CommandsManager.Create<object>(x =>
            {
                switch (x)
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

                    default:
                        break;
                }
            });
        }
    }

    #endregion
}
