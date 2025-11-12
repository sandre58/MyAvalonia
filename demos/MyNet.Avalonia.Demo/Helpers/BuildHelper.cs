// -----------------------------------------------------------------------
// <copyright file="BuildHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Styling;
using MyNet.Avalonia.Controls.Assists;
using MyNet.Avalonia.Enums;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Humanizer;
using MyNet.Utilities;
using MyNet.Utilities.Generator;
using MyNet.Utilities.Helpers;

namespace MyNet.Avalonia.Demo.Helpers;

internal enum DefaultStyleDisplay
{
    WithColors,

    WithoutColors,

    Hidden
}

internal enum Color
{
    Primary,

    Accent,

    Dark,

    Inverse,

    Positive,

    Negative,

    Warning,

    Information,

    Neutral
}

internal enum FontSize
{
    SubCaption,

    Caption,

    H6,

    H5,

    H4,

    H3,

    H2,

    H1
}

internal static class BuildHelper
{
    public const string ThemeKeyPattern = "MyNet.Theme.{0}.{1}";

    public static void Build(Grid grid, ControlThemeData theme, Func<ControlData, Control> create)
    {
        var row = 0;
        grid.RowDefinitions.AddRange(EnumerableHelper.Range(0, theme.Layouts.Count + Convert.ToInt32(theme.CustomControls.Count > 0)).Select(_ => new RowDefinition(GridLength.Auto)));

        // Layouts
        foreach (var layout in theme.Layouts)
        {
            var layoutRow = 0;

            var layoutGrid = new Grid();
            layoutGrid.RowDefinitions.AddRange(EnumerableHelper.Range(0, theme.Styles.Count + Convert.ToInt32(theme.Sizes.Count > 0) + Convert.ToInt32(theme.DefaultStyleDisplay != DefaultStyleDisplay.Hidden)).Select(_ => new RowDefinition(GridLength.Auto)));
            layoutGrid.ColumnDefinitions.AddRange(EnumerableHelper.Range(0, theme.Colors.Count + 1).Select(x => new ColumnDefinition(GridLength.Auto) { SharedSizeGroup = $"column{x}" }));
            var layoutContainer = new HeaderedContentControl
            {
                Header = layout,
                Content = layoutGrid,
                Background = Brushes.Transparent,
                ClipToBounds = false,
                HorizontalContentAlignment = HorizontalAlignment.Stretch
            };
            HeaderAssist.SetHorizontalAlignment(layoutContainer, HorizontalAlignment.Stretch);
            HeaderAssist.SetPadding(layoutContainer, new Thickness(10, 0));
            layoutContainer.Classes.AddRange(["H5"]);
            Grid.SetRow(layoutContainer, row);
            grid.Children.Add(layoutContainer);

            // Styles
            if (theme.DefaultStyleDisplay != DefaultStyleDisplay.Hidden)
            {
                BuildStyle(layoutGrid, layoutRow, theme, create, layout, null, theme.DefaultStyleDisplay == DefaultStyleDisplay.WithColors);
                layoutRow++;
            }

            foreach (var styles in theme.Styles)
            {
                BuildStyle(layoutGrid, layoutRow, theme, create, layout, styles, true);
                layoutRow++;
            }

            // Sizes
            if (theme.Sizes.Count > 0)
            {
                var label = new TextBlock
                {
                    Text = "Sizes",
                    Margin = new Thickness(10),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Opacity = 0.7
                };
                Grid.SetRow(label, layoutRow);
                layoutGrid.Children.Add(label);

                var sizePanel = new StackPanel { Orientation = Orientation.Horizontal };

                Grid.SetRow(sizePanel, layoutRow);
                Grid.SetColumn(sizePanel, 1);
                Grid.SetColumnSpan(sizePanel, layoutGrid.ColumnDefinitions.Count - 1);
                layoutGrid.Children.Add(sizePanel);

                foreach (var item in theme.Sizes.Select(size => CreateControl(create, theme.Name, layout, size: size)))
                {
                    sizePanel.Children.Add(item);
                }
            }

            row++;
        }

        // Custom Controls
        if (theme.CustomControls.Count == 0)
            return;

        var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 20 };
        var customContainer = new HeaderedContentControl
        {
            Header = "Custom",
            Content = panel,
            Background = Brushes.Transparent,
            ClipToBounds = false,
            HorizontalContentAlignment = HorizontalAlignment.Stretch
        };
        HeaderAssist.SetHorizontalAlignment(customContainer, HorizontalAlignment.Stretch);
        HeaderAssist.SetPadding(customContainer, new Thickness(10, 0));
        customContainer.Classes.AddRange(["H5"]);

