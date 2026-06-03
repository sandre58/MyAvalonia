// -----------------------------------------------------------------------
// <copyright file="MainViewDesignData.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Showcase.ViewModels;

namespace MyNet.Avalonia.Showcase.Composition;

/// <summary>
/// Design-time <see cref="MainViewModel"/> for Avalonia previewer (menu + shell chrome sample).
/// </summary>
internal static class MainViewDesignData
{
    /// <summary>Gets a configured main view model for XAML preview.</summary>
    public static MainViewModel MainViewModel { get; } = Create();

    private static MainViewModel Create()
    {
        var services = new AppComposition(static () => null).Build();
        var mainViewModel = AppComposition.ConfigureMainViewModel(services);
        mainViewModel.ShowShellChromeInView = true;
        return mainViewModel;
    }
}
