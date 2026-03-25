// -----------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Windows.Input;
using MyNet.UI.Commands;
using MyNet.UI.Navigation;
using MyNet.UI.Navigation.Models;
using MyNet.UI.Notifications;
using MyNet.UI.Services;
using MyNet.UI.ViewModels.Shell;
using MyNet.Utilities.Collections;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class MainViewModel : MainWindowViewModelBase
{
    private readonly ThreadSafeObservableCollection<INavigationPage> _pages = [];

    public MainViewModel(INotificationsManager notificationsManager,
                         IAppCommandsService appCommandsService,
                         INavigationService navigationService)
        : base(notificationsManager, appCommandsService, AppBusyManager.MainBusyService, UIContext.Globalization)
    {
        NavigationService = navigationService;
        Pages = new ReadOnlyObservableCollection<INavigationPage>(_pages);

        GoBackCommand = CommandsManager.Create(() => NavigationService.GoBack(), () => NavigationService.CanGoBack());
        GoForwardCommand = CommandsManager.Create(() => NavigationService.GoForward(), () => NavigationService.CanGoForward());
        NavigateCommand = CommandsManager.CreateNotNull<INavigationPage>(x => NavigationService.NavigateTo(x));

        NavigationService.Navigated += (_, _) => RefreshNavigationCommands();
        NavigationService.HistoryCleared += (_, _) => RefreshNavigationCommands();
    }

    public INavigationService NavigationService { get; }

    public ICommand GoBackCommand { get; }

    public ICommand GoForwardCommand { get; }

    public ICommand NavigateCommand { get; }

    public ReadOnlyObservableCollection<INavigationPage> Pages { get; }

    private void RefreshNavigationCommands()
    {
        (GoBackCommand as RelayCommand)?.OnCanExecuteChanged();
        (GoForwardCommand as RelayCommand)?.OnCanExecuteChanged();
    }

    public void AddPage(INavigationPage page) => _pages.Add(page);

    public void AddPages(params INavigationPage[] pages) => _pages.AddRange(pages);
}
