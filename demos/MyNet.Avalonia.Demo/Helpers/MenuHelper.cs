// -----------------------------------------------------------------------
// <copyright file="MenuHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using MyNet.Avalonia.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities.Generator;
using MyNet.Utilities.Helpers;

namespace MyNet.Avalonia.Demo.Helpers;

internal static class MenuHelper
{
    public static MenuItem RandomizeMenuItem(string? header = null, bool hasSubItems = false)
    {
        var item = new MenuItem
        {
            Header = header ?? RandomGenerator.String2(5, 10),
            IsChecked = !hasSubItems && RandomGenerator.Bool(),
            ToggleType = !hasSubItems ? RandomGenerator.Enum<MenuItemToggleType>() : MenuItemToggleType.None
        };

        if (RandomGenerator.Bool())
            item.Icon = RandomGenerator.Enum<IconData>().ToIcon();

        if (!hasSubItems && RandomGenerator.Bool())
            item.InputGesture = new KeyGesture(RandomGenerator.Enum<Key>(), RandomGenerator.Enum<KeyModifiers>());
        return item;
    }

    public static MenuItem[] RandomizeMenuItems(int currentDepth, int min = 0, int max = 10, int maxDepth = 5)
        => [.. EnumerableHelper.Range(1, RandomGenerator.Int(min, max)).Select(x =>
        {
            var addSubItems = currentDepth < maxDepth && RandomGenerator.Bool();
            var item = RandomizeMenuItem($"Sub menu {currentDepth}.{x}", addSubItems);
            if (addSubItems)
            {
                item.ItemsSource = RandomizeMenuItems(currentDepth + 1, min, max, maxDepth);
            }

            return item;
        })];
}
