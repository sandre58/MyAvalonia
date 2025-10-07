// -----------------------------------------------------------------------
// <copyright file="CalendarContext.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;

namespace MyNet.Avalonia.Controls.DateTimePickers;

public sealed class CalendarContext(int? year = null, int? month = null, int? startYear = null, int? endYear = null) : IComparable<CalendarContext>
{
    public int? Year { get; } = year;

    public int? Month { get; } = month;

    public int? StartYear { get; } = startYear;

    public int? EndYear { get; } = endYear;

    public static CalendarContext Today() => new(DateTime.Today.Year, DateTime.Today.Month);

    public CalendarContext Clone() => new(Year, Month, StartYear, EndYear);

    public CalendarContext With(int? year = null, int? month = null, int? startYear = null, int? endYear = null) => new(year ?? Year, month ?? Month, startYear ?? StartYear, endYear ?? EndYear);

    public CalendarContext NextMonth()
    {
        var year = Year;
        var month = Month;
        if (month == 12)
        {
            year++;
            month = 1;
        }
        else
        {
            month++;
        }

        month ??= 1;
        return new CalendarContext(year, month, StartYear, EndYear);
    }

    public CalendarContext PreviousMonth()
    {
        var year = Year;
        var month = Month;
        if (month == 1)
        {
            year--;
            month = 12;
        }
        else
        {
            month--;
        }

        month ??= 1;
        return new CalendarContext(year, month, StartYear, EndYear);
    }

    public CalendarContext NextYear() => new(Year + 1, Month, StartYear, EndYear);

    public CalendarContext PreviousYear() => new(Year - 1, Month, StartYear, EndYear);

    public int CompareTo(CalendarContext? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        var yearComparison = Nullable.Compare(Year, other.Year);
        return yearComparison != 0 ? yearComparison : Nullable.Compare(Month, other.Month);
    }

    public override string ToString() => $"Start: {StartYear?.ToString(CultureInfo.CurrentCulture) ?? "null"}, End: {EndYear?.ToString(CultureInfo.CurrentCulture) ?? "null"}, Year: {Year?.ToString(CultureInfo.CurrentCulture) ?? "null"}, Month: {Month?.ToString(CultureInfo.CurrentCulture) ?? "null"}";

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || (obj is CalendarContext other && Year == other.Year && Month == other.Month && StartYear == other.StartYear && EndYear == other.EndYear);

    public override int GetHashCode() => HashCode.Combine(Year, Month, StartYear, EndYear);

    public static bool operator ==(CalendarContext left, CalendarContext right) => left is null ? right is null : left.Equals(right);

    public static bool operator !=(CalendarContext left, CalendarContext right) => !(left == right);

    public static bool operator <(CalendarContext left, CalendarContext right) => left is null ? right is not null : left.CompareTo(right) < 0;

    public static bool operator <=(CalendarContext left, CalendarContext right) => left is null || left.CompareTo(right) <= 0;

    public static bool operator >(CalendarContext left, CalendarContext right) => left?.CompareTo(right) > 0;

    public static bool operator >=(CalendarContext left, CalendarContext right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
