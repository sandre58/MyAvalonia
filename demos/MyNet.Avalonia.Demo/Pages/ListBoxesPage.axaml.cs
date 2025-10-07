// -----------------------------------------------------------------------
// <copyright file="ListBoxesPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Assists;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Enums;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Observable.Translatables;
using MyNet.Utilities;
using MyNet.Utilities.Generator;
using MyNet.Utilities.Geography;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class ListBoxesPage : AutoBuildPage
{
    public ListBoxesPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        ListBox item;

        if (data.Theme.ContainsAny("Toggle", "Cards", "Icon", "Tabs"))
        {
            item = new ListBox
            {
                SelectionMode = SelectionMode.Toggle,
                HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center
            };

            var items = Enum.GetValues<DayOfWeek>().Take(4).Select(x =>
            {
                var listBoxItem = new ListBoxItem();

                if (!data.Theme.ContainsAny("Icon"))
                {
                    _ = listBoxItem.Bind(ContentProperty, new Binding(nameof(EnumTranslatable.Display))
                    {
                        Source = new EnumTranslatable(x)
                    });
                }
                else
                {
                    listBoxItem.Content = RandomGenerator.Enum<IconData>().ToIcon();
                }

                if (!data.Theme.ContainsAny("Icon"))
                    IconAssist.SetIcon(listBoxItem, RandomGenerator.Enum<IconData>().ToIcon());

                return listBoxItem;
            });
            items.ForEach(x => item.Items.Add(x));
        }
        else
        {
            item = new ListBox
            {
                ItemsSource = EnumClass.GetAll<Country>(),
                MaxHeight = 360,
                HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Stretch,
                SelectionMode = SelectionMode.Multiple,
                ItemTemplate = (DataTemplate?)this.FindResource("MyNet.DataTemplate.Country")
            };
        }

        item.SelectedIndex = RandomGenerator.Int(0, item.Items.Count - 1);
        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData(defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddThemeColors(false),

            new ControlThemeData("Cards", defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddLayouts("Circle")
            .AddStyles("Transparent", "Outlined", "Shadow")
            .AddCartesianStyles("Transparent", "Outlined", "Light")
            .AddThemeColors(false)
            .AddSizes("Small", "Medium", "Large")
            .AddCustomControls(() =>
            {
                var item = new ListBox
                {
                     ItemsSource = EnumClass.GetAll<Country>(),
                     MaxHeight = 360,
                     Width = 360,
                     SelectionMode = SelectionMode.Multiple,
                     ItemTemplate = (DataTemplate?)this.FindResource("MyNet.DataTemplate.Country.Large")
                };
                item.AddClasses("Outlined");

                var item2 = new ListBox
                {
                     ItemsSource = EnumClass.GetAll<Country>(),
                     MaxHeight = 360,
                     SelectionMode = SelectionMode.Toggle,
                     ItemTemplate = (DataTemplate?)this.FindResource("MyNet.DataTemplate.Country")
                };
                item2.AddClasses("Vertical");
                return [item, item2];
            }),

            new ControlThemeData("Toggle", defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddStyles("Spacing", "Shadow", "Vertical")
            .AddCartesianStyles("Spacing", "Shadow", "Vertical")
            .AddThemeColors(false)
            .AddSizes("Small", "Medium", "Large")
            .AddCustomControls(() =>
            {
                var item = new ListBox
                {
                    SelectionMode = SelectionMode.Toggle,
                    HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center
                };

                var items = Enum.GetValues<TextAlignment>().Select(x =>
                {
                    var listBoxItem = new ListBoxItem
                    {
                        Content = x switch {
                            TextAlignment.Justify => IconData.FormatAlignJustify.ToIcon(),
                            TextAlignment.Center => IconData.FormatAlignCenter.ToIcon(),
                            TextAlignment.Left => IconData.FormatAlignLeft.ToIcon(),
                            TextAlignment.Right => IconData.FormatAlignRight.ToIcon(),
                            TextAlignment.Start => IconData.FormatAlignBottom.ToIcon(),
                            TextAlignment.End => IconData.FormatAlignTop.ToIcon(),
                            TextAlignment.DetectFromContent => IconData.FormatAlignMiddle.ToIcon(),
                            _ => null
                        }
                    };
                    _ = listBoxItem.Bind(ToolTip.TipProperty, new Binding(nameof(EnumTranslatable.Display))
                    {
                        Source = new EnumTranslatable(x)
                    });

                    return listBoxItem;
                });
                items.ForEach(x => item.Items.Add(x));
                return [item];
            }),

            new ControlThemeData("Tabs", defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddStyles("Vertical")
            .AddThemeColors(false),

            new ControlThemeData("Icon", DefaultStyleDisplay.WithColors)
            .AddStyles("Vertical")
            .AddThemeColors(false)
            .AddSizes("Small", "Medium", "Large")
        ];
}
