// -----------------------------------------------------------------------
// <copyright file="DrawerAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using Avalonia.Controls;
using MyNet.Avalonia.Commands;

namespace MyNet.Avalonia.Theme.Assists;

public static class DrawerAssist
{
    public static ICommand? ToggleCommand { get; } = ActionCommand.Create<DrawerPage>(Toggle);

    public static void Toggle(DrawerPage element) => element.IsOpen = !element.IsOpen;
}
