// -----------------------------------------------------------------------
// <copyright file="DataGridsViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Avalonia.Collections;
using Avalonia.Media;
using DynamicData;
using DynamicData.Binding;
using MyNet.Avalonia.Extensions;
using MyNet.Humanizer;
using MyNet.Observable;
using MyNet.Observable.Attributes;
using MyNet.UI.Selection.Models;
using MyNet.UI.ViewModels.Workspace;
using MyNet.Utilities;
using MyNet.Utilities.Generator;
using MyNet.Utilities.Generator.Extensions;
using MyNet.Utilities.Geography;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class DataGridsViewModel : NavigableWorkspaceViewModel
{
    private readonly ObservableCollection<SelectedFixture> _fixtures = [.. RandomGenerator.ListItems(EnumClass.GetAll<Country>()).Select(x => new SelectedFixture(new Fixture(x)))];

    public DataGridCollectionView Fixtures { get; }

    public ObservableCollection<string> AvailableReferees { get; } = RandomGenerator.Int(5, 15).Range().Select(_ => NameGenerator.FullName()).OrderBy(x => x).ToObservableCollection();

    public DataGridsViewModel()
    {
        Fixtures = new DataGridCollectionView(_fixtures);
        Fixtures.GroupDescriptions.Add(new DataGridPathGroupDescription("Item.Continent"));

        _fixtures.ForEach(x => x.Item.Referee = RandomGenerator.ListItem(AvailableReferees));

        Disposables.AddRange(
        [
            _fixtures.ToObservableChangeSet().WhenPropertyChanged(x => x.IsSelected).Subscribe(_ => OnPropertyChanged(nameof(AreAllSelected)))
        ]);
    }

    public bool? AreAllSelected
    {
        get
        {
            var selected = _fixtures.Select(item => item.IsSelected).Distinct().ToList();
            return selected.Count == 1 ? selected.Single() : null;
        }

        set
        {
            if (value.HasValue)
                _fixtures.ForEach(x => x.IsSelected = value.Value);
        }
    }
}

public class SelectedFixture(Fixture fixture) : SelectedWrapper<Fixture>(fixture);

public class Fixture(Country home) : ObservableObject
{
    [UpdateOnCultureChanged]
    public string? Continent => home.Continent.Humanize();

    public Country Home => home;

    [IsRequired]
    public Country Away { get; set; } = RandomGenerator.Country();

    public Color? HomeColor { get; set; } = RandomGenerator.Color().ToColor();

    public Color? AwayColor { get; set; } = RandomGenerator.Color().ToColor();

    [IsRequired]
    public DateTime? Date { get; set; } = RandomGenerator.Date(DateTime.Now.AddDays(-365), DateTime.Now.AddDays(365));

    [IsRequired]
    public TimeSpan Time { get; set; } = RandomGenerator.Date(DateTime.Now.AddDays(-365), DateTime.Now.AddDays(365)).TimeOfDay;

    public string? Venue { get; set; } = RandomGenerator.Country().Humanize();

    public string? Referee { get; set; }

    [Range(0, 10)]
    public int? HomeScore { get; set; } = RandomGenerator.Int(0, 4);

    [Range(0, 10)]
    public int? AwayScore { get; set; } = RandomGenerator.Int(0, 4);
}
