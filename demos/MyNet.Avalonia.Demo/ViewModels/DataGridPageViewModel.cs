// -----------------------------------------------------------------------
// <copyright file="DataGridPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Media;
using DynamicData;
using DynamicData.Binding;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Humanizer;
using MyNet.Observable;
using MyNet.Observable.Attributes;
using MyNet.UI.Selection.Models;
using MyNet.Utilities;
using MyNet.Utilities.Generator;
using MyNet.Utilities.Generator.Extensions;
using MyNet.Utilities.Geography;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class DataGridPageViewModel : ControlCatalogViewModel
{
    private readonly ObservableCollection<SelectedFixture> _fixtures = [.. RandomGenerator.ListItems(EnumClass.GetAll<Country>()).Select(x => new SelectedFixture(new Fixture(x)))];

    public DataGridPageViewModel()
        : base("DataGrid",
            [
                new ControlThemeBuilder()
                    .AddItemsThemeRoles()
            ])
    {
        Fixtures = new DataGridCollectionView(_fixtures);
        Fixtures.GroupDescriptions.Add(new DataGridPathGroupDescription("Item.Continent"));

        _fixtures.ForEach(x => x.Item.Referee = RandomGenerator.ListItem(AvailableReferees));

        Disposables.AddRange(
        [
            _fixtures.ToObservableChangeSet().WhenPropertyChanged(x => x.IsSelected).Subscribe(_ => OnPropertyChanged(nameof(AreAllSelected)))
        ]);
    }

    /// <inheritdoc/>
    public override IconData Icon => IconData.Table;

    public DataGridCollectionView Fixtures { get; }

    public ObservableCollection<string> AvailableReferees { get; } = RandomGenerator.Int(5, 15).Range().Select(_ => NameGenerator.FullName()).Order().ToObservableCollection();

    public bool CanUserSortColumns { get; set; } = true;

    public bool CanUserReorderColumns { get; set; } = true;

    public bool CanUserResizeColumns { get; set; } = true;

    public bool IsReadOnly { get; set; }

    public DataGridHeadersVisibility HeadersVisibility { get; set; } = DataGridHeadersVisibility.Column;

    public double ColumnHeaderHeight { get; set; } = 35;

    public double RowHeaderWidth { get; set; } = 35;

    public double RowHeight { get; set; } = 40;

    public DataGridGridLinesVisibility GridLinesVisibility { get; set; } = DataGridGridLinesVisibility.None;

    public DataGridSelectionMode SelectionMode { get; set; } = DataGridSelectionMode.Extended;

    public int FrozenColumnCount { get; set; }

    public bool UseAlternateRowBackground { get; set; } = true;

    public bool ShowSelection { get; set; } = true;

    public bool ShowCellSelection { get; set; }

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
