// -----------------------------------------------------------------------
// <copyright file="CalendarBlackoutDatesCollection.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Threading;
using MyNet.Avalonia.Controls.Internals.Calendar;
using MyNet.Primitives;
using MyNet.Primitives.Intervals;
using MyNet.Primitives.Temporal;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public sealed class CalendarBlackoutDatesCollection(Calendar owner) : ObservableCollection<Period>
{
    private readonly Calendar _owner = owner ?? throw new ArgumentNullException(nameof(owner));

    public bool Contains(DateTime date) => this.Any(x => x.Contains(date));

    public bool Contains(DateTime start, DateTime end)
    {
        var (rangeStart, rangeEnd) = start.Normalize(end);
        return this.Any(x => x.Contains(new Period(rangeStart, rangeEnd)));
    }

    public bool ContainsAny(Period range) => this.Any(r => r.Contains(range));

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
            throw new ArgumentOutOfRangeException(nameof(item));
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
            throw new ArgumentOutOfRangeException(nameof(item));
        }

        base.SetItem(index, item);
    }

    private static void EnsureValidThread() => Dispatcher.UIThread.VerifyAccess();

    private bool IsValid(Period item) => _owner.SelectedDates.All(day => !item.Contains(day));
}
