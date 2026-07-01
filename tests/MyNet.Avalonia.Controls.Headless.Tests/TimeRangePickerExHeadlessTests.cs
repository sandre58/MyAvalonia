// -----------------------------------------------------------------------
// <copyright file="TimeRangePickerExHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Styling;
using Avalonia.VisualTree;
using Avalonia.Threading;
using FluentAssertions;
using MyNet.Avalonia.Controls.Internals;
using MyNet.Primitives.Temporal;

namespace MyNet.Avalonia.Controls.Headless.Tests;

public class TimeRangePickerExHeadlessTests
{
    private sealed class TestableTimeRangePickerEx : TimeRangePickerEx
    {
        public TimeRangeView? RangePreviewer => Previewer;
    }

    [AvaloniaFact]
    public void OpenPopup_ProvidesPreviewerWithBoundarySelector()
    {
        var picker = CreatePicker();
        picker.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var previewer = GetPreviewer(picker);
        var selector = HeadlessControlHost.FindByName<TabControl>(previewer, TimeRangeView.PartBoundarySelector);
        selector.Should().NotBeNull();
        selector!.ItemCount.Should().Be(2);
        selector.SelectedIndex.Should().Be(0);
        previewer.ActiveBoundary.Should().Be(TimeRangeBoundary.Start);
    }

    [AvaloniaFact]
    public void OpenPopup_FocusesStartHourComponent()
    {
        var picker = CreatePicker();
        picker.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var previewer = GetPreviewer(picker);
        var timeView = HeadlessControlHost.FindByName<TimeView>(previewer, TimeRangeView.PartStartTimeView);
        timeView.Should().NotBeNull();
        timeView!.SelectedComponent.Should().Be(TimeComponent.Hour);

        var hourComponent = HeadlessControlHost.FindByName<NumericUpDownTimeComponent>(timeView, "PART_Hour");
        hourComponent.Should().NotBeNull();
        hourComponent!.IsActive.Should().BeTrue();

        var hourEditor = hourComponent.GetVisualDescendants().OfType<TextBox>().FirstOrDefault();
        if (hourEditor is not null)
            hourEditor.IsFocused.Should().BeTrue();
        else
            hourComponent.IsFocused.Should().BeTrue();
    }

    [AvaloniaFact]
    public void OpenPopup_AfterStartInputCompleted_StaysOnEndBoundary()
    {
        var picker = CreatePicker();
        picker.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var previewer = GetPreviewer(picker);
        previewer.StartTime = new TimeSpan(9, 0, 0);
        previewer.SwitchToEnd(autoAdvance: true);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        previewer.ActiveBoundary.Should().Be(TimeRangeBoundary.End);

        var selector = HeadlessControlHost.FindByName<TabControl>(previewer, TimeRangeView.PartBoundarySelector);
        selector!.SelectedIndex.Should().Be(1);
    }

    [AvaloniaFact]
    public void BoundarySelector_SwitchesActiveBoundary()
    {
        var picker = CreatePicker();
        picker.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var previewer = GetPreviewer(picker);
        var selector = HeadlessControlHost.FindByName<TabControl>(previewer, TimeRangeView.PartBoundarySelector);
        selector.Should().NotBeNull();

        selector!.SelectedIndex = 1;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        previewer.ActiveBoundary.Should().Be(TimeRangeBoundary.End);
    }

    [AvaloniaFact]
    public void TabSwitch_FocusesHourOnActiveBoundary()
    {
        var picker = CreatePicker();
        picker.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var previewer = GetPreviewer(picker);
        var selector = HeadlessControlHost.FindByName<TabControl>(previewer, TimeRangeView.PartBoundarySelector);
        selector!.SelectedIndex = 1;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var endTimeView = HeadlessControlHost.FindByName<TimeView>(previewer, TimeRangeView.PartEndTimeView);
        endTimeView.Should().NotBeNull();
        endTimeView!.SelectedComponent.Should().Be(TimeComponent.Hour);

        var hourComponent = HeadlessControlHost.FindByName<NumericUpDownTimeComponent>(endTimeView, "PART_Hour");
        hourComponent.Should().NotBeNull();
        hourComponent!.IsActive.Should().BeTrue();
    }

    [AvaloniaFact]
    public void ClosePopup_WithCompletePreview_KeepsCommittedValue()
    {
        var picker = CreatePicker();
        var start = new TimeSpan(9, 0, 0);
        var end = new TimeSpan(17, 0, 0);
        picker.SelectedValue = TimeRangeHelper.BuildPeriod(start, end, System.DateTime.Today).Period;

        picker.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var previewer = GetPreviewer(picker);
        previewer.StartTime = new TimeSpan(10, 0, 0);
        previewer.EndTime = new TimeSpan(18, 0, 0);

        picker.IsDropDownOpen = false;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.StartTime.Should().Be(new TimeSpan(10, 0, 0));
        picker.EndTime.Should().Be(new TimeSpan(18, 0, 0));
    }

    [AvaloniaFact]
    public void ClosePopup_WithIncompletePreview_RestoresCommittedValue()
    {
        var picker = CreatePicker();
        var start = new TimeSpan(9, 0, 0);
        var end = new TimeSpan(17, 0, 0);
        picker.SelectedValue = TimeRangeHelper.BuildPeriod(start, end, System.DateTime.Today).Period;

        picker.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var previewer = GetPreviewer(picker);
        previewer.StartTime = new TimeSpan(10, 0, 0);
        previewer.EndTime = null;

        picker.IsDropDownOpen = false;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.StartTime.Should().Be(start);
        picker.EndTime.Should().Be(end);
    }

    private static TestableTimeRangePickerEx CreatePicker()
    {
        HeadlessTestApp.EnsureGlobalizationServices();
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        Application.Current!.TryGetResource(typeof(TimeRangePickerEx), null, out var themeObj).Should().BeTrue();
        var theme = themeObj as ControlTheme;
        theme.Should().NotBeNull();

        var picker = new TestableTimeRangePickerEx
        {
            DisplayFormat = @"hh\:mm",
            Width = 320,
            Height = 40,
            Theme = theme,
        };

        HeadlessControlHost.Show(picker, new(420, 360));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Loaded);

        return picker;
    }

    private static TimeRangeView GetPreviewer(TestableTimeRangePickerEx picker)
    {
        picker.RangePreviewer.Should().NotBeNull();
        return picker.RangePreviewer!;
    }
}
