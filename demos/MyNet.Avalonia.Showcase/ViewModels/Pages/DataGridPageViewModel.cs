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
using Material.Icons;
using MyNet.Avalonia.Geography;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Controls.Assists;
using MyNet.Collections;
using MyNet.Fakers.Static;
using MyNet.Generator.Facade;
using MyNet.Geography;
using MyNet.Humanizer.Facade;
using MyNet.Observable;
using MyNet.Observable.Behaviors.Metadata.Attributes;
using MyNet.Primitives;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class DataGridPageViewModel : ShowcaseViewModel
{
    private readonly ObservableCollection<SelectedFixture> _fixtures = [.. CountrySource.GetAllOrderedByDisplay().Select(x => new SelectedFixture(new(x)))];

    public DataGridPageViewModel(ICommandFactory commands)
        : base(nameof(DataGrid),
            commands,
            [
                new ControlThemeBuilder()
                    .AddItemsThemeRoles()
                    .AddProperty(DataGrid.CanUserSortColumnsProperty, true, x => x.DisplayName(nameof(SettingsResources.CanUserSortColumns)))
                    .AddProperty(DataGrid.CanUserReorderColumnsProperty, false, x => x.DisplayName(nameof(SettingsResources.CanUserReorderColumns)))
                    .AddProperty(DataGrid.CanUserResizeColumnsProperty, false, x => x.DisplayName(nameof(SettingsResources.CanUserResizeColumns)))
                    .AddProperty(DataGrid.IsReadOnlyProperty, false, x => x.DisplayName(nameof(SettingsResources.IsReadOnly)))
                    .AddEnumProperty<DataGridHeadersVisibility, ListBoxEditor>(DataGrid.HeadersVisibilityProperty,
                        DataGridHeadersVisibility.Column,
                        x => x.DisplayName(nameof(SettingsResources.HeadersVisibility)),
                        configureChoice: (x, y) =>
                        {
                            switch (x)
                            {
                                case DataGridHeadersVisibility.Column:
                                    y.WithIcon(MaterialIconKind.TableRow);
                                    break;
                                case DataGridHeadersVisibility.Row:
                                    y.WithIcon(MaterialIconKind.TableColumn);
                                    break;
                                case DataGridHeadersVisibility.All:
                                    y.WithIcon(MaterialIconKind.TableHeadersEye);
                                    break;
                                case DataGridHeadersVisibility.None:
                                    y.WithIcon(MaterialIconKind.CircleOffOutline);
                                    break;
                            }
                        })
                    .AddProperty(DataGrid.ColumnHeaderHeightProperty, 35, x => x.DisplayName(nameof(SettingsResources.ColumnHeaderHeight)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(0, 100)))
                    .AddProperty(DataGrid.RowHeaderWidthProperty, 40, x => x.DisplayName(nameof(SettingsResources.RowHeaderWidth)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(0, 100)))
                    .AddProperty(DataGrid.RowHeightProperty, 40, x => x.DisplayName(nameof(SettingsResources.RowHeight)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(0, 100)))
                    .AddEnumProperty<DataGridGridLinesVisibility, ListBoxEditor>(DataGrid.GridLinesVisibilityProperty,
                        DataGridGridLinesVisibility.None,
                        x => x.DisplayName(nameof(SettingsResources.GridLinesVisibility)),
                        configureChoice: (x, y) =>
                        {
                            switch (x)
                            {
                                case DataGridGridLinesVisibility.Horizontal:
                                    y.WithIcon(MaterialIconKind.BorderHorizontal);
                                    break;
                                case DataGridGridLinesVisibility.Vertical:
                                    y.WithIcon(MaterialIconKind.BorderVertical);
                                    break;
                                case DataGridGridLinesVisibility.All:
                                    y.WithIcon(MaterialIconKind.TableHeadersEye);
                                    break;
                                case DataGridGridLinesVisibility.None:
                                    y.WithIcon(MaterialIconKind.CircleOffOutline);
                                    break;
                            }
                        })
                    .AddEnumProperty<DataGridSelectionMode, ListBoxEditor>(DataGrid.SelectionModeProperty,
                        DataGridSelectionMode.Extended,
                        x => x.DisplayName(nameof(SettingsResources.SelectionMode)),
                        configureChoice: (x, y) =>
                        {
                            switch (x)
                            {
                                case DataGridSelectionMode.Extended:
                                    y.WithIcon(MaterialIconKind.CheckboxMultipleMarked);
                                    break;
                                case DataGridSelectionMode.Single:
                                    y.WithIcon(MaterialIconKind.CheckboxMarked);
                                    break;
                            }
                        })
                    .AddProperty(DataGrid.FrozenColumnCountProperty, 0, x => x.DisplayName(nameof(SettingsResources.FrozenColumnCount)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(0, 5)))
                    .AddProperty(DataGridAssist.UseAlternateRowBackgroundProperty, true, x => x.DisplayName(nameof(SettingsResources.UseAlternateRowBackground)))
                    .AddProperty(DataGridAssist.ShowSelectionProperty, true, x => x.DisplayName(nameof(SettingsResources.ShowSelection)))
                    .AddProperty(DataGridAssist.ShowCellSelectionProperty, false, x => x.DisplayName(nameof(SettingsResources.ShowCellSelection)))
            ])
    {
        Fixtures = new(_fixtures);
        Fixtures.GroupDescriptions.Add(new DataGridPathGroupDescription("Item.Continent"));

        _fixtures.ForEach(x => x.Item.Referee = RandomGenerator.Current.Item(AvailableReferees));

        Disposables.AddRange(
        [
            _fixtures.ToObservableChangeSet().WhenPropertyChanged(x => x.IsSelected).Subscribe(_ => OnPropertyChanged(nameof(AreAllSelected), null, AreAllSelected))
        ]);
    }

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Table;

    public DataGridCollectionView Fixtures { get; }

    public ObservableCollection<string> AvailableReferees { get; } = Enumerable.Range(0, RandomGenerator.Current.Int(5, 15)).Select(_ => Faker.Names.FullName(GenderType.Male)).Order().ToObservableCollection();

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

public sealed class SelectedFixture(Fixture fixture) : ObservableObject
{
    public Fixture Item { get; } = fixture;

    public bool IsSelected
    {
        get;
        set => SetProperty(ref field, value);
    }
}

public class Fixture(Country home) : ObservableObject
{
    [UpdateOnCultureChanged]
    public string Continent => home.Continent.Humanize();

    public Country Home => home;

    public Country Away { get; set; } = Faker.Countries.Country();

    public Color? HomeColor { get; set; } = Faker.Colors.Hex().ToColor();

    public Color? AwayColor { get; set; } = Faker.Colors.Hex().ToColor();

    public DateTime? Date { get; set; } = RandomGenerator.Current.Date(DateTime.Now.AddDays(-365), DateTime.Now.AddDays(365));

    public TimeSpan Time { get; set; } = RandomGenerator.Current.Date(DateTime.Now.AddDays(-365), DateTime.Now.AddDays(365)).TimeOfDay;

    public string? Venue { get; set; } = Faker.Countries.Country().Humanize();

    public string? Referee { get; set; }

    [Range(0, 10)]
    public int? HomeScore { get; set; } = RandomGenerator.Current.Int(0, 4);

    [Range(0, 10)]
    public int? AwayScore { get; set; } = RandomGenerator.Current.Int(0, 4);
}
