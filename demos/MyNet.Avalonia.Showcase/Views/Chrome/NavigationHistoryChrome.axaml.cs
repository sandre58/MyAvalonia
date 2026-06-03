// -----------------------------------------------------------------------
// <copyright file="NavigationHistoryChrome.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace MyNet.Avalonia.Showcase.Views.Chrome;

/// <summary>
/// Back / forward controls for the navigation journal (title bar or shell band).
/// </summary>
public partial class NavigationHistoryChrome : UserControl
{
    public static readonly StyledProperty<ICommand?> GoBackCommandProperty =
        AvaloniaProperty.Register<NavigationHistoryChrome, ICommand?>(nameof(GoBackCommand));

    public static readonly StyledProperty<ICommand?> GoForwardCommandProperty =
        AvaloniaProperty.Register<NavigationHistoryChrome, ICommand?>(nameof(GoForwardCommand));

    public NavigationHistoryChrome() => InitializeComponent();

    public ICommand? GoBackCommand
    {
        get => GetValue(GoBackCommandProperty);
        set => SetValue(GoBackCommandProperty, value);
    }

    public ICommand? GoForwardCommand
    {
        get => GetValue(GoForwardCommandProperty);
        set => SetValue(GoForwardCommandProperty, value);
    }
}
