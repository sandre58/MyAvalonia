// -----------------------------------------------------------------------
// <copyright file="MenuHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Fakers.Static;
using MyNet.Geography;
using MyNet.Geography.Resources;

namespace MyNet.Avalonia.Showcase.Helpers;

internal static class MenuHelper
{
    public static MenuItem RandomizeMenuItem(string? header = null, bool hasSubItems = false)
    {
        var item = new MenuItem
        {
            Header = header ?? Faker.Texts.Words(1, 4),
            IsChecked = !hasSubItems && RandomGenerator.Current.Bool(),
            ToggleType = !hasSubItems ? RandomGenerator.Current.Enum<MenuItemToggleType>() : MenuItemToggleType.None
        };

        if (RandomGenerator.Current.Bool())
            item.Icon = RandomGenerator.Current.Enum<MaterialIconKind>().ToIcon();

        if (!hasSubItems && RandomGenerator.Current.Bool())
            item.InputGesture = new(RandomGenerator.Current.Enum<Key>(), RandomGenerator.Current.Enum<KeyModifiers>());
        return item;
    }

    public static MenuItem[] RandomizeMenuItems(int currentDepth, int min = 0, int max = 10, int maxDepth = 5)
        =>
        [
            .. 1.Range(1, RandomGenerator.Current.Int(min, max)).Select(x =>
            {
                var addSubItems = currentDepth < maxDepth && RandomGenerator.Current.Bool();
                var item = RandomizeMenuItem($"Sub menu {currentDepth}.{x}", addSubItems);
                if (addSubItems)
                {
                    item.ItemsSource = RandomizeMenuItems(currentDepth + 1, min, max, maxDepth);
                }

                return item;
            })
        ];