        Grid.SetRow(customContainer, row);
        grid.Children.Add(customContainer);

        foreach (var item in theme.CustomControls)
        {
            panel.Children.Add(item);
        }
    }

    private static void BuildStyle(Grid grid, int row, ControlThemeData theme, Func<ControlData, Control> create, string? layout, string[]? styles, bool showColors)
    {
        var column = 0;

        var label = new TextBlock
        {
            Text = styles?.Humanize(" "),
            Margin = new Thickness(10),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
            FontStyle = FontStyle.Italic
        };
        Grid.SetColumn(label, column);
        Grid.SetRow(label, row);
        grid.Children.Add(label);
        column++;

        // Colors
        BuildColor(grid, row, column, create, theme.Name, layout, styles, null);
        column++;
        if (!showColors)
            return;
        foreach (var color in theme.Colors)
        {
            BuildColor(grid, row, column, create, theme.Name, layout, styles, color);
            column++;
        }
    }

    private static void BuildColor(Grid grid, int row, int column, Func<ControlData, Control> create, string? themeName, string? layout, string[]? styles, string? color)
    {
        var item = CreateControl(create, themeName, layout, styles, color);

        Grid.SetColumn(item, column);
        Grid.SetRow(item, row);
        grid.Children.Add(item);
    }

    private static Control CreateControl(Func<ControlData, Control> create, string? themeName = null, string? layout = null, string[]? styles = null, string? color = null, string? size = null)
    {
        var item = create(new ControlData(themeName, layout, styles, color, size));
        item.Margin = new Thickness(10);

        if (!string.IsNullOrEmpty(themeName))
        {
            var themeKey = ThemeKeyPattern.FormatWith(item.GetType().Name, themeName);
            if (MyTheme.Current.TryGetResource(themeKey, null, out var value) && value is ControlTheme theme)
                item.Theme = theme;
        }

        if (!string.IsNullOrEmpty(layout))
            item.AddClasses(layout);

        if (styles is not null)
            item.AddClasses(styles);

        if (!string.IsNullOrEmpty(color))
            item.AddClasses(color);

        if (!string.IsNullOrEmpty(size))
            item.AddClasses(size);

        return item;
    }

    public static void ExecuteOnChildren<T>(Panel? panel, Action<T> action) => panel?.GetLogicalDescendants().OfType<T>().ForEach(action);

    public static void AddIconOnChildren<T>(Panel? panel, int index, Func<T, bool>? canExecute = null)
        where T : TemplatedControl
        => ExecuteOnChildren<T>(panel, x =>
        {
            if (canExecute?.Invoke(x) == false) return;

            var list = new List<string> { "Left", "Right", "Top", "Bottom" };
            x.Classes.RemoveAll(list.Select(y => $"Icon{y}"));

            if (index > 0)
            {
                IconAssist.SetIcon(x, RandomGenerator.Enum<IconData>().ToIcon());
                x.AddClasses($"Icon{list[index - 1]}");
            }
            else
            {
                IconAssist.SetIcon(x, null);
            }
        });

    public static void AddClassesOnChildren<T>(Panel panel, string[] classes, int index, Func<T, bool>? canExecute = null)
        where T : TemplatedControl
        => ExecuteOnChildren<T>(panel, x =>
        {
            if (canExecute?.Invoke(x) == false) return;

            x.Classes.RemoveAll(classes);
            x.AddClasses(classes.GetByIndex(index).OrEmpty());
        });
}

