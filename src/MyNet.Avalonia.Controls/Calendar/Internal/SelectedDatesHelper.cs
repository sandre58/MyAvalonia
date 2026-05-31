// -----------------------------------------------------------------------
// <copyright file="SelectedDatesHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class SelectedDatesHelper
{
    public static IEnumerable<DateTime> EnumerateDateRange(DateTime start, DateTime end)
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
}
