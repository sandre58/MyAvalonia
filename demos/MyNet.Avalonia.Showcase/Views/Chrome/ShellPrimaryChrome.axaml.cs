// -----------------------------------------------------------------------
// <copyright file="ShellPrimaryChrome.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace MyNet.Avalonia.Showcase.Views.Chrome;

/// <summary>
/// Left shell cluster: navigation history and application brand (title bar or browser band).
/// </summary>
public partial class ShellPrimaryChrome : UserControl
{
    public static readonly StyledProperty<ICommand?> GoBackCommandProperty =
        AvaloniaProperty.Register<ShellPrimaryChrome, ICommand?>(nameof(GoBackCommand));

    public static readonly StyledProperty<ICommand?> GoForwardCommandProperty =
        AvaloniaProperty.Register<ShellPrimaryChrome, ICommand?>(nameof(GoForwardCommand));

    public static readonly StyledProperty<string?> ProductNameProperty =
        AvaloniaProperty.Register<ShellPrimaryChrome, string?>(nameof(ProductName));

    public ShellPrimaryChrome() => InitializeComponent();

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

    public string? ProductName
    {
        get => GetValue(ProductNameProperty);
        set => SetValue(ProductNameProperty, value);
    }
}