internal sealed record ControlData(string? Theme = null, string? Layout = null, string[]? Styles = null, string? Color = null, string? Size = null);

internal sealed class ControlThemeData(string? name = null, DefaultStyleDisplay defaultStyleDisplay = DefaultStyleDisplay.WithoutColors)
{
    public string? Name { get; } = name;

    public DefaultStyleDisplay DefaultStyleDisplay { get; } = defaultStyleDisplay;

    public List<string> Layouts { get; } = [string.Empty];

    public List<string[]> Styles { get; } = [];

    public List<string> Colors { get; } = [];

    public List<string> Sizes { get; } = [];

    public List<Control> CustomControls { get; } = [];

    public static List<List<string>> GetCombinations(IEnumerable<string> list)
    {
        var result = new List<List<string>>();
        GenerateCombinations([.. list], 0, [], result);
        return result;
    }

    private static void GenerateCombinations(List<string> list, int index, List<string> current, List<List<string>> result)
    {
        if (index == list.Count)
        {
            result.Add([.. current]);
            return;
        }

        // Exclude the current element
        GenerateCombinations(list, index + 1, current, result);

        // Include the current element
        current.Add(list[index]);
        GenerateCombinations(list, index + 1, current, result);
        current.RemoveAt(current.Count - 1);
    }

    public ControlThemeData AddCustomControls(Func<Control[]> createControls) => AddCustomControls(createControls());

    public ControlThemeData AddCustomControls(params Control[] customControls)
    {
        customControls.ForEach(x =>
        {
            var themeKey = BuildHelper.ThemeKeyPattern.FormatWith(x.GetType().Name, Name);
            if (MyTheme.Current.TryGetResource(themeKey, null, out var value) && value is ControlTheme theme)
                x.Theme = theme;
        });
        CustomControls.AddRange(customControls);
        return this;
    }

    public ControlThemeData AddLayouts(params string[] layouts)
    {
        Layouts.AddRange(layouts);
        return this;
    }

    public ControlThemeData AddDefaultLayout() => AddLayouts(string.Empty);

    public ControlThemeData AddStyles(params string[] styles)
    {
        Styles.AddRange(styles.Select(x => new List<string> { x }.ToArray()));
        return this;
    }

    public ControlThemeData AddDefaultStyle() => AddStyles(string.Empty);

    public ControlThemeData AddCartesianStyles(params string[] styles)
    {
        Styles.AddRange(GetCombinations(styles).Where(x => x.Count >= 2).Select(x => x.ToArray()));
        return this;
    }

    public ControlThemeData AddDefaultColors(bool withPrimaryColor = true)
    {
        var colors = new List<Color> { Color.Primary, Color.Accent, Color.Inverse, Color.Positive, Color.Negative, Color.Warning, Color.Information };

        if (!withPrimaryColor)
            colors = [.. colors.Except([Color.Primary])];
        return AddColors(colors.ToArray());
    }

    public ControlThemeData AddThemeColors(bool withPrimaryColor = true)
    {
        var colors = new List<Color> { Color.Primary, Color.Accent, Color.Inverse };

        if (!withPrimaryColor)
            colors = [.. colors.Except([Color.Primary])];
        return AddColors(colors.ToArray());
    }

    public ControlThemeData AddAllColors() => AddColors(Enum.GetValues<Color>());

    public ControlThemeData AddColors(params Color[] colors) => AddColors(colors.Select(x => x.ToString()).ToArray());

    public ControlThemeData AddColors(params string[] colors)
    {
        Colors.AddRange(colors);
        return this;
    }

    public ControlThemeData AddAllSizes() => AddSizes(Enum.GetValues<FontSize>());

    public ControlThemeData AddSizes(params FontSize[] colors) => AddSizes(colors.Select(x => x.ToString()).ToArray());

    public ControlThemeData AddSizes(params string[] sizes)
    {
        Sizes.AddRange(sizes);
        return this;
    }
}
