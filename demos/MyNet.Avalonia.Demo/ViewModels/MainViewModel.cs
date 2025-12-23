// -----------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Windows.Input;
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
    public MainViewModel(INotificationsManager notificationsManager, IAppCommandsService appCommandsService, INavigationService navigationService, IViewModelLocator viewModelLocator)
        : base(notificationsManager, appCommandsService, AppBusyManager.MainBusyService, UIContext.Globalization)
    {
        NavigationService = navigationService;

        GoBackCommand = CommandsManager.Create(() => NavigationService.GoBack(), () => NavigationService.CanGoBack());
        GoForwardCommand = CommandsManager.Create(() => NavigationService.GoForward(), () => NavigationService.CanGoForward());
        NavigateCommand = CommandsManager.CreateNotNull<Type>(x => NavigationService.NavigateTo((INavigationPage)viewModelLocator.Get(x)));

        NavigationService.Navigated += NavigationService_Navigated;
    }

    public PageViewModel? CurrentPage { get; private set; }

    public INavigationService NavigationService { get; }

    public ICommand NavigateCommand { get; }

    public ICommand GoBackCommand { get; }

    public ICommand GoForwardCommand { get; }

    private void NavigationService_Navigated(object? sender, NavigationEventArgs e)
    {
        ((RelayCommand)GoBackCommand).OnCanExecuteChanged();
        ((RelayCommand)GoForwardCommand).OnCanExecuteChanged();

        CurrentPage = e.NewPage as PageViewModel;
    }

    protected override void Cleanup()
    {
        NavigationService.Navigated -= NavigationService_Navigated;
        base.Cleanup();
    }
}
