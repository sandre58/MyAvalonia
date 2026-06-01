// -----------------------------------------------------------------------
// <copyright file="SplashWindow.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;

namespace MyNet.Avalonia.Showcase.Views;

/// <summary>
/// Splash screen window displayed during initial theme resource loading.
/// Binds to <see cref="ViewModels.SplashScreenViewModel"/> for status messages, version, and copyright info.
/// </summary>
public partial class SplashWindow : Window
{
    public SplashWindow() => InitializeComponent();
}
