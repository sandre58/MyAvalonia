// -----------------------------------------------------------------------
// <copyright file="DateTimePickerPanelAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reflection;
using Avalonia.Controls.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Helper to set the internal <c>FormatDate</c> context on Avalonia's
/// <see cref="DateTimePickerPanel"/> (required for day/month formatting).
/// </summary>
internal static class DateTimePickerPanelAssist
{
    private static readonly PropertyInfo? FormatDateProperty =
        typeof(DateTimePickerPanel).GetProperty("FormatDate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

    public static void SetFormatDate(DateTimePickerPanel panel, DateTime date) => FormatDateProperty?.SetValue(panel, date.Date);
}
