// -----------------------------------------------------------------------
// <copyright file="CalendarBlackoutDatesCollection.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Threading;
using MyNet.Utilities;
using MyNet.Utilities.DateTimes;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public sealed class CalendarBlackoutDatesCollection(Calendar owner) : ObservableCollection<Period>
{
    private readonly Calendar _owner = owner ?? throw new ArgumentNullException(nameof(owner));

    public bool Contains(DateTime date) => this.Any(x => x.Contains(date));

    public bool Contains(DateTime start, DateTime end)
    {
        DateTime rangeStart;
        DateTime rangeEnd;

        if (start.IsBefore(end))
        {
            rangeStart = start.DiscardTime();
            rangeEnd = end.DiscardTime();
        }
        else
        {
            rangeStart = end.DiscardTime();
            rangeEnd = start.DiscardTime();
        }

        return this.Any(x => x.Contains(new Period(rangeStart, rangeEnd)));
    }

    public bool ContainsAny(Period range) => this.Any(r => RangeContainsAny(r, range));

    protected override void ClearItems()
    {
        EnsureValidThread();

        base.ClearItems();
    }

    protected override void InsertItem(int index, Period item)
    {
        EnsureValidThread();

        if (!IsValid(item))
        {
            throw new ArgumentOutOfRangeException(nameof(item), "Value is not valid.");
        }

        base.InsertItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
        EnsureValidThread();

        base.RemoveItem(index);
    }

    protected override void SetItem(int index, Period item)
    {
        EnsureValidThread();

        if (!IsValid(item))
        {
            throw new ArgumentOutOfRangeException(nameof(item), "Value is not valid.");
        }

        base.SetItem(index, item);
    }

    private static void EnsureValidThread() => Dispatcher.UIThread.VerifyAccess();

    private static bool RangeContainsAny(Period source, Period range)
    {
        _ = range ?? throw new ArgumentNullException(nameof(range));

        var start = DateTime.Compare(source.Start, range.Start);

        // Check if any part of the supplied range is contained by this
        // range or if the supplied range completely covers this range.
        return (start <= 0 && DateTime.Compare(source.End, range.Start) >= 0) ||
            (start >= 0 && DateTime.Compare(source.Start, range.End) <= 0);
    }

    private bool IsValid(Period item)
    {
        foreach (var day in _owner.SelectedDates)
        {
            if (day.InRange(item.Start, item.End))
            {
                return false;
            }
        }

        return true;
    }
}
