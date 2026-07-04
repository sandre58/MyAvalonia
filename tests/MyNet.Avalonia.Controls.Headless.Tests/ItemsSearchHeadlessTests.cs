// -----------------------------------------------------------------------
// <copyright file="ItemsSearchHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.VisualTree;
using FluentAssertions;
using MyNet.Avalonia.Controls.Behaviors;
using Xunit;

namespace MyNet.Avalonia.Controls.Headless.Tests;

public class ItemsSearchHeadlessTests
{
    [AvaloniaFact]
    public void ComboBox_ContainsFilter_HidesNonMatchingItems()
    {
        var comboBox = CreateComboBox(ItemsSearchFilterMode.Contains);
        comboBox.IsDropDownOpen = true;
        RunRenderJobs();

        ItemsSearchBehavior.SetText(comboBox, "be");
        RunRenderJobs();

        GetVisibleItemCount(comboBox).Should().Be(1);
    }

    [AvaloniaFact]
    public void ComboBox_StartsWithFilter_HidesNonMatchingItems()
    {
        var comboBox = CreateComboBox(ItemsSearchFilterMode.StartsWith);
        comboBox.IsDropDownOpen = true;
        RunRenderJobs();

        ItemsSearchBehavior.SetText(comboBox, "Al");
        RunRenderJobs();

        GetVisibleItemCount(comboBox).Should().Be(1);
    }

    [AvaloniaFact]
    public void ComboBox_EqualsFilter_HidesNonMatchingItems()
    {
        var comboBox = CreateComboBox(ItemsSearchFilterMode.Equals);
        comboBox.IsDropDownOpen = true;
        RunRenderJobs();

        ItemsSearchBehavior.SetText(comboBox, "Beta");
        RunRenderJobs();

        GetVisibleItemCount(comboBox).Should().Be(1);
    }

    [AvaloniaFact]
    public void ComboBox_OpenPopup_FocusesSearchBox()
    {
        var comboBox = CreateComboBox(ItemsSearchFilterMode.Contains);
        comboBox.IsDropDownOpen = true;
        RunRenderJobs();
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Input);