    public static MenuItem[] BuildMainMenu()
    {
        // File menu
        var file = new MenuItem { Header = "_File" };
        var @new = new MenuItem
        {
            Header = "_New",
            InputGesture = new(Key.N, KeyModifiers.Control),
            Icon = MaterialIconKind.File.ToIcon()
        };
        var open = new MenuItem
        {
            Header = "_Open",
            InputGesture = new(Key.O, KeyModifiers.Control),
            Icon = MaterialIconKind.FolderOpen.ToIcon()
        };
        var save = new MenuItem
        {
            Header = "_Save",
            InputGesture = new(Key.S, KeyModifiers.Control),
            Icon = MaterialIconKind.ContentSave.ToIcon()
        };
        var saveAs = new MenuItem
        {
            Header = "Save _As...",
            InputGesture = new(Key.S, KeyModifiers.Control | KeyModifiers.Shift),
            Icon = MaterialIconKind.ContentSaveAll.ToIcon()
        };
        var close = new MenuItem
        {
            Header = "_Close",
            InputGesture = new(Key.Q, KeyModifiers.Control),
            Icon = MaterialIconKind.Close.ToIcon()
        };

        _ = file.Items.Add(@new);
        _ = file.Items.Add(open);
        _ = file.Items.Add(new Separator());
        _ = file.Items.Add(save);
        _ = file.Items.Add(saveAs);
        _ = file.Items.Add(new Separator());
        _ = file.Items.Add(close);

        // Edit menu
        var edit = new MenuItem { Header = "_Edit" };
        var undo = new MenuItem
        {
            Header = "_Undo",
            InputGesture = new(Key.Z, KeyModifiers.Control),
            Icon = MaterialIconKind.Undo.ToIcon()
        };
        var redo = new MenuItem
        {
            Header = "_Redo",
            InputGesture = new(Key.Y, KeyModifiers.Control),
            Icon = MaterialIconKind.Redo.ToIcon()
        };
        var cut = new MenuItem
        {
            Header = "Cu_t",
            InputGesture = new(Key.X, KeyModifiers.Control),
            Icon = MaterialIconKind.ContentCut.ToIcon()
        };
        var copy = new MenuItem
        {
            Header = "_Copy",
            InputGesture = new(Key.C, KeyModifiers.Control),
            Icon = MaterialIconKind.ContentCopy.ToIcon()
        };
        var paste = new MenuItem
        {
            Header = "_Paste",
            InputGesture = new(Key.V, KeyModifiers.Control),
            Icon = MaterialIconKind.ContentPaste.ToIcon(),
            IsEnabled = false
        };

        var encoding = new MenuItem { Header = "_Encoding", Icon = MaterialIconKind.FormatText.ToIcon() };
        var ansi = new MenuItem
        {
            Header = "ANSI",
            ToggleType = MenuItemToggleType.Radio,
            GroupName = "encoding"
        };
        var utf8 = new MenuItem
        {
            Header = "UTF-8",
            ToggleType = MenuItemToggleType.Radio,
            GroupName = "encoding",
            IsChecked = true
        };
        var utf8Bom = new MenuItem
        {
            Header = "UTF-8-BOM",
            ToggleType = MenuItemToggleType.Radio,
            GroupName = "encoding"
        };
        var usc2 = new MenuItem
        {
            Header = "UCS-2 BE BOM",
            ToggleType = MenuItemToggleType.Radio,
            GroupName = "encoding"
        };

        _ = encoding.Items.Add(ansi);
        _ = encoding.Items.Add(utf8);
        _ = encoding.Items.Add(utf8Bom);
        _ = encoding.Items.Add(usc2);

        _ = edit.Items.Add(undo);
        _ = edit.Items.Add(redo);
        _ = edit.Items.Add(new Separator());
        _ = edit.Items.Add(cut);
        _ = edit.Items.Add(copy);
        _ = edit.Items.Add(paste);
        _ = edit.Items.Add(new Separator());
        _ = edit.Items.Add(encoding);

        // View menu
        var view = new MenuItem { Header = "_View" };
        var showGrid = new MenuItem
        {
            Header = "Show _Grid",
            IsChecked = true,
            Icon = MaterialIconKind.Grid.ToIcon(),
            ToggleType = MenuItemToggleType.CheckBox
        };
        var showToolbar = new MenuItem
        {
            Header = "Show _Toolbar",
            IsChecked = true,
            Icon = MaterialIconKind.Wrench.ToIcon(),
            ToggleType = MenuItemToggleType.CheckBox
        };
        var showStatusBar = new MenuItem
        {
            Header = "Show _Status Bar",
            IsChecked = false,
            Icon = MaterialIconKind.DockBottom.ToIcon(),
            ToggleType = MenuItemToggleType.CheckBox
        };

        _ = view.Items.Add(showGrid);
        _ = view.Items.Add(showToolbar);
        _ = view.Items.Add(showStatusBar);

        // Tools menu
        var tools = new MenuItem { Header = "_Tools" };
        var languages = new MenuItem { Header = "_Languages", Icon = MaterialIconKind.Translate.ToIcon() };

        CountrySource.GetAllOrderedByDisplay().ForEach(x =>
        {
            var item = new MenuItem
            {
                Header = x.Humanize(),
                ToggleType = MenuItemToggleType.Radio,
                GroupName = "language"
            };

            using var memoryStream = x.GetFlag(FlagSize.Pixel24);
            item.Icon = new Image { Source = new Bitmap(memoryStream) };

            _ = languages.Items.Add(item);
        });

        var preferences = new MenuItem { Header = "_Preferences", Icon = MaterialIconKind.Cog.ToIcon() };

        _ = tools.Items.Add(languages);
        _ = tools.Items.Add(new Separator());
        _ = tools.Items.Add(preferences);

        // Help menu
        var help = new MenuItem { Header = "_Help" };
        var documentation = new MenuItem
        {
            Header = "_Documentation",
            InputGesture = new(Key.F1),
            Icon = MaterialIconKind.BookOpenPageVariant.ToIcon()
        };
        var about = new MenuItem { Header = "_About", Icon = MaterialIconKind.InformationOutline.ToIcon() };

        _ = help.Items.Add(documentation);
        _ = help.Items.Add(new Separator());
        _ = help.Items.Add(about);

        return [file, edit, view, tools, help];
    }
}
