// -----------------------------------------------------------------------
// <copyright file="BuildHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Styling;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Helpers;
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Avalonia.Theme.Palettes;
using MyNet.Humanizer;
using MyNet.Utilities;
using MyNet.Utilities.Generator;
using MyNet.Utilities.Helpers;
using MyNet.Utilities.Logging;

namespace MyNet.Avalonia.Demo.Helpers;

/// <summary>
/// Specifies how default styles are displayed in theme sections.
/// </summary>
internal enum DefaultStyleDisplay
{
    /// <summary>
    /// Show default styles with roles.
    /// </summary>
    WithRoles,

    /// <summary>
    /// Show default styles without roles.
    /// </summary>
    WithoutRoles,

    /// <summary>
    /// Hide default styles.
    /// </summary>
    Hidden
}

/// <summary>
/// Font size options for theme controls.
/// </summary>
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

/// <summary>
/// Helper for building themed control grids for demo pages.
/// </summary>
internal static class BuildHelper
{
    private static readonly Thickness DefaultMargin = new(10);
    private static readonly ConcurrentDictionary<(Type ControlType, string ThemeName), ControlTheme> ThemeCache = new();

    /// <summary>
    /// Builds a grid of themed controls for a demo page, organized by layouts, styles, roles, and sizes.
    /// </summary>
    /// <param name="grid">The grid to populate.</param>
    /// <param name="theme">Theme data describing layouts, styles, roles, sizes, and custom controls.</param>
    /// <param name="create">Factory for creating controls.</param>
    /// <param name="cancellationToken">Optional cancellation token for aborting build early.</param>
    public static void Build(Grid grid, ControlThemeData theme, Func<ControlData, Control> create, CancellationToken cancellationToken = default)
    {
        grid.RowDefinitions.Clear();
        grid.Children.Clear();

        var rowCount = theme.Layouts.Count + Convert.ToInt32(theme.CustomControls.Count > 0);
        grid.RowDefinitions.AddRange(EnumerableHelper.Range(0, Math.Max(1, rowCount)).Select(_ => new RowDefinition(GridLength.Auto)));

        var themeLabel = FormatThemeName(theme.Name);

        using (PerformanceMonitor.Measure($"[PERF] BuildHelper - Theme '{themeLabel}' | {theme.Layouts.Count} layout(s)", 50.Milliseconds(), 200.Milliseconds()))
        {
            var row = 0;
            foreach (var layout in theme.Layouts)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                var layoutGrid = CreateLayoutGrid(theme);
                var layoutContainer = CreateLayoutContainer(layout, layoutGrid);
                Grid.SetRow(layoutContainer, row);
                grid.Children.Add(layoutContainer);

                using (PerformanceMonitor.Measure($"BuildHelper - Layout '{FormatLayoutName(layout)}'", 5.Milliseconds(), 20.Milliseconds()))
                {
                    var layoutRow = 0;

                    if (theme.DefaultStyleDisplay != DefaultStyleDisplay.Hidden)
                    {
                        BuildStyle(layoutGrid, layoutRow++, theme, create, layout, null, theme.DefaultStyleDisplay == DefaultStyleDisplay.WithRoles, cancellationToken);
                    }

                    foreach (var styles in theme.Styles)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            break;

                        BuildStyle(layoutGrid, layoutRow++, theme, create, layout, styles, true, cancellationToken);
                    }

                    if (!cancellationToken.IsCancellationRequested && theme.Sizes.Count > 0)
                    {
                        BuildSizes(layoutGrid, layoutRow, theme, create, layout, cancellationToken);
                    }
                }

                LogManager.Debug($"[PERF] BuildHelper - Layout '{FormatLayoutName(layout)}' rows: {layoutGrid.RowDefinitions.Count}");
                row++;
            }

            if (cancellationToken.IsCancellationRequested || theme.CustomControls.Count == 0)
            {
                LogManager.Debug($"[PERF] BuildHelper - Theme '{themeLabel}' {(cancellationToken.IsCancellationRequested ? "cancelled" : "completed")} with {grid.Children.Count} container(s)");
                return;
            }

            BuildCustomControls(grid, theme, row, cancellationToken);

