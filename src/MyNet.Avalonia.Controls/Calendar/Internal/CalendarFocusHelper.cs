// -----------------------------------------------------------------------

// <copyright file="CalendarFocusHelper.cs" company="Stéphane ANDRE">

// Copyright (c) Stéphane ANDRE. All rights reserved.

// </copyright>

// -----------------------------------------------------------------------



using System.Collections.Generic;

using System.Linq;

using Avalonia;

using Avalonia.Controls;

using Avalonia.Input;

using Avalonia.VisualTree;

using MyNet.Avalonia.Controls.Primitives;

using MyNet.Avalonia.Controls.Primitives.Internal;



#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace MyNet.Avalonia.Controls;

#pragma warning restore IDE0130 // Namespace does not match folder structure



internal static class CalendarFocusHelper

{

    public static bool TryHandleTextBoxTab(Calendar calendar, TextBox textBox, KeyEventArgs e)

    {

        if (e.Key != Key.Tab || !ReferenceEquals(e.Source, textBox))

            return false;



        if (e.KeyModifiers.HasFlag(KeyModifiers.Shift))

            FocusLastDay(calendar);

        else

            calendar.FocusSelectedDay();



        return true;

    }



    public static bool TryHandlePreviewerTab(Calendar calendar, TextBox? textBox, KeyEventArgs e)

    {

        if (e.Key != Key.Tab || textBox is null)

            return false;



        var focusables = GetTabFocusables(calendar);

        if (focusables.Count == 0)

            return false;



        var index = TextPickerPopupFocusHelper.GetFocusableIndex(focusables, e.Source);

        if (index < 0)

            return false;



        var shift = e.KeyModifiers.HasFlag(KeyModifiers.Shift);



        if ((!shift && index == focusables.Count - 1) || (shift && index == 0))

        {

            textBox.Focus(NavigationMethod.Tab);

            return true;

        }



        return false;

    }



    public static void FocusLastDay(Calendar calendar)

    {

        var last = GetTabFocusables(calendar).LastOrDefault();

        last?.Focus(NavigationMethod.Tab);

    }



    internal static IReadOnlyList<Control> GetTabFocusables(Calendar calendar) =>

        [.. calendar.GetVisualDescendants()

            .OfType<CalendarDayButton>()

            .Where(static c => c is { Focusable: true, IsTabStop: true, IsEffectivelyEnabled: true, IsVisible: true })

            .OrderBy(static c => KeyboardNavigation.GetTabIndex(c))

            .ThenBy(static c => c, VisualTreeOrderComparer.Instance)];



    private sealed class VisualTreeOrderComparer : IComparer<Control>

    {

        internal static VisualTreeOrderComparer Instance { get; } = new();



        public int Compare(Control? x, Control? y) => ReferenceEquals(x, y) ? 0 : x is null ? -1 : y is null ? 1 : x.IsVisualAncestorOf(y) ? -1 : y.IsVisualAncestorOf(x) ? 1 : 0;

    }

}


