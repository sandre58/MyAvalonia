// -----------------------------------------------------------------------
// <copyright file="ShellTitleBarChrome.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using MyNet.UI.ViewModels.Shell.Chrome;

namespace MyNet.Avalonia.Showcase.Views.Chrome;

public partial class ShellTitleBarChrome : UserControl
{
    public static readonly StyledProperty<ShellCultureViewModel?> CultureManagerProperty = AvaloniaProperty.Register<ShellTitleBarChrome, ShellCultureViewModel?>(nameof(CultureManager));

    public static readonly StyledProperty<ShellThemeViewModel?> ThemeManagerProperty = AvaloniaProperty.Register<ShellTitleBarChrome, ShellThemeViewModel?>(nameof(ThemeManager));

    public ShellTitleBarChrome() => InitializeComponent();

    public ShellCultureViewModel? CultureManager
    {
        get => GetValue(CultureManagerProperty);
        set => SetValue(CultureManagerProperty, value);
    }

    public ShellThemeViewModel? ThemeManager
    {
        get => GetValue(ThemeManagerProperty);
        set => SetValue(ThemeManagerProperty, value);
    }
}
