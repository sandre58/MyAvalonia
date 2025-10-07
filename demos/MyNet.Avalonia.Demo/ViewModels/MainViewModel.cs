// -----------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using MyNet.UI.Commands;
using MyNet.UI.Navigation;
using MyNet.UI.Notifications;
using MyNet.UI.Services;
using MyNet.UI.ViewModels.Shell;

namespace MyNet.Avalonia.Demo.ViewModels;

public class MainViewModel : MainWindowViewModelBase
{
    public MainViewModel(INotificationsManager notificationsManager, IAppCommandsService appCommandsService, INavigationService navigationService)
        : base(notificationsManager, appCommandsService, AppBusyManager.MainBusyService)
    {
        NavigationService = navigationService;

        GoBackCommand = CommandsManager.Create(() => NavigationService.GoBack(), () => NavigationService.CanGoBack());
        GoForwardCommand = CommandsManager.Create(() => NavigationService.GoForward(), () => NavigationService.CanGoForward());

        NavigationService.Navigated += NavigationService_Navigated;
    }

    public INavigationService NavigationService { get; }

    public ICommand GoBackCommand { get; }

    public ICommand GoForwardCommand { get; }

    private void NavigationService_Navigated(object? sender, NavigationEventArgs e)
    {
        ((RelayCommand)GoBackCommand).OnCanExecuteChanged();
        ((RelayCommand)GoForwardCommand).OnCanExecuteChanged();
    }

    protected override void Cleanup()
    {
        NavigationService.Navigated -= NavigationService_Navigated;
        base.Cleanup();
    }
}