            LogManager.Debug($"[PERF] BuildHelper - Theme '{themeLabel}' completed with {grid.Children.Count} container(s)");
        }
    }

    /// <summary>
    /// Creates a grid for a layout section, with shared size columns for roles/colors.
    /// </summary>
    private static Grid CreateLayoutGrid(ControlThemeData theme)
    {
        var layoutGrid = new Grid();
        Grid.SetIsSharedSizeScope(layoutGrid, true);

        var styleRowCount = theme.Styles.Count + Convert.ToInt32(theme.Sizes.Count > 0) + Convert.ToInt32(theme.DefaultStyleDisplay != DefaultStyleDisplay.Hidden);
        layoutGrid.RowDefinitions.AddRange(EnumerableHelper.Range(0, Math.Max(1, styleRowCount)).Select(_ => new RowDefinition(GridLength.Auto)));

        var colorColumnCount = Math.Max(1, theme.Colors.Count + 1);
        layoutGrid.ColumnDefinitions.AddRange(EnumerableHelper.Range(0, colorColumnCount).Select(x => new ColumnDefinition(GridLength.Auto) { SharedSizeGroup = $"column{x}" }));

        return layoutGrid;
    }

    /// <summary>
    /// Creates a headered container for a layout section.
    /// </summary>
    private static HeaderedContentControl CreateLayoutContainer(string layout, Grid layoutGrid)
    {
        var layoutContainer = new HeaderedContentControl
        {
            Header = FormatLayoutName(layout),
            Content = layoutGrid,
            Background = Brushes.Transparent,
            ClipToBounds = false,
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            Margin = new Thickness(0, 0, 0, 8)
        };

        HeaderAssist.SetHorizontalAlignment(layoutContainer, HorizontalAlignment.Stretch);
        HeaderAssist.SetPadding(layoutContainer, new Thickness(10, 0));
        layoutContainer.Classes.AddRange(["H5"]);

        return layoutContainer;
    }

    /// <summary>
    /// Builds a horizontal panel of controls for each size variant.
    /// </summary>
    private static void BuildSizes(Grid layoutGrid, int layoutRow, ControlThemeData theme, Func<ControlData, Control> create, string? layout, CancellationToken cancellationToken)
    {
        var label = new TextBlock
        {
            Text = "Sizes",
            Margin = DefaultMargin,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
            Opacity = 0.7
        };
        Grid.SetRow(label, layoutRow);
        layoutGrid.Children.Add(label);

        var sizePanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 12 };
        Grid.SetRow(sizePanel, layoutRow);
        Grid.SetColumn(sizePanel, 1);
        Grid.SetColumnSpan(sizePanel, Math.Max(1, layoutGrid.ColumnDefinitions.Count - 1));
        layoutGrid.Children.Add(sizePanel);

        foreach (var size in theme.Sizes)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            var item = CreateControl(create, theme.Name, layout, size: size);
            sizePanel.Children.Add(item);
        }
    }

    /// <summary>
    /// Builds a row of controls for a style variant, optionally with color/role columns.
    /// </summary>
    private static void BuildStyle(Grid grid, int row, ControlThemeData theme, Func<ControlData, Control> create, string? layout, string[]? styles, bool showColors, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        var column = 0;

        var label = new TextBlock
        {
            Text = styles?.Humanize(" "),
            Margin = DefaultMargin,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
            FontStyle = FontStyle.Italic
        };
        Grid.SetColumn(label, column);
        Grid.SetRow(label, row);
        grid.Children.Add(label);
        column++;

        BuildColor(grid, row, column++, create, theme.Name, layout, styles, null, cancellationToken);
        if (!showColors)
            return;

        foreach (var color in theme.Colors)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            BuildColor(grid, row, column++, create, theme.Name, layout, styles, color, cancellationToken);
        }
    }

    /// <summary>
    /// Builds a control for a specific color/role in a style row.
    /// </summary>
    private static void BuildColor(Grid grid, int row, int column, Func<ControlData, Control> create, string? themeName, string? layout, string[]? styles, ThemeRole? role, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        var item = CreateControl(create, themeName, layout, styles, role);
        Grid.SetColumn(item, column);
        Grid.SetRow(item, row);
        grid.Children.Add(item);
    }

    /// <summary>
    /// Builds a wrap panel of custom controls for the theme section.
    /// </summary>
    private static void BuildCustomControls(Grid grid, ControlThemeData theme, int row, CancellationToken cancellationToken)
    {
        var panel = new WrapPanel { Orientation = Orientation.Horizontal };
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
            if (cancellationToken.IsCancellationRequested)
                break;

            if (item.Margin.Right == 0)
                item.Margin = new Thickness(item.Margin.Left, item.Margin.Top, 10, item.Margin.Bottom);

            panel.Children.Add(item);
        }
    }

    /// <summary>
    /// Creates a themed control using the provided factory and applies theme, role, layout, style, and size.
    /// </summary>
    private static Control CreateControl(Func<ControlData, Control> create, string? themeName = null, string? layout = null, string[]? styles = null, ThemeRole? role = null, string? size = null)
    {
        var item = create(new ControlData(themeName, layout, styles, role, size));
        item.Margin = DefaultMargin;

        AttachTheme(item, themeName);

        if (role.HasValue)
            ThemeAssist.SetRole(item, role.Value);

        if (!string.IsNullOrEmpty(layout))
            item.AddClasses(layout);

        if (styles is not null)
            item.AddClasses(styles);

        if (!string.IsNullOrEmpty(size))
            item.AddClasses(size);

        return item;
    }

    /// <summary>
    /// Attaches a cached theme to a control if available, otherwise resolves and caches it.
    /// </summary>
    /// <param name="control">The control to theme.</param>
    /// <param name="themeName">The theme name.</param>
    internal static void AttachTheme(Control control, string? themeName)
    {
        if (string.IsNullOrWhiteSpace(themeName))
            return;

        var key = (control.GetType(), themeName);
        if (ThemeCache.TryGetValue(key, out var cachedTheme))
        {
            control.Theme = cachedTheme;
            return;
        }

        var themeKey = ThemeResourceKeyFactory.Theme(control.GetType().Name, themeName);
        if (MyTheme.Current.TryGetResource(themeKey, null, out var value) && value is ControlTheme theme)
        {
            ThemeCache[key] = theme;
            control.Theme = theme;
        }
    }

    /// <summary>
    /// Executes an action on all children of a panel of type <typeparamref name="T"/>.
    /// </summary>
    public static void ExecuteOnChildren<T>(Panel? panel, Action<T> action) => panel?.GetLogicalDescendants().OfType<T>().ForEach(action);

    /// <summary>
    /// Adds icon classes to children of type <typeparamref name="T"/> based on index.
    /// </summary>
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

    /// <summary>
    /// Adds classes to children of type <typeparamref name="T"/> based on index.
    /// </summary>
    public static void AddClassesOnChildren<T>(Panel panel, string[] classes, int index, Func<T, bool>? canExecute = null)
        where T : TemplatedControl
        => ExecuteOnChildren<T>(panel, x =>
        {
            if (canExecute?.Invoke(x) == false) return;

            x.Classes.RemoveAll(classes);
            x.AddClasses(classes.GetByIndex(index).OrEmpty());
        });

    private static string FormatThemeName(string? themeName) => themeName.Or("Default");

    private static string FormatLayoutName(string? layout) => layout.Or(string.Empty);
}

