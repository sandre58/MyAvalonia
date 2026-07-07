// -----------------------------------------------------------------------
// <copyright file="ToolBarHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Automation;
using Avalonia.Automation.Peers;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Headless.XUnit;
using Avalonia.Layout;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.VisualTree;
using FluentAssertions;
using Material.Icons;
using MyNet.Avalonia.Controls.Primitives;

namespace MyNet.Avalonia.Controls.Headless.Tests;

public class ToolBarHeadlessTests
{
    // ── Template application ─────────────────────────────────────────────────

    [AvaloniaFact]
    public void ApplyTemplate_CreatesItemsPresenter()
    {
        var toolbar = new ToolBar();
        HeadlessControlHost.Show(toolbar, new(400, 40));

        toolbar.Presenter.Should().NotBeNull();
    }

    [AvaloniaFact]
    public void ApplyTemplate_CreatesToolBarPanel()
    {
        var toolbar = new ToolBar();
        HeadlessControlHost.Show(toolbar, new(400, 40));

        toolbar.Presenter?.Panel.Should().BeOfType<ToolBarPanel>();
    }

    [AvaloniaFact]
    public void ApplyTemplate_FindsOverflowButton()
    {
        var toolbar = new ToolBar();
        HeadlessControlHost.Show(toolbar, new(400, 40));

        HeadlessControlHost.FindByName<ToggleButton>(toolbar, ToolBar.PartOverflowButton)
            .Should().NotBeNull();
    }

    // ── AutomationControlType ────────────────────────────────────────────────

    [AvaloniaFact]
    public void AutomationControlType_IsToolBar()
    {
        var toolbar = new ToolBar();
        HeadlessControlHost.Show(toolbar, new(400, 40));

        AutomationProperties.GetControlTypeOverride(toolbar)
            .Should().Be(AutomationControlType.ToolBar);
    }

    // ── Pseudo-classes ───────────────────────────────────────────────────────

    [AvaloniaFact]
    public void PseudoClass_Horizontal_SetByDefault()
    {
        var toolbar = new ToolBar();
        HeadlessControlHost.Show(toolbar, new(400, 40));

        toolbar.Classes.Should().Contain(":horizontal");
        toolbar.Classes.Should().NotContain(":vertical");
    }

    [AvaloniaFact]
    public void PseudoClass_Vertical_SetWhenOrientationIsVertical()
    {
        var toolbar = new ToolBar { Orientation = Orientation.Vertical };
        HeadlessControlHost.Show(toolbar, new(40, 400));

        toolbar.Classes.Should().Contain(":vertical");
        toolbar.Classes.Should().NotContain(":horizontal");
    }

    [AvaloniaFact]
    public void PseudoClass_Compact_SetWhenLayoutModeIsCompact()
    {
        var toolbar = new ToolBar { LayoutMode = ToolBarLayoutMode.Compact };
        toolbar.Items.Add(new ToolBarItem { Header = "Item" });
        HeadlessControlHost.Show(toolbar, new(400, 40));

        // Trigger a layout pass so UpdateOverflowState fires
        toolbar.UpdateLayout();

        toolbar.Classes.Should().Contain(":compact");
    }

    // ── Overflow behavior ────────────────────────────────────────────────────

    [AvaloniaFact]
    public void IsOverflowAvailable_FalseByDefault_WithEnoughSpace()
    {
        var toolbar = new ToolBar { OverflowMode = ToolBarOverflowMode.Adaptive };
        toolbar.Items.Add(new ToolBarItem { Header = "A" });
        toolbar.Items.Add(new ToolBarItem { Header = "B" });
        HeadlessControlHost.Show(toolbar, new(400, 40));

        toolbar.IsOverflowAvailable.Should().BeFalse();
        toolbar.Classes.Should().NotContain(":overflow");
    }

    [AvaloniaFact]
    public void IsOverflowAvailable_TrueWhenToolBarIsNarrow()
    {
        // Arrange: 5 items × ~80px min-width each, toolbar = 100px → overflow expected
        var toolbar = new ToolBar { OverflowMode = ToolBarOverflowMode.Adaptive, ItemSpacing = 2 };
        for (var i = 0; i < 5; i++)
            toolbar.Items.Add(new ToolBarItem { Header = $"Item {i}", Width = 70 });

        HeadlessControlHost.Show(toolbar, new(100, 40));

        toolbar.IsOverflowAvailable.Should().BeTrue();
        toolbar.Classes.Should().Contain(":overflow");
    }

    [AvaloniaFact]
    public void OverflowButton_NotVisibleWhenNoOverflow()
    {
        var toolbar = new ToolBar { OverflowMode = ToolBarOverflowMode.Adaptive };
        toolbar.Items.Add(new ToolBarItem { Header = "Single item" });
        HeadlessControlHost.Show(toolbar, new(400, 40));

        var overflowButton = HeadlessControlHost.FindByName<ToggleButton>(toolbar, ToolBar.PartOverflowButton);
        overflowButton?.IsVisible.Should().BeFalse();
    }

