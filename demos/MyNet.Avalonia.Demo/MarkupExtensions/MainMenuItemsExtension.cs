// -----------------------------------------------------------------------
// <copyright file="MainMenuItemsExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Demo.Helpers;

namespace MyNet.Avalonia.Demo.MarkupExtensions;

internal sealed class MainMenuItemsExtension : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
        => MenuHelper.BuildMainMenu();
}
