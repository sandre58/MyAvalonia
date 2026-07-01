// -----------------------------------------------------------------------
// <copyright file="TimeRangePickerExTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Reflection;
using Avalonia.Controls;
using FluentAssertions;
using MyNet.Avalonia.Controls.Internals;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Primitives;
using MyNet.Primitives.Intervals;
using MyNet.Primitives.Temporal;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.DateTimePickers;

public class TimeRangePickerExTests
{
    [Fact]
    public void SelectedValue_SyncsStartTimeAndEndTime()
    {
        var picker = CreatePicker();
        var start = new TimeSpan(9, 0, 0);
        var end = new TimeSpan(17, 30, 0);

        picker.SelectedValue = TimeRangeHelper.BuildPeriod(start, end, DateTime.Today).Period;

        picker.StartTime.Should().Be(start);
        picker.EndTime.Should().Be(end);
    }

    [Fact]
    public void StartTimeAndEndTime_UpdateSelectedValue()
    {
        var picker = CreatePicker();

        picker.StartTime = new TimeSpan(8, 0, 0);
        picker.EndTime = new TimeSpan(12, 0, 0);

        picker.SelectedValue.Should().NotBeNull();
        picker.StartTime.Should().Be(new TimeSpan(8, 0, 0));
        picker.EndTime.Should().Be(new TimeSpan(12, 0, 0));
    }

    [Fact]
    public void EndTimeAfterStartTime_WhenEndEditedLast_CoercesStartToEnd()
    {
        var picker = CreatePicker();

        picker.StartTime = new TimeSpan(18, 0, 0);
        picker.EndTime = new TimeSpan(10, 0, 0);

        picker.StartTime.Should().Be(new TimeSpan(10, 0, 0));
        picker.EndTime.Should().Be(new TimeSpan(10, 0, 0));
    }

    [Fact]
    public void StartTimeAfterEndTime_WhenStartEditedLast_CoercesEndToStart()
    {
        var picker = CreatePicker();
        picker.StartTime = new TimeSpan(9, 0, 0);
        picker.EndTime = new TimeSpan(17, 0, 0);

        picker.StartTime = new TimeSpan(19, 0, 0);

        picker.StartTime.Should().Be(new TimeSpan(19, 0, 0));
        picker.EndTime.Should().Be(new TimeSpan(19, 0, 0));
    }

    [Fact]
    public void ConvertValueFromString_InvalidSameDayRange_CoercesEndToStart()
    {
        var picker = CreatePicker();
        picker.DisplayFormat = @"HH\:mm";
        picker.RangeSeparator = " – ";

        var period = InvokeConvertValueFromString(picker, "19:00 – 18:55");
        picker.SelectedValue = period;

        picker.StartTime.Should().Be(new TimeSpan(19, 0, 0));
        picker.EndTime.Should().Be(new TimeSpan(19, 0, 0));
    }

    [Fact]
    public void SelectedValue_UpdatesTextWithRangeSeparator()
    {
        var picker = CreatePicker();
        picker.DisplayFormat = @"HH\:mm";
        picker.RangeSeparator = " -> ";
        picker.SelectedValue = TimeRangeHelper.BuildPeriod(new(9, 0, 0), new(17, 0, 0), DateTime.Today).Period;

        picker.Text.Should().Be("09:00 -> 17:00");
    }

    [Fact]
    public void AllowOvernight_PreservesOvernightRange()
    {
        var picker = CreatePicker();
        picker.AllowOvernight = true;
        picker.SelectedValue = TimeRangeHelper.BuildPeriod(
            new(22, 0, 0),
            new(2, 0, 0),
            DateTime.Today,
            allowOvernight: true).Period;

        picker.StartTime.Should().Be(new TimeSpan(22, 0, 0));
        picker.EndTime.Should().Be(new TimeSpan(2, 0, 0));
        TimeRangeHelper.SpansOvernight(picker.SelectedValue!).Should().BeTrue();
    }

    [Fact]
    public void ShowOvernightIndicator_AppendsSuffix()
    {
        var picker = CreatePicker();
        picker.AllowOvernight = true;
        picker.ShowOvernightIndicator = true;
        picker.DisplayFormat = @"hh\:mm";
        picker.SelectedValue = TimeRangeHelper.BuildPeriod(
            new(22, 0, 0),
            new(2, 0, 0),
            DateTime.Today,
            allowOvernight: true).Period;

        picker.Text.Should().Contain("(+1)");
    }

    [Fact]
    public void IncrementValue_ShiftsEntirePeriod()
    {
        var picker = CreatePicker();
        var start = new TimeSpan(9, 0, 0);
        var end = new TimeSpan(10, 0, 0);
        picker.SelectedValue = TimeRangeHelper.BuildPeriod(start, end, DateTime.Today).Period;

        picker.Increment(30);

        picker.StartTime.Should().Be(start.Add(TimeSpan.FromMinutes(30)));
        picker.EndTime.Should().Be(end.Add(TimeSpan.FromMinutes(30)));
    }

    [Fact]
    public void AutoCommit_DefaultIsTrue()
    {
        CreatePicker().AutoCommit.Should().BeTrue();
    }

    [Fact]
    public void AutoCommit_WhenCompleteRangeInPreviewer_UpdatesSelectedValue()
    {
        var picker = CreatePickerWithPreviewer();
        var start = new TimeSpan(9, 0, 0);
        var end = new TimeSpan(17, 0, 0);
        picker.SelectedValue = TimeRangeHelper.BuildPeriod(start, end, DateTime.Today).Period;
        picker.IsDropDownOpen = true;

        var previewer = picker.TestPreviewer!;
        previewer.StartTime = new TimeSpan(10, 0, 0);
        previewer.EndTime = new TimeSpan(18, 0, 0);

        picker.StartTime.Should().Be(new TimeSpan(10, 0, 0));
        picker.EndTime.Should().Be(new TimeSpan(18, 0, 0));
    }

