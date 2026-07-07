// -----------------------------------------------------------------------
// <copyright file="SelectedDatesCollection.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Threading;
using MyNet.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public sealed class SelectedDatesCollection(Calendar owner) : ObservableCollection<DateTime>
{
    private int _updateDepth;

    public void AddRange(DateTime start, DateTime end)
    {
        var (rangeStart, rangeEnd) = start.DiscardTime().MinMax(end.DiscardTime());

        BeginUpdate();
        try
        {
            foreach (var date in rangeStart.Range(rangeEnd, rangeStart <= rangeEnd ? 1 : -1))
                Add(date);
        }
        finally
        {
            EndUpdate();
        }
    }

    public void RemoveRange(DateTime start, DateTime end)
    {
        var (rangeStart, rangeEnd) = start.DiscardTime().MinMax(end.DiscardTime());

        BeginUpdate();
        try
        {
            foreach (var date in rangeStart.Range(rangeEnd, rangeStart <= rangeEnd ? 1 : -1))
                Remove(date);
        }
        finally
        {
            EndUpdate();
        }
    }

    public void Set(DateTime date)
    {
        date = date.DiscardTime();
        var datesToRemove = this.Except([date]).ToList();

        BeginUpdate();
        try
        {
            foreach (var item in datesToRemove)
                Remove(item);

            if (!Contains(date))
                Add(date);
        }
        finally
        {
            EndUpdate();
        }
    }

    public void Set(DateTime start, DateTime end)
    {
        start = start.DiscardTime();
        end = end.DiscardTime();

        if (start == end)
        {
            Set(start);
            return;
        }

        var (rangeStart, rangeEnd) = start.MinMax(end);
        var period = rangeStart.ToPeriod(rangeEnd);
        var datesToRemove = this.Where(x => !period.Contains(x)).ToList();

        BeginUpdate();
        try
        {
            foreach (var item in datesToRemove)
                Remove(item);

            foreach (var date in rangeStart.Range(rangeEnd, rangeStart <= rangeEnd ? 1 : -1))
            {
                if (!Contains(date))
                    Add(date);
            }
        }
        finally
        {
            EndUpdate();
        }
    }

    protected override void ClearItems()
    {
        EnsureValidThread();

        if (_updateDepth > 0)
        {
            base.ClearItems();
            return;
        }

        base.ClearItems();

        // The event fires after Value changes
        if (owner.SelectionMode != CalendarSelectionMode.None && owner.SelectedDate != null)
            owner.SelectedDate = null;
    }

    protected override void InsertItem(int index, DateTime item)
    {
        EnsureValidThread();

        var date = item.DiscardTime();
        if (!Contains(date) && IsValid(date))
        {
            base.InsertItem(index, date);

            if (_updateDepth == 0)
                owner.SyncSelectedDateAfterInsertAt(index, date);
        }
    }

    protected override void RemoveItem(int index)
    {
        EnsureValidThread();

        base.RemoveItem(index);

        if (_updateDepth == 0)
            owner.SyncSelectedDateAfterRemovalAt(index);
    }

    protected override void SetItem(int index, DateTime item)
    {
        EnsureValidThread();

        if (!Contains(item) && IsValid(item))
        {
            base.SetItem(index, item);

            if (_updateDepth == 0 && index == 0)
                owner.SyncSelectedDateAfterInsertAt(index, item);
        }
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (_updateDepth > 0)
            return;

        base.OnCollectionChanged(e);
    }

    internal void ClearInternal()
    {
        EnsureValidThread();

        base.ClearItems();
    }

    private void BeginUpdate() => _updateDepth++;

    private void EndUpdate()
    {
        if (_updateDepth == 0)
            return;

        _updateDepth--;

        if (_updateDepth > 0)
            return;

        OnCollectionChanged(new(NotifyCollectionChangedAction.Reset));
        owner.SyncSelectedDateFromCollection();
    }

    private static void EnsureValidThread() => Dispatcher.UIThread.VerifyAccess();

    private bool IsValid(DateTime date)
        => owner.SelectionMode != CalendarSelectionMode.None
        && (owner.SelectionMode != CalendarSelectionMode.SingleDate || Count <= 0)
        && ((owner.SelectionMode != CalendarSelectionMode.SingleRange
            || _updateDepth > 0
            || this.Concat([date]).IsConsecutiveDays())
        && owner.BlackoutDates.All(x => !x.Contains(date)));
}