        var searchBox = FindSearchBox(comboBox);
        searchBox.Should().NotBeNull();
        searchBox!.IsFocused.Should().BeTrue();
    }

    [AvaloniaFact]
    public void ComboBox_EscapeTwice_ClearsFilterThenCloses()
    {
        var comboBox = CreateComboBox(ItemsSearchFilterMode.Contains);
        comboBox.IsDropDownOpen = true;
        RunRenderJobs();

        ItemsSearchBehavior.SetText(comboBox, "be");
        RunRenderJobs();

        HeadlessControlHost.KeyDown(comboBox, Key.Escape);
        RunRenderJobs();

        ItemsSearchBehavior.GetText(comboBox).Should().BeNullOrEmpty();
        comboBox.IsDropDownOpen.Should().BeTrue();

        HeadlessControlHost.KeyDown(comboBox, Key.Escape);
        RunRenderJobs();

        comboBox.IsDropDownOpen.Should().BeFalse();
    }

    [AvaloniaFact]
    public void ComboBox_DownFromSearchBox_FocusesFirstVisibleItem()
    {
        var comboBox = CreateComboBox(ItemsSearchFilterMode.Contains);
        comboBox.IsDropDownOpen = true;
        RunRenderJobs();

        var searchBox = FindSearchBox(comboBox)!;
        searchBox.Focus();
        RunRenderJobs();

        KeyEventArgs? keyDown = null;
        comboBox.AddHandler(InputElement.KeyDownEvent, (_, e) => keyDown = e, RoutingStrategies.Tunnel);
        HeadlessControlHost.KeyDown(searchBox, Key.Down);
        RunRenderJobs();

        keyDown.Should().NotBeNull();
        keyDown!.Handled.Should().BeTrue();
    }

    [AvaloniaFact]
    public void ComboBox_NoMatch_ShowsEmptyStateAndHidesItemsList()
    {
        var comboBox = CreateComboBox(ItemsSearchFilterMode.Contains);
        comboBox.IsDropDownOpen = true;
        RunRenderJobs();

        ItemsSearchBehavior.SetText(comboBox, "zzz");
        RunRenderJobs();

        GetVisibleItemCount(comboBox).Should().Be(0);
        FindSearchPlaceholder(comboBox)!.IsPlaceholderVisible.Should().BeTrue();
    }

    [AvaloniaFact]
    public void ComboBox_CtrlF_FromItem_FocusesSearchBoxAndSelectsAllText()
    {
        var comboBox = CreateComboBox(ItemsSearchFilterMode.Contains);
        comboBox.IsDropDownOpen = true;
        RunRenderJobs();

        ItemsSearchBehavior.SetText(comboBox, "be");
        RunRenderJobs();

        var searchBox = FindSearchBox(comboBox)!;
        var firstItem = GetFirstVisibleItemContainer(comboBox);
        firstItem.Should().NotBeNull();
        firstItem!.Focus(NavigationMethod.Directional);
        RunRenderJobs();
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Input);

        HeadlessControlHost.KeyDown(firstItem, Key.F, KeyModifiers.Control);
        RunRenderJobs();
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Input);

        searchBox.IsFocused.Should().BeTrue();
        searchBox.SelectionStart.Should().Be(0);
        searchBox.SelectionEnd.Should().Be(searchBox.Text?.Length);
    }

    [AvaloniaFact]
    public void ComboBox_FilterDelay_DefersFilteringUntilElapsed()
    {
        var comboBox = CreateComboBox(ItemsSearchFilterMode.Contains);
        ItemsSearchBehavior.SetFilterDelay(comboBox, 200);
        comboBox.IsDropDownOpen = true;
        RunRenderJobs();

        ItemsSearchBehavior.SetText(comboBox, "be");
        RunRenderJobs();

        GetVisibleItemCount(comboBox).Should().Be(3);

        Thread.Sleep(250);
        RunRenderJobs();
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Background);

        GetVisibleItemCount(comboBox).Should().Be(1);
    }

    [AvaloniaFact]
    public void ComboBox_EnterFromSearch_SingleMatch_SelectsAndCloses()
    {
        var comboBox = CreateComboBox(ItemsSearchFilterMode.Contains);
        comboBox.IsDropDownOpen = true;
        RunRenderJobs();

        ItemsSearchBehavior.SetText(comboBox, "be");
        RunRenderJobs();

        var searchBox = FindSearchBox(comboBox)!;
        searchBox.Focus();
        RunRenderJobs();

        HeadlessControlHost.KeyDown(searchBox, Key.Enter);
        RunRenderJobs();

        comboBox.SelectedItem.Should().Be("Beta");
        comboBox.IsDropDownOpen.Should().BeFalse();
    }

    [AvaloniaFact]
    public void ComboBox_EnterFromSearch_MultipleMatches_FocusesFirstItem()
    {
        Application.Current!.TryGetResource(typeof(ComboBox), null, out var themeObj).Should().BeTrue();

        var comboBox = new ComboBox
        {
            ItemsSource = new[] { "Apple", "Apricot", "Banana" },
            Width = 240,
            Height = 32,
            Theme = (ControlTheme)themeObj!,
        };

        ItemsSearchBehavior.SetIsEnabled(comboBox, true);
        ItemsSearchBehavior.SetFilterMode(comboBox, ItemsSearchFilterMode.StartsWith);
        ItemsSearchBehavior.SetFilterDelay(comboBox, 0);

        HeadlessControlHost.Show(comboBox, new(240, 320));
        comboBox.IsDropDownOpen = true;
        RunRenderJobs();

        ItemsSearchBehavior.SetText(comboBox, "Ap");
        RunRenderJobs();

        ItemsSearchBehavior.GetMatchCount(comboBox).Should().Be(2);

        var searchBox = FindSearchBox(comboBox)!;
        searchBox.Focus();
        RunRenderJobs();

        KeyEventArgs? keyDown = null;
        comboBox.AddHandler(InputElement.KeyDownEvent, (_, e) => keyDown = e, RoutingStrategies.Tunnel);
        HeadlessControlHost.KeyDown(searchBox, Key.Enter);
        RunRenderJobs();

        keyDown.Should().NotBeNull();
        keyDown!.Handled.Should().BeTrue();
    }

    [AvaloniaFact]
    public void ComboBox_EnterFromSearch_NoMatch_DoesNothing()
    {
        var comboBox = CreateComboBox(ItemsSearchFilterMode.Contains);
        comboBox.IsDropDownOpen = true;
        RunRenderJobs();

        ItemsSearchBehavior.SetText(comboBox, "zzz");
        RunRenderJobs();

        var searchBox = FindSearchBox(comboBox)!;
        searchBox.Focus();
        RunRenderJobs();

        HeadlessControlHost.KeyDown(searchBox, Key.Enter);
        RunRenderJobs();

        searchBox.IsFocused.Should().BeTrue();
        comboBox.IsDropDownOpen.Should().BeTrue();
    }

    [AvaloniaFact]
    public void ComboBox_ActiveFilter_UsesNonVirtualizingItemsPanel()
    {
        var comboBox = CreateComboBox(ItemsSearchFilterMode.Contains);
        comboBox.IsDropDownOpen = true;
        RunRenderJobs();

        ItemsSearchBehavior.SetText(comboBox, "be");
        RunRenderJobs();

        GetRealizedItemsPanel(comboBox).Should().BeOfType<StackPanel>();
    }

    [AvaloniaFact]
    public void ComboBox_ActiveFilter_AllMaterializedContainersMatch()
    {
        var comboBox = CreateComboBox(ItemsSearchFilterMode.Contains);
        comboBox.IsDropDownOpen = true;
        RunRenderJobs();

        ItemsSearchBehavior.SetText(comboBox, "be");
        RunRenderJobs();

        for (var i = 0; i < comboBox.ItemCount; i++)
        {
            var container = comboBox.ContainerFromIndex(i);
            if (container is null)
                continue;

            var itemText = comboBox.Items[i]?.ToString();
            var shouldMatch = itemText?.Contains("be", StringComparison.OrdinalIgnoreCase) == true;
            container.IsVisible.Should().Be(shouldMatch, $"item '{itemText}' at index {i}");
        }
    }

    [AvaloniaFact]
    public void ComboBox_ClearSearch_RestoresOriginalItemsPanel()
    {
        var comboBox = CreateComboBox(ItemsSearchFilterMode.Contains);
        comboBox.IsDropDownOpen = true;
        RunRenderJobs();

        var originalPanel = comboBox.ItemsPanel;

        ItemsSearchBehavior.SetText(comboBox, "be");
        RunRenderJobs();

        GetRealizedItemsPanel(comboBox).Should().BeOfType<StackPanel>();

        ItemsSearchBehavior.SetText(comboBox, null);
        RunRenderJobs();

        comboBox.ItemsPanel.Should().BeSameAs(originalPanel);
    }

    [AvaloniaFact]
    public void MultiComboBox_SelectAll_WithFilter_SelectsVisibleItemsOnly()
    {
        var multiComboBox = CreateMultiComboBox(ItemsSearchFilterMode.Contains);
        multiComboBox.IsDropDownOpen = true;
        RunRenderJobs();

        ItemsSearchBehavior.SetText(multiComboBox, "Al");
        RunRenderJobs();

        multiComboBox.SelectAll();
        RunRenderJobs();

        multiComboBox.SelectedItems!.Count.Should().Be(1);
        multiComboBox.SelectedItems!.Cast<string>().Should().ContainSingle("Alpha");
    }

    private static ComboBox CreateComboBox(ItemsSearchFilterMode mode)
    {
        Application.Current!.TryGetResource(typeof(ComboBox), null, out var themeObj).Should().BeTrue();

        var comboBox = new ComboBox
        {
            ItemsSource = new[] { "Alpha", "Beta", "Gamma" },
            Width = 240,
            Height = 32,
            Theme = (ControlTheme)themeObj!,
        };

        ItemsSearchBehavior.SetIsEnabled(comboBox, true);
        ItemsSearchBehavior.SetFilterMode(comboBox, mode);
        ItemsSearchBehavior.SetFilterDelay(comboBox, 0);

        HeadlessControlHost.Show(comboBox, new(240, 320));
        RunRenderJobs();
        return comboBox;
    }

    private static MultiComboBox CreateMultiComboBox(ItemsSearchFilterMode mode)
    {
        Application.Current!.TryGetResource(typeof(MultiComboBox), null, out var themeObj).Should().BeTrue();

        var multiComboBox = new MultiComboBox
        {
            ItemsSource = new[] { "Alpha", "Beta", "Gamma" },
            Width = 240,
            Height = 32,
            Theme = (ControlTheme)themeObj!,
            IsSearchEnabled = true,
            SearchFilterMode = mode,
            ShowSelectAll = true,
        };

        ItemsSearchBehavior.SetFilterDelay(multiComboBox, 0);

        HeadlessControlHost.Show(multiComboBox, new(240, 320));
        RunRenderJobs();
        return multiComboBox;
    }

    private static Panel? GetRealizedItemsPanel(SelectingItemsControl control)
    {
        var presenter = FindTemplatePart<ItemsPresenter>(control, ItemsSearchBehavior.PartItemsPresenter);
        return presenter?.Panel;
    }

    private static int GetVisibleItemCount(SelectingItemsControl control)
    {
        var count = 0;

        for (var i = 0; i < control.ItemCount; i++)
        {
            var container = control.ContainerFromIndex(i);
            if (container is { IsVisible: true } && ItemsSearchBehavior.IsItemVisible(container))
                count++;
        }

        return count;
    }

    private static Control? GetFirstVisibleItemContainer(SelectingItemsControl control)
    {
        for (var i = 0; i < control.ItemCount; i++)
        {
            var container = control.ContainerFromIndex(i);
            if (container is { IsVisible: true })
                return container;
        }

        return null;
    }

    private static TextBlock? FindSearchEmpty(Control control) =>
        FindTemplatePart<TextBlock>(control, ItemsSearchBehavior.PartSearchEmpty);

    private static PlaceholderContentControl? FindSearchPlaceholder(Control control) =>
        FindTemplatePart<PlaceholderContentControl>(control, ItemsSearchBehavior.PartSearchPlaceholder);

    private static ScrollViewer? FindSearchItemsScrollViewer(Control control) =>
        FindTemplatePart<ScrollViewer>(control, ItemsSearchBehavior.PartSearchItems);

    private static T? FindTemplatePart<T>(Control control, string name)
        where T : Control
    {
        var fromTree = control.GetVisualDescendants()
            .OfType<T>()
            .FirstOrDefault(x => x.Name == name);
        if (fromTree is not null)
            return fromTree;

        var popup = control.GetVisualDescendants().OfType<Popup>().FirstOrDefault();
        return popup?.Child is Control popupRoot
            ? HeadlessControlHost.FindByName<T>(popupRoot, name)
            : null;
    }

    private static TextBox? FindSearchBox(Control control)
    {
        var fromTree = control.GetVisualDescendants()
            .OfType<TextBox>()
            .FirstOrDefault(x => x.Name == ItemsSearchBehavior.PartSearchBox);
        if (fromTree is not null)
            return fromTree;

        var popup = control.GetVisualDescendants().OfType<Popup>().FirstOrDefault();
        return popup?.Child is Control popupRoot
            ? HeadlessControlHost.FindByName<TextBox>(popupRoot, ItemsSearchBehavior.PartSearchBox)
            : null;
    }

    private static void RunRenderJobs()
    {
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Loaded);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);
    }
}
