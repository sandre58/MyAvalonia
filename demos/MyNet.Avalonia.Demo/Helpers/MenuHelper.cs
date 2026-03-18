// -----------------------------------------------------------------------
// <copyright file="MenuHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.Generator;
using MyNet.Utilities.Geography;
using MyNet.Utilities.Geography.Extensions;
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

    public static MenuItem[] BuildMainMenu()
    {
        // File menu
        var file = new MenuItem { Header = "_File" };
        var @new = new MenuItem { Header = "_New", InputGesture = new KeyGesture(Key.N, KeyModifiers.Control), Icon = IconData.File.ToIcon() };
        var open = new MenuItem { Header = "_Open", InputGesture = new KeyGesture(Key.O, KeyModifiers.Control), Icon = IconData.FolderOpen.ToIcon() };
        var save = new MenuItem { Header = "_Save", InputGesture = new KeyGesture(Key.S, KeyModifiers.Control), Icon = IconData.ContentSave.ToIcon() };
        var saveAs = new MenuItem { Header = "Save _As...", InputGesture = new KeyGesture(Key.S, KeyModifiers.Control | KeyModifiers.Shift), Icon = IconData.ContentSaveAll.ToIcon() };
        var close = new MenuItem { Header = "_Close", InputGesture = new KeyGesture(Key.Q, KeyModifiers.Control), Icon = IconData.Close.ToIcon() };

        _ = file.Items.Add(@new);
        _ = file.Items.Add(open);
        _ = file.Items.Add(new Separator());
        _ = file.Items.Add(save);
        _ = file.Items.Add(saveAs);
        _ = file.Items.Add(new Separator());
        _ = file.Items.Add(close);

        // Edit menu
        var edit = new MenuItem { Header = "_Edit" };
        var undo = new MenuItem { Header = "_Undo", InputGesture = new KeyGesture(Key.Z, KeyModifiers.Control), Icon = IconData.Undo.ToIcon() };
        var redo = new MenuItem { Header = "_Redo", InputGesture = new KeyGesture(Key.Y, KeyModifiers.Control), Icon = IconData.Redo.ToIcon() };
        var cut = new MenuItem { Header = "Cu_t", InputGesture = new KeyGesture(Key.X, KeyModifiers.Control), Icon = IconData.ContentCut.ToIcon() };
        var copy = new MenuItem { Header = "_Copy", InputGesture = new KeyGesture(Key.C, KeyModifiers.Control), Icon = IconData.ContentCopy.ToIcon() };
        var paste = new MenuItem { Header = "_Paste", InputGesture = new KeyGesture(Key.V, KeyModifiers.Control), Icon = IconData.ContentPaste.ToIcon(), IsEnabled = false };

        var encoding = new MenuItem { Header = "_Encoding", Icon = IconData.FormatText.ToIcon() };
        var ansi = new MenuItem { Header = "ANSI", ToggleType = MenuItemToggleType.Radio, GroupName = "encoding" };
        var utf8 = new MenuItem { Header = "UTF-8", ToggleType = MenuItemToggleType.Radio, GroupName = "encoding", IsChecked = true };
        var utf8Bom = new MenuItem { Header = "UTF-8-BOM", ToggleType = MenuItemToggleType.Radio, GroupName = "encoding" };
        var usc2 = new MenuItem { Header = "UCS-2 BE BOM", ToggleType = MenuItemToggleType.Radio, GroupName = "encoding" };

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
        var showGrid = new MenuItem { Header = "Show _Grid", IsChecked = true, Icon = IconData.Grid.ToIcon(), ToggleType = MenuItemToggleType.CheckBox };
        var showToolbar = new MenuItem { Header = "Show _Toolbar", IsChecked = true, Icon = IconData.Wrench.ToIcon(), ToggleType = MenuItemToggleType.CheckBox };
        var showStatusBar = new MenuItem { Header = "Show _Status Bar", IsChecked = false, Icon = IconData.DockBottom.ToIcon(), ToggleType = MenuItemToggleType.CheckBox };

        _ = view.Items.Add(showGrid);
        _ = view.Items.Add(showToolbar);
        _ = view.Items.Add(showStatusBar);

        // Tools menu
        var tools = new MenuItem { Header = "_Tools" };
        var languages = new MenuItem { Header = "_Languages", Icon = IconData.Translate.ToIcon() };

        EnumClass.GetAll<Country>().OrderBy(x => x.ResourceKey.Translate()).Take(10).ForEach(x =>
        {
            var item = new MenuItem
            {
                Header = x.ResourceKey.Translate(),
                ToggleType = MenuItemToggleType.Radio,
                GroupName = "language"
            };

            if (x.GetFlag(FlagSize.Pixel24) is { } flag)
            {
                using var memoryStream = new MemoryStream(flag);
                item.Icon = new Image
                {
                    Source = new Bitmap(memoryStream)
                };
            }

            _ = languages.Items.Add(item);
        });

        var preferences = new MenuItem { Header = "_Preferences", Icon = IconData.Cog.ToIcon() };

        _ = tools.Items.Add(languages);
        _ = tools.Items.Add(new Separator());
        _ = tools.Items.Add(preferences);

        // Help menu
        var help = new MenuItem { Header = "_Help" };
        var documentation = new MenuItem { Header = "_Documentation", InputGesture = new KeyGesture(Key.F1), Icon = IconData.BookOpenPageVariant.ToIcon() };
        var about = new MenuItem { Header = "_About", Icon = IconData.InformationOutline.ToIcon() };

        _ = help.Items.Add(documentation);
        _ = help.Items.Add(new Separator());
        _ = help.Items.Add(about);

        return [file, edit, view, tools, help];
    }
}
