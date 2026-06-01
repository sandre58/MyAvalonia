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
using MyNet.UI.Loading;
using MyNet.UI.Navigation;
using MyNet.UI.Navigation.Models;
using MyNet.UI.Services;
using MyNet.UI.ViewModels.Shell.Chrome;

namespace MyNet.Avalonia.Showcase.ViewModels;

internal sealed class MainViewModel : ObservableObject
{
    private readonly ObservableCollection<IMenuItemViewModel> _menuItems = [];

    public MainViewModel(IApplicationInfo applicationInfo,
                         ShellCultureViewModel cultureChrome,
                         ShellThemeViewModel themeChrome,
                         IBusyService applicationBusy,
                         INavigationService navigationService,
                         ICommandFactory commandFactory)
    {
        ApplicationInfo = applicationInfo;
        Culture = cultureChrome;
        Theme = themeChrome;
        ApplicationBusy = applicationBusy;
        NavigationService = navigationService;
        MenuItems = new(_menuItems);

        var navigationCommands = new NavigationCommands(navigationService, commandFactory);
        GoBackCommand = navigationCommands.GoBackCommand;
        GoForwardCommand = navigationCommands.GoForwardCommand;
        NavigateCommand = navigationCommands.NavigateCommand;
        navigationCommands.SubscribeToNavigationStateChanges();
        navigationService.StateChanged += OnNavigationStateChanged;

        ChangeCultureCommand = commandFactory.Create<CultureInfo>(cultureInfo => Culture.SelectedCulture = cultureInfo);
    }

    public IApplicationInfo ApplicationInfo { get; }

    public ShellCultureViewModel Culture { get; }

    public ShellThemeViewModel Theme { get; }

    public string ProductName => ApplicationInfo.ProductName;

    public IBusyService ApplicationBusy { get; }

    public INavigationService NavigationService { get; }

    public ICommand GoBackCommand { get; }

    public ICommand GoForwardCommand { get; }

    public ICommand NavigateCommand { get; }

    public ICommand ChangeCultureCommand { get; }

    public ReadOnlyObservableCollection<IMenuItemViewModel> MenuItems { get; }

    public IMenuItemViewModel? SelectedMenuItem
    {
        get;
        private set => SetProperty(ref field, value);
    }

    public void AddMenuItem(params IMenuItemViewModel[] item) => _menuItems.AddRange(item);

    private void OnNavigationStateChanged(object? sender, NavigationStateChangedEventArgs e)
    {
        var matchingItem = FindMatchingMenuItem(e.CurrentContext?.To);

        if (matchingItem is not null && !ReferenceEquals(SelectedMenuItem, matchingItem))
            SelectedMenuItem = matchingItem;
    }

    private PageViewModel? FindMatchingMenuItem(object? page)
    {
        if (page is not INavigationPage navigationPage)
            return null;

        foreach (var item in MenuItems)
        {
            switch (item)
            {
                case PageViewModel pageViewModel when ReferenceEquals(pageViewModel, navigationPage):
                    return pageViewModel;
                case PagesGroupViewModel group:
                    {
                        var match = group.Pages.FirstOrDefault(x => ReferenceEquals(x, navigationPage));

                        if (match is not null)
                            return match;
                        break;
                    }
            }
        }

        return null;
    }
}
