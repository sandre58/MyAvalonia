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
using MyNet.Avalonia.Colors;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Controls.Assists;
using MyNet.Humanizer;
using MyNet.Observable;
using MyNet.Observable.Attributes;
using MyNet.UI.Selection.Models;
using MyNet.Utilities;
using MyNet.Utilities.Generator;
using MyNet.Utilities.Generator.Extensions;
using MyNet.Utilities.Geography;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class DataGridPageViewModel : ShowcaseViewModel
{
    private readonly ObservableCollection<SelectedFixture> _fixtures = [.. RandomGenerator.ListItems(EnumClass.GetAll<Country>()).Select(x => new SelectedFixture(new(x)))];

    public DataGridPageViewModel()
        : base(nameof(DataGrid),
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

        _fixtures.ForEach(x => x.Item.Referee = RandomGenerator.ListItem(AvailableReferees));

        Disposables.AddRange(
        [
            _fixtures.ToObservableChangeSet().WhenPropertyChanged(x => x.IsSelected).Subscribe(_ => OnPropertyChanged(nameof(AreAllSelected)))
        ]);
    }

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Table;

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