/// <summary>
/// Data describing a control's theme, layout, style, role, and size.
/// </summary>
internal sealed record ControlData(string? Theme = null, string? Layout = null, string[]? Styles = null, ThemeRole? Role = null, string? Size = null);

/// <summary>
/// Describes the theme section for building controls, including layouts, styles, roles, sizes, and custom controls.
/// </summary>
internal sealed class ControlThemeData(string? name = null, DefaultStyleDisplay defaultStyleDisplay = DefaultStyleDisplay.WithoutRoles)
{
    /// <summary>
    /// Gets the theme name.
    /// </summary>
    public string? Name { get; } = name;

    /// <summary>
    /// Gets how default styles are displayed.
    /// </summary>
    public DefaultStyleDisplay DefaultStyleDisplay { get; } = defaultStyleDisplay;

    /// <summary>
    /// Gets the list of layouts.
    /// </summary>
    public List<string> Layouts { get; } = [string.Empty];

    /// <summary>
    /// Gets the list of style combinations.
    /// </summary>
    public List<string[]> Styles { get; } = [];

    /// <summary>
    /// Gets the list of theme roles/colors.
    /// </summary>
    public List<ThemeRole> Colors { get; } = [];

    /// <summary>
    /// Gets the list of sizes.
    /// </summary>
    public List<string> Sizes { get; } = [];

