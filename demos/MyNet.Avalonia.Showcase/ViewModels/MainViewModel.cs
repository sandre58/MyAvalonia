// -----------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using MyNet.Avalonia.Showcase.ViewModels.Base;
using MyNet.UI.Commands;
using MyNet.UI.Navigation;
using MyNet.UI.Navigation.Models;
using MyNet.UI.Notifications;
using MyNet.UI.Services;
using MyNet.Globalization.Culture;
using MyNet.UI.ViewModels.Shell;
using MyNet.Utilities.Collections;

namespace MyNet.Avalonia.Showcase.ViewModels;

internal sealed class MainViewModel : MainWindowViewModelBase
{
    private readonly ThreadSafeObservableCollection<IMenuItemViewModel> _menuItems = [];

    public MainViewModel(INotificationsManager notificationsManager,
                         IAppCommandsService appCommandsService,
                         INavigationService navigationService,
                         ICultureService cultureService)
        : base(notificationsManager, appCommandsService, AppBusyManager.MainBusyService, cultureService)
    {
        NavigationService = navigationService;
        MenuItems = new(_menuItems);

        GoBackCommand = CommandsManager.Create(() => NavigationService.GoBack(), () => NavigationService.CanGoBack());
        GoForwardCommand = CommandsManager.Create(() => NavigationService.GoForward(), () => NavigationService.CanGoForward());
        NavigateCommand = CommandsManager.CreateNotNull<INavigationPage>(x => NavigationService.NavigateTo(x));
        ChangeCultureCommand = CommandsManager.CreateNotNull<CultureInfo>(x => SelectedCulture = x);

        NavigationService.Navigated += (_, _) => RefreshNavigationCommands();
        NavigationService.HistoryCleared += (_, _) => RefreshNavigationCommands();
    }

    public INavigationService NavigationService { get; }

    public ICommand GoBackCommand { get; }

    public ICommand GoForwardCommand { get; }

    public ICommand NavigateCommand { get; }

    public ICommand ChangeCultureCommand { get; }

    public ReadOnlyObservableCollection<IMenuItemViewModel> MenuItems { get; }

    private void RefreshNavigationCommands()
    {
        (GoBackCommand as RelayCommand)?.OnCanExecuteChanged();
        (GoForwardCommand as RelayCommand)?.OnCanExecuteChanged();
    }

    public void AddMenuItem(params IMenuItemViewModel[] item) => _menuItems.AddRange(item);
}
