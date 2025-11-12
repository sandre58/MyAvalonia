// -----------------------------------------------------------------------
// <copyright file="DateContext.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using MyNet.Utilities;
using MyNet.Utilities.Helpers;

namespace MyNet.Avalonia.Controls.DateTimePickers;

public abstract record DateContext : ISimilar<DateTime>
{
    public abstract DateContext FromDate(DateTime date);

    public abstract DateTime ToDate();

    public abstract bool IsSimilar(DateTime date);

    public abstract DateContext Next();

    public abstract DateContext FastNext();

    public abstract DateContext Previous();

    public abstract DateContext FastPrevious();
}

public record DayContext(int Day, int Month, int Year) : DateContext
{
    public override DateContext FromDate(DateTime date) => new DayContext(date.Day, date.Month, date.Year);

    public override DateTime ToDate() => new(Year, Month, Day);

    public override bool IsSimilar(DateTime date) => date.Day == Day && date.Month == Month && date.Year == Year;

    public override DateContext Next()
    {
        var day = ToDate().AddDays(1);

        return new DayContext(day.Day, day.Month, day.Year);
    }

    public override DateContext FastNext()
    {
        var day = ToDate().AddMonths(1);

        return new DayContext(day.Day, day.Month, day.Year);
    }

    public override DateContext Previous()
    {
        var day = ToDate().AddDays(-1);

        return new DayContext(day.Day, day.Month, day.Year);
    }

    public override DateContext FastPrevious()
    {
        var day = ToDate().AddMonths(-1);

        return new DayContext(day.Day, day.Month, day.Year);
    }

    public override string ToString() => Day.ToString(DateTimeHelper.GetCurrentDateTimeFormatInfo());
}

public record MonthContext(int Month, int Year) : DateContext
{
    public override DateContext FromDate(DateTime date) => new MonthContext(date.Month, date.Year);

    public override DateTime ToDate() => new(Year, Month, 1);

    public override bool IsSimilar(DateTime date) => date.Month == Month && date.Year == Year;

    public override DateContext Next()
    {
        var (nextYear, nextMonth) = Month == 12 ? (Year + 1, 1) : (Year, Month + 1);

        return new MonthContext(nextMonth, nextYear);
    }

    public override DateContext FastNext() => new MonthContext(Month, Year + 1);

    public override DateContext Previous()
    {
        var (nextYear, nextMonth) = Month == 1 ? (Year - 1, 12) : (Year, Month - 1);

        return new MonthContext(nextMonth, nextYear);
    }

    public override DateContext FastPrevious() => new MonthContext(Month, Year - 1);

    public MonthContext Add(int value)
    {
        var totalMonths = (Year * 12) + Month - 1 + value;
        var newYear = totalMonths / 12;
        var newMonth = (totalMonths % 12) + 1;
        return new MonthContext(newMonth, newYear);
    }

    public MonthContext AddYears(int value) => new(Month, Year + value);

    public MonthContext AddDecades(int value) => new(Month, Year + (value * 10));

    public MonthContext AddCenturies(int value) => new(Month, Year + (value * 100));

    public MonthContext BeginningOfYear() => new(1, Year);

    public MonthContext BeginningOfDecade() => new(1, DateTimeHelper.GetDecade(Year).Start);

    public MonthContext BeginningOfCentury() => new(1, DateTimeHelper.GetCentury(Year).Start);

    public MonthContext EndOfYear() => new(12, Year);

    public MonthContext EndOfDecade() => new(12, DateTimeHelper.GetDecade(Year).End);

    public MonthContext EndOfCentury() => new(12, DateTimeHelper.GetCentury(Year).End);

    public override string ToString() => CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[Month - 1];
}

public record YearContext(int Year) : DateContext
{
    public override DateContext FromDate(DateTime date) => new YearContext(date.Year);

    public override DateTime ToDate() => new(Year, 1, 1);

    public override bool IsSimilar(DateTime date) => date.Year == Year;

    public override DateContext Next() => new YearContext(Year + 1);

    public override DateContext FastNext() => new YearContext(Year + 10);

    public override DateContext Previous() => new YearContext(Year - 1);

    public override DateContext FastPrevious() => new YearContext(Year - 10);

    public override string ToString() => Year.ToString(CultureInfo.CurrentCulture);
}

public record DecadeContext(int StartYear) : DateContext
{
    public override DateContext FromDate(DateTime date) => new DecadeContext(date.Year);

    public int EndYear => StartYear + 10;

    public override DateTime ToDate() => new(StartYear, 1, 1);

    public override bool IsSimilar(DateTime date) => date.Year >= StartYear && date.Year <= EndYear;

    public override DateContext Next() => new DecadeContext(StartYear + 10);

    public override DateContext FastNext() => new DecadeContext(StartYear + 100);

    public override DateContext Previous() => new DecadeContext(StartYear - 10);

    public override DateContext FastPrevious() => new DecadeContext(StartYear - 100);

    public override string ToString() => StartYear + "-" + EndYear;
}

public record CenturyContext(int StartYear) : DateContext
{
    public override DateContext FromDate(DateTime date) => new CenturyContext(date.Year);

    public int EndYear => StartYear + 100;

    public override DateTime ToDate() => new(StartYear, 1, 1);

    public override bool IsSimilar(DateTime date) => date.Year >= StartYear && date.Year <= EndYear;

    public override DateContext Next() => new CenturyContext(StartYear + 100);

    public override DateContext FastNext() => new CenturyContext(StartYear + 1000);

    public override DateContext Previous() => new CenturyContext(StartYear - 100);

    public override DateContext FastPrevious() => new CenturyContext(StartYear - 1000);

    public override string ToString() => StartYear + "-" + EndYear;
}
