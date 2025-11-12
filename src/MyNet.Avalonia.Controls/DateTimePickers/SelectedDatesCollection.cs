// -----------------------------------------------------------------------
// <copyright file="SelectedDatesCollection.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Threading;
using MyNet.Utilities;
using MyNet.Utilities.Helpers;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public sealed class SelectedDatesCollection(Calendar owner) : ObservableCollection<DateTime>
{
    private readonly Calendar _owner = owner;

    public void AddRange(DateTime start, DateTime end)
    {
        foreach (var date in ToDates(start, end))
            Add(date);
    }

    public void RemoveRange(DateTime start, DateTime end)
    {
        foreach (var date in ToDates(start, end))
            Remove(date);
    }

    public void Set(DateTime date)
    {
        var datesToRemove = this.Except([date.DiscardTime()]).ToList();
        datesToRemove.ForEach(x => Remove(x));

        Add(date);
    }

    public void Set(DateTime start, DateTime end)
    {
        var period = start.ToPeriod(end);

        var datesToRemove = this.Where(x => !period.Contains(x)).ToList();
        datesToRemove.ForEach(x => Remove(x));

        AddRange(start, end);
    }

    protected override void ClearItems()
    {
        EnsureValidThread();

        base.ClearItems();

        // The event fires after SelectedDate changes
        if (_owner.SelectionMode != CalendarSelectionMode.None && _owner.SelectedDate != null)
            _owner.SelectedDate = null;
    }

    protected override void InsertItem(int index, DateTime item)
    {
        EnsureValidThread();

        var date = item.DiscardTime();
        if (!Contains(date) && IsValid(date))
        {
            base.InsertItem(index, date);

            // The event fires after SelectedDate changes
            if (index == 0 && !(_owner.SelectedDate.HasValue && DateTime.Compare(_owner.SelectedDate.Value, date) == 0))
                _owner.SelectedDate = date;
        }
    }

    protected override void RemoveItem(int index)
    {
        EnsureValidThread();

        base.RemoveItem(index);

        // The event fires after SelectedDate changes
        if (index == 0)
            _owner.SelectedDate = Count > 0 ? this[0] : null;
    }

    protected override void SetItem(int index, DateTime item)
    {
        EnsureValidThread();

        if (!Contains(item) && IsValid(item))
        {
            base.SetItem(index, item);

            // The event fires after SelectedDate changes
            if (index == 0 && !(_owner.SelectedDate.HasValue && DateTime.Compare(_owner.SelectedDate.Value, item) == 0))
                _owner.SelectedDate = item;
        }
    }

    internal void ClearInternal()
    {
        EnsureValidThread();

        base.ClearItems();
    }

    private static IEnumerable<DateTime> ToDates(DateTime start, DateTime end)
    {
        if (start < end)
        {
            for (var date = start; date <= end; date = date.AddDays(1))
                yield return date;
        }
        else
        {
            for (var date = start; date >= end; date = date.AddDays(-1))
                yield return date;
        }
    }

    private static void EnsureValidThread() => Dispatcher.UIThread.VerifyAccess();

    private bool IsValid(DateTime date)
        => _owner.SelectionMode != CalendarSelectionMode.None
        && (_owner.SelectionMode != CalendarSelectionMode.SingleDate || Count <= 0)
        && ((_owner.SelectionMode != CalendarSelectionMode.SingleRange || this.Concat([date]).IsConsecutiveDays())
        && _owner.BlackoutDates.All(x => !x.Contains(date)));
}