    [AvaloniaFact]
    public void OverflowMode_None_NeverSetsOverflowAvailable()
    {
        var toolbar = new ToolBar { OverflowMode = ToolBarOverflowMode.None };
        for (var i = 0; i < 10; i++)
            toolbar.Items.Add(new ToolBarItem { Header = $"Item {i}", Width = 80 });

        // Very narrow — but overflow mode is None so no overflow
        HeadlessControlHost.Show(toolbar, new(50, 40));

        toolbar.IsOverflowAvailable.Should().BeFalse();
    }

    [AvaloniaFact]
    public void OverflowItems_Populated_ForDirectXamlChildren()
    {
        var toolbar = new ToolBar { OverflowMode = ToolBarOverflowMode.Adaptive, ItemSpacing = 2 };
        for (var i = 0; i < 5; i++)
            toolbar.Items.Add(new ToolBarItem { Header = $"Item {i}", Width = 70 });

        HeadlessControlHost.Show(toolbar, new(100, 40));
        toolbar.UpdateLayout();
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        toolbar.IsOverflowAvailable.Should().BeTrue();
        toolbar.OverflowItems.Count.Should().BeGreaterThan(0);
        toolbar.OverflowItems.Should().AllBeOfType<ToolBarOverflowEntry>();
    }

    [AvaloniaFact]
    public void IsOverflowAvailable_False_WithItemsSource_WhenWideEnough()
    {
        var items = new List<object>
        {
            new TestToolBarDataItem { Title = "New", Icon = MaterialIconKind.FileOutline },
            new TestToolBarDataItem { Title = "Open", Icon = MaterialIconKind.FolderOpenOutline },
            new TestToolBarDataItem { Title = "Save", Icon = MaterialIconKind.ContentSaveOutline },
            new ToolBarSeparatorItem(),
            new TestToolBarDataItem { Title = "Undo", Icon = MaterialIconKind.Undo },
            new TestToolBarDataItem { Title = "Redo", Icon = MaterialIconKind.Redo },
        };

        var theme = new ControlTheme(typeof(ToolBarItem))
        {
            Setters =
            {
                new Setter(ToolBarItem.HeaderProperty, new Binding("Title")),
                new Setter(ToolBarItem.IconProperty, new Binding("Icon")),
            },
        };

        var toolbar = new ToolBar
        {
            OverflowMode = ToolBarOverflowMode.Adaptive,
            ItemsSource = items,
            ItemContainerTheme = theme,
        };

        HeadlessControlHost.Show(toolbar, new(480, 40));
        toolbar.UpdateLayout();
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        toolbar.IsOverflowAvailable.Should().BeFalse();
    }

    [AvaloniaFact]
    public void OverflowPopup_RendersItems_FromOverflowItems()
    {
        var toolbar = new ToolBar { OverflowMode = ToolBarOverflowMode.Adaptive, ItemSpacing = 2 };
        for (var i = 0; i < 5; i++)
            toolbar.Items.Add(new ToolBarItem { Header = $"Item {i}", Width = 70 });

        HeadlessControlHost.Show(toolbar, new(100, 40));
        toolbar.UpdateLayout();

        toolbar.OverflowItems.Count.Should().BeGreaterThan(0);

        var overflowButton = HeadlessControlHost.FindByName<ToggleButton>(toolbar, ToolBar.PartOverflowButton);
        overflowButton.Should().NotBeNull();
        overflowButton!.IsChecked = true;
        toolbar.UpdateLayout();

        var popup = HeadlessControlHost.FindByName<Popup>(toolbar, ToolBar.PartOverflowPopup);
        popup.Should().NotBeNull();
        popup!.IsOpen.Should().BeTrue();

        var popupItems = popup.GetVisualDescendants().OfType<Button>().ToList();
        popupItems.Count.Should().BeGreaterThan(0);
    }

    private sealed class TestToolBarDataItem
    {
        public string Title { get; init; } = string.Empty;

        public MaterialIconKind Icon { get; init; }
    }

    // ── ToolBarSeparator ─────────────────────────────────────────────────────

    [AvaloniaFact]
    public void ToolBarSeparator_IsNotFocusable()
    {
        var separator = new ToolBarSeparator();
        separator.Focusable.Should().BeFalse();
    }

    [AvaloniaFact]
    public void ToolBarSeparator_AutomationControlType_IsSeparator()
    {
        var separator = new ToolBarSeparator();
        AutomationProperties.GetControlTypeOverride(separator)
            .Should().Be(AutomationControlType.Separator);
    }

    // ── ToolBarPanel layout context ───────────────────────────────────────────

    [AvaloniaFact]
    public void ToolBarPanel_LayoutContext_SetAfterApplyTemplate()
    {
        var toolbar = new ToolBar();
        HeadlessControlHost.Show(toolbar, new(400, 40));

        var panel = toolbar.ItemsPanelRoot as ToolBarPanel;
        panel.Should().NotBeNull();
        panel!.LayoutContext.Should().NotBeNull();
    }