    [Fact]
    public void ShouldRollbackOnClose_ReturnsFalse_WhenPreviewComplete()
    {
        var picker = CreatePickerWithPreviewer();
        picker.TestPreviewer!.StartTime = new TimeSpan(9, 0, 0);
        picker.TestPreviewer.EndTime = new TimeSpan(17, 0, 0);

        picker.TestShouldRollbackOnClose().Should().BeFalse();
    }

    [Fact]
    public void ShouldRollbackOnClose_ReturnsTrue_WhenOnlyStartSet()
    {
        var picker = CreatePickerWithPreviewer();
        picker.TestPreviewer!.StartTime = new TimeSpan(9, 0, 0);

        picker.TestShouldRollbackOnClose().Should().BeTrue();
    }

    [Fact]
    public void TryUpdateSelectedValue_KeepsActiveBoundaryOnEnd()
    {
        var view = new TimeRangeView();
        view.StartTime = new TimeSpan(9, 0, 0);
        view.EndTime = new TimeSpan(17, 0, 0);
        view.SwitchBoundary(TimeRangeBoundary.End);

        view.EndTime = new TimeSpan(18, 0, 0);

        view.ActiveBoundary.Should().Be(TimeRangeBoundary.End);
    }

    [Fact]
    public void SwitchToEnd_SeedsEndTimeAndKeepsEndBoundary()
    {
        var view = new TimeRangeView();
        view.StartTime = new TimeSpan(9, 0, 0);

        view.SwitchToEnd(autoAdvance: true);

        view.ActiveBoundary.Should().Be(TimeRangeBoundary.End);
        view.EndTime.Should().Be(new TimeSpan(10, 0, 0));
    }

    [Fact]
    public void SwitchToEnd_SyncsSelectedValueWhenBothBoundariesValid()
    {
        var view = new TimeRangeView();
        view.StartTime = new TimeSpan(9, 0, 0);

        view.SwitchToEnd(autoAdvance: true);

        view.ActiveBoundary.Should().Be(TimeRangeBoundary.End);
        view.EndTime.Should().Be(new TimeSpan(10, 0, 0));
        view.SelectedValue.Should().NotBeNull();
        TimeRangeHelper.GetPeriodStartTime(view.SelectedValue!).Should().Be(new(9, 0, 0));
        TimeRangeHelper.GetPeriodEndTime(view.SelectedValue!).Should().Be(new(10, 0, 0));
    }

    [Fact]
    public void InputCompleted_OnEndWithoutStart_ReturnsToStartBoundary()
    {
        var view = new TimeRangeView();
        view.SwitchBoundary(TimeRangeBoundary.End);
        view.EndTime = new TimeSpan(17, 0, 0);

        view.GetType().GetMethod("OnTimeViewInputCompleted", BindingFlags.NonPublic | BindingFlags.Instance)!
            .Invoke(view, [null, new TimeInputCompletedEventArgs(TimeSelectorBase.InputCompletedEvent) { Mode = TimeInputCompletionMode.FieldAdvance }]);

        view.ActiveBoundary.Should().Be(TimeRangeBoundary.Start);
    }

    [Fact]
    public void CoerceDuringFlyout_WhenEndEdited_AdjustsStartToEnd()
    {
        var view = new TimeRangeView();
        view.StartTime = new TimeSpan(15, 32, 0);
        view.EndTime = new TimeSpan(18, 55, 0);
        view.SwitchBoundary(TimeRangeBoundary.End);

        view.EndTime = new TimeSpan(14, 12, 0);

        view.ActiveBoundary.Should().Be(TimeRangeBoundary.End);
        view.StartTime.Should().Be(new TimeSpan(14, 12, 0));
        view.EndTime.Should().Be(new TimeSpan(14, 12, 0));
    }

    [Fact]
    public void CoerceDuringFlyout_WhenStartEdited_AdjustsEndToStart()
    {
        var view = new TimeRangeView();
        view.StartTime = new TimeSpan(15, 32, 0);
        view.EndTime = new TimeSpan(18, 55, 0);
        view.SwitchBoundary(TimeRangeBoundary.Start);

        view.StartTime = new TimeSpan(19, 0, 0);

        view.StartTime.Should().Be(new TimeSpan(19, 0, 0));
        view.EndTime.Should().Be(new TimeSpan(19, 0, 0));
    }

    private static Period InvokeConvertValueFromString(TimeRangePickerEx picker, string text) =>
        (Period)typeof(TimeRangePickerEx).GetMethod(
            "ConvertValueFromString",
            BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(picker, [text])!;

    private static TimeRangePickerEx CreatePicker()
    {
        var picker = new TimeRangePickerEx { DisplayFormat = @"hh\:mm" };
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        return picker;
    }

    private static TestableTimeRangePickerEx CreatePickerWithPreviewer()
    {
        var picker = new TestableTimeRangePickerEx { DisplayFormat = @"hh\:mm" };
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        picker.AttachPreviewer(new());
        return picker;
    }

    private sealed class TestableTimeRangePickerEx : TimeRangePickerEx
    {
        public TimeRangeView? TestPreviewer { get; private set; }

        public void AttachPreviewer(TimeRangeView view)
        {
            TestPreviewer = view;
            typeof(TextPicker<Period?, TimeRangeView>)
                .GetProperty("Previewer", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(this, view);

            // Direct subscription: OnLoading defers until Loaded, which does not fire without a visual tree.
            view.SelectedValueChanged += (_, _) => OnPreviewValueChanged();
        }

        public bool TestShouldRollbackOnClose() => ShouldRollbackOnClose();
    }
}