    /// <summary>
    /// Gets the list of custom controls.
    /// </summary>
    public List<Control> CustomControls { get; } = [];

    /// <summary>
    /// Gets all combinations of a list of strings.
    /// </summary>
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

        GenerateCombinations(list, index + 1, current, result);

        current.Add(list[index]);
        GenerateCombinations(list, index + 1, current, result);
        current.RemoveAt(current.Count - 1);
    }

    /// <summary>
    /// Adds custom controls to the theme section.
    /// </summary>
    public ControlThemeData AddCustomControls(Func<Control[]> createControls) => AddCustomControls(createControls());

    /// <summary>
    /// Adds custom controls to the theme section.
    /// </summary>
    public ControlThemeData AddCustomControls(params Control[] customControls)
    {
        customControls.ForEach(x => BuildHelper.AttachTheme(x, Name));
        CustomControls.AddRange(customControls);
        return this;
    }

    /// <summary>
    /// Adds layouts to the theme section.
    /// </summary>
    public ControlThemeData AddLayouts(params string[] layouts)
    {
        Layouts.AddRange(layouts);
        return this;
    }

    /// <summary>
    /// Adds a default layout to the theme section.
    /// </summary>
    public ControlThemeData AddDefaultLayout() => AddLayouts(string.Empty);

    /// <summary>
    /// Adds styles to the theme section.
    /// </summary>
    public ControlThemeData AddStyles(params string[] styles)
    {
        Styles.AddRange(styles.Select(x => new List<string> { x }.ToArray()));
        return this;
    }

    /// <summary>
    /// Adds a default style to the theme section.
    /// </summary>
    public ControlThemeData AddDefaultStyle() => AddStyles(string.Empty);

    /// <summary>
    /// Adds cartesian style combinations to the theme section.
    /// </summary>
    public ControlThemeData AddCartesianStyles(params string[] styles)
    {
        Styles.AddRange(GetCombinations(styles).Where(x => x.Count >= 2).Select(x => x.ToArray()));
        return this;
    }

    /// <summary>
    /// Adds all default roles to the theme section.
    /// </summary>
    public ControlThemeData AddDefaultRoles(bool withPrimaryRole = true)
    {
        var colors = new List<ThemeRole> { ThemeRole.Primary, ThemeRole.Accent, ThemeRole.Inverse, ThemeRole.Success, ThemeRole.Error, ThemeRole.Warning, ThemeRole.Information };

        if (!withPrimaryRole)
            colors = [.. colors.Except([ThemeRole.Primary])];
        return AddRoles([.. colors]);
    }

    /// <summary>
    /// Adds theme roles to the theme section.
    /// </summary>
    public ControlThemeData AddThemeRoles(bool withPrimaryRole = true)
    {
        var colors = new List<ThemeRole> { ThemeRole.Primary, ThemeRole.Accent, ThemeRole.Inverse };

        if (!withPrimaryRole)
            colors = [.. colors.Except([ThemeRole.Primary])];
        return AddRoles([.. colors]);
    }

    /// <summary>
    /// Adds all roles to the theme section.
    /// </summary>
    public ControlThemeData AddAllRoles() => AddRoles([ThemeRole.Primary, ThemeRole.Accent, ThemeRole.Inverse, ThemeRole.Dark, ThemeRole.Success, ThemeRole.Error, ThemeRole.Warning, ThemeRole.Information]);

    /// <summary>
    /// Adds roles to the theme section.
    /// </summary>
    public ControlThemeData AddRoles(params ThemeRole[] roles)
    {
        Colors.AddRange(roles);
        return this;
    }

    /// <summary>
    /// Adds all font sizes to the theme section.
    /// </summary>
    public ControlThemeData AddAllSizes() => AddSizes(Enum.GetValues<FontSize>());

    /// <summary>
    /// Adds font sizes to the theme section.
    /// </summary>
    public ControlThemeData AddSizes(params FontSize[] colors) => AddSizes(colors.Select(x => x.ToString()).ToArray());

    /// <summary>
    /// Adds sizes to the theme section.
    /// </summary>
    public ControlThemeData AddSizes(params string[] sizes)
    {
        Sizes.AddRange(sizes);
        return this;
    }
}
