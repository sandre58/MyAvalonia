// -----------------------------------------------------------------------
// <copyright file="ToolBarPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Layout;
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class ToolBarPageViewModel : ShowcaseViewModel
{
    private static readonly MaterialIconKind[] AddItemIcons =
    [
        MaterialIconKind.FileOutline,
        MaterialIconKind.ContentSaveOutline,
        MaterialIconKind.Undo,
        MaterialIconKind.Redo,
        MaterialIconKind.ContentCut,
        MaterialIconKind.ContentCopy,
        MaterialIconKind.ContentPaste,
        MaterialIconKind.Magnify,
        MaterialIconKind.CogOutline,
        MaterialIconKind.HelpCircleOutline
    ];

    private int _itemCounter;
    private readonly ICommand _noopCommand;

    public ToolBarPageViewModel(ICommandFactory commands)
        : base(nameof(ToolBar), commands, [
            new ControlThemeBuilder()
                .AddEnumProperty<ToolBarLayoutMode, ComboBoxEditor>(
                    ToolBar.LayoutModeProperty,
                    configure: x => x.DisplayName(nameof(SettingsResources.Layout)))
                .AddEnumProperty<ToolBarOverflowMode, ComboBoxEditor>(
                    ToolBar.OverflowModeProperty,
                    configure: x => x.DisplayName(nameof(ToolBarPageResources.OverflowMode)))
                .AddEnumProperty<Orientation, ListBoxEditor>(
                    ToolBar.OrientationProperty,
                    Orientation.Horizontal,
                    x => x.DisplayName(nameof(SettingsResources.Orientation)),
                    configureChoice: (orientation, choice) => choice.WithIcon(orientation == Orientation.Horizontal
                        ? MaterialIconKind.ArrowLeftRight
                        : MaterialIconKind.ArrowUpDown))
                .AddProperty(ToolBar.ItemSpacingProperty, 2d, x => x
                    .DisplayName(nameof(ToolBarPageResources.ItemSpacing))
                    .Of<SliderEditor>(editor => editor.WithRange(0, 20)))
        ])
    {
        _noopCommand = commands.Create(() => { });
        AddItemCommand = commands.Create(AddItem);
        RemoveItemCommand = commands.Create(RemoveItem);
        ResetItemsCommand = commands.Create(ResetItems);
        ResetItems();
    }

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Wrench;

    public ObservableCollection<object> Items { get; } = [];

    public double PreviewWidth
    {
        get;
        set => SetProperty(ref field, value);
    } 
= 480;

    public ICommand AddItemCommand { get; }

    public ICommand RemoveItemCommand { get; }

    public ICommand ResetItemsCommand { get; }

    private void AddItem()
    {
        _itemCounter++;
        Items.Add(CreateItem($"Item {_itemCounter}", AddItemIcons[(_itemCounter - 1) % AddItemIcons.Length]));
    }

    private void RemoveItem()
    {
        for (var i = Items.Count - 1; i >= 0; i--)
        {
            if (Items[i] is ToolBarItemViewModel)
            {
                Items.RemoveAt(i);
                return;
            }
        }
    }

    private void ResetItems() => LoadStandardPreset();

    private void LoadStandardPreset()
    {
        Items.Clear();
        _itemCounter = 8;
        Items.Add(CreateItem("New", MaterialIconKind.FileOutline));
        Items.Add(CreateItem("Open", MaterialIconKind.FolderOpenOutline));
        Items.Add(CreateItem("Save", MaterialIconKind.ContentSaveOutline));
        Items.Add(new ToolBarSeparatorItem());
        Items.Add(CreateItem("Undo", MaterialIconKind.Undo));
        Items.Add(CreateItem("Redo", MaterialIconKind.Redo));
        Items.Add(CreateItem("Cut", MaterialIconKind.ContentCut));
        Items.Add(CreateItem("Copy", MaterialIconKind.ContentCopy));
        Items.Add(CreateItem("Paste", MaterialIconKind.ContentPaste));
    }

    private ToolBarItemViewModel CreateItem(string title, MaterialIconKind icon, ToolBarOverflowPriority priority = ToolBarOverflowPriority.Normal) => new()
    {
        Title = title,
        Icon = icon,
        Command = _noopCommand,
        OverflowPriority = priority
    };
}
