// -----------------------------------------------------------------------
// <copyright file="SelectedDatesCollection.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Threading;
using MyNet.Avalonia.Controls.Internals;
using MyNet.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public sealed class SelectedDatesCollection(Calendar owner) : ObservableCollection<DateTime>
{
    public void AddRange(DateTime start, DateTime end)
    {
        foreach (var date in SelectedDatesHelper.EnumerateDateRange(start, end))
            Add(date);
    }

    public void RemoveRange(DateTime start, DateTime end)
    {
        foreach (var date in SelectedDatesHelper.EnumerateDateRange(start, end))
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

            // The event fires after Value changes
            if (index == 0 && !(owner.SelectedDate.HasValue && DateTime.Compare(owner.SelectedDate.Value, date) == 0))
                owner.SelectedDate = date;
        }
    }

    protected override void RemoveItem(int index)
    {
        EnsureValidThread();

        base.RemoveItem(index);

        // The event fires after Value changes
        if (index == 0)
            owner.SelectedDate = Count > 0 ? this[0] : null;
    }

    protected override void SetItem(int index, DateTime item)
    {
        EnsureValidThread();

        if (!Contains(item) && IsValid(item))
        {
            base.SetItem(index, item);

            // The event fires after Value changes
            if (index == 0 && !(owner.SelectedDate.HasValue && DateTime.Compare(owner.SelectedDate.Value, item) == 0))
                owner.SelectedDate = item;
        }
    }

    internal void ClearInternal()
    {
        EnsureValidThread();

        base.ClearItems();
    }

    private static void EnsureValidThread() => Dispatcher.UIThread.VerifyAccess();

    private bool IsValid(DateTime date)
        => owner.SelectionMode != CalendarSelectionMode.None
        && (owner.SelectionMode != CalendarSelectionMode.SingleDate || Count <= 0)
        && ((owner.SelectionMode != CalendarSelectionMode.SingleRange || this.Concat([date]).IsConsecutiveDays())
        && owner.BlackoutDates.All(x => !x.Contains(date)));
}
