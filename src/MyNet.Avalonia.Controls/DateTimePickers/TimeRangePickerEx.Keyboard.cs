// -----------------------------------------------------------------------

// <copyright file="TimeRangePickerEx.Keyboard.cs" company="Stéphane ANDRE">

// Copyright (c) Stéphane ANDRE. All rights reserved.

// </copyright>

// -----------------------------------------------------------------------



using Avalonia.Controls;

using Avalonia.Input;



#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace MyNet.Avalonia.Controls;

#pragma warning restore IDE0130 // Namespace does not match folder structure



public partial class TimeRangePickerEx

{

    protected override bool ProcessKey(KeyEventArgs e)

    {

        if (IsDropDownOpen && Previewer is { } view && TextBox is { } textBox)

        {

            if (ReferenceEquals(e.Source, textBox) && TimeRangeViewFocusHelper.TryHandleTextBoxTab(view, textBox, e))

                return true;



            if (TimeRangeViewFocusHelper.TryHandlePreviewerTab(view, TextBox, e))

                return true;

        }



        return base.ProcessKey(e);

    }



    protected override void OnPreviewerKeyDown(object? sender, KeyEventArgs e)

    {

        if (IsDropDownOpen && Previewer is { } view && TimeRangeViewFocusHelper.TryHandlePreviewerTab(view, TextBox, e))

        {

            e.Handled = true;

            return;

        }



        base.OnPreviewerKeyDown(sender, e);

    }



    protected override void FocusPreviewerOnTabFromTextBox(Control previewer)

    {

        if (previewer is TimeRangeView view)

            view.FocusStartHour();

        else

            base.FocusPreviewerOnTabFromTextBox(previewer);

    }

}


