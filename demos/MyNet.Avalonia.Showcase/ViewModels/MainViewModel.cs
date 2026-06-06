// -----------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using MyNet.Avalonia.Extended.Commands;
using MyNet.Avalonia.Showcase.ViewModels.Menu;
using MyNet.Globalization.Culture;
using MyNet.Globalization.Facade;
using MyNet.Observable;
using MyNet.UI.Commands;
using MyNet.UI.Loading;
using MyNet.UI.Navigation;
using MyNet.UI.Navigation.Models;
using MyNet.UI.Services;
using MyNet.UI.ViewModels.Shell.Chrome;

namespace MyNet.Avalonia.Showcase.ViewModels;

internal sealed class MainViewModel : ObservableObject
{
    private readonly ObservableCollection<IMenuItemViewModel> _allMenuItems = [];
    private readonly ObservableCollection<IMenuItemViewModel> _filteredMenuItems = [];
    private readonly INavigationService _navigationService;

    public MainViewModel(IApplicationInfo applicationInfo,
                         ShellCultureViewModel cultureChrome,
                         ShellThemeViewModel themeChrome,
                         IBusyService applicationBusy,
                         INavigationService navigationService,
                         ICommandFactory commandFactory)
    {
        _navigationService = navigationService;
        ApplicationInfo = applicationInfo;
        Culture = cultureChrome;
        Theme = themeChrome;
        ApplicationBusy = applicationBusy;
        MenuItems = new(_filteredMenuItems);

        var navigationCommands = new NavigationCommands(navigationService, commandFactory);
        GoBackCommand = navigationCommands.GoBackCommand;
        GoForwardCommand = navigationCommands.GoForwardCommand;
        NavigateCommand = commandFactory.CreateRequired<IMenuItemViewModel>(NavigateMenuItemAsync);
        navigationCommands.SubscribeToNavigationStateChanges();
        navigationService.StateChanged += OnNavigationStateChanged;
        GlobalizationServices.Current.CultureChanged += OnCultureChanged;
    }

    public IApplicationInfo ApplicationInfo { get; }

    public ShellCultureViewModel Culture { get; }

    public ShellThemeViewModel Theme { get; }

    public string ProductName => ApplicationInfo.ProductName;

    /// <summary>
    /// Gets or sets a value indicating whether culture/theme chrome is shown inside <see cref="Views.MainView"/> (browser host).
    /// Desktop hosts it in the window title bar instead.
    /// </summary>
    public bool ShowShellChromeInView
    {
        get;
        set => SetProperty(ref field, value);
    }

    public IBusyService ApplicationBusy { get; }

    public ICommand GoBackCommand { get; }

    public ICommand GoForwardCommand { get; }

    public ICommand NavigateCommand { get; }

    /// <summary>Gets menu items after applying <see cref="MenuSearchText"/>.</summary>
    public ReadOnlyObservableCollection<IMenuItemViewModel> MenuItems { get; }

    public IMenuItemViewModel? SelectedMenuItem
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <summary>Gets or sets the menu filter query (title contains, culture-aware).</summary>
    public string? MenuSearchText
    {
        get;
        set
        {
            if (!SetProperty(ref field, value))
                return;

            RebuildFilteredMenuItems();
            OnPropertyChanged(nameof(HasActiveMenuFilter), null, HasActiveMenuFilter);
            OnPropertyChanged(nameof(ShowMenuFilterEmpty), null, ShowMenuFilterEmpty);
        }
    }

    /// <summary>Gets a value indicating whether <see cref="MenuSearchText"/> is filtering the menu.</summary>
    public bool HasActiveMenuFilter => !string.IsNullOrWhiteSpace(MenuSearchText);

    /// <summary>Gets a value indicating whether the filter is active but returned no entries.</summary>
    public bool ShowMenuFilterEmpty => HasActiveMenuFilter && _filteredMenuItems.Count == 0;

    public void AddMenuItem(params IMenuItemViewModel[] item)
    {
        _allMenuItems.AddRange(item);
        RebuildFilteredMenuItems();
    }

    private void RebuildFilteredMenuItems()
    {
        _filteredMenuItems.Clear();
        foreach (var item in MenuFilter.Apply(_allMenuItems, MenuSearchText))
            _filteredMenuItems.Add(item);

        OnPropertyChanged(nameof(ShowMenuFilterEmpty), null, ShowMenuFilterEmpty);
    }

    private void OnCultureChanged(object? sender, CultureChangedEventArgs e)
    {
        if (HasActiveMenuFilter)
            RebuildFilteredMenuItems();
    }

    private void OnNavigationStateChanged(object? sender, NavigationStateChangedEventArgs e)
    {
        var matchingItem = FindMatchingMenuItem(e.CurrentContext?.To);

        if (matchingItem is not null && !ReferenceEquals(SelectedMenuItem, matchingItem))
            SelectedMenuItem = matchingItem;
    }

    private Task NavigateMenuItemAsync(IMenuItemViewModel item)
    {
        ArgumentNullException.ThrowIfNull(item);

        return item switch
        {
            LazyPageMenuItem lazy => _navigationService.NavigateToAsync(lazy.ResolvePage()),
            _ when item.NavigationTarget is not null => _navigationService.NavigateToAsync(item.NavigationTarget),
            _ => Task.CompletedTask
        };
    }

    private LazyPageMenuItem? FindMatchingMenuItem(object? page)
    {
        if (page is not INavigationPage navigationPage)
            return null;

        var pageType = navigationPage.GetType();

        foreach (var item in _allMenuItems)
        {
            if (item.IsSectionHeader)
                continue;

            switch (item)
            {
                case LazyPageMenuItem lazy when lazy.ViewModelType == pageType:
                    return lazy;
                case PagesGroupViewModel group:
                    {
                        var match = group.Pages
                            .OfType<LazyPageMenuItem>()
                            .FirstOrDefault(x => x.ViewModelType == pageType);

                        if (match is not null)
                            return match;
                        break;
                    }
            }
        }

        return null;
    }
}
