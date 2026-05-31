// -----------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using MyNet.Avalonia.Extended.Commands;
using MyNet.Avalonia.Showcase.ViewModels.Base;
using MyNet.UI.Commands;
using MyNet.UI.Navigation;
using MyNet.UI.Navigation.Models;
using MyNet.UI.Notifications;
using MyNet.UI.Loading;
using MyNet.UI.Services;
using MyNet.Globalization.Culture;
using MyNet.UI.ViewModels.Shell;
using MyNet.Utilities.Collections;

namespace MyNet.Avalonia.Showcase.ViewModels;

internal sealed class MainViewModel : MainWindowViewModelBase
{
    private readonly ThreadSafeObservableCollection<IMenuItemViewModel> _menuItems = [];
    private readonly NavigationCommands _navigationCommands;
    private IMenuItemViewModel? _selectedMenuItem;

    public MainViewModel(INotificationsManager notificationsManager,
                         IAppCommandsService appCommandsService,
                         IBusyService applicationBusy,
                         INavigationService navigationService,
                         ICommandFactory commandFactory,
                         ICultureService cultureService)
        : base(notificationsManager, appCommandsService, applicationBusy, cultureService)
    {
        ApplicationBusy = applicationBusy;
        NavigationService = navigationService;
        MenuItems = new(_menuItems);

        _navigationCommands = new NavigationCommands(navigationService, commandFactory);
        GoBackCommand = _navigationCommands.GoBackCommand;
        GoForwardCommand = _navigationCommands.GoForwardCommand;
        NavigateCommand = _navigationCommands.NavigateCommand;
        _navigationCommands.SubscribeToNavigationStateChanges();
        navigationService.StateChanged += OnNavigationStateChanged;

        ChangeCultureCommand = commandFactory.Create<CultureInfo>(cultureInfo => SelectedCulture = cultureInfo);
    }

    public IBusyService ApplicationBusy { get; }

    public INavigationService NavigationService { get; }

    public ICommand GoBackCommand { get; }

    public ICommand GoForwardCommand { get; }

    public ICommand NavigateCommand { get; }

    public ICommand ChangeCultureCommand { get; }

    public ReadOnlyObservableCollection<IMenuItemViewModel> MenuItems { get; }

    public IMenuItemViewModel? SelectedMenuItem
    {
        get => _selectedMenuItem;
        private set => RaiseAndSetIfChanged(ref _selectedMenuItem, value);
    }

    public void AddMenuItem(params IMenuItemViewModel[] item) => _menuItems.AddRange(item);

    private void OnNavigationStateChanged(object? sender, NavigationStateChangedEventArgs e)
    {
        var matchingItem = FindMatchingMenuItem(e.CurrentContext?.To);

        if (matchingItem is not null && !ReferenceEquals(SelectedMenuItem, matchingItem))
            SelectedMenuItem = matchingItem;
    }

    private IMenuItemViewModel? FindMatchingMenuItem(object? page)
    {
        if (page is not INavigationPage navigationPage)
            return null;

        foreach (var item in MenuItems)
        {
            if (item is PageViewModel pageViewModel && ReferenceEquals(pageViewModel, navigationPage))
                return pageViewModel;

            if (item is PagesGroupViewModel group)
            {
                var match = group.Pages.FirstOrDefault(x => ReferenceEquals(x, navigationPage));

                if (match is not null)
                    return match;
            }
        }

        return null;
    }
}
