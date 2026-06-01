// -----------------------------------------------------------------------
// <copyright file="SplashScreenViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.Services;

namespace MyNet.Avalonia.Showcase.ViewModels;

/// <summary>
/// Splash screen for the showcase desktop host.
/// </summary>
public class SplashScreenViewModel(IApplicationInfo applicationInfo)
    : MyNet.UI.ViewModels.Shell.Startup.SplashScreenViewModel(applicationInfo);
