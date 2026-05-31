// -----------------------------------------------------------------------
// <copyright file="ThemeListViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;

namespace MyNet.Avalonia.PerfTest.ViewModels;

/// <summary>
/// Stress test with MyNet theme classes and <c>has-role</c> / <c>ThemeRole</c> bindings (1000 items).
/// </summary>
public class ThemeListViewModel : ViewModelBase
{
    public ObservableCollection<ListItem> Items { get; } = new();

    public ThemeListViewModel()
    {
        var random = new Random(42);

        for (var i = 0; i < 1000; i++)
        {
            Items.Add(new()
            {
                Id = i + 1,
                Title = $"Theme Item {i + 1}",
                Subtitle = $"Role + variant styling",
                Description = $"Item {i + 1} uses variant-outlined and has-role TextBlocks to stress theme bindings.",
                ImageUrl = $"https://picsum.photos/seed/theme{i}/48/48",
                Rating = random.Next(1, 6),
                Price = random.Next(10, 1000) + 0.99m,
                InStock = random.Next(0, 2) == 1
            });
        }
    }
}
