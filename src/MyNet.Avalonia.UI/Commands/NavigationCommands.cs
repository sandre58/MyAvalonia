// -----------------------------------------------------------------------
// <copyright file="NavigationCommands.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Windows.Input;
using MyNet.UI.Commands;
using MyNet.UI.Navigation;

namespace MyNet.Avalonia.UI.Commands;

public static class NavigationCommands
{
    public static ICommand GoBackCommand { get; } = CommandsManager.Create(GoBack, CanGoBack);

    public static ICommand GoForwardCommand { get; } = CommandsManager.Create(GoForward, CanGoForward);

    public static ICommand NavigateCommand => CommandsManager.CreateNotNull<Type>(x => NavigationManager.NavigateTo(x));

    private static void GoBack() => NavigationManager.GoBack();

    private static bool CanGoBack() => NavigationManager.CanGoBack();

    private static void GoForward() => NavigationManager.GoForward();

    private static bool CanGoForward() => NavigationManager.CanGoForward();
}
