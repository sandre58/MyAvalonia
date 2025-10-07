// -----------------------------------------------------------------------
// <copyright file="MenusPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Demo.Views.Samples;
using MyNet.Avalonia.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.Geography;
using MyNet.Utilities.Geography.Extensions;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class MenusPage : AutoBuildPage
{
    public MenusPage()
    {
        InitializeComponent();

        var menu = this.Find<Menu>("Menu")!;
        var file = new MenuItem { Header = "_File" };
        var edit = new MenuItem { Header = "_Edit" };
        var tools = new MenuItem { Header = "_Tools" };
        var random = new MenuItem { Header = "Random" };
        _ = menu.Items.Add(file);
        _ = menu.Items.Add(edit);
        _ = menu.Items.Add(tools);
        _ = menu.Items.Add(random);

        // file
        var @new = new MenuItem { Header = "_New", InputGesture = new KeyGesture(Key.N, KeyModifiers.Control), Icon = IconData.File.ToIcon() };
        var open = new MenuItem { Header = "_Open", InputGesture = new KeyGesture(Key.O, KeyModifiers.Control) };
        var save = new MenuItem { Header = "_Save", InputGesture = new KeyGesture(Key.S, KeyModifiers.Control), Icon = IconData.ContentSave.ToIcon() };
        var close = new MenuItem { Header = "_Close", InputGesture = new KeyGesture(Key.Q, KeyModifiers.Control) };
        _ = file.Items.Add(@new);
        _ = file.Items.Add(open);
        _ = file.Items.Add(new Separator());
        _ = file.Items.Add(save);
        _ = file.Items.Add(new Separator());
        _ = file.Items.Add(close);

        // edit
        var cut = new MenuItem { Header = "_Cut", InputGesture = new KeyGesture(Key.X, KeyModifiers.Control), Icon = IconData.ContentCut.ToIcon() };
        var copy = new MenuItem { Header = "_Copy", InputGesture = new KeyGesture(Key.C, KeyModifiers.Control), Icon = IconData.ContentCopy.ToIcon() };
        var paste = new MenuItem { Header = "_Paste", InputGesture = new KeyGesture(Key.V, KeyModifiers.Control), Icon = IconData.ContentPaste.ToIcon(), IsEnabled = false };
        var encoding = new MenuItem { Header = "_Encoding" };
        var ansi = new MenuItem { Header = "ANSI", ToggleType = MenuItemToggleType.Radio, GroupName = "encoding" };
        var utf8 = new MenuItem { Header = "UTF-8", ToggleType = MenuItemToggleType.Radio, GroupName = "encoding" };
        var utf8Bom = new MenuItem { Header = "UTF-8-BOM", ToggleType = MenuItemToggleType.Radio, GroupName = "encoding" };
        var usc2 = new MenuItem { Header = "UCS-2 BE BOM", ToggleType = MenuItemToggleType.Radio, GroupName = "encoding" };
        var others = new MenuItem { Header = "Others", ToggleType = MenuItemToggleType.Radio, GroupName = "encoding" };
        var ansi2 = new MenuItem { Header = "ANSI Bis", ToggleType = MenuItemToggleType.Radio, GroupName = "encoding" };
        var utf82 = new MenuItem { Header = "UTF-8 Bis", ToggleType = MenuItemToggleType.Radio, GroupName = "encoding" };
        var utf8Bom2 = new MenuItem { Header = "UTF-8-BOM Bis", ToggleType = MenuItemToggleType.Radio, GroupName = "encoding" };
        var usc22 = new MenuItem { Header = "UCS-2 BE BOM Bis", ToggleType = MenuItemToggleType.Radio, GroupName = "encoding" };

        _ = others.Items.Add(ansi2);
        _ = others.Items.Add(utf82);
        _ = others.Items.Add(utf8Bom2);
        _ = others.Items.Add(usc22);
        _ = encoding.Items.Add(ansi);
        _ = encoding.Items.Add(utf8);
        _ = encoding.Items.Add(utf8Bom);
        _ = encoding.Items.Add(usc2);
        _ = encoding.Items.Add(others);
        _ = edit.Items.Add(cut);
        _ = edit.Items.Add(copy);
        _ = edit.Items.Add(paste);
        _ = edit.Items.Add(new Separator());
        _ = edit.Items.Add(encoding);

        // tools
        var languages = new MenuItem { Header = "_Languages" };
        EnumClass.GetAll<Country>().OrderBy(x => x.ResourceKey.Translate()).ForEach(x =>
        {
            var item = new MenuItem
            {
                Header = x.ResourceKey.Translate(),
                ToggleType = MenuItemToggleType.Radio
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
        var showGrid = new MenuItem { Header = "Show grid", IsChecked = true, Icon = IconData.Grid.ToIcon(), ToggleType = MenuItemToggleType.CheckBox };
        var showColumns = new MenuItem { Header = "Show columns", ToggleType = MenuItemToggleType.CheckBox };
        _ = tools.Items.Add(languages);
        _ = tools.Items.Add(new Separator());
        _ = tools.Items.Add(showGrid);
        _ = tools.Items.Add(showColumns);

        // random
        random.ItemsSource = MenuHelper.RandomizeMenuItems(1, 4, 10, 4);

        this.Find<Border>("Border")!.ContextMenu = new ContextMenu
        {
            ItemsSource = MenuHelper.RandomizeMenuItems(1, 4, 10, 4)
        };

        this.Find<Border>("Border2")!.ContextFlyout = new MenuFlyout
        {
            ItemsSource = MenuHelper.RandomizeMenuItems(1, 4, 10, 4)
        };

        this.Find<DropDownButton>("DropDownButton")!.Flyout = new MenuFlyout
        {
            ItemsSource = MenuHelper.RandomizeMenuItems(1, 4, 10, 4)
        };

        this.Find<Border>("Border3")!.ContextFlyout = new Flyout
        {
            Content = new LargeContent1()
        };

        this.Find<DropDownButton>("DropDownButton2")!.Flyout = new Flyout
        {
            Content = new LargeContent1()
        };
    }

    protected override Control CreateControl(ControlData data) => new();

    protected override IEnumerable<ControlThemeData> ProvideThemes() => [];
}
