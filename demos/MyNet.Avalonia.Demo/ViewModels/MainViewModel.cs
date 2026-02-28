// -----------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Windows.Input;
using MyNet.Avalonia.Extended.WarmUp;
using MyNet.UI.Commands;
using MyNet.UI.Locators;
using MyNet.UI.Navigation;
using MyNet.UI.Navigation.Models;
using MyNet.UI.Notifications;
using MyNet.UI.Services;
using MyNet.UI.ViewModels.Shell;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class MainViewModel : MainWindowViewModelBase
{
    public MainViewModel(INotificationsManager notificationsManager,
                         IAppCommandsService appCommandsService,
                         INavigationService navigationService,
                         IWarmUpService warmUpService,
                         IViewModelLocator viewModelLocator)
        : base(notificationsManager, appCommandsService, AppBusyManager.MainBusyService, UIContext.Globalization)
    {
        NavigationService = navigationService;
        WarmUpService = warmUpService;

        GoBackCommand = CommandsManager.Create(() => NavigationService.GoBack(), () => NavigationService.CanGoBack());
        GoForwardCommand = CommandsManager.Create(() => NavigationService.GoForward(), () => NavigationService.CanGoForward());
        NavigateCommand = CommandsManager.CreateNotNull<Type>(x => NavigationService.NavigateTo((INavigationPage)viewModelLocator.Get(x)));

        NavigationService.Navigated += (_, _) => RefreshNavigationCommands();
        NavigationService.HistoryCleared += (_, _) => RefreshNavigationCommands();
    }

    public INavigationService NavigationService { get; }

    public IWarmUpService WarmUpService { get; }

    public ICommand GoBackCommand { get; }

    public ICommand GoForwardCommand { get; }

    public ICommand NavigateCommand { get; }

    private void RefreshNavigationCommands()
    {
        (GoBackCommand as RelayCommand)?.OnCanExecuteChanged();
        (GoForwardCommand as RelayCommand)?.OnCanExecuteChanged();
    }
}