    [AvaloniaFact]
    public void Arrange_PlacesItemsHorizontallyWithoutOverlap()
    {
        var toolbar = new ToolBar { ItemSpacing = 2 };
        toolbar.Items.Add(new ToolBarItem { Header = "A" });
        toolbar.Items.Add(new ToolBarItem { Header = "B" });
        toolbar.Items.Add(new ToolBarItem { Header = "C" });
        HeadlessControlHost.Show(toolbar, new(400, 40));
        toolbar.UpdateLayout();

        var items = toolbar.GetVisualDescendants().OfType<ToolBarItem>().Take(3).ToList();
        items.Should().HaveCount(3);

        var bounds = items.Select(i => i.Bounds).ToList();
        bounds[0].X.Should().BeLessThan(bounds[1].X);
        bounds[1].X.Should().BeLessThan(bounds[2].X);
    }

    [AvaloniaFact]
    public void StripItems_VisibleAt480_WithStandardPreset()
    {
        var items = new List<object>
        {
            new TestToolBarDataItem { Title = "New", Icon = MaterialIconKind.FileOutline },
            new TestToolBarDataItem { Title = "Open", Icon = MaterialIconKind.FolderOpenOutline },
            new TestToolBarDataItem { Title = "Save", Icon = MaterialIconKind.ContentSaveOutline },
            new ToolBarSeparatorItem(),
            new TestToolBarDataItem { Title = "Undo", Icon = MaterialIconKind.Undo },
            new TestToolBarDataItem { Title = "Redo", Icon = MaterialIconKind.Redo },
            new TestToolBarDataItem { Title = "Cut", Icon = MaterialIconKind.ContentCut },
            new TestToolBarDataItem { Title = "Copy", Icon = MaterialIconKind.ContentCopy },
            new TestToolBarDataItem { Title = "Paste", Icon = MaterialIconKind.ContentPaste },
        };

        var theme = new ControlTheme(typeof(ToolBarItem))
        {
            Setters =
            {
                new Setter(ToolBarItem.HeaderProperty, new Binding("Title")),
                new Setter(ToolBarItem.IconProperty, new Binding("Icon")),
            },
        };

        var toolbar = new ToolBar
        {
            OverflowMode = ToolBarOverflowMode.Adaptive,
            ItemsSource = items,
            ItemContainerTheme = theme,
        };

        HeadlessControlHost.Show(toolbar, new(480, 40));
        toolbar.UpdateLayout();
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        toolbar.IsOverflowAvailable.Should().BeFalse();

        var stripItems = toolbar.GetVisualDescendants()
            .OfType<ToolBarItem>()
            .Where(i => i.Bounds.Width > 0 && i.Bounds.X >= 0)
            .ToList();

        stripItems.Count.Should().BeGreaterThanOrEqualTo(6);
    }

    [AvaloniaFact]
    public void OverflowPopup_DefaultTemplate_ButtonIsHitTestable()
    {
        var toolbar = new ToolBar { OverflowMode = ToolBarOverflowMode.Adaptive, ItemSpacing = 2 };
        for (var i = 0; i < 5; i++)
            toolbar.Items.Add(new ToolBarItem { Header = $"Item {i}", Width = 70 });

        HeadlessControlHost.Show(toolbar, new(100, 40));
        toolbar.UpdateLayout();
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var overflowButton = HeadlessControlHost.FindByName<ToggleButton>(toolbar, ToolBar.PartOverflowButton);
        overflowButton.Should().NotBeNull();
        overflowButton!.IsChecked = true;
        toolbar.UpdateLayout();
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var popup = HeadlessControlHost.FindByName<Popup>(toolbar, ToolBar.PartOverflowPopup);
        popup.Should().NotBeNull();

        var popupButtons = popup!.GetVisualDescendants()
            .OfType<Button>()
            .Where(b => b.IsVisible && b.Bounds.Width > 0 && b.Bounds.Height > 0)
            .ToList();

        popupButtons.Count.Should().BeGreaterThan(0);
    }

    [AvaloniaFact]
    public void OverflowPopup_DefaultTemplate_ShowsHeaderText()
    {
        var toolbar = new ToolBar { OverflowMode = ToolBarOverflowMode.Adaptive, ItemSpacing = 2 };
        for (var i = 0; i < 5; i++)
            toolbar.Items.Add(new ToolBarItem { Header = $"Item {i}", Width = 70 });

        HeadlessControlHost.Show(toolbar, new(100, 40));
        toolbar.UpdateLayout();
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var overflowButton = HeadlessControlHost.FindByName<ToggleButton>(toolbar, ToolBar.PartOverflowButton);
        overflowButton!.IsChecked = true;
        toolbar.UpdateLayout();
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var popup = HeadlessControlHost.FindByName<Popup>(toolbar, ToolBar.PartOverflowPopup);
        popup.Should().NotBeNull();

        var headerTexts = popup!.GetVisualDescendants()
            .OfType<Button>()
            .Select(b => b.Content?.ToString())
            .Where(text => text is not null)
            .ToList();

        headerTexts.Should().Contain(text => text!.Contains("Item", StringComparison.Ordinal));
    }
}
